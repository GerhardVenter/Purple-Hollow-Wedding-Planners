<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary_Add.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary_Add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Adding Itinerary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <div class="guest-wrapper">
      <h2 class="guest-title">Itinerary List</h2>

      <div class="guest-section">
          <div class="guest-container">
              <h3 class="guest-subtitle">Adding itinerary</h3>

              <table>
    <tr>

        <%-- First row --%>
        <td>
            <asp:Label runat="server" Text="Itinerary item:"></asp:Label>
        </td>

        <td class="right-guest-td">
            <input id="inpNam" type="text" runat="server" autofocus="autofocus" placeholder="Please enter your itinerary item's name here..."/>
        </td>

        <td class="add_guest_left_padding_second">
            <asp:Label runat="server" Text="Short description:"></asp:Label>
        </td>

        <td class="right-guest-td">
            <input id="inpDesc" type="text" runat="server" placeholder="Please enter a short description of your itinerary item here..."/>
        </td>


    </tr>
    <%-- Second row --%>

    <tr>

        <td>
            <asp:Label ID="Label1" runat="server" Text="StartTime"></asp:Label>
        </td>

        <td class="right-guest-td">
            <input id="inpST" type="number" runat="server" autofocus="autofocus" placeholder="Please enter your itinerary item's StartTime here..." min="0" max="2359" step="1"/>
        </td>

        <td class="add_guest_left_padding_second">
    <asp:Label ID="Label2" runat="server" Text="EndTime"></asp:Label>
</td>

        <td class="right-guest-td">
            
            <input id="inpET" type="number" runat="server" autofocus="autofocus" placeholder="Please enter your itinerary items's EndTime here..." min="0" max="2359" step="1"/>

        </td>
    </tr>



</table>

              <div class="filters">
                  <div>



                     <label>Sort By</label><br />
                      <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="styled-dropdown" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged" AutoPostBack="True">
                      </asp:DropDownList>
                  </div>
              </div>

               

              <%-- Guest grid --%>
              <asp:GridView ID="gvGuests" runat="server" AutoGenerateColumns="True" CssClass="guest-grid" GridLines="None">
              </asp:GridView>

              <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="help-btn" OnClientClick="showHelpPopupToDo(); return false;"/>

                <asp:Button ID="Button1" runat="server" Text="Confirm" CssClass="confirm-btn" OnClick="btnConfirm_Click" OnClientClick="return confirm('Are you sure you want to add a new guest?');" />


              <div class="button-row">
                  <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click1" />
                  <asp:Button ID="btnTimeLine" runat="server" Text="Time Line" CssClass="action-btn" OnClick="btnTimeLine_Click"/>
                  <asp:Button ID="btnShare" runat="server" Text="Share" CssClass="action-btn" OnClick="btnShare_Click" />
                  <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" OnClick="btnEdit_Click" />
                  <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" OnClick="btnDelete_Click" />
              </div>
          </div>
      </div>
  

          <%-- Help pop-up massage --%>
<div id="helpPopup" class="popupOverlayToDo">
          <div class="popup-content">
              <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
    <p>
      Woah! Careful there, almost thought you were trying to challenge my domain! You just need help hey? Sounds good kid.<br />
        <br />Enter your itinerary items name and description in the inputs asking for it.<br />
        <br />Choose a startime and endtime for your item, your endtime has to be shorter than your startime.<br />
        <br />Please ensure that you enter your times in integer values between (0000 - 2399)<br />
        <br />Click the confirm button when you are ready and if you are unsure then you can always click cancel.<br />
        <br />Everything execept the description is compulsory, just like university attendance!<br />
        <br />The description may not exceed 128 characters in length(It is a short description after all you sorcerer).<br />
        <br />Remember you can always edit your itinerary item by clicking on the edit button below if you want to make any changes to them like Mahito did to that one kid.
    </p>
    <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
  </div>

      </div>

          <%-- Add error --%>

    <div id="AddedErrorPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Neither item name nor starttime nor endtime may be empty!</p>
      <p>Please also ensure you enter the values as a numerical value between 0000 - 2399</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

          <%-- Add confirmation --%>
                        <div id="AddedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Itinerary item added successfully!</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

                          <%-- Add eroor not num --%>

<div id="ItiNotNumPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Please ensure you enter your number in the correct format!</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>
    

                              <%-- Short descr too long --%>
    <div id="ItiTooLongPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Please make your description shorter.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

         <div id="EditedNullErrorGuest" class="popupOverlayToDo" runat="server" ClientIDMode="Static">
  <div class="popup-content">
    <p>Please use unique names for your itinerary item.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <div id="DeletedErrorNoMatchGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Endtime cannot be shorter than or equal to starttime.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

</asp:Content>
