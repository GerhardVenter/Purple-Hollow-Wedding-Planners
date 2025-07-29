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

        <!-- Actions -->
        <div class="vendor-actions">
            <button class="add-button">Add</button>
            <button class="edit-button">Edit</button>
            <button class="delete-button">Delete</button>
        </div>
    </div>
</asp:Content>
