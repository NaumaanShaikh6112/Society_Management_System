using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Society_Management_System.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = Request.QueryString["msg"];
            if (msg == "logged_out")
            {
                pnlInfo.Visible = true;
                lblInfo.Text = "You have been logged out successfully.";
            }
            else if (msg == "session_expired")
            {
                pnlError.Visible = true;
                lblError.Text = "Session expired. Please log in again.";
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string conStr = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UserLoginProc", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                Session["user_id"] = rdr["user_id"].ToString();
                                Session["username"] = rdr["username"].ToString();
                                Session["role"] = rdr["role"] != DBNull.Value ? rdr["role"].ToString().ToLower() : "user";

                                if (Session["role"].ToString() == "admin")
                                    Response.Redirect("~/Admin/AdminDashboard.aspx");
                                else
                                    Response.Redirect("~/Member/MemberDashboard.aspx");
                            }
                            else
                            {
                                pnlError.Visible = true;
                                lblError.Text = "Invalid Username or Password.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "Something went wrong. Please try again later.";
                System.Diagnostics.Trace.TraceError("Login error: " + ex.ToString());
            }
        }
    }
}