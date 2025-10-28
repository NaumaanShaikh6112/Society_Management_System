using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Society_Management_System.Admin
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize and open connection
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);
            con.Open();

            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void LoadDashboardStats()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAdminDashboardStats", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblTotalSocieties.Text = dr["TotalSocieties"].ToString();
                            lblTotalMembers.Text = dr["TotalMembers"].ToString();
                            lblTotalBuildings.Text = dr["TotalBuildings"].ToString();
                            lblOpenComplaints.Text = dr["OpenComplaints"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them, show a friendly message)
                lblTotalSocieties.Text = "N/A";
                lblTotalMembers.Text = "N/A";
                lblTotalBuildings.Text = "N/A";
                lblOpenComplaints.Text = "N/A";
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}