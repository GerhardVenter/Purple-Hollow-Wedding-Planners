<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guest_Add.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guest_Add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Adding Guest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="guest-wrapper">
        <h2 class="guest-title">Guest List <img src="Images/guests.png" alt="Bride and bridesmaids" /></h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Adding guests</h3>

                <div class="guest-update-add-tbl">
                    <table>
                        <tr>
                            <td class="add_guest_left_padding_first">
                                <asp:Label runat="server" Text="First Name"></asp:Label>
                            </td>

                            <td class="right-guest-td">
                                <input id="Text1" type="text" autofocus="autofocus" placeholder="Please enter your guest's first name here"/>
                            </td>

                            <td class="add_guest_left_padding_second">
                                <asp:Label runat="server" Text="Last Name"></asp:Label>
                            </td>

                            <td class="right-guest-td">
                                <input id="Text2" type="text" placeholder="Please enter your guest's first name here"/>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Button ID="btnHelp" runat="server" Text="Need help?" CssClass="help-btn" />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="confirm-btn" />

                <div class="button-row">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn"  />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>
