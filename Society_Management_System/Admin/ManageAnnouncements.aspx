<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageAnnouncements.aspx.cs" Inherits="Society_Management_System.Admin.ManageAnnouncements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Announcements
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect p-6 rounded-2xl shadow-lg max-w-4xl mx-auto mb-8">
        <h2 class="text-xl font-semibold mb-4 text-white">Add New Announcement</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            
            <!-- Society Selection -->
            <div class="flex flex-col">
                <label for="ddlSociety" class="mb-1 text-white font-medium">Select Society</label>
                <asp:DropDownList ID="ddlSociety" runat="server" CssClass="glass-input"></asp:DropDownList>
            </div>
            
            <!-- Title -->
            <div class="flex flex-col">
                <label for="txtTitle" class="mb-1 text-white font-medium">Title</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="glass-input" placeholder="Enter announcement title"></asp:TextBox>
            </div>
            
            <!-- Visible From -->
            <div class="flex flex-col">
                <label for="txtVisibleFrom" class="mb-1 text-white font-medium">Visible From</label>
                <asp:TextBox ID="txtVisibleFrom" runat="server" CssClass="glass-input" placeholder="Select start date" TextMode="Date"></asp:TextBox>
            </div>
            
            <!-- Visible To -->
            <div class="flex flex-col">
                <label for="txtVisibleTo" class="mb-1 text-white font-medium">Visible To</label>
                <asp:TextBox ID="txtVisibleTo" runat="server" CssClass="glass-input" placeholder="Select end date" TextMode="Date"></asp:TextBox>
            </div>
            
            <!-- Content -->
            <div class="flex flex-col md:col-span-2">
                <label for="txtContent" class="mb-1 text-white font-medium">Content</label>
                <asp:TextBox ID="txtContent" runat="server" CssClass="glass-input h-32" TextMode="MultiLine" placeholder="Write announcement details"></asp:TextBox>
            </div>
            
        </div>
        <div class="mt-4">
            <asp:Button ID="btnAddAnnouncement" runat="server" Text="Add Announcement" CssClass="btn btn-primary" OnClick="btnAddAnnouncement_Click" />
        </div>
    </div>

    <!-- Announcements Grid -->
    <div class="glass-effect p-6 rounded-2xl shadow-lg max-w-6xl mx-auto">
        <h2 class="text-xl font-semibold mb-4 text-white">All Announcements</h2>
        <asp:GridView ID="gvAnnouncements" runat="server" CssClass="glass-grid" AutoGenerateColumns="False" OnRowCommand="gvAnnouncements_RowCommand">
            <Columns>
                <asp:BoundField DataField="announcement_id" HeaderText="ID" />
                <asp:BoundField DataField="title" HeaderText="Title" />
                <asp:BoundField DataField="content" HeaderText="Content" />
                <asp:BoundField DataField="visible_from" HeaderText="Visible From" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="visible_to" HeaderText="Visible To" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm rounded-pill"
                            CommandName="DeleteAnnouncement" CommandArgument='<%# Eval("announcement_id") %>'
                            OnClientClick="return confirm('Are you sure you want to delete this announcement?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>