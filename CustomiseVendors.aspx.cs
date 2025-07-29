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
                string query = "SELECT vendorName, vendorPrice, vendorProvince, vendorCity, category FROM vendor";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                rptVendors.DataSource = dt;
                rptVendors.DataBind();
            }
        }

        protected void btnShowAdd_Click(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = true;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAddVendor.Visible = false;
        }

        protected void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "INSERT INTO vendor (vendorName, vendorPrice, vendorProvince, vendorCity, category) " +
                               "VALUES (@name, @price, @province, @city, @category)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@province", ddlProvince.SelectedValue);
                cmd.Parameters.AddWithValue("@city", txtCity.Text);
                cmd.Parameters.AddWithValue("@category", ddlCategory.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Hide Add Modal and show Success
            pnlAddVendor.Visible = false;
            pnlSuccess.Visible = true;

            // Refresh vendor list
            LoadVendors();
        }

        protected void btnCloseSuccess_Click(object sender, EventArgs e)
        {
            pnlSuccess.Visible = false;
        }
    }
}