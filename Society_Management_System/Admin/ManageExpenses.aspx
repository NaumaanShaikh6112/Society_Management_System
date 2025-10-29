<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageExpenses.aspx.cs" Inherits="Society_Management_System.Admin.ManageExpenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Expenses
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="container py-4">
    <div class="card glass-effect p-4 rounded-4">
        <h2 class="text-2xl font-bold text-center text-white mb-4">Manage Expenses</h2>

        <!-- Add Expense Section -->
        <div class="row mb-3">
            <div class="col-md-3">
                <label class="form-label text-white">Select Society</label>
                <asp:DropDownList ID="ddlSociety" runat="server" CssClass="form-select glass-input">
                    <asp:ListItem Text="-- Select Society --" Value="" />
                </asp:DropDownList>
            </div>

            <div class="col-md-2">
                <label class="form-label text-white">Date</label>
                <asp:TextBox ID="txtExpenseDate" runat="server" CssClass="form-control glass-input" TextMode="Date" placeholder="Select date"></asp:TextBox>
            </div>

            <div class="col-md-2">
                <label class="form-label text-white">Category</label>
                <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control glass-input" placeholder="e.g. Electricity, Maintenance"></asp:TextBox>
            </div>

            <div class="col-md-2">
                <label class="form-label text-white">Amount</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control glass-input" placeholder="Enter amount" TextMode="Number"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label class="form-label text-white">Notes</label>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control glass-input" placeholder="Additional notes (optional)"></asp:TextBox>
            </div>
        </div>

        <div class="text-end">
            <asp:Button ID="btnAddExpense" runat="server" Text="Add Expense" CssClass="btn btn-primary rounded-pill px-4" OnClick="btnAddExpense_Click" />
        </div>

        <hr />

        <!-- Expenses Grid -->
        <h4 class="text-white mt-3 mb-2">Expense Records</h4>
        <asp:GridView ID="gvExpenses" runat="server" CssClass="table table-hover glass-grid" AutoGenerateColumns="False" OnRowCommand="gvExpenses_RowCommand">
            <Columns>
                <asp:BoundField DataField="expense_id" HeaderText="ID" />
                <asp:BoundField DataField="society_name" HeaderText="Society" />
                <asp:BoundField DataField="expense_date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="category" HeaderText="Category" />
                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="notes" HeaderText="Notes" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm rounded-pill"
                        CommandName="DeleteExpense" CommandArgument='<%# Eval("expense_id") %>' OnClientClick="return confirm('Are you sure you want to delete this expense?');" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</div>
</asp:Content>