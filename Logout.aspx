<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="register-wrapper">
        <!-- Logout Box -->
        <div class="register-box">

            <div id="reg_log_img">
                <img src="Images/Register.jpg" alt="image of bride and groom" />
            </div>

            <h2>Logout?</h2>
            <p>Are you sure you want to leave so soon <%= Session["username"] %>?</p>

            <asp:Button ID="btnLogout" runat="server" Text="Logout :(" CssClass="register-btn" OnClick="btnLogout_Click" />

        </div>
    </div>
</asp:Content>
