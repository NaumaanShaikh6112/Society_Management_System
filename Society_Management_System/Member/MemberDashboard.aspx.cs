using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls; // Required for Literal and Label

namespace Society_Management_System.Member
{
    public partial class MemberDashboard : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize and open connection
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);
            con.Open();

            // Session check should be handled by Master page, but double-check is okay
             if (Session["user_id"] == null || Session["role"]?.ToString() != "user")
             {
                 Response.Redirect("~/Account/Login.aspx?msg=invalid_access");
                 return;
             }

            if (!IsPostBack)
            {
                LoadDashboardStats();
                // Set Welcome message using Master Page's label value (if available)
                Label lblUserNameMaster = this.Master.FindControl("lblUserName") as Label;
                if (lblUserNameMaster != null && !string.IsNullOrEmpty(lblUserNameMaster.Text) && lblUserNameMaster.Text != "user")
                {
                    litMemberName.Text = lblUserNameMaster.Text;
                }
                else
                {
                    // Fallback if master page label wasn't set yet or has default value
                     LoadMemberNameDirectly(); // Load name directly if needed
                }
            }
        }

         protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        // Optional: Load name directly if master page label isn't ready
        private void LoadMemberNameDirectly()
        {
             try
            {
                if (Session["user_id"] != null)
                {
                    long userId = Convert.ToInt64(Session["user_id"]);
                    using (SqlCommand cmd = new SqlCommand("sp_Members_GetNameByUserID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            litMemberName.Text = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading member name directly: " + ex.Message);
            }
        }


        private void LoadDashboardStats()
        {
             if (Session["user_id"] == null) return; // Should not happen due to master page check

            try
            {
                 long userId = Convert.ToInt64(Session["user_id"]);
                 using (SqlCommand cmd = new SqlCommand("sp_GetMemberDashboardStats", con))
                 {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblUnpaidBillsCount.Text = dr["UnpaidBillsCount"].ToString();
                            lblOpenComplaintsCount.Text = dr["OpenComplaintsCount"].ToString();
                            lblActiveBookingsCount.Text = dr["ActiveBookingsCount"].ToString();
                        }
                    }
                 }
            }
            catch(Exception ex)
            {
                 System.Diagnostics.Debug.WriteLine("Error loading member dashboard stats: " + ex.Message);
                 lblUnpaidBillsCount.Text = "N/A";
                 lblOpenComplaintsCount.Text = "N/A";
                 lblActiveBookingsCount.Text = "N/A";
            }
        }
    }
}
