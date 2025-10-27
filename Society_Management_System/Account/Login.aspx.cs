using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Diagnostics;
using Society_Management_System; // <-- makes DBHelper visible from Account code-behind

namespace Society_Management_System.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session.Clear();
        }

        // PascalCase to satisfy naming rules and match markup
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername?.Text?.Trim() ?? string.Empty;
                string password = txtPassword?.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ShowError("Please enter username and password.");
                    return;
                }

                const string query = @"SELECT u.user_id, u.member_id, u.username, r.name AS RoleName,
                                        ur.society_id, m.full_name, m.email
                                       FROM users u
                                       JOIN user_roles ur ON u.user_id = ur.user_id
                                       JOIN roles r ON ur.role_id = r.role_id
                                       LEFT JOIN members m ON u.member_id = m.member_id
                                       WHERE u.username = @username
                                         AND u.password_hash = @password
                                         AND u.is_active = 1";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password) // TODO: use a secure hash comparison in production
                };

                DataTable dt = DBHelper.GetData(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    Session["UserID"] = row["user_id"];
                    Session["Username"] = row["username"].ToString();
                    Session["Role"] = row["RoleName"].ToString();
                    Session["SocietyID"] = row["society_id"];

                    if (row["member_id"] != DBNull.Value)
                    {
                        Session["MemberID"] = row["member_id"];
                        Session["FullName"] = row["full_name"];
                        Session["Email"] = row["email"];
                    }

                    const string updateQuery = "UPDATE users SET last_login_at = GETDATE() WHERE user_id = @userID";
                    SqlParameter[] updateParams = new SqlParameter[] { new SqlParameter("@userID", row["user_id"]) };
                    DBHelper.Execute(updateQuery, updateParams);

                    long userId = Convert.ToInt64(row["user_id"]);
                    LogAuditEntry(userId, "Login", "users", userId);

                    string role = row["RoleName"].ToString();
                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        Response.Redirect("~/Admin/AdminDashboard.aspx");
                    else
                        Response.Redirect("~/Member/MemberDashboard.aspx");
                }
                else
                {
                    ShowError("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // ex is used here, no unused-variable warning
                Trace.TraceError("Login error: " + ex);
                ShowError("An error occurred during login. Please try again later.");
            }
        }

        private void ShowError(string message)
        {
            if (pnlError != null && lblError != null)
            {
                pnlError.Visible = true;
                lblError.Text = message;
            }
        }

        private void LogAuditEntry(long userId, string action, string entityType, long entityId)
        {
            try
            {
                const string query = @"INSERT INTO audit_logs (user_id, action, entity_type, entity_id, created_at)
                                       VALUES (@userID, @action, @entityType, @entityID, GETDATE())";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@userID", userId),
                    new SqlParameter("@action", action),
                    new SqlParameter("@entityType", entityType),
                    new SqlParameter("@entityID", entityId)
                };

                DBHelper.Execute(query, parameters);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Login error: " + ex);
                System.Diagnostics.Trace.TraceWarning("Audit logging failed: " + ex);
            }
        }
    }
}