<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageComplaints.aspx.cs" Inherits="Society_Management_System.Admin.ManageComplaints" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <title>Manage Complaints</title>
    <style type="text/css">
        /* Glassmorphism CSS */
        body {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            background-attachment: fixed;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            overflow-x: hidden;
        }
        .glass-effect {
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255, 255, 255, 0.18);
            border-radius: 1rem;
            box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
            padding: 2rem;
            max-width: 900px;
            margin: auto;
        }
        .glass-input {
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 0.5rem;
            padding: 0.75rem 1rem;
            color: #1a202c;
            transition: all 0.2s ease-in-out;
            width: 100%;
            box-sizing: border-box;
            margin-bottom: 1rem;
        }
        .glass-input:focus {
            outline: none;
            border-color: rgba(255, 255, 255, 0.7);
            background: rgba(255, 255, 0.3);
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.5);
            color: #1a202c;
        }
        select.glass-input option {
             background: #FFFFFF;
             color: #1a202c;
        }
        .glass-grid {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            border-radius: 1rem;
            overflow: hidden;
            color: #FFFFFF;
            margin-bottom: 2rem;
        }
        .glass-grid th, .glass-grid td {
            padding: 1rem;
            text-align: left;
            border-bottom: 1px solid rgba(255, 255, 255, 0.2);
        }
        .glass-grid th {
            background: rgba(255, 255, 255, 0.2);
            backdrop-filter: blur(5px);
            -webkit-backdrop-filter: blur(5px);
            color: #FFFFFF;
            font-size: 0.875rem;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }
        .glass-grid tr:last-child td {
            border-bottom: none;
        }
        .glass-grid tr:hover {
            background: rgba(255, 255, 255, 0.05);
        }
        .edit-row, .glass-grid tr.edit-row td {
            background: rgba(255, 255, 255, 0.1) !important;
            backdrop-filter: blur(12px) !important;
            -webkit-backdrop-filter: blur(12px) !important;
            border: 1px solid rgba(255, 255, 255, 0.18) !important;
            color: #fff !important;
        }
        .btn-primary {
             background-color: #fcd34d !important;
             border-color: #fcd34d !important;
             color: #1a202c !important;
             font-weight: 600 !important;
             padding: .5rem 1rem;
             border-radius: .5rem;
             cursor: pointer;
        }
        .btn-primary:hover {
             background-color: #facc15 !important;
             border-color: #facc15 !important;
        }
        .main-content {
            color: #FFFFFF;
            margin-bottom: 1rem;
        }
        #lblMessage {
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect">

        <h2 class="main-content">Complaint Management</h2>

        <asp:GridView ID="gvComplaints" runat="server" AutoGenerateColumns="False" DataKeyNames="complaint_id"
            CssClass="glass-grid" Width="100%"
            OnRowEditing="GvComplaints_RowEditing" OnRowUpdating="GvComplaints_RowUpdating"
            OnRowCancelingEdit="GvComplaints_RowCancelingEdit" OnRowDeleting="GvComplaints_RowDeleting"
            OnRowDataBound="GvComplaints_RowDataBound">
            <Columns>
                <asp:BoundField DataField="complaint_id" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="title" HeaderText="Title" />
                <asp:BoundField DataField="category" HeaderText="Category" />
                <asp:BoundField DataField="status" HeaderText="Status" />
                <asp:BoundField DataField="created_at" HeaderText="Created" DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="True" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <h3 class="main-content">Add Complaint</h3>

        <asp:TextBox ID="txtTitle" runat="server" CssClass="glass-input" Placeholder="Title" />
        <asp:TextBox ID="txtCategory" runat="server" CssClass="glass-input" Placeholder="Category" />
        <asp:TextBox ID="txtDescription" runat="server" CssClass="glass-input" TextMode="MultiLine" Rows="4" Placeholder="Description" />

        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="glass-input" style="width:200px;">
            <asp:ListItem Value="Open" Text="Open" />
            <asp:ListItem Value="Closed" Text="Closed" />
            <asp:ListItem Value="In Progress" Text="In Progress" />
        </asp:DropDownList>

        <asp:Button ID="btnAdd" runat="server" Text="Add Complaint" CssClass="btn-primary" OnClick="BtnAdd_Click" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="#f87171" Font-Bold="True" />
    </div>
</asp:Content>