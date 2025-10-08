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
                string query = "SELECT vendorName, vendorCity, vendorProvince, vendorPrice, image_filename FROM vendor WHERE category = @category AND userID = @UserID";

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
                if (userId == 0)
                    return "error:userid-null";

                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    int budgetId = 0;
                    using (var cmd = new MySqlCommand("SELECT budgetID FROM budget WHERE userID = @uid", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", userId);
                        var result = cmd.ExecuteScalar();
                        if (result != null) budgetId = Convert.ToInt32(result);
                    }

                    if (budgetId == 0)
                    {
                        string insertQuery = "INSERT INTO budget (userID, totalBudget, isPaid) VALUES (@uid, @total, 0)";
                        using (var insertCmd = new MySqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@uid", userId);
                            insertCmd.Parameters.AddWithValue("@total", total);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateQuery = "UPDATE budget SET totalBudget = totalBudget + @total WHERE budgetID = @bid";
                        using (var updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@total", total);
                            updateCmd.Parameters.AddWithValue("@bid", budgetId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
                return "success";
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
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