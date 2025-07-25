using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class ToDo : System.Web.UI.Page
    {
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
                    }
                }

                
                string getTasksQuery = "SELECT taskID, taskDescription FROM Task WHERE userID = @userID";
                using (MySqlCommand taskCmd = new MySqlCommand(getTasksQuery, conn))
                {
                    taskCmd.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader reader = taskCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int taskID = reader.GetInt32("taskID"); 
                            string task = reader.GetString("taskDescription");

                            TableRow row = new TableRow();
                            row.CssClass = "task-row";
                            TableCell cell = new TableCell();

                          
                            Control taskDisplay;

                            if (taskID == editingTaskID)
                            {
                                // Show textbox and save button
                                TextBox editBox = new TextBox();
                                editBox.ID = "txtEdit_" + taskID;
                                editBox.Text = task;
                                editBox.CssClass = "editInput";

                                Button saveBtn = new Button();
                                saveBtn.ID = "save_" + taskID;
                                saveBtn.Text = "Save";
                                saveBtn.CssClass = "saveBtn";
                                saveBtn.CommandArgument = taskID.ToString();
                                saveBtn.Click += new EventHandler(SaveTask_Click);

                                Button cancelBtn = new Button();
                                cancelBtn.ID = "cancel_" + taskID;
                                cancelBtn.Text = "Cancel";
                                cancelBtn.CssClass = "cancelBtn"; 
                                cancelBtn.CommandArgument = taskID.ToString();
                                cancelBtn.Click += new EventHandler(CancelEdit_Click);


                                Panel editPanel = new Panel();
                                editPanel.Controls.Add(editBox);
                                editPanel.Controls.Add(saveBtn);
                                editPanel.Controls.Add(cancelBtn);
                                taskDisplay = editPanel;
                            }
                            else
                            {
                                
                                LiteralControl literal = new LiteralControl($@"
                                <div class='checker'>
                                <input type='checkbox' class='checkbox' onchange='toggleStrike(this)'/>
                                <span class='task-text'>{task}</span>
                                </div>
                                ");
                                taskDisplay = literal;
                            }

                            cell.Controls.Add(taskDisplay);

                            // Creates Edit button
                            Button editBtn = new Button();
                            editBtn.ID = "edit_" + taskID;
                            editBtn.Text = "Edit";
                            editBtn.CssClass = "editBtn";
                            editBtn.CommandArgument = taskID.ToString();
                            editBtn.Click += new EventHandler(EditTask_Click);

                            // Creates Delete button
                            Button deleteBtn = new Button();
                            deleteBtn.ID = "delete_" + taskID;
                            deleteBtn.Text = "Delete";
                            deleteBtn.CssClass = "deleteBtn";
                            deleteBtn.CommandArgument = taskID.ToString();

                            // Add this to show confirmation BEFORE server-side DeleteTask_Click
                            deleteBtn.OnClientClick = "return confirm('Are you sure you want to delete this task?');";

                            deleteBtn.Click += new EventHandler(DeleteTask_Click);






                            Panel buttonPanel = new Panel();
                            buttonPanel.CssClass = "actionButtons";
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

            
            foreach (TableRow row in taskTable.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    TextBox txt = cell.FindControl("txtEdit_" + taskID) as TextBox;
                    if (txt != null)
                    {
                        newDescription = txt.Text.Trim();
                        break;
                    }
                }
                if (newDescription != null) break;
            }

            if (string.IsNullOrEmpty(newDescription))
            {
                lblMsg.Text = "Description cannot be empty or textbox not found.";
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

                
                string updateQuery = "UPDATE Task SET taskDescription = @desc WHERE taskID = @taskID AND userID = @userID";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@desc", newDescription);
                    updateCmd.Parameters.AddWithValue("@taskID", taskID);
                    updateCmd.Parameters.AddWithValue("@userID", userID);
                    int rows = updateCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        lblMsg.Text = "";
                        ClientScript.RegisterStartupScript(this.GetType(), "taskUpdatedPopup", "showUpdatedPopup();", true);


                    }
                    else
                    {
                        lblMsg.Text = "Failed to update task.";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }
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
            if (string.IsNullOrEmpty(taskDescription))
            {
                lblMsg.Text = "Please enter a task description.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
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

                    string insertTaskQuery = "INSERT INTO Task (userID, taskDescription) VALUES (@userID, @taskDescription)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertTaskQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@userID", userID);
                        insertCmd.Parameters.AddWithValue("@taskDescription", taskDescription);
                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            txtTaskDescription.Text = "";
                            lblMsg.Text = "";
                            LoadTasks();

                            // Trigger JavaScript popup from server
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
