<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManagePayments.aspx.cs" Inherits="Society_Management_System.Admin.ManagePayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">Manage Payments</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect p-6 max-w-5xl mx-auto mb-8">
        <h2>Record Payment</h2>

        <asp:Label ID="LblMessage" runat="server" CssClass="text-success mb-3 d-block" />

        <div class="mb-3">
            <label>Select Bill</label>
            <asp:DropDownList ID="DdlBills" runat="server" CssClass="form-select" />
        </div>
        <div class="mb-3">
            <label>Payment Amount</label>
            <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="mb-3">
            <label>Payment Mode</label>
            <asp:DropDownList ID="DdlPaymentMode" runat="server" CssClass="form-select">
                <asp:ListItem Text="Cash" Value="Cash" />
                <asp:ListItem Text="Online Transfer" Value="Online Transfer" />
                <asp:ListItem Text="Cheque" Value="Cheque" />
            </asp:DropDownList>
        </div>
        <div class="mb-3">
            <label>Reference Number</label>
            <asp:TextBox ID="TxtReference" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="BtnRecordPayment" runat="server" Text="Record Payment" CssClass="btn btn-primary" OnClick="BtnRecordPayment_Click" />

        <h3 class="mt-5">Recent Payments</h3>
        <asp:GridView ID="GvPayments" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="payment_id" HeaderText="Payment ID" ReadOnly="True" />
                <asp:BoundField DataField="bill_id" HeaderText="Bill ID" />
                <asp:BoundField DataField="paid_on" HeaderText="Paid On" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:C2}" />
                <asp:BoundField DataField="mode" HeaderText="Payment Mode" />
                <asp:BoundField DataField="reference_no" HeaderText="Reference Number" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
