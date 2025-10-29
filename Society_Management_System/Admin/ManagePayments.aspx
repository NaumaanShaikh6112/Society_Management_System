<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManagePayments.aspx.cs" Inherits="Society_Management_System.Admin.ManagePayments" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-effect p-6 rounded-xl">
        <h1 class="text-3xl font-semibold text-white mb-4">Manage Payments</h1>

        <!-- ✅ Add Payment Button -->
        <asp:Button ID="btnAddPayment" runat="server" CssClass="btn btn-primary mb-4" Text="Add Payment" OnClick="btnAddPayment_Click" />

        <!-- ✅ GridView for Payments -->
        <asp:GridView ID="gvPayments" runat="server" CssClass="glass-grid" AutoGenerateColumns="False" OnRowCommand="gvPayments_RowCommand">
            <Columns>
                <asp:BoundField DataField="payment_id" HeaderText="Payment ID" SortExpression="payment_id" />
                <asp:BoundField DataField="bill_id" HeaderText="Bill ID" SortExpression="bill_id" />
                <asp:BoundField DataField="paid_on" HeaderText="Paid On" SortExpression="paid_on" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="amount" HeaderText="Amount (₹)" SortExpression="amount" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="mode" HeaderText="Payment Mode" SortExpression="mode" />
                
                <asp:TemplateField>
                    <ItemTemplate>
                        
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                            CommandName="DeletePayment" CommandArgument='<%# Eval("payment_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>