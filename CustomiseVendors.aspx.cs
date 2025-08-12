using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class CustomiseVendors : System.Web.UI.Page
    {
        private DataTable VendorsTable
        {
            get
            {
                return ViewState["VendorsTable"] as DataTable;
            }
            set
            {
                ViewState["VendorsTable"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication check
            if (Session["userID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadVendors();
            }
        }

        private void LoadVendors()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT vendorID, vendorName, vendorPrice, vendorProvince, vendorCity, category FROM vendor WHERE userID = @UserID";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@UserID", Session["userID"]);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                VendorsTable = dt;
                rptVendors.DataSource = dt;
                rptVendors.DataBind();
            }
        }

        // Undo just reloads data
        protected void UndoChanges_Click(object sender, EventArgs e)
        {
            LoadVendors();
            lblMessage.Text = "All changes have been undone!";
            lblMessage.Visible = true;
        }

        // Save commits to database if any local changes (extendable)
        protected void SaveChanges_Click(object sender, EventArgs e)
        {
            // For now, we only reload confirmation, if using staging logic extend this to update DB
            LoadVendors();
            lblMessage.Text = "Changes saved successfully!";
            lblMessage.Visible = true;
        }

        // Show add popup
        protected void ShowAddPopup(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = true;
        }

        // Add vendor logic
        protected void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtVendorName.Text) ||
                string.IsNullOrWhiteSpace(txtVendorPrice.Text) ||
                ddlProvince.SelectedValue == "" ||
                ddlCity.SelectedValue == "" ||
                ddlCategory.SelectedValue == "" ||
                !fuVendorImage.HasFile)
            {
                lblMessage.Text = "Please complete all fields and upload an image.";
                lblMessage.Visible = true;
                return;
            }

            // Validate price is a positive decimal and within allowed range
            const decimal MAX_VENDOR_PRICE = 1000000.00M;
            if (!decimal.TryParse(txtVendorPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                lblMessage.Text = "Price must be a positive number.";
                lblMessage.Visible = true;
                return;
            }
            if (price > MAX_VENDOR_PRICE)
            {
                lblMessage.Text = $"Price must not exceed {MAX_VENDOR_PRICE}.";
                lblMessage.Visible = true;
                return;
            }

            // Validate image extension
            string extension = System.IO.Path.GetExtension(fuVendorImage.FileName).ToLower();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
            {
                lblMessage.Text = "Only .png, .jpg, or .jpeg images are allowed.";
                lblMessage.Visible = true;
                pnlAddVendor.Visible = true;
                return;
            }

            // Validate image MIME type
            string mimeType = fuVendorImage.PostedFile.ContentType.ToLower();
            if (mimeType != "image/png" && mimeType != "image/jpeg")
            {
                lblMessage.Text = "Uploaded file is not a valid image.";
                lblMessage.Visible = true;
                pnlAddVendor.Visible = true;
                return;
            }

            // Optional: Prevent duplicate vendor names for the same user
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM vendor WHERE vendorName = @Name AND userID = @UserID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Name", txtVendorName.Text.Trim());
                checkCmd.Parameters.AddWithValue("@UserID", Session["userID"]);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    lblMessage.Text = "A vendor with this name already exists.";
                    lblMessage.Visible = true;
                    return;
                }
            }

            // Handle image name and auto-renaming if exists
            string imageFileName = System.IO.Path.GetFileName(fuVendorImage.FileName);
            string savePath = Server.MapPath("~/Images/Vendors/") + imageFileName;
            int counter = 1;
            while (System.IO.File.Exists(savePath))
            {
                string fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(imageFileName);
                imageFileName = $"{fileNameWithoutExt}-{counter}{extension}";
                savePath = Server.MapPath("~/Images/Vendors/") + imageFileName;
                counter++;
            }

            // Save the uploaded file
            fuVendorImage.SaveAs(savePath);

            // Save vendor details in database
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category, image_filename, userID)
                 VALUES (@Name, @Price, @Province, @City, @Category, @Image, @UserID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtVendorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Province", ddlProvince.SelectedValue);
                    cmd.Parameters.AddWithValue("@City", ddlCity.SelectedValue);
                    cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Image", imageFileName);
                    cmd.Parameters.AddWithValue("@UserID", Session["userID"]);

                    cmd.ExecuteNonQuery();
                }

                // Show the success message
                lblMessage.Text = "Changes saved successfully!";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;

                // Success Message
                Session["VendorAddSuccess"] = true;
                Response.Redirect("Vendors.aspx");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry error code
                {
                    lblMessage.Text = "A vendor with this name already exists for your account. Please use the 'Add Existing Vendors' button.";
                }
                else
                {
                    lblMessage.Text = "A database error occurred. Please try again.";
                }
                lblMessage.Visible = true;
                pnlAddVendor.Visible = true;
                // Optionally log ex.Message for diagnostics
            }
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = false;
        }

        protected void btnCloseSuccess_Click(object sender, EventArgs e)
        {
            pnlSuccess.Visible = false;
        }

        // Handle delete button clicks from repeater
        protected void rptVendors_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteVendor")
            {
                int vendorID = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    string query = "DELETE FROM vendor WHERE vendorID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", vendorID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadVendors();
                lblMessage.Text = "Vendor deleted!";
                lblMessage.Visible = true;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Vendors.aspx");
        }
        protected void btnUpdateVendor_Click(object sender, EventArgs e)
        {
            // TODO: Show update vendor popup or redirect to update page
            lblMessage.Text = "Update Vendor Details feature coming soon!";
            lblMessage.Visible = true;
        }
        protected void ShowAddExistingPopup(object sender, EventArgs e)
        {
            // Populate dropdown with all vendor names (excluding those already added by this user)
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                    SELECT DISTINCT vendorName
                    FROM vendor
                    WHERE vendorName NOT IN (
                        SELECT vendorName FROM vendor WHERE userID = @UserID
                    )";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"]);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlExistingVendors.Items.Clear();
                    ddlExistingVendors.Items.Add(new ListItem("Select a vendor", ""));
                    while (reader.Read())
                    {
                        ddlExistingVendors.Items.Add(new ListItem(reader.GetString(0)));
                    }
                }
            }
            pnlAddExistingVendor.Visible = true;
        }
        protected void btnConfirmAddExistingVendor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlExistingVendors.SelectedValue))
            {
                lblAddExistingVendorMessage.Text = "Please select a vendor.";
                lblAddExistingVendorMessage.Visible = true;
                pnlAddExistingVendor.Visible = true;
                return;
            }

            // Copy the selected vendor's details (except userID) and insert for this user
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string selectQuery = "SELECT vendorPrice, vendorProvince, vendorCity, category, image_filename FROM vendor WHERE vendorName = @Name LIMIT 1";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@Name", ddlExistingVendors.SelectedValue);
                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal price = reader.GetDecimal(0);
                        string province = reader.GetString(1);
                        string city = reader.GetString(2);
                        string category = reader.GetString(3);
                        string imageFile = reader.GetString(4);

                        reader.Close();

                        // Insert for this user
                        string insertQuery = @"INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category, image_filename, userID)
                                               VALUES (@Name, @Price, @Province, @City, @Category, @Image, @UserID)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@Name", ddlExistingVendors.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Price", price);
                        insertCmd.Parameters.AddWithValue("@Province", province);
                        insertCmd.Parameters.AddWithValue("@City", city);
                        insertCmd.Parameters.AddWithValue("@Category", category);
                        insertCmd.Parameters.AddWithValue("@Image", imageFile);
                        insertCmd.Parameters.AddWithValue("@UserID", Session["userID"]);
                        try
                        {
                            insertCmd.ExecuteNonQuery();
                            Session["VendorAddSuccess"] = true;
                            Response.Redirect("Vendors.aspx");
                        }
                        catch (MySqlException ex)
                        {
                            if (ex.Number == 1062)
                                lblAddExistingVendorMessage.Text = "You have already added this vendor.";
                            else
                                lblAddExistingVendorMessage.Text = "A database error occurred. Please try again.";
                            lblAddExistingVendorMessage.Visible = true;
                            pnlAddExistingVendor.Visible = true;
                        }
                    }
                    else
                    {
                        lblAddExistingVendorMessage.Text = "Vendor not found.";
                        lblAddExistingVendorMessage.Visible = true;
                        pnlAddExistingVendor.Visible = true;
                    }
                }
            }
        }

        protected void btnCancelAddExistingVendor_Click(object sender, EventArgs e)
        {
            pnlAddExistingVendor.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int vendorID = int.Parse(hfDeleteVendorID.Value);

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string imageFileName = null;

            // 1. Get the image filename before deleting the vendor
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string selectQuery = "SELECT image_filename FROM vendor WHERE vendorID=@id";
                using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@id", vendorID);
                    object result = selectCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        imageFileName = result.ToString();
                }

                // 2. Delete the vendor
                string deleteQuery = "DELETE FROM vendor WHERE vendorID=@id";
                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id", vendorID);
                    deleteCmd.ExecuteNonQuery();
                }
            }

            // 3. Delete the image file from disk (if it exists)
            if (!string.IsNullOrEmpty(imageFileName))
            {
                string imagePath = Server.MapPath("~/Images/Vendors/" + imageFileName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            LoadVendors();

            // Show the success popup
            pnlSuccess.Visible = true;
            // Optionally, change the message for delete
            pnlSuccess.Controls[0].Visible = true; // Make sure the label is visible
            if (pnlSuccess.Controls[0] is Label label)
            {
                label.Text = "Vendor deleted successfully!";
            }
            else
            {
                // Handle the case where the control is not a Label
                throw new InvalidOperationException("The first control in pnlSuccess is not a Label.");
            }
        }
    }
}