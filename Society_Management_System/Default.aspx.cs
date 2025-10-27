using System;
using System.Web;
using System.Web.UI;

namespace Society_Management_System
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
        }

        protected void BtnGoToLogin_Click(object sender, EventArgs e)
        {
            RedirectSafe("~/Account/Login.aspx");
        }

        protected void BtnGetStarted_Click(object sender, EventArgs e)
        {
            RedirectSafe("~/Account/Register.aspx");
        }

        private void RedirectSafe(string relativeUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativeUrl))
                    throw new ArgumentException("URL must be provided.", nameof(relativeUrl));

                Response.Redirect(relativeUrl, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Redirect failed to '{relativeUrl}': {ex}");
                ShowClientAlert("Navigation failed. Please try again or contact support.");
            }
        }

        private void ShowClientAlert(string message)
        {
            var encoded = System.Web.HttpUtility.JavaScriptStringEncode(message, addDoubleQuotes: true);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert({encoded});", true);
        }
    }
}