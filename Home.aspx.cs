using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["justLoggedIn"] != null && (bool)Session["justLoggedIn"])
            {
                // FInd the popup from the master page & show it
                var popup = (Panel)Master.FindControl("welcomePopup");
                if (popup != null)
                {
                    popup.Visible = true;
                }

                // Reset the flag so it doesn't show again on refresh
                Session["justLoggedIn"] = null;
            }
        }
    }
}