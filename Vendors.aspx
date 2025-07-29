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
                <ul class="vendor-categories">
                    <li class="active"><a href="Vendors.aspx?category=Photography">Photographers</a></li>
                    <li><a href="Vendors.aspx?category=Bakers">Bakers</a></li>
                    <li><a href="Vendors.aspx?category=DJs">DJs</a></li>
                    <li><a href="Vendors.aspx?category=Florists">Florists</a></li>
                    <li><a href="Vendors.aspx?category=Catering">Catering</a></li>
                    <li><a href="Vendors.aspx?category=Venues">Venues</a></li>
                    <li><a href="Vendors.aspx?category=Videographers">Videographers</a></li>
                    <li><a href="Vendors.aspx?category=Jewelers">Jewelers</a></li>
                    <li><a href="Vendors.aspx?category=Dance Lessons">Dance Lessons</a></li>
                    <li><a href="Vendors.aspx?category=Dress Designers">Dress Designers</a></li>
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
                <select class="province-dropdown">
                    <option value="">Select a province</option>
                    <option value="Eastern Cape">Eastern Cape</option>
                    <option value="Free State">Free State</option>
                    <option value="Gauteng">Gauteng</option>
                    <option value="KwaZulu-Natal">KwaZulu-Natal</option>
                    <option value="Limpopo">Limpopo</option>
                    <option value="Mpumalanga">Mpumalanga</option>
                    <option value="North West">North West</option>
                    <option value="Northern Cape">Northern Cape</option>
                    <option value="Western Cape">Western Cape</option>
                </select>

                <button class="help-button">Need help?</button>

                <select class="sort-dropdown">
                    <option value="price-asc">Sort on price ↑</option>
                    <option value="price-desc">Sort on price ↓</option>
                </select>
            </div>

            <!-- Vendor Cards -->
            <asp:Repeater ID="rptVendors" runat="server">
                <ItemTemplate>
                    <div class="vendor-card">
                        <div class="frame">
                            <img src='<%# Eval("imagePath") %>' alt="Vendor Image" class="vendor-image" />
                            <%--<img src="Images/picture-frame.svg" class="frame-overlay" alt="Frame" />--%>
                        </div>

                        <div class="vendor-info">
                            <p class="vendor-name"><%# Eval("vendorName") %></p>
                            <p class="vendor-location"><%# Eval("vendorCity") %>, <%# Eval("vendorProvince") %></p>
                            <p class="vendor-price">R<%# Eval("vendorPrice") %></p>
                        </div>

                        <button class="add-button" title="Add to list">
                            <img src="Images/cart-icon.svg" alt="Cart Icon" />
                        </button>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
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