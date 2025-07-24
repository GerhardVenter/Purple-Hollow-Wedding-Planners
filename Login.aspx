<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <!-- Font Awesome for icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="register-wrapper">
        <!-- ✅ Removed fade-in class -->
        <div class="register-box">

            <div id="reg_log_img">
                <img src="Images/Register.jpg" alt="image of bride and groom" />
            </div>

            <h2>Login</h2>
            <p>Sign into your account</p>

            <asp:Label ID="lblMessage" runat="server" CssClass="success-label" Visible="false" />

            <!-- Username with icon -->
            <div class="input-wrapper">
                <i class="fa fa-user icon"></i>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" placeholder="Username" />
            </div>

            <!-- Password with icon and toggle -->
            <div class="input-wrapper">
                <i class="fa fa-lock icon"></i>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input password-input" TextMode="Password" placeholder="Password" />
                <span class="toggle-password"><i class="fa fa-eye"></i></span>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="register-btn" OnClick="btnLogin_Click" />

        </div>
    </div>

    <!-- Toggle password script -->
    <script>
        document.addEventListener('DOMContentLoaded', () => {
            const toggles = document.querySelectorAll('.toggle-password');
            toggles.forEach(toggle => {
                toggle.addEventListener('click', () => {
                    const input = toggle.previousElementSibling;
                    const icon = toggle.querySelector('i');
                    const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
                    input.setAttribute('type', type);
                    icon.classList.toggle('fa-eye');
                    icon.classList.toggle('fa-eye-slash');
                });
            });
        });
    </script>

</asp:Content>