<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guests.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Guest List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <asp:GridView ID="gvGuests" runat="server"></asp:GridView>
</asp:Content>
