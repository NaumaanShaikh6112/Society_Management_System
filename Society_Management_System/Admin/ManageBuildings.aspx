<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageBuildings.aspx.cs" Inherits="Society_Management_System.Admin.ManageBuildings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Buildings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <!-- Add/Edit Form Card -->
        <div class="glass-effect p-8 mb-6">
            <h2 class="text-2xl font-semibold text-white mb-6">
                <asp:Label ID="lblFormTitle" runat="server" Text="Add New Building" />
            </h2>

            <asp:HiddenField ID="hfBuildingID" runat="server" Value="0" />
            
            <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
                <!-- Society Dropdown -->
                <div>
                    <asp:Label runat="server" Text="Select Society" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:DropDownList ID="ddlSocieties" runat="server" CssClass="glass-input w-full"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSocieties_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvSociety" runat="server" ErrorMessage="Please select a society" 
                        ControlToValidate="ddlSocieties" InitialValue="0" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <!-- Building Name -->
                <div>
                    <asp:Label runat="server" Text="Building Name / Wing" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtName" runat="server" CssClass="glass-input w-full" placeholder="e.g., A-Wing" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required" 
                        ControlToValidate="txtName" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                 <!-- Total Floors -->
                <div>
                    <asp:Label runat="server" Text="Total Floors" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtFloors" runat="server" CssClass="glass-input w-full" placeholder="e.g., 12" TextMode="Number" />
                    <asp:RequiredFieldValidator ID="rfvFloors" runat="server" ErrorMessage="Floors are required" 
                        ControlToValidate="txtFloors" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                     <asp:RangeValidator ID="rvFloors" runat="server" ErrorMessage="Must be > 0" ControlToValidate="txtFloors"
                         MinimumValue="1" MaximumValue="100" Type="Integer" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="flex justify-end gap-4 mt-8">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"
                    CssClass="btn btn-outline-secondary py-2 px-5 rounded-lg" />
                <asp:Button ID="btnSave" runat="server" Text="Save Building" OnClick="btnSave_Click" 
                    CssClass="btn btn-primary py-2 px-5 rounded-lg border-0" />
            </div>
        </div>

        <!-- Buildings List Card -->
        <div class="glass-effect p-8">
             <h2 class="text-2xl font-semibold text-white mb-6">
                 Buildings in <asp:Literal ID="litSocietyName" runat="server" Text="Selected Society" />
             </h2>
            
            <asp:GridView ID="gvBuildings" runat="server"
                AutoGenerateColumns="False"
                OnRowCommand="gvBuildings_RowCommand"
                OnRowDeleting="gvBuildings_RowDeleting"
                CssClass="glass-grid"
                DataKeyNames="building_id">
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="Building/Wing Name" SortExpression="name" />
                    <asp:BoundField DataField="floors" HeaderText="Total Floors" SortExpression="floors" />
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRow" 
                                CommandArgument='<%# Eval("building_id") %>'
                                CssClass="grid-link"><i class="bi bi-pencil-square"></i> Edit</asp:LinkButton>
                            
                            <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("building_id") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this building?');"
                                CssClass="grid-link delete-link"><i class="bi bi-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                 <EmptyDataTemplate>
                    <div class="text-center p-4 text-white">No buildings found for this society.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
