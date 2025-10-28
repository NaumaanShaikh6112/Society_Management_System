<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageSocieties.aspx.cs" Inherits="Society_Management_System.Admin.ManageSocieties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Societies
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <!-- Add/Edit Form Card -->
        <div class="glass-effect p-8 mb-6">
            <h2 class="text-2xl font-semibold text-white mb-6">
                <asp:Label ID="lblFormTitle" runat="server" Text="Add New Society" />
            </h2>
            
            <!-- HiddenField to store SocietyID for editing -->
            <asp:HiddenField ID="hfSocietyID" runat="server" Value="0" />
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <!-- Name -->
                <div>
                    <asp:Label runat="server" Text="Society Name" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtName" runat="server" CssClass="glass-input w-full" placeholder="e.g., Green Valley Apartments" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required" 
                        ControlToValidate="txtName" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Pincode -->
                <div>
                    <asp:Label runat="server" Text="Pincode" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtPincode" runat="server" CssClass="glass-input w-full" placeholder="e.g., 400001" />
                     <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ErrorMessage="Pincode is required" 
                        ControlToValidate="txtPincode" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Address Line 1 -->
                <div class="md:col-span-2">
                    <asp:Label runat="server" Text="Address Line 1" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="glass-input w-full" placeholder="Flat No, Building" />
                    <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ErrorMessage="Address is required" 
                        ControlToValidate="txtAddress1" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Address Line 2 -->
                <div class="md:col-span-2">
                    <asp:Label runat="server" Text="Address Line 2 (Optional)" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="glass-input w-full" placeholder="Landmark, Area" />
                </div>
                <!-- City -->
                <div>
                    <asp:Label runat="server" Text="City" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtCity" runat="server" CssClass="glass-input w-full" placeholder="e.g., Mumbai" />
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ErrorMessage="City is required" 
                        ControlToValidate="txtCity" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- State -->
                <div>
                    <asp:Label runat="server" Text="State" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtState" runat="server" CssClass="glass-input w-full" placeholder="e.g., Maharashtra" />
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" ErrorMessage="State is required" 
                        ControlToValidate="txtState" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
            </div>
            
            <!-- Action Buttons -->
            <div class="flex justify-end gap-4 mt-8">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"
                    CssClass="btn btn-outline-secondary py-2 px-5 rounded-lg" />
                <asp:Button ID="btnSave" runat="server" Text="Save Society" OnClick="btnSave_Click" 
                    CssClass="btn btn-primary py-2 px-5 rounded-lg border-0" />
            </div>
        </div>
        
        <!-- Societies List Card -->
        <div class="glass-effect p-8">
             <h2 class="text-2xl font-semibold text-white mb-6">Existing Societies</h2>
            
            <asp:GridView ID="gvSocieties" runat="server"
                AutoGenerateColumns="False"
                OnRowCommand="gvSocieties_RowCommand"
                OnRowDeleting="gvSocieties_RowDeleting"
                CssClass="glass-grid"
                DataKeyNames="society_id">
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" />
                    <asp:BoundField DataField="city" HeaderText="City" SortExpression="city" />
                    <asp:BoundField DataField="state" HeaderText="State" SortExpression="state" />
                    <asp:BoundField DataField="pincode" HeaderText="Pincode" SortExpression="pincode" />
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRow" 
                                CommandArgument='<%# Eval("society_id") %>'
                                CssClass="grid-link"><i class="bi bi-pencil-square"></i> Edit</asp:LinkButton>
                            
                            <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("society_id") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this society? This action cannot be undone.');"
                                CssClass="grid-link delete-link"><i class="bi bi-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                 <EmptyDataTemplate>
                    <div class="text-center p-4 text-white">No societies found. Add one using the form above.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
