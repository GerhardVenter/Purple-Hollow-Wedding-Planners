<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section id="homeContainer">

    <article id="homeWords">
        <h1 class="headings">Welcome to<br/> Purple Hollow<br/> Wedding Planner</h1>
        <br/>
        <p class="regText">"For love is stronger than infinity—Let us help you plan a limitless wedding."</p>
    </article>

    <article id="homeImage">
        <img src="Images/homeImage.png" alt="image of bride and groom"/>
    </article>

    </section>
</asp:Content>
