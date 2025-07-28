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
                    <li>Bakers</li>
                    <li class="active">Photographers</li>
                    <li>DJs</li>
                    <li>Florists</li>
                    <li>Catering</li>
                    <li>Venues</li>
                    <li>Videographers</li>
                    <li>Jewelers</li>
                    <li>Dance Lessons</li>
                    <li>Dress Designers</li>
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
