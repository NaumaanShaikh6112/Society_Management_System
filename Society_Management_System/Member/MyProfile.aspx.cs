using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Society_Management_System.Member
{
    public partial class MyProfile : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);

            if (Session["user_id"] == null || Session["role"]?.ToString() != "user")
            {
                Response.Redirect("~/Account/Login.aspx?msg=invalid_access");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfileDetails();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
        }

        private void LoadProfileDetails()
        {
            if (Session["user_id"] == null) return;

            try
            {
                long userId = Convert.ToInt64(Session["user_id"]);

                using (SqlCommand cmd = new SqlCommand("sp_Members_GetProfileDetailsByUserID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Always check for DBNull before accessing
                            lblFullName.Text = dr["full_name"] == DBNull.Value ? "-" : dr["full_name"].ToString();
                            lblEmail.Text = dr["email"] == DBNull.Value ? "-" : dr["email"].ToString();
                            lblPhone.Text = dr["phone"] == DBNull.Value ? "-" : dr["phone"].ToString();
                            lblSocietyName.Text = dr["society_name"] == DBNull.Value ? "Not Linked" : dr["society_name"].ToString();
                            lblBuildingName.Text = dr["building_name"] == DBNull.Value ? "Not Linked" : dr["building_name"].ToString();
                            lblUnitNo.Text = dr["unit_no"] == DBNull.Value ? "Not Assigned" : dr["unit_no"].ToString();
                            lblOccupancyType.Text = dr["occupancy_type"] == DBNull.Value ? "N/A" : dr["occupancy_type"].ToString();
                        }
                        else
                        {
                            lblError.Text = "No profile data found. Please contact admin.";
                            lblError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading profile details: " + ex.Message;
                lblError.Visible = true;

                lblFullName.Text = lblEmail.Text = lblPhone.Text =
                lblSocietyName.Text = lblBuildingName.Text = lblUnitNo.Text = lblOccupancyType.Text = "-";
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
