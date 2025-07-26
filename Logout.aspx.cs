using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the username session variable
            Session["username"] = null;

            // Optionally abandon the session entirely
            Session.Abandon();

            // Redirect to the home page (or Login.aspx if you prefer)
            Response.Redirect("Home.aspx");
        }
    }
}