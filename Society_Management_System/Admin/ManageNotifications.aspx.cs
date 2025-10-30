using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Society_Management_System.Admin
{
    public partial class ManageNotifications : System.Web.UI.Page
    {
        private string cs = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
                LoadNotifications();

                if (Request.QueryString["msg"] == "added")
                {
                    // ✅ Show alert only once
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Notification added successfully!');", true);

                    // ✅ Remove query string from URL after showing message (without reloading)
                    ScriptManager.RegisterStartupScript(this, GetType(), "removeMsgParam",
                        "if (window.history.replaceState) { " +
                        "const url = new URL(window.location.href); " +
                        "url.searchParams.delete('msg'); " +
                        "window.history.replaceState({}, document.title, url.toString()); }", true);
                }
            }
        }

        private void LoadUsers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT user_id, username FROM users WHERE is_active = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "username";
                ddlUsers.DataValueField = "user_id";
                ddlUsers.DataBind();

                ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select User --", ""));
            }
        }

        protected void btnAddNotification_Click(object sender, EventArgs e)
        {
            if (ddlUsers.SelectedValue == "")
                return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO notifications (user_id, title, message, link_url) VALUES (@user_id, @title, @message, @link)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", ddlUsers.SelectedValue);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@message", txtMessage.Text.Trim());
                cmd.Parameters.AddWithValue("@link", string.IsNullOrEmpty(txtLink.Text) ? (object)DBNull.Value : txtLink.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // ✅ Redirect to same page to clear form & stop resubmission
            Response.Redirect(Request.RawUrl + "?msg=added");
        }

        private void LoadNotifications()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT notification_id, user_id, title, message, created_at FROM notifications ORDER BY created_at DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvNotifications.DataSource = dt;
                gvNotifications.DataBind();
            }
        }

        protected void gvNotifications_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteNotification")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "DELETE FROM notifications WHERE notification_id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                LoadNotifications();
            }
        }
    }
}