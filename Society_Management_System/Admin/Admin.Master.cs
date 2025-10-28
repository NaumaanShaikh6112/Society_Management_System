using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Society_Management_System.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);
            con.Open();

            if (Session["user_id"] == null || Session["role"]?.ToString() != "admin")
            {
                Response.Redirect("~/Account/Login.aspx?msg=session_expired");
            }

            if (!IsPostBack)
            {
                SetActiveLink();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void SetActiveLink()
        {
            string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            switch (pageName)
            {
                case "admindashboard.aspx": lnkDashboard.CssClass += " active"; break;
                case "managesocieties.aspx": lnkSocieties.CssClass += " active"; break;
                case "managebuildings.aspx": lnkBuildings.CssClass += " active"; break;
                case "manageunits.aspx": lnkUnits.CssClass += " active"; break;
                case "managemembers.aspx": lnkMembers.CssClass += " active"; break;
                case "managebills.aspx": lnkBills.CssClass += " active"; break;
                case "managepayments.aspx": lnkPayments.CssClass += " active"; break;
                case "managecomplaints.aspx": lnkComplaints.CssClass += " active"; break;
                case "managevehicles.aspx": lnkVehicles.CssClass += " active"; break;
                case "managevendors.aspx": lnkVendors.CssClass += " active"; break;
                case "manageexpenses.aspx": lnkExpenses.CssClass += " active"; break;
                case "manageannouncements.aspx": lnkAnnouncements.CssClass += " active"; break;
                case "manageamenities.aspx": lnkAmenities.CssClass += " active"; break;
                case "managebookings.aspx": lnkBookings.CssClass += " active"; break;
                case "managegatelogs.aspx": lnkGateLogs.CssClass += " active"; break;
                case "managedocuments.aspx": lnkDocuments.CssClass += " active"; break;
                case "manageauditlogs.aspx": lnkAuditLogs.CssClass += " active"; break;
            }
        }
    }
}