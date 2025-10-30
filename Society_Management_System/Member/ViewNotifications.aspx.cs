using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Society_Management_System.Member
{
    public partial class ViewNotifications : System.Web.UI.Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user_id"] != null)
                {
                    int userId = Convert.ToInt32(Session["user_id"]);

                    // ✅ Load all notifications for current member
                    LoadNotifications(userId);
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx?msg=session_expired_member");
                }
            }
        }

        private void LoadNotifications(int userId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT notification_id, title, message, is_read, created_at, link_url
                                 FROM notifications
                                 WHERE user_id = @uid
                                 ORDER BY created_at DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptNotifications.DataSource = dt;
                    rptNotifications.DataBind();
                    lblNoNotifications.Visible = false;
                }
                else
                {
                    lblNoNotifications.Visible = true;
                }
            }
        }

        //private void MarkAsRead(int userId)
        //{
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        string query = "UPDATE notifications SET is_read = 1 WHERE user_id = @uid AND is_read = 0";
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@uid", userId);

        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        protected void rptNotifications_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "OpenNotification")
            {
                int notifId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    // 1️⃣ Get link URL for that notification
                    string query = "SELECT link_url FROM notifications WHERE notification_id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", notifId);
                    con.Open();
                    string link = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();

                    // 2️⃣ Mark as read
                    string updateQuery = "UPDATE notifications SET is_read = 1 WHERE notification_id = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", notifId);
                    con.Open();
                    updateCmd.ExecuteNonQuery();
                    con.Close();

                    // 3️⃣ Redirect if link exists
                    if (!string.IsNullOrEmpty(link))
                    {
                        Response.Redirect(link);
                    }
                    else
                    {
                        LoadNotifications(Convert.ToInt32(Session["user_id"]));
                    }
                }
            }
        }
    }
}