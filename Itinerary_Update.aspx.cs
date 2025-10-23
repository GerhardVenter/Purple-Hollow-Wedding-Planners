using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Itinerary_Update : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid
                fillGrid();
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
                String query = ("SELECT itineraryID as 'Itinerary ID', itineraryName AS 'Item name', itineraryStartTime AS 'Start time', itineraryEndTime AS 'End time', itineraryDescription AS 'Short description' FROM itinerary WHERE userID = @userID");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                MySqlDataReader dbRdr = cmd.ExecuteReader();
                gvGuests.DataSource = dbRdr;
                gvGuests.DataBind();
                conn.Close();
            }
        }

        private void fillSort()
        {
            ddlSortBy.Items.Add(new ListItem("None", "None"));
            ddlSortBy.Items.Add(new ListItem("StartTime Asc", "AscitineraryStartTime"));
            ddlSortBy.Items.Add(new ListItem("StartTime Desc", "DescitineraryStartTime"));

            ddlSortBy.Items.Add(new ListItem("EndTime Asc", "AscitineraryEndTime"));
            ddlSortBy.Items.Add(new ListItem("EndTime Desc", "DescitineraryEndTime"));
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

        protected void btnEditGuest_Click(object sender, EventArgs e)
        {
            //try
            //{
                int guestID = int.Parse(Text1.Value);

                CheckIfUserExist(guestID);


            //}
            //catch (Exception)
            //{

            //    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNullEntryPopupGuest();", true);
            //}
        }

        private void EditGuest()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"].ToString();
            int itiID = int.Parse(Text1.Value);

            getUserId(username);

            //Assigning variables
            String iName = Text2.Value;
            String iDescr = Text3.Value;
            String startTime = inpST.Value;
            String endTime = inpET.Value;

            if (string.IsNullOrEmpty(iName) && string.IsNullOrEmpty(iDescr) && string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showEditedNullErrorPopup", "showEditedNullError();", true);
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    String editQuery = "";
                    if (!string.IsNullOrEmpty(iName))
                    {
                        conn.Open();
                        editQuery = "UPDATE itinerary SET itineraryName = @itiName WHERE itineraryID = @itiID AND userID = @userID";
                        using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@itiID", itiID);
                            cmd.Parameters.AddWithValue("@userID", userID);
                            cmd.Parameters.AddWithValue("@itiName", iName);
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }

                    if (!string.IsNullOrEmpty(iDescr))
                    {
                        conn.Open();
                        editQuery = "UPDATE itinerary SET itineraryDescription = @itiDesc WHERE itineraryID = @itiID AND userID = @userID";
                        using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@itiID", itiID);
                            cmd.Parameters.AddWithValue("@userID", userID);
                            cmd.Parameters.AddWithValue("@itiDesc", iDescr);
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }

                    if (!string.IsNullOrEmpty(startTime))
                    {
                        try
                        {
                            int eT = int.Parse(endTime);

                            conn.Open();
                            editQuery = "UPDATE itinerary SET itineraryEndTime = @itiET WHERE itineraryID = @itiID AND userID = @userID";
                            using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@itiID", itiID);
                                cmd.Parameters.AddWithValue("@userID", userID);
                                cmd.Parameters.AddWithValue("@itiET", eT);
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();

                        }
                        catch (Exception)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showErrorPopupGuest();", true);
                        }
                        
                    }
                }
                ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteSuccessPopupGuest();", true);
                fillGrid();
            }
                
        }

        

        private void CheckIfUserExist(int itineraryID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = ("SELECT itineraryID FROM itinerary WHERE userID = @userID AND itineraryID = @itiID");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@itiID", itineraryID);

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet itinerary = new DataSet();
                adapter.Fill(itinerary);
                conn.Close();

                if (itinerary.Tables[0].Rows.Count != 0)
                {
                    EditGuest();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNoMatchEntryPopupGuest();", true);
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
           
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Add.aspx");
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortBy();
        }

        void sortBy()
        {

            String selected = ddlSortBy.SelectedValue;
            String username = Session["username"].ToString();
            String ascDesc = "ASC";
            String fL = "itineraryStartTime";
            String query = "";

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

                    if (selected[0] == 'A')
                    {
                        ascDesc = "ASC";
                        if (selected[12] == 'S')
                        {
                            fL = "itineraryStartTime";
                        }
                        else
                        {
                            fL = "itineraryEndTime";
                        }
                    }
                    else
                    {
                        ascDesc = "Desc";

                        if (selected[13] == 'S')
                        {
                            fL = "itineraryStartTime";
                        }
                        else
                        {
                            fL = "itineraryEndTime";
                        }
                    }

                    query = ($@"SELECT itineraryName AS 'Item name', itineraryStartTime AS 'Start time', itineraryEndTime AS 'End time', itineraryDescription AS 'Short description' FROM itinerary WHERE userID = @userID ORDER BY {fL} {ascDesc}");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    MySqlDataReader dbRdr = cmd.ExecuteReader();
                    gvGuests.DataSource = dbRdr;
                    gvGuests.DataBind();
                    conn.Close();
                }
            }

        }
    }
}