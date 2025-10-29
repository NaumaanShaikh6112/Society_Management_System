using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageComplaints : Page
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComplaints();
            }
        }

        private void BindComplaints()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT complaint_id, title, category, status, created_at FROM complaints ORDER BY created_at DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvComplaints.DataSource = dt;
                gvComplaints.DataBind();
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                lblMessage.Text = "Title and Category are required.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO complaints (society_id, raised_by_user_id, unit_id, category, title, description, status, created_at)
                                VALUES (@society_id, @raised_by_user_id, @unit_id, @category, @title, @description, @status, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@society_id", 1); // example static value
                    cmd.Parameters.AddWithValue("@raised_by_user_id", 1); // example static value
                    cmd.Parameters.AddWithValue("@unit_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@category", txtCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.LawnGreen;
                        lblMessage.Text = "Complaint added successfully!";
                        ClearInput();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Error adding complaint.";
                    }
                }
            }

            BindComplaints();
        }

        private void ClearInput()
        {
            txtTitle.Text = "";
            txtCategory.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void GvComplaints_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvComplaints.EditIndex = e.NewEditIndex;
            BindComplaints();
        }

        protected void GvComplaints_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvComplaints.EditIndex = -1;
            BindComplaints();
        }

        protected void GvComplaints_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvComplaints.Rows[e.RowIndex];
            int complaintId = Convert.ToInt32(gvComplaints.DataKeys[e.RowIndex].Value);

            string title = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string category = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string status = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE complaints SET title=@title, category=@category, status=@status WHERE complaint_id=@complaint_id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@complaint_id", complaintId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            gvComplaints.EditIndex = -1;
            BindComplaints();
        }

        protected void GvComplaints_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int complaintId = Convert.ToInt32(gvComplaints.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM complaints WHERE complaint_id=@complaint_id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@complaint_id", complaintId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            BindComplaints();
        }

        protected void GvComplaints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                e.Row.CssClass = "edit-row";

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox tb)
                        {
                            tb.CssClass = "glass-input";
                        }
                        else if (control is DropDownList ddl)
                        {
                            ddl.CssClass = "glass-input";
                        }
                    }
                }
            }
        }
    }
}