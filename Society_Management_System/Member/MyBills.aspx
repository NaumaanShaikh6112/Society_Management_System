<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.Master" AutoEventWireup="true" CodeBehind="MyBills.aspx.cs" Inherits="Society_Management_System.Member.MyBills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        .bill-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .bill-table th, .bill-table td {
            border: 1px solid #ddd;
            padding: 10px;
        }
        .bill-table th {
            background-color: #007bff;
            color: white;
            text-align: center;
        }
        .bill-table td {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 🧾 My Bills Section -->
<div class="container-fluid mt-4">
    <div class="card shadow-sm border-0">
        <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
            <h5 class="mb-0"><i class="fas fa-file-invoice"></i> My Maintenance Bills</h5>
        </div>

        <div class="card-body">
            <!-- 🔍 Filter or Search (Optional Future Feature)
            <div class="mb-3 d-flex">
                <input type="text" class="form-control me-2" placeholder="Search bills..." />
                <button class="btn btn-outline-primary"><i class="fas fa-search"></i></button>
            </div>
            -->

            <!-- 💡 GridView showing user’s bills -->
            <asp:GridView ID="GvMyBills" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered"
                EmptyDataText="No bills found." HeaderStyle-CssClass="table-primary" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="bill_id" HeaderText="Bill ID" />
                    <asp:BoundField DataField="society_name" HeaderText="Society" />
                    <asp:BoundField DataField="unit_no" HeaderText="Unit No" />
                    <asp:BoundField DataField="total_amount" HeaderText="Total Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="status" HeaderText="Status" />
                    <asp:BoundField DataField="last_payment_date" HeaderText="Last Payment" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
            </asp:GridView>

            <hr />

            <!-- 💰 Payment History Section -->
            <h6 class="mt-4 mb-3"><i class="fas fa-money-bill-wave"></i> My Recent Payments</h6>
            <asp:GridView ID="GvMyPayments" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered"
                EmptyDataText="No payments recorded yet." HeaderStyle-CssClass="table-success" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="payment_id" HeaderText="Payment ID" />
                    <asp:BoundField DataField="bill_id" HeaderText="Bill ID" />
                    <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="mode" HeaderText="Mode" />
                    <asp:BoundField DataField="paid_on" HeaderText="Paid On" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="reference_no" HeaderText="Reference No" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

</asp:Content>
