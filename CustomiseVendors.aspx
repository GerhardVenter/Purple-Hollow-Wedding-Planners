<%@ Page Title="Customise Vendors" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomiseVendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.CustomiseVendors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Customise Vendors
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="customise-container">
        <h2>Customise Vendors</h2>

        <!-- Toolbar -->
        <div class="customise-toolbar">
            <button type="button" class="help-button">Need help?</button>

            <div class="toolbar-right">
                <asp:Button ID="btnUndo" runat="server" CssClass="undo-button" Text="Undo" OnClick="UndoChanges_Click" />
                <asp:Button ID="btnSave" runat="server" CssClass="save-button" Text="Save" OnClick="SaveChanges_Click" />
            </div>
        </div>

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
                            <asp:Button ID="btnDelete" runat="server" CssClass="delete-button" Text="Del" 
                                CommandName="DeleteVendor" CommandArgument='<%# Eval("vendorID") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <!-- Message Label -->
        <div class="message-container">
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false"></asp:Label>
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
                <asp:FileUpload ID="fuVendorImage" runat="server" CssClass="input-field" accept=".png" onchange="validateFields()" />
            </div>

            <span id="errorMessage" style="color:red; display:none;">Please fill in all fields before confirming.</span>

            <div class="button-group">
                <asp:Button ID="btnConfirmAdd" runat="server" CssClass="confirm-button" Text="Confirm" Enabled="false" OnClick="btnConfirmAdd_Click" />
                <asp:Button ID="btnCancelAdd" runat="server" CssClass="cancel-button" Text="Cancel" OnClick="btnCancelAdd_Click" />
            </div>
        </asp:Panel>

        <!-- SUCCESS MESSAGE POPUP -->
        <asp:Panel ID="pnlSuccess" runat="server" CssClass="popup" Visible="false">
            <h3>Vendor successfully added</h3>
            <asp:Button ID="btnCloseSuccess" runat="server" CssClass="close-button" Text="Close" OnClick="btnCloseSuccess_Click" />
        </asp:Panel>

        <!-- Add Button -->
        <div class="vendor-actions">
            <asp:Button ID="btnShowAddPopup" runat="server" CssClass="add-button-cust" Text="Add Vendor" OnClick="ShowAddPopup" />
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

        var confirmButton = document.getElementById('<%= btnConfirmAdd.ClientID %>');
            var errorMsg = document.getElementById('errorMessage');

            if (name && price && province && city && category && image) {
                confirmButton.disabled = false;
                errorMsg.style.display = "none";
            } else {
                confirmButton.disabled = true;
                errorMsg.style.display = "block";
            }
        }
    </script>
</asp:Content>