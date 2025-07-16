<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Register" %>
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
                Register
            </h2>

            <p>Create your account</p>
            
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" placeholder="Username" />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="Email Address" />
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="Password" />
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="Confirm Password" />
            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="register-btn" />

        </div>
    </div>
</asp:Content>
