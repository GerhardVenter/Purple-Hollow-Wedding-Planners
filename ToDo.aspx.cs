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
            LoadTasks(); // ✅ Called early enough for event handling to work
        }

        protected void Page_Load(object sender, EventArgs e)
        {



        
            if (!IsPostBack || ViewState["EditingTaskID"] != null)
            {
                LoadTasks(); // Ensure controls are rebuilt on first load or edit mode
            }
        
        
           
            
        

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

                // Get tasks for the user
                string getTasksQuery = "SELECT taskID, taskDescription FROM Task WHERE userID = @userID";
                using (MySqlCommand taskCmd = new MySqlCommand(getTasksQuery, conn))
                {
                    taskCmd.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader reader = taskCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int taskID = reader.GetInt32("taskID"); // ✅ THIS IS IMPORTANT
                            string task = reader.GetString("taskDescription");

                            TableRow row = new TableRow();
                            row.CssClass = "task-row";
                            TableCell cell = new TableCell();

                            // Literal for checkbox + task text
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
                                cancelBtn.CssClass = "cancelBtn"; // Optional: for styling
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
                                // Show read-only task
                                LiteralControl literal = new LiteralControl($@"
    <div class='checker'>
        <input type='checkbox' class='checkbox'/>
        <span class='task-text'>{task}</span>
    </div>
    ");
                                taskDisplay = literal;
                            }

                            cell.Controls.Add(taskDisplay);

                            // Create Edit button
                            Button editBtn = new Button();
                            editBtn.ID = "edit_" + taskID;
                            editBtn.Text = "Edit";
                            editBtn.CssClass = "editBtn";
                            editBtn.CommandArgument = taskID.ToString();
                            editBtn.Click += new EventHandler(EditTask_Click);

                            // Create Delete button
                            Button deleteBtn = new Button();
                            deleteBtn.ID = "delete_" + taskID;
                            deleteBtn.Text = "Delete";
                            deleteBtn.CssClass = "deleteBtn";
                            deleteBtn.CommandArgument = taskID.ToString();
                            deleteBtn.Click += new EventHandler(DeleteTask_Click);

                            // Wrap both in a div
                            Panel buttonPanel = new Panel();
                            buttonPanel.CssClass = "actionButtons";
                            buttonPanel.Controls.Add(editBtn);
                            buttonPanel.Controls.Add(deleteBtn);

                            cell.Controls.Add(buttonPanel);    // ✅ Add buttons to cell
                            row.Cells.Add(cell);               // ✅ Add cell to row
                            taskTable.Rows.Add(row);           // ✅ Add row to table


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

            // Search for the TextBox in the taskTable
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

                // Update the task
                string updateQuery = "UPDATE Task SET taskDescription = @desc WHERE taskID = @taskID AND userID = @userID";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@desc", newDescription);
                    updateCmd.Parameters.AddWithValue("@taskID", taskID);
                    updateCmd.Parameters.AddWithValue("@userID", userID);
                    int rows = updateCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        lblMsg.Text = "Task updated!";
                        lblMsg.ForeColor = System.Drawing.Color.Green;
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

            // Show the taskID being deleted (debugging)
            lblMsg.Text = $"Attempting to delete task with ID: {taskID}";
            lblMsg.ForeColor = System.Drawing.Color.Blue;

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            // Get current username and userID
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

                    // Delete only the task that matches taskID AND belongs to userID
                    string deleteQuery = "DELETE FROM Task WHERE taskID = @taskID AND userID = @userID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskID", taskID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        int rowsDeleted = cmd.ExecuteNonQuery();

                        if (rowsDeleted > 0)
                        {
                            lblMsg.Text = $"Task with ID {taskID} deleted successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Purple;
                        }
                        else
                        {
                            lblMsg.Text = $"No task deleted. Either task does not exist or does not belong to you.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                LoadTasks(); // Refresh task list after deletion
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error deleting task: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }



        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";  // Clear previous messages

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
                            lblMsg.Text = "Task added successfully!";
                            lblMsg.ForeColor = System.Drawing.Color.Purple;

                            // Clear the textbox only if successful
                            txtTaskDescription.Text = "";
                            lblMsg.Text = "";
                            LoadTasks();
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
