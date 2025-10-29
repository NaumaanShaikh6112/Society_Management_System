<%@ Page Title="My Bills" Language="C#" MasterPageFile="~/Member/Member.Master" AutoEventWireup="true" CodeBehind="MyBills.aspx.cs" Inherits="Society_Management_System.Member.MyBills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4 text-primary fw-bold text-center">My Maintenance Bills</h3>

        <asp:GridView ID="GvMyBills" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered text-center shadow-sm"
            EmptyDataText="No bills found for your unit.">
            <Columns>
                <asp:BoundField DataField="SocietyName" HeaderText="Society" />
                <asp:BoundField DataField="BuildingName" HeaderText="Building" />
                <asp:BoundField DataField="unit_no" HeaderText="Unit No" />
                <asp:BoundField DataField="bill_month" HeaderText="Bill Month" DataFormatString="{0:yyyy-MM}" />
                <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="total_amount" HeaderText="Total Amount" DataFormatString="₹ {0:N2}" />

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# Eval("status").ToString() == "Paid" ? "badge bg-success" : "badge bg-danger" %>'>
                            <%# Eval("status") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnDownload" runat="server" Text="Download PDF"
                            CssClass="btn btn-outline-primary btn-sm"
                            CommandArgument='<%# Eval("bill_id") %>' OnClick="btnDownload_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
