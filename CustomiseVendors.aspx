<%@ Page Title="Customise Vendors" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomiseVendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.CustomiseVendors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Customise Vendors
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="customise-container">
        <h2>Customise Vendors</h2>

        <!-- Toolbar -->
        <div class="customise-toolbar">
            <button type="button" class="help-button">Need help?</button>

            <div class="toolbar-right">
                <!-- Exit Button -->
                <asp:LinkButton ID="Button1" runat="server" CssClass="save-button" OnClick="btnExit_Click" >
                    <i class="fa fa-sign-out-alt"></i> Exit
                </asp:LinkButton>
            </div>
        </div>

        <div class="divider-table-container" style="display: flex; align-items: center; justify-content: center;">
            <!-- Table Wrapper -->
            <div class="customise-table-container">
                <asp:Repeater ID="rptVendors" runat="server">
                    <HeaderTemplate>
                        <table class="customise-table">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Price</th>
                                    <th>Province</th>
                                    <th>City</th>
                                    <th>Category</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("vendorName") %></td>
                            <td>R<%# Eval("vendorPrice") %></td>
                            <td><%# Eval("vendorProvince") %></td>
                            <td><%# Eval("vendorCity") %></td>
                            <td><%# Eval("category") %></td>
                            <td>
                                <!-- Delete Button -->
                                <button type="button" class="delete-button" onclick="showDeleteModal(<%# Eval("vendorID") %>)">
                                    <i class="fa fa-trash"></i> Delete
                                </button>
                                <!-- Update Button -->
                                <button type="button" class="save-button" onclick="showUpdateModal(<%# Eval("vendorID") %>)">
                                    <i class="fa fa-edit"></i> Update
                                </button>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <!-- Hidden ASP.NET Button for Server Post Back - Delete -->
                <asp:HiddenField ID="hfDeleteVendorID" runat="server" />
                <asp:Button ID="btnDeleteHidden" runat="server" style="display:none;" OnClick="btnDelete_Click" />
                <!-- Hidden Field and ASP.NET Button for Update -->
                <asp:HiddenField ID="hfUpdateVendorID" runat="server" />
                <asp:Button ID="btnUpdateHidden" runat="server" style="display:none;" OnClick="btnShowAddPopup_Click" />
            </div>

            <!-- ADD VENDOR POPUP -->
            <asp:Panel ID="pnlAddVendor" runat="server" CssClass="popup" Visible="false">
                <h3>Add Vendor</h3>

                <div class="form-group">
                    <label>Name:</label>
                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="input-field" onkeyup="validateFields()"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Price:</label>
                    <asp:TextBox ID="txtVendorPrice" runat="server" CssClass="input-field" onkeyup="validateFields()"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Province:</label>
                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="input-field" onchange="validateFields()">
                        <asp:ListItem Value="">Select province</asp:ListItem>
                        <asp:ListItem Value="">Select province</asp:ListItem>
                        <asp:ListItem>Eastern Cape</asp:ListItem>
                        <asp:ListItem>Free State</asp:ListItem>
                        <asp:ListItem>Gauteng</asp:ListItem>
                        <asp:ListItem>KwaZulu-Natal</asp:ListItem>
                        <asp:ListItem>Limpopo</asp:ListItem>
                        <asp:ListItem>Mpumalanga</asp:ListItem>
                        <asp:ListItem>Northern Cape</asp:ListItem>
                        <asp:ListItem>North West</asp:ListItem>
                        <asp:ListItem>Western Cape</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label>City:</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="input-field" onchange="validateFields()">
                        <asp:ListItem Value="">Select city</asp:ListItem>
        
                        <asp:ListItem>Johannesburg</asp:ListItem>
                        <asp:ListItem>Pretoria</asp:ListItem>
                        <asp:ListItem>Sandton</asp:ListItem>
        
                        <asp:ListItem>Cape Town</asp:ListItem>
                        <asp:ListItem>Stellenbosch</asp:ListItem>
        
                        <asp:ListItem>Durban</asp:ListItem>
                        <asp:ListItem>Pietermaritzburg</asp:ListItem>
        
                        <asp:ListItem>Gqeberha</asp:ListItem>
                        <asp:ListItem>East London</asp:ListItem>
        
                        <asp:ListItem>Bloemfontein</asp:ListItem>
        
                        <asp:ListItem>Polokwane</asp:ListItem>
        
                        <asp:ListItem>Nelspruit</asp:ListItem>
        
                        <asp:ListItem>Kimberley</asp:ListItem>
        
                        <asp:ListItem>Rustenburg</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label>Category:</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input-field" onchange="validateFields()">
                        <asp:ListItem Value="">Select category</asp:ListItem>
                        <asp:ListItem>Photography</asp:ListItem>
                        <asp:ListItem>Bakery</asp:ListItem>
                        <asp:ListItem>Music</asp:ListItem>
                        <asp:ListItem>Flowers</asp:ListItem>
                        <asp:ListItem>Catering</asp:ListItem>
                        <asp:ListItem>Venue</asp:ListItem>
                        <asp:ListItem>Videography</asp:ListItem>
                        <asp:ListItem>Jewelry</asp:ListItem>
                        <asp:ListItem>Dance Lessons</asp:ListItem>
                        <asp:ListItem>Dress Designers</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label>Upload Image:</label>
                      <asp:FileUpload ID="fuVendorImage" runat="server" CssClass="input-field" accept=".png,.jpg,.jpeg" onchange="validateFields()" />
                </div>

                <!-- Server-side error message label (moved here) -->
                <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false"></asp:Label>

                <span id="errorMessage" style="color:red; display:none;">Please fill in all fields before confirming.</span>

                <div class="button-group">
                    <!-- Confirm Add -->
                    <button type="button" id="btnConfirmAdd" class="confirm-button" disabled onclick="showConfirmModal();">
                        <i class="fa fa-check"></i> Confirm
                    </button>
                    
                    <!-- Cancel -->
                    <asp:LinkButton ID="btnCancelAdd" runat="server" CssClass="cancel-button" OnClick="btnCancelAdd_Click" >
                        <i class="fa fa-times"></i> Cancel
                    </asp:LinkButton>

                    <asp:Button ID="btnConfirmAddHidden" runat="server" CssClass="d-none" OnClick="btnConfirmAdd_Click" style="display:none;" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlAddExistingVendor" runat="server" CssClass="popup" Visible="false">
                <h3>Choose an existing vendor to add</h3>
                <div class="form-group">
                    <label for="ddlExistingVendors">Existing Vendors:</label>
                    <asp:DropDownList ID="ddlExistingVendors" runat="server" CssClass="input-field"></asp:DropDownList>
                </div>
                <asp:Label ID="lblAddExistingVendorMessage" runat="server" CssClass="message-label" Visible="false"></asp:Label>
                <div class="button-group">
                    <asp:Button ID="btnConfirmAddExistingVendor" runat="server" CssClass="confirm-button" Text="Add"
                        OnClick="btnConfirmAddExistingVendor_Click" />
                    <asp:Button ID="btnCancelAddExistingVendor" runat="server" CssClass="cancel-button" Text="Cancel"
                        OnClick="btnCancelAddExistingVendor_Click" />
                </div>
            </asp:Panel>   

            <!-- SUCCESS MESSAGE POPUP -->
            <asp:Panel ID="pnlSuccess" runat="server" CssClass="popup" Visible="false">
                <asp:Label ID="lblSuccessMessage" runat="server" CssClass="message-label" Visible="false"></asp:Label>
                <h3>Vendor successfully added</h3>
                <asp:Button ID="btnCloseSuccess" runat="server" CssClass="close-button" Text="Close" OnClick="btnCloseSuccess_Click" />
            </asp:Panel>
        </div> <!-- END OF "divider-table-container" -->      

        <!-- Add/Delete/Update Button -->
        <div class="vendor-actions">
            <asp:LinkButton ID="btnShowAddPopup" runat="server" CssClass="add-button-cust"
                OnClick="ShowAddPopup">
                <i class="fa fa-plus"></i> Add New Vendor
            </asp:LinkButton>
            <asp:LinkButton ID="btnShowAddExistingPopup" runat="server" CssClass="add-button-cust" 
                OnClick="ShowAddExistingPopup" >
                <i class="fa fa-plus"></i> Add Existing Vendor
            </asp:LinkButton>
        </div>

        <!-- Custom Confirmation Modal -->
        <div id="confirmModal" class="popup" style="display:none; z-index:2000;">
            <h3>Confirm Add Vendor</h3>
            <p>Are you sure you want to add this vendor?</p>
            <div class="button-group">
                <button type="button" class="confirm-button" onclick="submitAddVendor()">Yes, Add</button>
                <button type="button" class="cancel-button" onclick="closeConfirmModal()">Cancel</button>
            </div>
        </div>

        <!-- Delete Confirmation Modal -->
        <div id="deleteModal" class="popup" style="display:none; z-index:2000;">
            <h3>Confirm Delete Vendor</h3>
            <p>Are you sure you want to delete this vendor?</p>
            <div class="button-group">
                <button type="button" class="confirm-button" onclick="submitDeleteVendor()">Yes, Delete</button>
                <button type="button" class="cancel-button" onclick="closeDeleteModal()">Cancel</button>
            </div>
        </div>                                          

    </div>

    <script>
        function validateFields() {
            var name = document.getElementById('<%= txtVendorName.ClientID %>').value.trim();
            var price = document.getElementById('<%= txtVendorPrice.ClientID %>').value.trim();
            var province = document.getElementById('<%= ddlProvince.ClientID %>').value;
            var city = document.getElementById('<%= ddlCity.ClientID %>').value;

            var category = document.getElementById('<%= ddlCategory.ClientID %>').value;
            var image = document.getElementById('<%= fuVendorImage.ClientID %>').value;

            var confirmButton = document.getElementById('btnConfirmAdd');
            var errorMsg = document.getElementById('errorMessage');

            var priceValid = !isNaN(price) && Number(price) > 0;

            if (name && price && priceValid && province && city && category && image) {
                confirmButton.disabled = false;
                errorMsg.style.display = "none";
            } else {
                confirmButton.disabled = true;
                errorMsg.style.display = "block";
                if (!priceValid && price) {
                    errorMsg.textContent = "Price must be a positive number.";
                } else {
                    errorMsg.textContent = "Please fill in all fields before confirming.";
                }
            }
        }

        function showConfirmModal() {
            document.getElementById('confirmModal').style.display = 'block';
        }

        function closeConfirmModal() {
            document.getElementById('confirmModal').style.display = 'none';
        }

        function submitAddVendor() {
            // Trigger the hidden ASP.NET button for server postback
            document.getElementById('<%= btnConfirmAddHidden.ClientID %>').click();
            closeConfirmModal();
        }

        function showDeleteModal(vendorID) {
            document.getElementById('<%= hfDeleteVendorID.ClientID %>').value = vendorID;
            document.getElementById('deleteModal').style.display = 'block';
        }

        function closeDeleteModal() {
            document.getElementById('deleteModal').style.display = 'none';
        }

        function submitDeleteVendor() {
            document.getElementById('<%= btnDeleteHidden.ClientID %>').click();
            closeDeleteModal();
        }

        function showUpdateModal(vendorID) {
            document.getElementById('<%= hfUpdateVendorID.ClientID %>').value = vendorID;
            document.getElementById('<%= btnUpdateHidden.ClientID %>').click(); // Trigger server-side postback
        }
    </script>
</asp:Content>