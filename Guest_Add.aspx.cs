using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Guest_Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlRS.Items.Add(new ListItem("None", "None"));
                ddlRS.Items.Add(new ListItem("RSVP Reception Only", "Reception Only"));
                ddlRS.Items.Add(new ListItem("RSVP All Events", "All Events"));
                ddlRS.Items.Add(new ListItem("RSVP Ceremony Only", "Ceremony Only"));
                ddlRS.Items.Add(new ListItem("RSVP NA", "NA"));

                ddlDS.Items.Add(new ListItem("Vegan", "Vegan"));
                ddlDS.Items.Add(new ListItem("Vegetarian", "Vegetarian"));
                ddlDS.Items.Add(new ListItem("Standard", "Standard"));
                ddlDS.Items.Add(new ListItem("Gluten-Free", "Gluten-Free"));
            }
        }
    }
}