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
            lblAddMessage.Text = "All changes have been undone!";
            lblAddMessage.Visible = true;
        }

        // Save commits to database if any local changes (extendable)
        protected void SaveChanges_Click(object sender, EventArgs e)
        {
            // For now, we only reload confirmation, if using staging logic extend this to update DB
            LoadVendors();
            lblAddMessage.Text = "Changes saved successfully!";
            lblAddMessage.Visible = true;
        }

        // Add vendor logic
        protected void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddVendorName.Text) ||
                string.IsNullOrWhiteSpace(txtAddVendorPrice.Text) ||
                ddlAddProvince.SelectedValue == "" ||
                ddlAddCity.SelectedValue == "" ||
                ddlAddCategory.SelectedValue == "" ||
                !fuAddVendorImage.HasFile)
            {
                lblAddMessage.Text = "Please complete all fields and upload an image.";
                lblAddMessage.Visible = true;
                return;
            }

            const decimal MAX_VENDOR_PRICE = 1000000.00M;
            if (!decimal.TryParse(txtAddVendorPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                lblAddMessage.Text = "Price must be a positive number.";
                lblAddMessage.Visible = true;
                return;
            }
            if (price > MAX_VENDOR_PRICE)
            {
                lblAddMessage.Text = $"Price must not exceed {MAX_VENDOR_PRICE}.";
                lblAddMessage.Visible = true;
                return;
            }

            string extension = Path.GetExtension(fuAddVendorImage.FileName).ToLower();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
            {
                lblAddMessage.Text = "Only .png, .jpg, or .jpeg images are allowed.";
                lblAddMessage.Visible = true;
                pnlAddVendor.Visible = true;
                return;
            }

            string mimeType = fuAddVendorImage.PostedFile.ContentType.ToLower();
            if (mimeType != "image/png" && mimeType != "image/jpeg")
            {
                lblAddMessage.Text = "Uploaded file is not a valid image.";
                lblAddMessage.Visible = true;
                pnlAddVendor.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM vendor WHERE vendorName = @Name AND userID = @UserID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Name", txtAddVendorName.Text.Trim());
                checkCmd.Parameters.AddWithValue("@UserID", Session["userID"]);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    lblAddMessage.Text = "A vendor with this name already exists.";
                    lblAddMessage.Visible = true;
                    return;
                }
            }

            string imageFileName = Path.GetFileName(fuAddVendorImage.FileName);
            string savePath = Server.MapPath("~/Images/Vendors/") + imageFileName;
            int counter = 1;
            while (File.Exists(savePath))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                imageFileName = $"{fileNameWithoutExt}-{counter}{extension}";
                savePath = Server.MapPath("~/Images/Vendors/") + imageFileName;
                counter++;
            }

            fuAddVendorImage.SaveAs(savePath);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category, image_filename, userID)
                 VALUES (@Name, @Price, @Province, @City, @Category, @Image, @UserID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtAddVendorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Province", ddlAddProvince.SelectedValue);
                    cmd.Parameters.AddWithValue("@City", ddlAddCity.SelectedValue);
                    cmd.Parameters.AddWithValue("@Category", ddlAddCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Image", imageFileName);
                    cmd.Parameters.AddWithValue("@UserID", Session["userID"]);

                    cmd.ExecuteNonQuery();
                }

                lblAddMessage.Text = "Changes saved successfully!";
                lblAddMessage.CssClass = "success-message";
                lblAddMessage.Visible = true;

                Session["VendorAddSuccess"] = true;
                Response.Redirect("Vendors.aspx");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    lblAddMessage.Text = "A vendor with this name already exists for your account. Please use the 'Add Existing Vendors' button.";
                }
                else
                {
                    lblAddMessage.Text = "A database error occurred. Please try again.";
                }
                lblAddMessage.Visible = true;
                pnlAddVendor.Visible = true;
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
                lblAddMessage.Text = "Vendor deleted!";
                lblAddMessage.Visible = true;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Vendors.aspx");
        }
        protected void btnUpdateVendor_Click(object sender, EventArgs e)
        {
            // TODO: Show update vendor popup or redirect to update page
            lblAddMessage.Text = "Update Vendor Details feature coming soon!";
            lblAddMessage.Visible = true;
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


            var label = pnlSuccess.FindControl("lblSuccessMessage") as Label;
            if (label != null)
            {
                label.Visible = true;
                label.Text = "Vendor deleted successfully!";
            }
            else
            {
                throw new InvalidOperationException("lblSuccessMessage not found in pnlSuccess.");
            }
        }

        protected void btnShowAddPopup_Click(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = true;
            pnlUpdateVendor.Visible = false;
            // Clear fields for add mode
            txtAddVendorName.Text = "";
            txtAddVendorPrice.Text = "";
            ddlAddProvince.SelectedIndex = 0;
            ddlAddCity.SelectedIndex = 0;
            ddlAddCategory.SelectedIndex = 0;
            lblAddMessage.Visible = false;
        }

        protected void btnShowUpdatePopup_Click(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = false;
            pnlUpdateVendor.Visible = true;
            lblUpdateMessage.Visible = false;

            int vendorID = int.Parse(hfUpdateVendorID.Value);
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT vendorName, vendorPrice, vendorProvince, vendorCity, category FROM vendor WHERE vendorID=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", vendorID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUpdateVendorName.Text = reader.GetString(0);
                            txtUpdateVendorPrice.Text = reader.GetDecimal(1).ToString();
                            ddlUpdateProvince.SelectedValue = reader.GetString(2);
                            ddlUpdateCity.SelectedValue = reader.GetString(3);
                            ddlUpdateCategory.SelectedValue = reader.GetString(4);
                        }
                    }
                }
            }
        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            pnlUpdateVendor.Visible = false;
        }

        protected void btnUpdateConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfUpdateVendorID.Value))
                return;

            int vendorID = int.Parse(hfUpdateVendorID.Value);

            decimal price;
            if (!decimal.TryParse(txtUpdateVendorPrice.Text.Trim(), out price) || price <= 0)
            {
                lblUpdateMessage.Text = "Please enter a valid positive price.";
                lblUpdateMessage.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string newImageFileName = null;

            if (fuUpdateVendorImage.HasFile)
            {
                string extension = Path.GetExtension(fuUpdateVendorImage.FileName).ToLower();
                if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                {
                    lblUpdateMessage.Text = "Only .png, .jpg, or .jpeg images are allowed.";
                    lblUpdateMessage.Visible = true;
                    return;
                }

                string mimeType = fuUpdateVendorImage.PostedFile.ContentType.ToLower();
                if (mimeType != "image/png" && mimeType != "image/jpeg")
                {
                    lblUpdateMessage.Text = "Uploaded file is not a valid image.";
                    lblUpdateMessage.Visible = true;
                    return;
                }

                string tempImageFileName = Path.GetFileName(fuUpdateVendorImage.FileName);

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string checkImageQuery = "SELECT COUNT(*) FROM vendor WHERE image_filename = @ImageFileName";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkImageQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ImageFileName", tempImageFileName);
                        int imageCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (imageCount > 0)
                        {
                            lblUpdateMessage.Text = "An image with this filename already exists. Please rename your image and try again.";
                            lblUpdateMessage.Visible = true;
                            return;
                        }
                    }
                }

                string oldImageFileName = null;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string selectQuery = "SELECT image_filename FROM vendor WHERE vendorID=@id";
                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@id", vendorID);
                        object result = selectCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            oldImageFileName = result.ToString();
                    }
                }

                newImageFileName = tempImageFileName;
                string savePath = Server.MapPath("~/Images/Vendors/") + newImageFileName;
                int counter = 1;
                while (File.Exists(savePath))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(newImageFileName);
                    newImageFileName = $"{fileNameWithoutExt}-{counter}{extension}";
                    savePath = Server.MapPath("~/Images/Vendors/") + newImageFileName;
                    counter++;
                }
                fuUpdateVendorImage.SaveAs(savePath);

                if (!string.IsNullOrEmpty(oldImageFileName))
                {
                    string oldImagePath = Server.MapPath("~/Images/Vendors/" + oldImageFileName);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query;
                MySqlCommand cmd;

                if (newImageFileName != null)
                {
                    query = @"UPDATE vendor SET vendorPrice=@Price, vendorProvince=@Province, vendorCity=@City, category=@Category, image_filename=@Image
                      WHERE vendorID=@id";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Image", newImageFileName);
                }
                else
                {
                    query = @"UPDATE vendor SET vendorPrice=@Price, vendorProvince=@Province, vendorCity=@City, category=@Category
                      WHERE vendorID=@id";
                    cmd = new MySqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Province", ddlUpdateProvince.SelectedValue);
                cmd.Parameters.AddWithValue("@City", ddlUpdateCity.SelectedValue);
                cmd.Parameters.AddWithValue("@Category", ddlUpdateCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@id", vendorID);
                cmd.ExecuteNonQuery();
            }

            LoadVendors();
            pnlUpdateVendor.Visible = false;
            pnlSuccess.Visible = true;
            lblSuccessMessage.Text = "Vendor updated successfully!";
            lblSuccessMessage.Visible = true;
        }


    }
}