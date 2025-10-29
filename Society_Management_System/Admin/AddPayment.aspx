<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddPayment.aspx.cs" Inherits="Society_Management_System.Admin.AddPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect p-6 rounded-xl">
        <h1 class="text-3xl font-semibold text-white mb-4">Add New Payment</h1>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-green-500 mb-4" />

        <div class="mb-4">
            <asp:Label ID="lblBillId" runat="server" Text="Bill ID:" CssClass="text-white" />
            <asp:TextBox ID="txtBillId" runat="server" CssClass="glass-input" />
        </div>

        <div class="mb-4">
            <asp:Label ID="lblAmount" runat="server" Text="Amount:" CssClass="text-white" />
            <asp:TextBox ID="txtAmount" runat="server" CssClass="glass-input" />
        </div>

        <div class="mb-4">
            <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode:" CssClass="text-white" />
            <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="glass-input">
                <asp:ListItem Text="Cash" Value="Cash" />
                <asp:ListItem Text="Cheque" Value="Cheque" />
                <asp:ListItem Text="Online" Value="Online" />
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnSavePayment" runat="server" Text="Save Payment" CssClass="btn btn-primary" OnClick="btnSavePayment_Click" />
    </div>
</asp:Content>