<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Vendor Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="vendor-layout">
        <div class="column vendors">
            <h2>Vendors</h2>
            <!-- Add Vendor Navigation Links Here -->
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
