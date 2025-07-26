using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                // Show welcome and logout
                welcomeText.Text = "Welcome " + Session["username"].ToString();
                welcomeText.Visible = true;
                logoutLink.Visible = true;
                loginLink.Visible = false;
                registerLink.Visible = false;
            }
            else
            {
                // Show login and register
                welcomeText.Visible = false;
                logoutLink.Visible = false;
                loginLink.Visible = true;
                registerLink.Visible = true;
            }
        }
    }
}