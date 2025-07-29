<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Vendor Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Vendor Page Container -->
    <div class="vendor-layout">
        <!-- LEFT: Column Vendors -->
        <div class="column vendors">
            <div class="vendors-top">
                <h2>Vendors</h2>
            </div>

            <div class="vendors-middle">
                <% string currentCategory = Request.QueryString["category"] ?? "Photography"; %>
                <ul class="vendor-categories">
                    <li class='<% = (currentCategory == "Photography") ? "active" : "" %>'><a href="Vendors.aspx?category=Photography">Photographers</a></li>
                    <li class='<% = (currentCategory == "Bakery") ? "active" : "" %>'><a href="Vendors.aspx?category=Bakery">Bakers</a></li>
                    <li class='<% = (currentCategory == "Music") ? "active" : "" %>'><a href="Vendors.aspx?category=Music">DJs</a></li>
                    <li class='<% = (currentCategory == "Flowers") ? "active" : "" %>'><a href="Vendors.aspx?category=Flowers">Florists</a></li>
                    <li class='<% = (currentCategory == "Catering") ? "active" : "" %>'><a href="Vendors.aspx?category=Catering">Catering</a></li>
                    <li class='<% = (currentCategory == "Venue") ? "active" : "" %>'><a href="Vendors.aspx?category=Venue">Venues</a></li>
                    <li class='<% = (currentCategory == "Videography") ? "active" : "" %>'><a href="Vendors.aspx?category=Videography">Videographers</a></li>
                    <li class='<% = (currentCategory == "Jewelry") ? "active" : "" %>'><a href="Vendors.aspx?category=Jewelry">Jewelers</a></li>
                    <li class='<% = (currentCategory == "Dance Lessons") ? "active" : "" %>'><a href="Vendors.aspx?category=Dance Lessons">Dance Lessons</a></li>
                    <li class='<% = (currentCategory == "Dress Designers") ? "active" : "" %>'><a href="Vendors.aspx?category=Dress Designers">Dress Designers</a></li>
                </ul>
            </div>

            <div class="vendors-bottom">
                <button class="customise-button">Customise Vendors</button>
            </div>
        </div>

        <!-- Fancy Divider -->
        <div class="divider-wrapper">
            <img src="Images/Divider.svg" alt="Divider" id="fancyDivider" />
        </div>

        <!-- MIDDLE: Column Photographers -->
        <div class="column photographers">
            <h2>Wedding Photographers</h2>
            
            <!-- Toolbar -->
            <div class="vendor-toolbar">
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="province-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                    <asp:ListItem Value="">Select a province</asp:ListItem>
                    <asp:ListItem>Eastern Cape</asp:ListItem>
                    <asp:ListItem>Free State</asp:ListItem>
                    <asp:ListItem>Gauteng</asp:ListItem>
                    <asp:ListItem>KwaZulu-Natal</asp:ListItem>
                    <asp:ListItem>Limpopo</asp:ListItem>
                    <asp:ListItem>Mpumalanga</asp:ListItem>
                    <asp:ListItem>North West</asp:ListItem>
                    <asp:ListItem>Northern Cape</asp:ListItem>
                    <asp:ListItem>Western Cape</asp:ListItem>
                </asp:DropDownList>

                <button class="help-button">Need help?</button>

                <asp:DropDownList ID="ddlSortPrice" runat="server" CssClass="sort-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSortPrice_SelectedIndexChanged">
                    <asp:ListItem Value="price-asc">Price: Low to High ↑</asp:ListItem>
                    <asp:ListItem Value="price-desc">Price: High to Low ↓</asp:ListItem>
                </asp:DropDownList>
            </div>


            <!-- Vendor Cards -->
            <div class="vendor-cards-container">
                <asp:Repeater ID="rptVendors" runat="server">
                    <ItemTemplate>
                        <div class="vendor-card">
                            <div class="frame">
                                <img src='<%# Eval("imagePath") %>' alt="Vendor Image" class="vendor-image" />
                            </div>

                            <!-- Horizontal layout: text + button -->
                            <div class="vendor-details">
                                <div class="vendor-info">
                                    <p class="vendor-name"><%# Eval("vendorName") %></p>
                                    <p class="vendor-location"><%# Eval("vendorCity") %>, <%# Eval("vendorProvince") %></p>
                                    <p class="vendor-price">R<%# Eval("vendorPrice") %></p>
                                </div>
                                <button class="add-button">
                                    <img src="Images/cart-icon.svg" alt="Cart Icon" />
                                </button>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>

        <!-- Plain Divider -->
        <div class="plain-divider"></div>

        <!-- RIGHT: Column List -->
        <div class="column list">
            <h2>List</h2>
            <!-- Add Cart/List Items Here -->
        </div>
    </div>
</asp:Content>