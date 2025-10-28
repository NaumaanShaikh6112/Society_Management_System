using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Society_Management_System.Admin
{
    public partial class ManageSocieties : System.Web.UI.Page
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
                BindGridView();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void BindGridView()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_Societies_GetAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvSocieties.DataSource = dt;
                        gvSocieties.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void ClearForm()
        {
            hfSocietyID.Value = "0";
            lblFormTitle.Text = "Add New Society";
            btnSave.Text = "Save Society";
            txtName.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtPincode.Text = string.Empty;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string spName;

                if (hfSocietyID.Value == "0")
                {
                    // INSERT
                    spName = "sp_Societies_Insert";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address1", txtAddress1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address2", txtAddress2.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // UPDATE
                    spName = "sp_Societies_Update";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", Convert.ToInt64(hfSocietyID.Value));
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address1", txtAddress1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address2", txtAddress2.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                BindGridView();
                ClearForm();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // You can show an error message to the user here
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void gvSocieties_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                try
                {
                    long societyID = Convert.ToInt64(e.CommandArgument);
                    using (SqlCommand cmd = new SqlCommand("sp_Societies_GetByID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", societyID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                hfSocietyID.Value = row["society_id"].ToString();
                                txtName.Text = row["name"].ToString();
                                txtAddress1.Text = row["address_line1"].ToString();
                                txtAddress2.Text = row["address_line2"].ToString();
                                txtCity.Text = row["city"].ToString();
                                txtState.Text = row["state"].ToString();
                                txtPincode.Text = row["pincode"].ToString();

                                lblFormTitle.Text = "Edit Society";
                                btnSave.Text = "Update Society";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvSocieties_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long societyID = Convert.ToInt64(e.Keys[0]);
                using (SqlCommand cmd = new SqlCommand("sp_Societies_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SocietyID", societyID);
                    cmd.ExecuteNonQuery();
                }
                BindGridView();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // TODO: Show a user-friendly error if deletion fails (e.g., due to foreign key constraints)
            }
        }
    }
}