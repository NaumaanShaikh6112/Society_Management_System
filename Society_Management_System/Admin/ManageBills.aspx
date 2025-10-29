<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageBills.aspx.cs" Inherits="Society_Management_System.Admin.ManageBills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- ✅ Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        body {
            background-color: #f8f9fa;
            padding: 20px;
        }
        .table td, .table th {
            vertical-align: middle;
        }
    </style>

    <div class="container mt-4">
        <h3 class="mb-4 text-center">🏢 Manage Maintenance Bills</h3>

        <!-- ✅ Add Bill Button -->
        <div class="text-end mb-3">
            <asp:Button ID="btnAddBill" runat="server" CssClass="btn btn-primary" Text="+ Add New Bill" OnClick="btnAddBill_Click" />
        </div>

        <!-- ✅ Bills GridView -->
        <asp:GridView ID="gvBills" runat="server" CssClass="table table-bordered table-striped"
            AutoGenerateColumns="False" DataKeyNames="bill_id"
            OnRowEditing="gvBills_RowEditing"
            OnRowCancelingEdit="gvBills_RowCancelingEdit"
            OnRowUpdating="gvBills_RowUpdating"
            OnRowDeleting="gvBills_RowDeleting">

            <Columns>
                <asp:BoundField DataField="bill_id" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="unit_no" HeaderText="Unit No" ReadOnly="true" />
                <asp:BoundField DataField="bill_month" HeaderText="Month" DataFormatString="{0:MMM yyyy}" />
                <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="total_amount" HeaderText="Amount (₹)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="status" HeaderText="Status" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- ✅ Add Bill Modal -->
    <!-- ✅ Add Bill Modal -->
 <div class="modal fade" id="addBillModal" tabindex="-1" aria-labelledby="addBillModalLabel" aria-hidden="true">
     <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header bg-primary text-white">
                 <h5 class="modal-title" id="addBillModalLabel">Add New Bill</h5>
                 <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
             </div>
             <div class="modal-body">
                 <div class="mb-3">
                     <label for="ddlUnits" class="form-label">Select Unit</label>
                     <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-select"></asp:DropDownList>
                 </div>

                 <div class="mb-3">
                     <label for="txtBillMonth" class="form-label">Bill Month (YYYY-MM)</label>
                     <asp:TextBox ID="txtBillMonth" runat="server" CssClass="form-control" placeholder="e.g. 2025-10"></asp:TextBox>
                 </div>

                 <div class="mb-3">
                     <label for="txtDueDate" class="form-label">Due Date</label>
                     <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                 </div>

                 <div class="mb-3">
                     <label for="txtAmount" class="form-label">Total Amount (₹)</label>
                     <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                 </div>
             </div>

             <div class="modal-footer">
                 <asp:Button ID="btnSaveBill" runat="server" CssClass="btn btn-success" Text="Save Bill" OnClick="btnSaveBill_Click" />
                 <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
             </div>
         </div>
     </div>
 </div>

    <!-- ✅ Modal Script -->
    <script type="text/javascript">
        function showAddModal() {
            var modal = new bootstrap.Modal(document.getElementById('addBillModal'));
            modal.show();
        }
    </script>
</asp:Content>