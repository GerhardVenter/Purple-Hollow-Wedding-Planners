<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guest_Delete.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guest_Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Deleting Guest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="guest-wrapper">
        <h2 class="guest-title">Guest List</h2>

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
                            <asp:Button ID="btnRemoveGUest" runat="server" Text="Remove Guest" CssClass="action-btn" OnClick="btnRemoveGUest_Click" OnClientClick="return confirm('Are you sure you want to delete this guest?');"/>
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

                <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="help-btn" OnClientClick="showHelpPopupToDo(); return false;"/>

                <div class="button-row">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click"/>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" OnClick="btnEdit_Click" />
                </div>
            </div>
        </div>
    </div>

        <%-- Help pop-up massage --%>
<div id="helpPopup" class="popupOverlayToDo">
          <div class="popup-content">
              <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
    <p>
      So you want to infinite void technique one of your guests? Sounds good!<br />
        <br />Below is a graph just like in the view tab BUT this graph show's the Guest ID which is quite important.<br />
        <br />It also has the same sort/filter drop-down lists as the view tab which work the exact same.<br />
        <br />So have you found that pesky guest yet? Great! Look at their Guest ID and type it in the input box asking for it (Make sure to enter it just as a number).<br />
        <br />Then press that temping Remove guest button. And BAM they are now gone! (With a confirmation message ofcourse)<br />
        <br />Gojo's warning!! If you delete a guest it is permanet, NO takebacks!<br />
    </p>
    <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
  </div>

      </div>

    <%-- Delete confirmation --%>
    <div id="DeletedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Guest deleted successfully!</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <%-- Delete Error Null value --%>
    <div id="DeletedErrorNullGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Please make sure you ENTER a NUMBER in the inputbox.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <%-- Delete Error No Match value --%>
    <div id="DeletedErrorNoMatchGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>This guest does not exist on your account.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

</asp:Content>
