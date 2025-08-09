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
                string query = "SELECT vendorID, vendorName, vendorPrice, vendorProvince, vendorCity, category FROM vendor";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
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
            if (string.IsNullOrWhiteSpace(txtVendorName.Text) || string.IsNullOrWhiteSpace(txtVendorPrice.Text) ||
                ddlProvince.SelectedValue == "" || ddlCity.SelectedValue == "" || ddlCategory.SelectedValue == "" ||
                !fuVendorImage.HasFile)
            {
                // Show error message if any field is empty or no file uploaded
                lblMessage.Text = "Please complete all fields and upload an image.";
                lblMessage.Visible = true;
                return;
            }

            // Ensure the uploaded image is a .png
            string extension = System.IO.Path.GetExtension(fuVendorImage.FileName).ToLower();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
            {
                lblMessage.Text = "Only .png, .jpg, or .jpeg images are allowed.";
                lblMessage.Visible = true;
                return;
            }

            // Handle image name and auto-renaming if exists
            string imageFileName = System.IO.Path.GetFileName(fuVendorImage.FileName);
            string savePath = Server.MapPath("~/Images/Vendors/") + imageFileName;
            int counter = 1;

            // Add a suffix if file already exists
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
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category, image_filename)
                         VALUES (@Name, @Price, @Province, @City, @Category, @Image)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtVendorName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtVendorPrice.Text.Trim()));
                cmd.Parameters.AddWithValue("@Province", ddlProvince.SelectedValue);
                cmd.Parameters.AddWithValue("@City", ddlCity.SelectedValue);
                cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Image", imageFileName);

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
    }
}