<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class ="register-wrapper">
        <div class ="register-box">

            <div ID="reg_log_img">
                <img src="Images\Register.jpg"  alt="image of bride and groom"/>
            </div>
            
            <h2>
                Login
            </h2>

            <p>Sign into your account</p>
            
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" placeholder="Username" />
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="Password" />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="register-btn" />

        </div>
    </div>


</asp:Content>
