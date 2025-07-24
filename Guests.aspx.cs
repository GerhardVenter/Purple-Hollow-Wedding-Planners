using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Guests : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid
                fillGrid();

                //Filters
                fillFilter();
                

                //Sort
                fillSort();
            }
        }

        private void fillGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = ("SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                MySqlDataReader dbRdr = cmd.ExecuteReader();
                gvGuests.DataSource = dbRdr;
                gvGuests.DataBind();
                conn.Close();
            }
        }

        private void fillFilter() {
            ddlFilterBy.Items.Add(new ListItem("None", "None"));
            ddlFilterBy.Items.Add(new ListItem("RSVP Reception Only", "Reception Only"));
            ddlFilterBy.Items.Add(new ListItem("RSVP All Events", "All Events"));
            ddlFilterBy.Items.Add(new ListItem("RSVP Ceremony Only", "Ceremony Only"));
            ddlFilterBy.Items.Add(new ListItem("RSVP NA", "NA"));

            ddlFilterBy.Items.Add(new ListItem("Vegan", "Vegan"));
            ddlFilterBy.Items.Add(new ListItem("Vegetarian", "Vegetarian"));
            ddlFilterBy.Items.Add(new ListItem("Standard", "Standard"));
            ddlFilterBy.Items.Add(new ListItem("Gluten-Free", "Gluten-Free"));
        }

        private void fillSort()
        {
            ddlSortBy.Items.Add(new ListItem("None", "None"));
            ddlSortBy.Items.Add(new ListItem("First Name Asc", "AscguestFName"));
            ddlSortBy.Items.Add(new ListItem("First Name Desc", "DescguestFName"));

            ddlSortBy.Items.Add (new ListItem("Last Name Asc", "AscguestLName"));
            ddlSortBy.Items.Add (new ListItem("Last Name Desc", "DescguestLName"));
        }

        private int getUserId(String username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = ("SELECT userID FROM user WHERE username = @username");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);



                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userID = reader.GetInt32("userID");
                    }
                }

                conn.Close();

                return userID;
            }

        }

        protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter();
        }

        protected void filter()
        {
            String selected = ddlFilterBy.SelectedValue;
            String username = Session["username"].ToString();

            if (selected == "None")
            {
                fillGrid();
            }
            else
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    getUserId(username);

                    conn.Open();
                    String query = ("SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID AND (guestDSelection = @selected OR guestRSelection = @selected)");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@selected", selected);

                    MySqlDataReader dbRdr = cmd.ExecuteReader();
                    gvGuests.DataSource = dbRdr;
                    gvGuests.DataBind();
                    conn.Close();
                }
            }
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            String filterSelected = ddlFilterBy.SelectedValue;
            String selected = ddlSortBy.SelectedValue;
            String username = Session["username"].ToString();
            String ascDesc = "ASC";
            String fL = "guestFName";
            String query = "";

            if (selected == "None" && filterSelected == "None")
            {
                fillGrid();
            }
            else if (selected == "None")
            {
                filter();
            }
            else
            {
                

                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    getUserId(username);

                    conn.Open();

                    if (filterSelected == "None") 
                    {

                        if (selected[0] == 'A') 
                        {
                            ascDesc = "ASC";
                            if (selected[8] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }
                        else
                        {
                            ascDesc = "Desc";

                            if (selected[9] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }

                        query = ($@"SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID ORDER BY {fL} {ascDesc}");
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@userID", userID);

                        MySqlDataReader dbRdr = cmd.ExecuteReader();
                        gvGuests.DataSource = dbRdr;
                        gvGuests.DataBind();
                        conn.Close();
                    }
                    else
                    {
                        if (selected[0] == 'A')
                        {
                            ascDesc = "ASC";
                            if (selected[8] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }
                        else
                        {
                            ascDesc = "Desc";

                            if (selected[9] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }

                        query = ($@"SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID AND (guestDSelection = @selected OR guestRSelection = @selected) ORDER BY {fL} {ascDesc}");
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@selected", filterSelected);
                        MySqlDataReader dbRdr = cmd.ExecuteReader();
                        gvGuests.DataSource = dbRdr;
                        gvGuests.DataBind();
                        conn.Close();
                    }

                    
                }
            }
        }
    }
}
