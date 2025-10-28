<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="Society_Management_System.Admin.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <!-- Welcome Message -->
        <h1 class="text-3xl font-light text-white mb-6">
            Welcome back, <span class="font-medium">Admin!</span>
        </h1>
        
        <!-- Stats Cards Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            
            <!-- Total Societies -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">Total Societies</h3>
                    <asp:Label ID="lblTotalSocieties" runat="server" Text="0" CssClass="text-4xl font-bold text-white" />
                </div>
                <i class="bi bi-building text-5xl text-yellow-300 opacity-30"></i>
            </div>
            
            <!-- Total Members -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">Total Members</h3>
                    <asp:Label ID="lblTotalMembers" runat="server" Text="0" CssClass="text-4xl font-bold text-white" />
                </div>
                <i class="bi bi-people-fill text-5xl text-blue-300 opacity-30"></i>
            </div>
            
            <!-- Total Buildings -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">Total Buildings</h3>
                    <asp:Label ID="lblTotalBuildings" runat="server" Text="0" CssClass="text-4xl font-bold text-white" />
                </div>
                <i class="bi bi-bricks text-5xl text-green-300 opacity-30"></i>
            </div>
            
            <!-- Open Complaints -->
            <div class="glass-effect p-6 flex items-center justify-between">
                <div>
                    <h3 class="text-lg font-semibold text-gray-200">Open Complaints</h3>
                    <asp:Label ID="lblOpenComplaints" runat="server" Text="0" CssClass="text-4xl font-bold text-red-400" />
                </div>
                <i class="bi bi-exclamation-octagon text-5xl text-red-300 opacity-30"></i>
            </div>
            
        </div>
    </div>
</asp:Content>
