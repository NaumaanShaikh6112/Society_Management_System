using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls; // Make sure this is included for Label

// Ensure this namespace matches your project's structure
namespace Society_Management_System.Member
{
    public partial class Member : System.Web.UI.MasterPage
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize and open connection
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);
            con.Open();

            // Session Check for Member Role
            if (Session["user_id"] == null )
            {
                // If session is invalid or not a member, redirect to Login
                Response.Redirect("~/Account/Login.aspx?msg=session_expired_member");
                return; // Important to stop further processing
            }

            if (!IsPostBack)
            {
                LoadMemberName();
                SetActiveLink();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Close the connection when the page is unloaded
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void LoadMemberName()
        {
            try
            {
                if (Session["user_id"] != null)
                {
                    long userId = Convert.ToInt64(Session["user_id"]);
                    // Use a 'using' block for the command to ensure it's disposed
                    using (SqlCommand cmd = new SqlCommand("sp_Members_GetNameByUserID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            lblUserName.Text = result.ToString();
                        }
                        else
                        {
                             lblUserName.Text = "Member"; // Fallback
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading member name: " + ex.Message);
                lblUserName.Text = "Member"; // Fallback on error
            }
        }


        // Helper method to highlight the active page in the sidebar
        private void SetActiveLink()
        {
            string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            switch (pageName)
            {
                case "memberdashboard.aspx": lnkDashboard.CssClass += " active"; break;
                case "myprofile.aspx": lnkMyProfile.CssClass += " active"; break;
                case "mybills.aspx": lnkMyBills.CssClass += " active"; break;
                case "mypayments.aspx": lnkMyPayments.CssClass += " active"; break;
                case "mycomplaints.aspx": lnkMyComplaints.CssClass += " active"; break;
                case "mybookings.aspx": lnkMyBookings.CssClass += " active"; break;
                case "myvehicles.aspx": lnkMyVehicles.CssClass += " active"; break;
                case "announcements.aspx": lnkAnnouncements.CssClass += " active"; break;
            }
        }
    }
}

