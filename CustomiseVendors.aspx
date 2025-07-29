<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CustomiseVendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.CustomiseVendors" %>

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
            <div>
                <button class="help-button">Need help?</button>
            </div>
            <div>
                <button class="exit-button" onclick="window.location.href='Vendors.aspx'">Exit</button>
                <button type="button" class="save-button">Save</button>
            </div>
        </div>

        <!-- Table Wrapper -->
        <div class="customise-table-container">
            <div class="divider left-divider">
                <img src="Images/Divider.svg" alt="Divider" />
            </div>

            <table class="customise-table">
                <thead>
                    <tr>
                        <th scope="col">Name</th>
                        <th scope="col">Price</th>
                        <th scope="col">Province</th>
                        <th scope="col">City</th>
                        <th scope="col">Category</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptVendors" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("vendorName") %></td>
                                <td>R<%# Eval("vendorPrice") %></td>
                                <td><%# Eval("vendorProvince") %></td>
                                <td><%# Eval("vendorCity") %></td>
                                <td><%# Eval("category") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

            <div class="divider right-divider">
                <img src="Images/Divider.svg" alt="Divider" />
            </div>
        </div>

        <!-- Add Vendor Modal -->
        <asp:Panel ID="pnlAddVendor" CssClass="modal" runat="server" Visible="false">
            <div class="modal-content">
                <h3>Add Vendor</h3>
                <asp:TextBox ID="txtName" runat="server" Placeholder="Name"></asp:TextBox><br />
                <asp:TextBox ID="txtPrice" runat="server" Placeholder="Price"></asp:TextBox><br />
                <asp:DropDownList ID="ddlProvince" runat="server">
                    <asp:ListItem Value="">Select Province</asp:ListItem>
                    <asp:ListItem>Gauteng</asp:ListItem>
                    <asp:ListItem>Western Cape</asp:ListItem>
                    <asp:ListItem>KwaZulu-Natal</asp:ListItem>

                </asp:DropDownList><br />
                <asp:TextBox ID="txtCity" runat="server" Placeholder="City"></asp:TextBox><br />
                <asp:DropDownList ID="ddlCategory" runat="server">
                    <asp:ListItem Value="">Select Category</asp:ListItem>
                    <asp:ListItem>Photography</asp:ListItem>
                    <asp:ListItem>Bakery</asp:ListItem>
                    <asp:ListItem>Venue</asp:ListItem>
                    <asp:ListItem>Catering</asp:ListItem>
                    <asp:ListItem>Florist</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Button ID="btnConfirmAdd" runat="server" Text="Confirm" OnClick="btnConfirmAdd_Click" CssClass="confirm-btn" />
                <asp:Button ID="btnCancelAdd" runat="server" Text="Cancel" OnClick="btnCancelAdd_Click" CssClass="cancel-btn" />
            </div>
        </asp:Panel>

        <!-- Success Message Modal -->
        <asp:Panel ID="pnlSuccess" CssClass="modal" runat="server" Visible="false">
            <div class="modal-content">
                <h3>Vendor Successfully Added!</h3>
                <asp:Button ID="btnCloseSuccess" runat="server" Text="Close" OnClick="btnCloseSuccess_Click" />
            </div>
        </asp:Panel>

        <!-- Actions -->
        <div class="vendor-actions">
            <asp:Button ID="btnShowAdd" runat="server" Text="Add" OnClick="btnShowAdd_Click" CssClass="add-button" />
            <button class="edit-button">Edit</button>
            <button class="delete-button">Delete</button>
        </div>
    </div>
</asp:Content>
