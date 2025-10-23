<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Itinerary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div class="guest-wrapper">
      <h2 class="guest-title">Itinerary List</h2>

      <div class="guest-section">
          <div class="guest-container">
              <h3 class="guest-subtitle">Viewing Itinerary</h3>

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

              <div class="button-row">
                  <asp:Button ID="btnTimeLine" runat="server" Text="Time Line" CssClass="action-btn" OnClick="btnTimeLine_Click"/>
                  <asp:Button ID="btnShare" runat="server" Text="Share" CssClass="action-btn" OnClick="btnShare_Click" />
                  <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn" OnClick="btnAdd_Click" />
                  <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" OnClick="btnEdit_Click" />
                  <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" OnClick="btnDelete_Click" />
                  
              </div>
          </div>
      </div>
  </div>

  <div id="helpPopup" class="popupOverlayToDo">
        <div class="popup-content">
            <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
  <p>
    Matte, Matte! Need some help hey? Fine, I guess I can go past infinity for you.<br />
      <br />Below you is a graph of all your itinerary items you have added. Don't see any? Then you either haven't added any or you must choose 'none' on your filter.<br />
      <br />You can sort them accordingly by using the drop-down list on the left or filter them by using the one on the right.<br />
      <br />Want to add new items? Click on the add button below, hehehehe.<br />
      <br />Want to update an exisiting item huh? Click on the edit button below like Itadori.<br />
      <br />Want to delete an exisiting item huh? Well if you want to remove them like my infinite void technique then you will have to click on the delete button.<br />
      <br />If you want to go back to viewing your guest's just comeback here by pressing the view button.<br />
      <br />No item's yet? Click on the add button to get started!
  </p>
  <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
</div>

    </div>

</asp:Content>
