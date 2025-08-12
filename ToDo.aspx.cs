using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class ToDo : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlImportance;

        int userID = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTasks();
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            // Only load tasks on first load or after edit/save
            if (!IsPostBack || ViewState["EditingTaskID"] != null)
            {
                LoadTasks();
            }
        }
       
        private void DeleteTaskByID(int taskID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"]?.ToString();
            if (string.IsNullOrEmpty(username)) return;

            int userID = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Get userID
                string getUserQuery = "SELECT userID FROM user WHERE username = @username";
                using (MySqlCommand userCmd = new MySqlCommand(getUserQuery, conn))
                {
                    userCmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read()) userID = reader.GetInt32("userID");
                        else return;
                    }
                }

                // Delete task
                string deleteQuery = "DELETE FROM Task WHERE taskID = @taskID AND userID = @userID";
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadTasks();
        }

        
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (sourceControl == hiddenDeleteBtn)
            {
                int taskID;
                if (int.TryParse(eventArgument, out taskID))
                {
                    DeleteTaskByID(taskID);
                    LoadTasks();
                    ClientScript.RegisterStartupScript(this.GetType(), "deletedPopup", "showDeletedPopup();", true);
                }
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        private void LoadTasks()
        {
            taskTable.Rows.Clear();
            int editingTaskID = Convert.ToInt32(ViewState["EditingTaskID"] ?? "0");
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                lblMsg.Text = "User not logged in.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int userID = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Get userID
                using (MySqlCommand userCmd = new MySqlCommand(
                    "SELECT userID FROM user WHERE username = @username", conn))
                {
                    userCmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            userID = reader.GetInt32("userID");
                    }
                }

                // Get tasks
                using (MySqlCommand taskCmd = new MySqlCommand(
                    "SELECT taskID, taskDescription, Importance FROM Task WHERE userID = @userID", conn))
                {
                    taskCmd.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader reader = taskCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int taskID = reader.GetInt32("taskID");
                            string task = reader.GetString("taskDescription");
                            string importance = reader.IsDBNull(reader.GetOrdinal("Importance"))
                                                ? ""
                                                : reader.GetString("Importance");

                            TableRow row = new TableRow();
                            row.CssClass = "task-row";

                            TableCell cell = new TableCell();
                            Control taskDisplay;

                            if (taskID == editingTaskID)
                            {
                                // Flex container for edit mode
                                Panel editContainer = new Panel
                                {
                                    CssClass = "edit-mode"
                                };

                                // Description textbox
                                TextBox editBox = new TextBox
                                {
                                    ID = "txtEdit_" + taskID,
                                    Text = task,
                                    CssClass = "editInput"
                                };

                                // Importance dropdown
                                DropDownList ddlEditImportance = new DropDownList
                                {
                                    ID = "ddlEditImportance_" + taskID,
                                    CssClass = "editDropdown"
                                };
                                ddlEditImportance.Items.Add(new ListItem("-- Select Importance --", ""));
                                ddlEditImportance.Items.Add(new ListItem("Low", "Low"));
                                ddlEditImportance.Items.Add(new ListItem("Medium", "Medium"));
                                ddlEditImportance.Items.Add(new ListItem("High", "High"));
                                ddlEditImportance.SelectedValue = importance;

                                // Save & Cancel buttons
                                Button saveBtn = new Button
                                {
                                    ID = "save_" + taskID,
                                    Text = "Save",
                                    CssClass = "saveBtn",
                                    CommandArgument = taskID.ToString()
                                };
                                saveBtn.Click += SaveTask_Click;

                                Button cancelBtn = new Button
                                {
                                    ID = "cancel_" + taskID,
                                    Text = "Cancel",
                                    CssClass = "cancelBtn",
                                    CommandArgument = taskID.ToString()
                                };
                                cancelBtn.Click += CancelEdit_Click;

                                // Add controls to flex container
                                editContainer.Controls.Add(editBox);
                                editContainer.Controls.Add(ddlEditImportance);
                                editContainer.Controls.Add(saveBtn);
                                editContainer.Controls.Add(cancelBtn);

                                taskDisplay = editContainer;
                            }
                            else
                            {
                                // Display mode
                                LiteralControl literal = new LiteralControl($@"
                     <div class='checker'>
                         <input type='checkbox' class='checkbox' onchange='toggleStrike(this)'/>
                         <span class='task-text'>{task} <em>({importance})</em></span>
                     </div>
                 ");
                                taskDisplay = literal;
                            }

                            cell.Controls.Add(taskDisplay);

                            // Action buttons (edit/delete)
                            Panel buttonPanel = new Panel { CssClass = "actionButtons" };

                            Button editBtn = new Button
                            {
                                ID = "edit_" + taskID,
                                Text = "Edit",
                                CssClass = "editBtn",
                                CommandArgument = taskID.ToString()
                            };
                            editBtn.Click += EditTask_Click;

                            Button deleteBtn = new Button
                            {
                                ID = "delete_" + taskID,
                                Text = "Delete",
                                CssClass = "deleteBtn",
                                CommandArgument = taskID.ToString()
                            };
                            deleteBtn.OnClientClick = "return confirm('Are you sure you want to delete this task?');";
                            deleteBtn.Click += DeleteTask_Click;

                            buttonPanel.Controls.Add(editBtn);
                            buttonPanel.Controls.Add(deleteBtn);
                            cell.Controls.Add(buttonPanel);

                            row.Cells.Add(cell);
                            taskTable.Rows.Add(row);
                        }
                    }
                }
            }
        }
        protected void CancelEdit_Click(object sender, EventArgs e)
        {
            ViewState["EditingTaskID"] = null;
            LoadTasks();
        }

        protected void SaveTask_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int taskID = Convert.ToInt32(btn.CommandArgument);

            string newDescription = null;
            string newImportance = null;

            foreach (TableRow row in taskTable.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    TextBox txt = cell.FindControl("txtEdit_" + taskID) as TextBox;
                    DropDownList ddl = cell.FindControl("ddlEditImportance_" + taskID) as DropDownList;

                    if (txt != null) newDescription = txt.Text.Trim();
                    if (ddl != null) newImportance = ddl.SelectedValue;
                }
            }

            if (string.IsNullOrEmpty(newDescription))
            {
                lblMsg.Text = "Description cannot be empty.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"]?.ToString();
            int userID = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string getUserQuery = "SELECT userID FROM user WHERE username = @username";
                using (MySqlCommand userCmd = new MySqlCommand(getUserQuery, conn))
                {
                    userCmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            userID = reader.GetInt32("userID");
                        else
                            return;
                    }
                }

                string updateQuery = "UPDATE Task SET taskDescription = @desc, importance = @importance WHERE taskID = @taskID AND userID = @userID";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@desc", newDescription);
                    updateCmd.Parameters.AddWithValue("@importance", newImportance);
                    updateCmd.Parameters.AddWithValue("@taskID", taskID);
                    updateCmd.Parameters.AddWithValue("@userID", userID);

                    if (updateCmd.ExecuteNonQuery() > 0)
                        ClientScript.RegisterStartupScript(this.GetType(), "taskUpdatedPopup", "showUpdatedPopup();", true);
                }
            }

            ViewState["EditingTaskID"] = null;
            LoadTasks();
        }

        protected void EditTask_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int taskID = Convert.ToInt32(btn.CommandArgument);
            ViewState["EditingTaskID"] = taskID;
            LoadTasks();
        }

        protected void DeleteTask_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int taskID = 0;

            try
            {
                taskID = Convert.ToInt32(btn.CommandArgument);
            }
            catch
            {
                lblMsg.Text = "Invalid task ID.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }


            //lblMsg.Text = $"Attempting to delete task with ID: {taskID}";
            // lblMsg.ForeColor = System.Drawing.Color.Blue;

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;


            string username = Session["username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                lblMsg.Text = "User not logged in.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int userID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Get userID
                    string getUserQuery = "SELECT userID FROM user WHERE username = @username";
                    using (MySqlCommand userCmd = new MySqlCommand(getUserQuery, conn))
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
                                lblMsg.Text = "User not found.";
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                        }
                    }


                    string deleteQuery = "DELETE FROM Task WHERE taskID = @taskID AND userID = @userID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskID", taskID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        int rowsDeleted = cmd.ExecuteNonQuery();

                        if (rowsDeleted > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "deletedPopup", "showDeletedPopup();", true);

                        }
                        else
                        {
                            lblMsg.Text = $"No task deleted. Either task does not exist or does not belong to you.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                LoadTasks();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error deleting task: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

       

        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            string taskDescription = txtTaskDescription.Text.Trim();
            string Importance = ddlImportance.SelectedValue;
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(ddlImportance.SelectedValue))
            {
                errors.Add("Please select an importance level.");
            }

            if (string.IsNullOrEmpty(txtTaskDescription.Text))
            {
                errors.Add("Please enter a task description.");
            }

            if (errors.Count > 0)
            {
                lblMsg.Text = string.Join("<br/>", errors);
                lblMsg.ForeColor = Color.Red;
                return;  
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                lblMsg.Text = "User not logged in.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int userID = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string getUserQuery = "SELECT userID FROM user WHERE username = @username";
                    using (MySqlCommand userCmd = new MySqlCommand(getUserQuery, conn))
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
                                lblMsg.Text = "User not found.";
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                        }
                    }

                    string insertTaskQuery = "INSERT INTO Task (userID, taskDescription, Importance) VALUES (@userID, @taskDescription, @Importance)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertTaskQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@userID", userID);
                        insertCmd.Parameters.AddWithValue("@taskDescription", taskDescription);
                        insertCmd.Parameters.AddWithValue("@Importance", Importance);
                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            txtTaskDescription.Text = "";
                            ddlImportance.ClearSelection();
                            lblMsg.Text = "";
                            LoadTasks();

                            ClientScript.RegisterStartupScript(this.GetType(), "taskAddedPopup", "showTaskPopup();", true);
                        }
                        else
                        {
                            lblMsg.Text = "Failed to add the task. Please try again.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
