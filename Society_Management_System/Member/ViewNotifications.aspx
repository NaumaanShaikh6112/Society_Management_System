<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.Master" AutoEventWireup="true" CodeBehind="ViewNotifications.aspx.cs" Inherits="Society_Management_System.Member.ViewNotifications" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <div class="container mt-4">
        <h3 class="mb-3">🔔 My Notifications</h3>

        <!-- Notification List -->
        <asp:Repeater ID="rptNotifications" runat="server" OnItemCommand="rptNotifications_ItemCommand">
            <ItemTemplate>
                <div class="card mb-2 p-3 <%# Convert.ToBoolean(Eval("is_read")) ? "" : "border-warning" %>">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="mb-1"><%# Eval("title") %></h5>
                            <p class="mb-1 text-muted"><%# Eval("message") %></p>
                            <small class="text-secondary">📅 <%# String.Format("{0:dd-MMM-yyyy hh:mm tt}", Eval("created_at")) %></small>
                        </div>
                        <div>
                            <%# !Convert.ToBoolean(Eval("is_read")) ? "<span class='badge bg-warning text-dark'>New</span>" : "" %>
                            <asp:Button ID="btnOpen" runat="server" Text="Open" 
                                CommandName="OpenNotification"
                                CommandArgument='<%# Eval("notification_id") %>' 
                                CssClass="btn btn-primary btn-sm ms-2" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoNotifications" runat="server" CssClass="text-muted" Visible="false" Text="No notifications found." />
    </div>
</asp:Content>
