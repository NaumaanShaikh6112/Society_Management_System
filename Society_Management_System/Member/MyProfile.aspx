<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="Society_Management_System.Member.MyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    My Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <div class="glass-effect p-8">
            <h2 class="text-2xl font-semibold text-white mb-6">My Profile Details</h2>

             <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                 <!-- Left Column -->
                 <div class="space-y-4">
                     <div>
                         <asp:Label runat="server" Text="Full Name" CssClass="profile-label" />
                         <asp:Label ID="lblFullName" runat="server" Text="-" CssClass="profile-value" />
                         <%-- Alternative: Use disabled textbox
                         <asp:TextBox ID="txtFullName" runat="server" CssClass="glass-input" ReadOnly="true" />
                         --%>
                     </div>
                      <div>
                         <asp:Label runat="server" Text="Email Address" CssClass="profile-label" />
                         <asp:Label ID="lblEmail" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                      <div>
                         <asp:Label runat="server" Text="Phone Number" CssClass="profile-label" />
                         <asp:Label ID="lblPhone" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                 </div>

                 <!-- Right Column -->
                 <div class="space-y-4">
                      <div>
                         <asp:Label runat="server" Text="Society" CssClass="profile-label" />
                         <asp:Label ID="lblSocietyName" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                      <div>
                         <asp:Label runat="server" Text="Building / Wing" CssClass="profile-label" />
                         <asp:Label ID="lblBuildingName" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                      <div>
                         <asp:Label runat="server" Text="Unit Number" CssClass="profile-label" />
                         <asp:Label ID="lblUnitNo" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                      <div>
                         <asp:Label runat="server" Text="Occupancy Type" CssClass="profile-label" />
                         <asp:Label ID="lblOccupancyType" runat="server" Text="-" CssClass="profile-value" />
                     </div>
                 </div>
            </div>
             <asp:Label ID="lblError" runat="server" CssClass="text-red-400 mt-4 block" Visible="false" />
        </div>
    </div>
</asp:Content>
