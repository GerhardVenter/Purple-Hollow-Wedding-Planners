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
            string name = txtVendorName.Text.Trim();
            decimal price = decimal.Parse(txtVendorPrice.Text.Trim());
            string province = ddlProvince.SelectedValue;
            string city = txtCity.Text.Trim();
            string category = ddlCategory.SelectedValue;

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category) VALUES (@name, @price, @province, @city, @category)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@province", province);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@category", category);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Close popup and show success
            pnlAddVendor.Visible = false;
            pnlSuccess.Visible = true;
            LoadVendors();
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