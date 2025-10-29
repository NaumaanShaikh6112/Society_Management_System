using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageExpenses : System.Web.UI.Page
    {
        SqlConnection con;
        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString);
            if (!IsPostBack)
            {
                BindSocieties();
                BindExpenses();
            }
        }

        private void BindSocieties()
        {
            try
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

                    ddlSociety.Items.Insert(0, new ListItem("-- Select Society --", ""));
                }
            }
            catch
            {
                // if societies table is missing or empty
                ddlSociety.Items.Clear();
                ddlSociety.Items.Add(new ListItem("No societies found", ""));
            }
        }

        protected void BindExpenses()
        {
            string query = @"SELECT e.expense_id, s.name AS society_name, e.expense_date, e.category, e.amount, e.notes 
                             FROM expenses e
                             INNER JOIN societies s ON e.society_id = s.society_id
                             ORDER BY e.expense_date DESC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvExpenses.DataSource = dt;
                gvExpenses.DataBind();
            }
        }

        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlSociety.SelectedValue))
            {
                Response.Write("<script>alert('Please select a society first.');</script>");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategory.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                Response.Write("<script>alert('Please fill in category and amount.');</script>");
                return;
            }

            using (SqlCommand cmd = new SqlCommand("INSERT INTO expenses (society_id, expense_date, category, amount, notes) VALUES (@sid, @date, @cat, @amt, @note)", con))
            {
                cmd.Parameters.AddWithValue("@sid", ddlSociety.SelectedValue);
                cmd.Parameters.AddWithValue("@date", string.IsNullOrWhiteSpace(txtExpenseDate.Text) ? DateTime.Now : DateTime.Parse(txtExpenseDate.Text));
                cmd.Parameters.AddWithValue("@cat", txtCategory.Text.Trim());
                cmd.Parameters.AddWithValue("@amt", decimal.Parse(txtAmount.Text));
                cmd.Parameters.AddWithValue("@note", txtNotes.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // clear inputs
            ddlSociety.SelectedIndex = 0;
            txtExpenseDate.Text = txtCategory.Text = txtAmount.Text = txtNotes.Text = string.Empty;

            BindExpenses();
            Response.Write("<script>alert('Expense added successfully.');</script>");
        }

        protected void gvExpenses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteExpense")
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM expenses WHERE expense_id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                BindExpenses();
                Response.Write("<script>alert('Expense deleted successfully.');</script>");
            }
        }
    }
}