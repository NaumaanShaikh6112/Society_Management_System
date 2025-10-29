<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageVehicles.aspx.cs" Inherits="Society_Management_System.Admin.ManageVehicles" %>


<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <title>Manage Vehicles</title>
    <style type="text/css">
        /* Include your glassmorphism CSS styles here or external */
        /* Same CSS as used in ManageComplaints, especially glass-grid, glass-input, btn-primary */
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect" style="padding:2rem; max-width:900px; margin:auto;">
        <h2 class="main-content">Vehicle Management</h2>

        <asp:GridView ID="gvVehicles" runat="server" AutoGenerateColumns="False" DataKeyNames="vehicle_id"
            CssClass="glass-grid" Width="100%"
            OnRowEditing="GvVehicles_RowEditing" OnRowUpdating="GvVehicles_RowUpdating"
            OnRowCancelingEdit="GvVehicles_RowCancelingEdit" OnRowDeleting="GvVehicles_RowDeleting"
            OnRowDataBound="GvVehicles_RowDataBound" >
            <Columns>
                <asp:BoundField DataField="vehicle_id" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="registration_no" HeaderText="Registration No" />
                <asp:BoundField DataField="type" HeaderText="Type" />
                <asp:BoundField DataField="member_full_name" HeaderText="Owner Name" ReadOnly="True" />
                <asp:BoundField DataField="unit_no" HeaderText="Unit No" ReadOnly="True" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <h3 class="main-content">Add Vehicle</h3>
        <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="glass-input" Placeholder="Registration Number" />
        <asp:TextBox ID="txtType" runat="server" CssClass="glass-input" Placeholder="Type (Bike, Car etc)" />

        <asp:DropDownList ID="ddlMember" runat="server" CssClass="glass-input" />

        <asp:Button ID="btnAddVehicle" runat="server" Text="Add Vehicle" CssClass="btn-primary" OnClick="BtnAddVehicle_Click" />

        <asp:Label ID="lblVehicleMessage" runat="server" ForeColor="#f87171" Font-Bold="True" />
    </div>
</asp:Content>