<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guest_Delete.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guest_Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Deleting Guest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="guest-wrapper">
        <h2 class="guest-title">Guest List <img src="Images/guests.png" alt="Bride and bridesmaids" /></h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Deleting guests</h3>


                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Guest ID"></asp:Label>
                        </td>

                        <td class="delte-input-guest">
                            <input id="Text1" type="text" runat="server" placeholder="Please enter your guest's ID here that you want to delete..."/>
                        </td>

                        <td class="btnRemove-guest">
                            <asp:Button ID="btnRemoveGUest" runat="server" Text="Remove Guest" CssClass="action-btn" OnClick="btnRemoveGUest_Click"/>
                        </td>

                    </tr>
                </table>

                <div class="filters">
                    <div>



                       <label>Sort By</label><br />
                        <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="styled-dropdown" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label>Filter By</label><br />
                        <asp:DropDownList ID="ddlFilterBy" runat="server" CssClass="styled-dropdown" OnSelectedIndexChanged="ddlFilterBy_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>

                <%-- Guest grid --%>
                <asp:GridView ID="gvGuests" runat="server" AutoGenerateColumns="True" CssClass="guest-grid" GridLines="None">
                </asp:GridView>

                <asp:Button ID="btnHelp" runat="server" Text="Need help?" CssClass="help-btn" />

                <div class="button-row">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click"/>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
