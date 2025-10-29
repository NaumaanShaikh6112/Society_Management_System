using Society_Management_System.Admin;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageAnnouncements : System.Web.UI.Page
    {
        SqlConnection con;
        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString);
            if (!IsPostBack)
            {
                BindSocieties();
                BindAnnouncements();
            }
        }

        private void BindSocieties()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT society_id, name FROM societies ORDER BY name", con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSociety.DataSource = dt;
                ddlSociety.DataTextField = "name";
                ddlSociety.DataValueField = "society_id";
                ddlSociety.DataBind();

                ddlSociety.Items.Insert(0, new ListItem("-- Select Society --", "0"));
            }
        }

        protected void BindAnnouncements()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM announcements ORDER BY visible_from DESC", con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        protected void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a society');", true);
                return;
            }

            using (SqlCommand cmd = new SqlCommand("INSERT INTO announcements (society_id, title, content, visible_from, visible_to) VALUES (@society_id, @title, @content, @from, @to)", con))
            {
                cmd.Parameters.AddWithValue("@society_id", ddlSociety.SelectedValue);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@content", txtContent.Text.Trim());
                cmd.Parameters.AddWithValue("@from", txtVisibleFrom.Text);
                cmd.Parameters.AddWithValue("@to", string.IsNullOrEmpty(txtVisibleTo.Text) ? (object)DBNull.Value : txtVisibleTo.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                BindAnnouncements();
                ClearFields();
            }
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtContent.Text = "";
            txtVisibleFrom.Text = "";
            txtVisibleTo.Text = "";
            ddlSociety.SelectedIndex = 0;
        }

        protected void gvAnnouncements_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAnnouncement")
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM announcements WHERE announcement_id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    BindAnnouncements();
                }
            }
        }
    }
}