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

                            <%-- First row --%>
                            <td>
                                <asp:Label runat="server" Text="First Name:"></asp:Label>
                            </td>

                            <td class="right-guest-td">
                                <input id="Text1" type="text" runat="server" autofocus="autofocus" placeholder="Please enter your guest's first name here..."/>
                            </td>

                            <td class="add_guest_left_padding_second">
                                <asp:Label runat="server" Text="Last Name:"></asp:Label>
                            </td>

                            <td class="right-guest-td">
                                <input id="Text2" type="text" runat="server" placeholder="Please enter your guest's last name here..."/>
                            </td>


                        </tr>
                        <%-- Second row --%>

                        <tr>

                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Dietary Selection"></asp:Label>
                            </td>

                            <td class="ddlDS-td">
                                <asp:DropDownList ID="ddlDS" runat="server"></asp:DropDownList>
                            </td>

                            <td >
                                <asp:Label ID="Label2" runat="server" Text="RSVP Selection"></asp:Label>
                                <asp:DropDownList ID="ddlRS" runat="server"></asp:DropDownList>

                            </td>
                            
                            <td class="right-guest-td-email">                
                                <asp:Label ID="Label3" runat="server" Text="Email:"></asp:Label>
                                <input id="Text3" type="text" runat="server" placeholder="Please enter your guest's email here..."/>
                                
                            </td>
                        </tr>



                    </table>
                </div>

                <asp:Button ID="btnHelp" runat="server" Text="Need help?" CssClass="help-btn" />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="confirm-btn" OnClick="btnConfirm_Click" />

                <div class="button-row">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn"  />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>
