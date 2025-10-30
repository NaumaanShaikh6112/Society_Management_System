<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageNotifications.aspx.cs" Inherits="Society_Management_System.Admin.ManageNotifications" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" CssClass="container p-4">
    <h3 class="mb-3">Manage Notifications</h3>

    <!-- Add Notification -->
    <div class="card p-3 mb-4">
        <div class="form-group mb-3">
            <label>Select User:</label>
            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="form-group mb-3">
            <label style="color:black">Title:</label>
            <asp:TextBox ID="txtTitle" runat="server" placeholder="Title" CssClass="form-control" />
        </div>

        <div class="form-group mb-3">
            <label style="color:black">Message:</label>
            <asp:TextBox ID="txtMessage" runat="server"  placeholder="Message"  CssClass="form-control" TextMode="MultiLine" Rows="3"                  
                />
        </div>

        <div class="form-group mb-3">
            <label style="color:black">Link URL (optional):</label>
            <asp:TextBox ID="txtLink" runat="server" placeholder="/Member/Bills.aspx" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAddNotification" runat="server" Text="Add Notification" CssClass="btn btn-primary" OnClick="btnAddNotification_Click" />
    </div>

    <!-- Notification List -->
    <asp:GridView ID="gvNotifications" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
        OnRowCommand="gvNotifications_RowCommand" DataKeyNames="notification_id">
        <Columns>
            <asp:BoundField DataField="notification_id" HeaderText="ID" />
            <asp:BoundField DataField="user_id" HeaderText="User ID" />
            <asp:BoundField DataField="title" HeaderText="Title" />
            <asp:BoundField DataField="message" HeaderText="Message" />
            <asp:BoundField DataField="created_at" HeaderText="Created On" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteNotification" CommandArgument='<%# Eval("notification_id") %>' CssClass="btn btn-danger btn-sm" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>

</asp:Content>
