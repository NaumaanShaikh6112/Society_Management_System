<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageUnits.aspx.cs" Inherits="Society_Management_System.Admin.ManageUnits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Units
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-4">
        <!-- Add/Edit Form Card -->
        <div class="glass-effect p-8 mb-6">
            <h2 class="text-2xl font-semibold text-white mb-6">
                <asp:Label ID="lblFormTitle" runat="server" Text="Add New Unit" />
            </h2>

            <asp:HiddenField ID="hfUnitID" runat="server" Value="0" />

            <!-- Cascading Dropdowns -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                <div>
                    <asp:Label runat="server" Text="Select Society" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:DropDownList ID="ddlSocieties" runat="server" CssClass="glass-input w-full"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSocieties_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvSociety" runat="server" ErrorMessage="Society is required" 
                        ControlToValidate="ddlSocieties" InitialValue="0" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <div>
                    <asp:Label runat="server" Text="Select Building" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:DropDownList ID="ddlBuildings" runat="server" CssClass="glass-input w-full"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlBuildings_SelectedIndexChanged" />
                     <asp:RequiredFieldValidator ID="rfvBuilding" runat="server" ErrorMessage="Building is required" 
                        ControlToValidate="ddlBuildings" InitialValue="0" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
            </div>

            <!-- Form Fields -->
            <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div>
                    <asp:Label runat="server" Text="Unit No." CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtUnitNo" runat="server" CssClass="glass-input w-full" placeholder="e.g., 101" />
                    <asp:RequiredFieldValidator ID="rfvUnitNo" runat="server" ErrorMessage="Unit No is required" 
                        ControlToValidate="txtUnitNo" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <div>
                    <asp:Label runat="server" Text="Floor No." CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtFloorNo" runat="server" CssClass="glass-input w-full" placeholder="e.g., 1" TextMode="Number" />
                     <asp:RequiredFieldValidator ID="rfvFloorNo" runat="server" ErrorMessage="Floor No is required" 
                        ControlToValidate="txtFloorNo" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <div>
                    <asp:Label runat="server" Text="Carpet Area (sqft)" CssClass="block text-sm font-medium text-gray-200 mb-1" />
                    <asp:TextBox ID="txtCarpetArea" runat="server" CssClass="glass-input w-full" placeholder="e.g., 650.50" />
                    <asp:RequiredFieldValidator ID="rfvCarpetArea" runat="server" ErrorMessage="Area is required" 
                        ControlToValidate="txtCarpetArea" Display="Dynamic" CssClass="text-red-400 text-sm mt-1" />
                </div>
                <div class="flex items-center pt-6 text-white">
                    <asp:CheckBox ID="chkIsParkingAllocated" runat="server" Text="&nbsp; Parking Allocated" CssClass="mr-2" />
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="flex justify-end gap-4 mt-8">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"
                    CssClass="btn btn-outline-secondary py-2 px-5 rounded-lg" />
                <asp:Button ID="btnSave" runat="server" Text="Save Unit" OnClick="btnSave_Click" 
                    CssClass="btn btn-primary py-2 px-5 rounded-lg border-0" />
            </div>
        </div>

        <!-- Units List Card -->
        <div class="glass-effect p-8">
             <h2 class="text-2xl font-semibold text-white mb-6">
                 Units in <asp:Literal ID="litBuildingName" runat="server" Text="Selected Building" />
             </h2>
            
            <asp:GridView ID="gvUnits" runat="server"
                AutoGenerateColumns="False"
                OnRowCommand="gvUnits_RowCommand"
                OnRowDeleting="gvUnits_RowDeleting"
                CssClass="glass-grid"
                DataKeyNames="unit_id">
                <Columns>
                    <asp:BoundField DataField="unit_no" HeaderText="Unit No" SortExpression="unit_no" />
                    <asp:BoundField DataField="floor_no" HeaderText="Floor" SortExpression="floor_no" />
                    <asp:BoundField DataField="carpet_area_sqft" HeaderText="Carpet Area (sqft)" SortExpression="carpet_area_sqft" DataFormatString="{0:N2}" />
                    <asp:CheckBoxField DataField="is_parking_allocated" HeaderText="Parking?" SortExpression="is_parking_allocated" ReadOnly="true" />
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRow" 
                                CommandArgument='<%# Eval("unit_id") %>'
                                CssClass="grid-link"><i class="bi bi-pencil-square"></i> Edit</asp:LinkButton>
                            
                            <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("unit_id") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this unit?');"
                                CssClass="grid-link delete-link"><i class="bi bi-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                 <EmptyDataTemplate>
                    <div class="text-center p-4 text-white">No units found. Select a society and building.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

