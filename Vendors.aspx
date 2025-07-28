<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Vendor Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="vendor-layout">
        <div class="column vendors">
            <div class="vendors-top">
                <h2>Vendors</h2>
            </div>

            <div class="vendors-middle">
                <ul class="vendor-categories">
                    <li><a href="Vendors Pages/Bakers.aspx">Bakers</a></li>
                    <li class="active"><a href="Vendors Pages/Photographers.aspx">Photographers</a></li>
                    <li><a href="Vendors Pages/DJs.aspx">DJs</a></li>
                    <li><a href="Vendors Pages/Florists.aspx">Florists</a></li>
                    <li><a href="Vendors Pages/Catering.aspx">Catering</a></li>
                    <li><a href="Vendors Pages/Venues.aspx">Venues</a></li>
                    <li><a href="Vendors Pages/Videographers.aspx">Videographers</a></li>
                    <li><a href="Vendors Pages/Jewelers.aspx">Jewelers</a></li>
                    <li><a href="Vendors Pages/DanceLessons.aspx">Dance Lessons</a></li>
                    <li><a href="Vendors Pages/DressDesigners.aspx">Dress Designers</a></li>
                </ul>
            </div>

            <div class="vendors-bottom">
                <button class="customise-button">Customise Vendors</button>
            </div>
        </div>

        <div class="divider-wrapper">
            <img src="Images/Divider.svg" alt="Divider" id="fancyDivider" />
        </div>

        <div class="column photographers">
            <h2>Wedding Photographers</h2>
            <!-- Add Photographer Listings Here -->
        </div>

        <!-- Right plain vertical divider -->
        <div class="plain-divider"></div>

        <div class="column list">
            <h2>List</h2>
            <!-- Add Cart/List Items Here -->
        </div>
    </div>

</asp:Content>
