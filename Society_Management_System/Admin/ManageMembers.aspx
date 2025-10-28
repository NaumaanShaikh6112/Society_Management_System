<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageMembers.aspx.cs" Inherits="Society_Management_System.Admin.ManageMembers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Members
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <!-- Add/Edit Form Card -->
        <div class="glass-effect p-8 mb-6">
            <h2 class="text-2xl font-semibold text-white mb-6">
                <asp:Label ID="lblFormTitle" runat="server" Text="Add New Member" />
            </h2>

            <asp:HiddenField ID="hfMemberID" runat="server" Value="0" />

            <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
                <!-- Society Dropdown -->
                <div>
                    <asp:Label runat="server" Text="Select Society" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:DropDownList ID="ddlSocieties" runat="server" CssClass="glass-input w-full"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSocieties_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvSociety" runat="server" ErrorMessage="Please select a society" 
                        ControlToValidate="ddlSocieties" InitialValue="0" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Full Name -->
                <div>
                    <asp:Label runat="server" Text="Full Name" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="glass-input w-full" placeholder="e.g., Dhruv Patel" />
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Name is required" 
                        ControlToValidate="txtFullName" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Email -->
                <div>
                    <asp:Label runat="server" Text="Email" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="glass-input w-full" placeholder="e.g., dhruv@example.com" TextMode="Email" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required" 
                        ControlToValidate="txtEmail" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid email format"
                        ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Phone -->
                <div>
                    <asp:Label runat="server" Text="Phone" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="glass-input w-full" placeholder="e.g., 9876543210" TextMode="Phone" />
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ErrorMessage="Phone is required" 
                        ControlToValidate="txtPhone" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Status -->
                <div>
                    <asp:Label runat="server" Text="Status" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="glass-input w-full">
                        <asp:ListItem Value="Active" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="flex justify-end gap-4 mt-8">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"
                    CssClass="btn btn-outline-secondary py-2 px-5 rounded-lg" />
                <asp:Button ID="btnSave" runat="server" Text="Save Member" OnClick="btnSave_Click" 
                    CssClass="btn btn-primary py-2 px-5 rounded-lg border-0" />
            </div>
        </div>

        <!-- Members List Card -->
        <div class="glass-effect p-8">
             <h2 class="text-2xl font-semibold text-white mb-6">
                 Members in <asp:Literal ID="litSocietyName" runat="server" Text="Selected Society" />
             </h2>
            
            <asp:GridView ID="gvMembers" runat="server"
                AutoGenerateColumns="False"
                OnRowCommand="gvMembers_RowCommand"
                OnRowDeleting="gvMembers_RowDeleting"
                CssClass="glass-grid"
                DataKeyNames="member_id">
                <Columns>
                    <asp:BoundField DataField="full_name" HeaderText="Full Name" SortExpression="full_name" />
                    <asp:BoundField DataField="email" HeaderText="Email" SortExpression="email" />
                    <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                    <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRow" 
                                CommandArgument='<%# Eval("member_id") %>'
                                CssClass="grid-link"><i class="bi bi-pencil-square"></i> Edit</asp:LinkButton>
                            
                            <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("member_id") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this member?');"
                                CssClass="grid-link delete-link"><i class="bi bi-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                 <EmptyDataTemplate>
                    <div class="text-center p-4 text-white">No members found for this society.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
