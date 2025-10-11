using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Budget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect("Login.aspx?returnUrl=" + returnUrl);
                return;
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            rptItems.ItemCommand += rptItems_ItemCommand;

            if (!IsPostBack)
                LoadBudget();
        }

        protected int CurrentUserId =>
            Session["userID"] == null ? 0 : Convert.ToInt32(Session["userID"]);

        protected void btnBudgetHelp_Click(object sender, EventArgs e)
        {
            // Show the help popup for the budget page
            // Example:
            // pnlBudgetHelp.Visible = true;
        }

        private void LoadBudget()
        {
            int userId = Convert.ToInt32(Session["userID"]);
            string cs = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            string jsonData = "[]"; // default for chart

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                int budgetId = 0;
                using (var cmd = new MySqlCommand("SELECT budgetID FROM budget WHERE userID=@u", con))
                {
                    cmd.Parameters.AddWithValue("@u", userId);
                    var r = cmd.ExecuteScalar();
                    if (r != null) budgetId = Convert.ToInt32(r);
                }

                if (budgetId == 0)
                {
                    rptItems.DataSource = null;
                    rptItems.DataBind();
                    lblTotalBudget.Text = "R0";
                    lblTotalSpent.Text = "R0";
                    lblRemaining.Text = "R0";

                    // expose empty data to JS
                    ClientScript.RegisterStartupScript(this.GetType(), "budgetChartData",
                        "var budgetChartData = [];", true);
                    return;
                }

                // items
                var dt = new DataTable();
                using (var da = new MySqlDataAdapter(
                    "SELECT itemID, category, name, cost, isPaid FROM budget_items WHERE budgetID=@b ORDER BY category", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@b", budgetId);
                    da.Fill(dt);
                }
                rptItems.DataSource = dt;
                rptItems.DataBind();

                // KPIs
                decimal totalBudget = 0, totalSpent = 0;
                using (var cmd = new MySqlCommand("SELECT COALESCE(totalBudget,0) FROM budget WHERE budgetID=@b", con))
                { cmd.Parameters.AddWithValue("@b", budgetId); totalBudget = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0); }
                using (var cmd = new MySqlCommand("SELECT COALESCE(SUM(cost),0) FROM budget_items WHERE budgetID=@b AND isPaid=1", con))
                { cmd.Parameters.AddWithValue("@b", budgetId); totalSpent = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0); }

                var remaining = totalBudget - totalSpent;
                lblTotalBudget.Text = $"R{totalBudget:0,0.##}";
                lblTotalSpent.Text = $"R{totalSpent:0,0.##}";
                lblRemaining.Text = $"R{remaining:0,0.##}";

                // build chart data from dt
                var chartData = new List<object>();
                foreach (DataRow row in dt.Rows)
                {
                    chartData.Add(new
                    {
                        category = row["category"].ToString(),
                        cost = Convert.ToDecimal(row["cost"])
                    });
                }

                // serialize for JS
                var serializer = new JavaScriptSerializer();
                jsonData = serializer.Serialize(chartData);
            }

            // expose to the page
            ClientScript.RegisterStartupScript(this.GetType(), "budgetChartData",
                $"var budgetChartData = {jsonData};", true);
        }

        protected void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var item = (RepeaterItem)chk.NamingContainer;
            var hf = (HiddenField)item.FindControl("hfCategory");
            string category = hf.Value;
            bool isPaid = chk.Checked;

            int userId = Convert.ToInt32(Session["userID"]);
            string cs = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                // get this user's budgetID
                int budgetId = 0;
                using (var cmd = new MySqlCommand("SELECT budgetID FROM budget WHERE userID=@u", con))
                { cmd.Parameters.AddWithValue("@u", userId); var r = cmd.ExecuteScalar(); if (r != null) budgetId = Convert.ToInt32(r); }

                if (budgetId > 0)
                {
                    using (var cmd = new MySqlCommand(
                        "UPDATE budget_items SET isPaid=@p WHERE budgetID=@b AND category=@c", con))
                    {
                        cmd.Parameters.AddWithValue("@p", isPaid ? 1 : 0);
                        cmd.Parameters.AddWithValue("@b", budgetId);
                        cmd.Parameters.AddWithValue("@c", category);
                        cmd.ExecuteNonQuery();
                    }
                }

                // recalc header totals (spent = sum of paid items)
                using (var cmd = new MySqlCommand(@"
            UPDATE budget b
            JOIN ( SELECT budgetID, COALESCE(SUM(cost),0) sumCost
                   FROM budget_items WHERE budgetID=@b GROUP BY budgetID ) s
            ON b.budgetID=s.budgetID
            SET b.totalBudget = s.sumCost", con))
                {
                    cmd.Parameters.AddWithValue("@b", budgetId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadBudget();     // rebind table and KPI numbers
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                string category = e.CommandArgument.ToString();
                int userId = Convert.ToInt32(Session["userID"]);

                string cs = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
                using (var con = new MySqlConnection(cs))
                {
                    con.Open();

                    int budgetId = 0;
                    using (var cmd = new MySqlCommand("SELECT budgetID FROM budget WHERE userID=@u", con))
                    {
                        cmd.Parameters.AddWithValue("@u", userId);
                        var r = cmd.ExecuteScalar();
                        if (r != null) budgetId = Convert.ToInt32(r);
                    }

                    if (budgetId > 0)
                    {
                        using (var cmd = new MySqlCommand("DELETE FROM budget_items WHERE budgetID=@b AND category=@c", con))
                        {
                            cmd.Parameters.AddWithValue("@b", budgetId);
                            cmd.Parameters.AddWithValue("@c", category);
                            cmd.ExecuteNonQuery();
                        }

                        // Recalculate totalBudget
                        using (var cmd = new MySqlCommand(@"
                    UPDATE budget b
                    JOIN (
                        SELECT budgetID, COALESCE(SUM(cost), 0) AS totalCost
                        FROM budget_items
                        WHERE budgetID=@b
                        GROUP BY budgetID
                    ) s ON b.budgetID = s.budgetID
                    SET b.totalBudget = s.totalCost;", con))
                        {
                            cmd.Parameters.AddWithValue("@b", budgetId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                LoadBudget(); // reload table and totals
                ScriptManager.RegisterStartupScript(
    this, GetType(), "toastRemoved",
    "showToast('Vendor removed');", true);
            }
        }

    }
}