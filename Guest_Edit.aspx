<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Guest_Edit.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Guest_Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Editing Guest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <div class="guest-wrapper">
        <h2 class="guest-title">Guest List </h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Editing guests</h3>


                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Guest ID"></asp:Label>
                        </td>

                        <td class="delte-input-guest">
                            <input id="Text1" type="text" runat="server" placeholder="Please enter your guest's ID here that you want to edit..."/>
                        </td>

                        <td class="btnRemove-guest">
                            <asp:Button ID="btnEditGuest" runat="server" Text="Edit Guest" CssClass="action-btns" OnClientClick="return confirm('Are you sure you want to edit this guest?');" OnClick="btnEditGuest_Click"/>
                        </td>
                                             
                    </tr>

                    <%-- Editiing guest --%>


                    <tr>

    <%-- First row --%>
    <td>
        <asp:Label runat="server" Text="First Name:"></asp:Label>
    </td>

    <td class="right-guest-td">
        <input id="Text2" type="text" runat="server" autofocus="autofocus" placeholder="Please edit your guest's first name here..."/>
    </td>

    <td class="add_guest_left_padding_second">
        <asp:Label runat="server" Text="Last Name:"></asp:Label>
    </td>

    <td class="right-guest-td">
        <input id="Text3" type="text" runat="server" placeholder="Please edit your guest's last name here..."/>
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

    <td class="add_guest_left_padding_second">
        <asp:Label ID="Label2" runat="server" Text="RSVP Selection"></asp:Label>
    </td>

    <td class="right-guest-td">        
        <asp:DropDownList ID="ddlRS" runat="server"></asp:DropDownList>

    </td>
</tr>


                <%-- Editiing guest --%>
                </table>

                <div class="filters">
                    <div>



                       <label>Sort By</label><br />
                        <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="styled-dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label>Filter By</label><br />
                        <asp:DropDownList ID="ddlFilterBy" runat="server" CssClass="styled-dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlFilterBy_SelectedIndexChanged">
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
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" OnClick="btnDelete_Click"/>
                </div>
            </div>
        </div>
    </div>

        <%-- Help pop-up massage --%>
<div id="helpPopup" class="popupOverlayToDo">
          <div class="popup-content">
              <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
    <p>
      Want to Mahito someone? Great!<br />
        <br />Below is a graph just like in the view tab BUT this graph show's the Guest ID which is quite important.<br />
        <br />It also has the same sort/filter drop-down lists as the view tab which work the exact same.<br />
        <br />So have you found that pesky guest yet? Great! Look at their Guest ID and type it in the input box asking for it (Make sure to enter it just as a number).<br />
        <br />Edit your desired fields with new values and leave those empty that you don't want to change.<br />
        <br />Then press that JUICY edit button and boom guest changed.<br />
    </p>
    <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
  </div>

      </div>

    <%-- Delete confirmation --%>
    <div id="DeletedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Guest edited successfully!</p>
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

        <%-- Edit Error No Match value --%>
   <div id="EditedNullErrorGuest" class="popupOverlayToDo" runat="server" ClientIDMode="Static">
  <div class="popup-content">
    <p>Please change atleast one thing in order to edit.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

</asp:Content>
