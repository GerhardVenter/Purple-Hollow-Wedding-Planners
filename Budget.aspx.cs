using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Budget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // block anonymous access
            if (Session["userID"] == null)
            {
                var returnUrl = Server.UrlEncode(Request.RawUrl);
                Response.Redirect("Login.aspx?returnUrl=" + returnUrl);
                return;
            }

            // stop cached back-navigation showing this page after logout
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (!IsPostBack)
            {
                // load your budget data here
            }
        }

        protected int CurrentUserId =>
            Session["userID"] == null ? 0 : Convert.ToInt32(Session["userID"]);

        protected void btnBudgetHelp_Click(object sender, EventArgs e)
        {
            // Show the help popup for the budget page
            // Example:
            // pnlBudgetHelp.Visible = true;
        }
    }
}