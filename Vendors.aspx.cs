using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Vendor : System.Web.UI.Page
    {
        private string selectedProvince = string.Empty;
        private string sortOrder = "price-asc";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["VendorAddSuccess"] != null && (bool)Session["VendorAddSuccess"])
            {
                // Show the popup
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessPopup",
                    "showVendorSuccessPopup();", true);
                Session["VendorAddSuccess"] = null; // Clear after showing
            }

            // Authentication check
            if (Session["userID"] == null)
            {
                // Redirect to login with returnUrl
                string returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect("Login.aspx?returnUrl=" + returnUrl);
                return;
            }

            if (!IsPostBack)
            {
                string category = Request.QueryString["category"] ?? "Photography"; // default to Photography
                LoadVendors(category);
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProvince = ddlProvince.SelectedValue;
            string category = Request.QueryString["category"] ?? "Photography";
            LoadVendors(category);
        }

        protected void ddlSortPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortOrder = ddlSortPrice.SelectedValue;
            string category = Request.QueryString["category"] ?? "Photography";
            LoadVendors(category);
        }


        private void LoadVendors(string category)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT vendorID, vendorName, vendorCity, vendorProvince,
                        vendorPrice, image_filename
                 FROM vendor
                 WHERE category=@category AND userID=@UserID";


                // Add province filter if selected  
                if (!string.IsNullOrEmpty(ddlProvince.SelectedValue))
                {
                    query += " AND vendorProvince = @province";
                }

                // Add sorting  
                if (ddlSortPrice.SelectedValue == "price-desc")
                {
                    query += " ORDER BY vendorPrice DESC";
                }
                else
                {
                    query += " ORDER BY vendorPrice ASC";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"]);

                if (!string.IsNullOrEmpty(ddlProvince.SelectedValue))
                {
                    cmd.Parameters.AddWithValue("@province", ddlProvince.SelectedValue);
                }

                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                // Construct the image path from image_filename  
                dt.Columns.Add("imagePath", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string fileName = row["image_filename"]?.ToString();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        row["imagePath"] = "Images/Vendors/" + fileName;
                    }
                    else
                    {
                        row["imagePath"] = "Images/default-vendor.jpg"; // fallback if no filename  
                    }
                }

                rptVendors.DataSource = dt;
                rptVendors.DataBind();
            }
        }

        protected void btnPhotographers_Click(object sender, EventArgs e)
        {
            LoadVendors("Photography");
        }

        protected void btnCustomiseVendors_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomiseVendors.aspx");
        }

        protected void btnCloseVendorHelp_Click(object sender, EventArgs e)
        {
            pnlVendorHelp.Visible = false;
        }

        protected void btnShowVendorHelp_Click(object sender, EventArgs e)
        {
            pnlVendorHelp.Visible = true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string AddToBudget(decimal total, object[] vendors)
        {
            try
            {
                int userId = GetCurrentUserId();
                if (userId == 0) return "error:userid-null";

                string cs = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
                using (var conn = new MySqlConnection(cs))
                {
                    conn.Open();

                    // 1) Get or create budget row
                    int budgetId = 0;
                    using (var cmd = new MySqlCommand("SELECT budgetID FROM budget WHERE userID=@u", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", userId);
                        var r = cmd.ExecuteScalar();
                        if (r != null) budgetId = Convert.ToInt32(r);
                    }
                    if (budgetId == 0)
                    {
                        using (var ins = new MySqlCommand(
                            "INSERT INTO budget (userID, totalBudget, isPaid) VALUES (@u, 0, 0); SELECT LAST_INSERT_ID();", conn))
                        {
                            ins.Parameters.AddWithValue("@u", userId);
                            budgetId = Convert.ToInt32(ins.ExecuteScalar());   // <-- capture new id
                        }
                    }

                    // 2) Upsert items
                    using (var up = new MySqlCommand(@"
INSERT INTO budget_items (budgetID, vendorID, category, name, cost, isPaid)
VALUES (@bid, @vid, @cat, @name, @cost, 0)
ON DUPLICATE KEY UPDATE cost=VALUES(cost), name=VALUES(name);", conn))
                    {
                        up.Parameters.Add("@bid", MySqlDbType.Int32).Value = budgetId;
                        up.Parameters.Add("@vid", MySqlDbType.Int32);
                        up.Parameters.Add("@cat", MySqlDbType.VarChar);
                        up.Parameters.Add("@name", MySqlDbType.VarChar);
                        up.Parameters.Add("@cost", MySqlDbType.Decimal);

                        foreach (IDictionary<string, object> v in vendors)
                        {
                            up.Parameters["@vid"].Value = v.ContainsKey("vendorId") && v["vendorId"] != null
                                                            ? Convert.ToInt32(v["vendorId"])
                                                            : (object)DBNull.Value;
                            up.Parameters["@cat"].Value = v["category"]?.ToString();
                            up.Parameters["@name"].Value = v["name"]?.ToString();
                            up.Parameters["@cost"].Value = Convert.ToDecimal(v["price"]);
                            up.ExecuteNonQuery();
                        }
                    }

                    // 3) Recompute total from items (idempotent)
                    using (var recalc = new MySqlCommand(@"
UPDATE budget b
JOIN (SELECT budgetID, COALESCE(SUM(cost),0) AS sumCost
      FROM budget_items WHERE budgetID=@bid GROUP BY budgetID) s
  ON b.budgetID=s.budgetID
SET b.totalBudget=s.sumCost;", conn))
                    {
                        recalc.Parameters.AddWithValue("@bid", budgetId);
                        recalc.ExecuteNonQuery();
                    }
                }
                return "success";
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetChosenCategories()
        {
            var userId = Convert.ToInt32(HttpContext.Current.Session["userID"]);
            var cs = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            var list = new List<string>();

            using (var c = new MySqlConnection(cs))
            {
                c.Open();
                var q = @"SELECT DISTINCT bi.category
                  FROM budget_items bi
                  JOIN budget b ON b.budgetID = bi.budgetID
                  WHERE b.userID = @u";
                using (var cmd = new MySqlCommand(q, c))
                {
                    cmd.Parameters.AddWithValue("@u", userId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            list.Add(r.GetString(0));
                    }
                }
            }
            return list;
        }

        // Helper to get current user ID (implement according to your authentication)
        private static int GetCurrentUserId()
        {
            // Example: If using session
            if (HttpContext.Current != null && HttpContext.Current.Session["userID"] != null)
                return Convert.ToInt32(HttpContext.Current.Session["userID"]);
            return 0;
        }
    }
}