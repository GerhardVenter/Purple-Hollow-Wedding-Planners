<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guests.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Guest List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="guest-wrapper">
        <h2 class="guest-title">Guest List</h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Viewing guests</h3>

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
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" />
                </div>
            </div>

            <%-- Image --%>
            <div class="guest-image">
                <img src="Images/guests.png" alt="Bride and bridesmaids" />
            </div>
        </div>
    </div>
</asp:Content>
