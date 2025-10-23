using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (Session["username"] == null)
            {
                lblAccessMessage.Text = "You must be logged in to access this page.";
                Response.Redirect("~/Login.aspx");
                DisablePageInputs();
                return;
            }
            if (!IsPostBack)
            {
                LoadMenuItems();
             
            }
        }
        private void DisablePageInputs()
        {
            btnAddMenuItem.Enabled = false;
            txtDishName.Enabled = false;
            ddlCategory.Enabled = false;
            txtdishDescription.Enabled = false;
            gvMenuItems.Visible = false;
            btnHelpToDo.Enabled = false;
        }

        protected void btnAddMenuItem_Click(object sender, EventArgs e)
        {
            string dishName = txtDishName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string description = txtdishDescription.Text.Trim();

            if (string.IsNullOrEmpty(dishName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
            {
                
                return;
            }


            int userID = GetUserID();
            if (userID == -1)
            {
                
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Menu (userID, menuDishName,menuCategory, menuDescription) VALUES (@userID, @menuDishName, @menuCategory, @menuDescription)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@menuDishName", dishName);
                    cmd.Parameters.AddWithValue("@menuCategory", category);
                    cmd.Parameters.AddWithValue("@menuDescription", description);

                    cmd.ExecuteNonQuery();
                }
            }

           
            txtDishName.Text = "";
            ddlCategory.SelectedIndex = 0;
            txtdishDescription.Text = "";
            rfvDishName.IsValid = true;
            rfvCategory.IsValid = true;
            rfvDescription.IsValid = true;
            

            LoadMenuItems();
            
            ScriptManager.RegisterStartupScript(this, GetType(), "popup", "showDishAdded();", true);
        }
        protected void gvMenuItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMenuItems.EditIndex = e.NewEditIndex;
            LoadMenuItems();
        }
        protected void gvMenuItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMenuItems.EditIndex = -1;
            LoadMenuItems();
        }
        protected void gvMenuItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int menuID = Convert.ToInt32(e.CommandArgument);

                string username = Session["username"]?.ToString();
                if (string.IsNullOrEmpty(username))
                {
                   
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
                int userID = 0;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                   
                    string userQuery = "SELECT userID FROM user WHERE username = @username";
                    using (MySqlCommand userCmd = new MySqlCommand(userQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@username", username);
                        using (MySqlDataReader reader = userCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userID = reader.GetInt32("userID");
                            }
                            else
                            {
                                return; 
                            }
                        }
                    }

                    
                    string deleteQuery = "DELETE FROM menu WHERE menuID = @menuID AND userID = @userID";
                    using (MySqlCommand delCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        delCmd.Parameters.AddWithValue("@menuID", menuID);
                        delCmd.Parameters.AddWithValue("@userID", userID);
                        delCmd.ExecuteNonQuery();
                    }
                }

                
                LoadMenuItems();
                ScriptManager.RegisterStartupScript(this, GetType(), "popup", "showDishDeleted();", true);
            }
        }

        protected void gvMenuItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int menuID = Convert.ToInt32(gvMenuItems.DataKeys[e.RowIndex].Value);
            int userID = GetUserID();
            if (userID == -1) return;

            GridViewRow row = gvMenuItems.Rows[e.RowIndex];

            
            TextBox txtDishName = (TextBox)row.FindControl("txtEditDishName");

            
            DropDownList ddlEditCategory = (DropDownList)row.FindControl("ddlEditCategory");

          
            TextBox txtDescription = (TextBox)row.Cells[2].Controls[0];

            if (txtDishName == null || ddlEditCategory == null || txtDescription == null)
            {
                return; 
            }

            string updatedDishName = txtDishName.Text.Trim();
            string updatedCategory = ddlEditCategory.SelectedValue;
            string updatedDescription = txtDescription.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string updateQuery = @"
            UPDATE Menu 
            SET menuDishName = @dishName, menuCategory = @category, menuDescription = @description 
            WHERE menuID = @menuID AND userID = @userID";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@dishName", updatedDishName);
                    cmd.Parameters.AddWithValue("@category", updatedCategory);
                    cmd.Parameters.AddWithValue("@description", updatedDescription);
                    cmd.Parameters.AddWithValue("@menuID", menuID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    cmd.ExecuteNonQuery();
                }
            }

            gvMenuItems.EditIndex = -1;
            LoadMenuItems();
            ScriptManager.RegisterStartupScript(this, GetType(), "popup", "showDishUpdated();", true);
        }

        protected void ddlSortMenuCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortDirection = ddlSortMenuCategory.SelectedValue;
            int userID = GetUserID();
            if (userID == -1) return;

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT menuID, menuDishName, menuCategory, menuDescription FROM Menu WHERE userID = @userID";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);

                        
                        var sortedRows = table.AsEnumerable()
                            .OrderBy(row => GetCategoryOrder(row["menuCategory"].ToString(), sortDirection))
                            .CopyToDataTable();

                        gvMenuItems.DataSource = sortedRows;
                        gvMenuItems.DataBind();
                    }
                }
            }
        }
        private int GetCategoryOrder(string category, string direction)
        {
            Dictionary<string, int> ascOrder = new Dictionary<string, int>
    {
        { "Starter", 1 },
        { "Main", 2 },
        { "Dessert", 3 }
    };

            Dictionary<string, int> descOrder = new Dictionary<string, int>
    {
        { "Starter", 3 },
        { "Main", 2 },
        { "Dessert", 1 }
    };

            var orderMap = direction == "DESC" ? descOrder : ascOrder;
            return orderMap.ContainsKey(category) ? orderMap[category] : 4;
        }



        private int GetUserID()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
                return -1;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT userID FROM user WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32("userID");
                        }
                    }
                }
            }

            return -1;
        }

        private void LoadMenuItems()
        {
            int userID = GetUserID();
            if (userID == -1) return;

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT menuID, menuDishName, menuCategory, menuDescription FROM Menu WHERE userID = @userID";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        gvMenuItems.DataSource = table;
                        gvMenuItems.DataBind();
                    }
                }
            }
        }


    }
}