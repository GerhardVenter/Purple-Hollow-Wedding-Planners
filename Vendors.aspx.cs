using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Vendor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string category = Request.QueryString["category"] ?? "Photography"; // default to Photography
                LoadVendors(category);
            }
        }

        private void LoadVendors(string category)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT vendorName, vendorCity, vendorProvince, vendorPrice, image_filename FROM vendor WHERE category = @category";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@category", category);

                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                // Construct the image path from image_filename
                dt.Columns.Add("imagePath", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string fileName = row["image_filename"]?.ToString();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        row["imagePath"] = "Images/Vendors/" + fileName;
                    }
                    else
                    {
                        row["imagePath"] = "Images/default-vendor.jpg"; // fallback if no filename
                    }
                }

                rptVendors.DataSource = dt;
                rptVendors.DataBind();
            }
        }

        protected void btnPhotographers_Click(object sender, EventArgs e)
        {
            LoadVendors("Photography");
        }
    }
}