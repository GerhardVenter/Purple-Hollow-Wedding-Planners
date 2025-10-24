<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary_Update.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary_Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Editing Itinerary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

                <div class="guest-wrapper">
        <h2 class="guest-title">Itinerary List</h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Editing Itinerary</h3>


                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Itinerary ID"></asp:Label>
                        </td>

                        <td class="delte-input-guest">
                            <input id="Text1" type="text" runat="server" placeholder="Please enter your itinerary's ID here that you want to edit..."/>
                        </td>

                        <td class="btnRemove-guest">
                            <asp:Button ID="btnEditGuest" runat="server" Text="Edit Itinerary" CssClass="action-btns" OnClientClick="return confirm('Are you sure you want to edit this itinerary item?');" OnClick="btnEditGuest_Click"/>
                        </td>
                                             
                    </tr>

                    <%-- Editiing guest --%>


                    <tr>

    <%-- First row --%>
    <td>
        <asp:Label runat="server" Text="Itinerary name:"></asp:Label>
    </td>

    <td class="right-guest-td">
        <input id="Text2" type="text" runat="server" autofocus="autofocus" placeholder="Please enter your itinerary item's name here..."/>
    </td>

    <td class="add_guest_left_padding_second">
        <asp:Label runat="server" Text="Itinerary Description:"></asp:Label>
    </td>

    <td class="right-guest-td">
        <input id="Text3" type="text" runat="server" placeholder="Please edit your itinerary's description here..."/>
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


                <%-- Editiing guest --%>
                </table>

                <div class="filters">
                    <div>

                       <label>Sort By</label><br />
                        <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="styled-dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged">
                        </asp:DropDownList>
                    
                    </div>
                </div>

                <%-- Guest grid --%>
                <asp:GridView ID="gvGuests" runat="server" AutoGenerateColumns="True" CssClass="guest-grid" GridLines="None">
                </asp:GridView>

                <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="help-btn" OnClientClick="showHelpPopupToDo(); return false;"/>

                <div class="button-row">                   
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click"/>
                    <asp:Button ID="btnTimeLine" runat="server" Text="Time Line" CssClass="action-btn" OnClick="btnTimeLine_Click"/>
                    <asp:Button ID="btnShare" runat="server" Text="Share" CssClass="action-btn" OnClick="btnShare_Click" />
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
      Want to Mahito a list item? Great!<br />
        <br />Below is a graph just like in the view tab BUT this graph show's the itinerary item ID which is quite important.<br />
        <br />It also has the same sort/filter drop-down lists as the view tab which work the exact same.<br />
        <br />So have you found that pesky itinerary item yet? Great! Look at its corresponding ID and type it in the input box asking for it (Make sure to enter it just as a number).<br />
        <br />Edit your desired fields with new values and leave those empty that you don't want to change.<br />
        <br />Then press that JUICY edit button and boom guest changed.<br />
    </p>
    <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
  </div>

      </div>

    <%-- Delete confirmation --%>
    <div id="DeletedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Itinerary item edited successfully!</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <%-- Delete Error Null value --%>
    <div id="DeletedErrorNullGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Please make sure you ENTER a NUMBER in the inputbox/s.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <%-- Delete Error No Match value --%>
    <div id="DeletedErrorNoMatchGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>This itinerary number does not exist on your account.</p>
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

    <div id="ItiTooLongPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Please make your description shorter.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>
   
 <div id="AddedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Start time has to be smaller than End time.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <div id="ItiNotNumPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>This itinerary name already exists.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

</asp:Content>
