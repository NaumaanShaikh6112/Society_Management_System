<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.Master" AutoEventWireup="true" CodeBehind="MemberDashboard.aspx.cs" Inherits="Society_Management_System.Member.MemberDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="p-4">
        <!-- Welcome Message -->
        <h1 class="text-3xl font-light text-white mb-6">
            Welcome back, <asp:Literal ID="litMemberName" runat="server" Text="Member" />!
        </h1>

        <!-- Stats Cards Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">

            <!-- My Unpaid Bills -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">My Unpaid Bills</h3>
                    <asp:Label ID="lblUnpaidBillsCount" runat="server" Text="0" CssClass="text-4xl font-bold text-yellow-400" />
                </div>
                <i class="bi bi-receipt text-5xl text-yellow-300 opacity-30"></i>
            </div>

            <!-- My Open Complaints -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">My Open Complaints</h3>
                    <asp:Label ID="lblOpenComplaintsCount" runat="server" Text="0" CssClass="text-4xl font-bold text-red-400" />
                </div>
                <i class="bi bi-exclamation-octagon text-5xl text-red-300 opacity-30"></i>
            </div>

            <!-- My Active Bookings -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">My Active Bookings</h3>
                     <asp:Label ID="lblActiveBookingsCount" runat="server" Text="0" CssClass="text-4xl font-bold text-green-400" />
                </div>
                 <i class="bi bi-calendar2-check text-5xl text-green-300 opacity-30"></i>
            </div>

        </div>
    </div>
</asp:Content>
