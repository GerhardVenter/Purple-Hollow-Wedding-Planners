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
                return;
            }

            // Optional: Validate image file size (e.g., max 2MB)
            if (fuVendorImage.PostedFile.ContentLength > 2 * 1024 * 1024)
            {
                lblMessage.Text = "Image size must be less than 2MB.";
                lblMessage.Visible = true;
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

            // Redirect to Vendors.aspx after 2 seconds so the user can briefly see the message
            ScriptManager.RegisterStartupScript(this, GetType(), "redirect",
                "setTimeout(function(){ window.location='Vendors.aspx'; }, 2000);", true);
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
    }
}