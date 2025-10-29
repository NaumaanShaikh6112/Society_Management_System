<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageVendors.aspx.cs" Inherits="Society_Management_System.Admin.ManageVendors" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <title>Manage Vendors</title>
    <style type="text/css">
        /* Use same glassmorphism CSS as ManageComplaints */
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect" style="padding:2rem; max-width:900px; margin:auto;">

        <h2 class="main-content">Vendor Management</h2>

        <asp:GridView ID="gvVendors" runat="server" AutoGenerateColumns="False" DataKeyNames="vendor_id"
            CssClass="glass-grid" Width="100%"
            OnRowEditing="GvVendors_RowEditing" OnRowUpdating="GvVendors_RowUpdating"
            OnRowCancelingEdit="GvVendors_RowCancelingEdit" OnRowDeleting="GvVendors_RowDeleting"
            OnRowDataBound="GvVendors_RowDataBound">
            <Columns>
                <asp:BoundField DataField="vendor_id" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="name" HeaderText="Vendor Name" />
                <asp:BoundField DataField="phone" HeaderText="Phone" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <h3 class="main-content" style="margin-top:2rem;">Add Vendor</h3>

        <asp:TextBox ID="txtVendorName" runat="server" CssClass="glass-input" Placeholder="Vendor Name" />
        <asp:TextBox ID="txtVendorPhone" runat="server" CssClass="glass-input" Placeholder="Phone" />
        <asp:TextBox ID="txtVendorEmail" runat="server" CssClass="glass-input" Placeholder="Email" />

        <asp:Button ID="btnAddVendor" runat="server" Text="Add Vendor" CssClass="btn-primary" OnClick="BtnAddVendor_Click" />

        <asp:Label ID="lblVendorMessage" runat="server" ForeColor="#f87171" Font-Bold="True" />
    </div>
</asp:Content>
