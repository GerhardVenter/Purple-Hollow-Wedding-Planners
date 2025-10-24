<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary_Timeline.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary_Timeline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Itinerary Timeline
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        

            <div class="guest-wrapper">
      <h2 class="guest-title">Itinerary List</h2>

      <div class="guest-section">
          <div class="guest-container">
              <h3 class="guest-subtitle">Timeline of Itinerary</h3>               


          <asp:Repeater ID="rptTimeline" runat="server">
    <ItemTemplate>
        <div class="timeline-item">
            <strong><%# Eval("EventName") %></strong>
            <span><%# Eval("Description") %></span>
        </div>
    </ItemTemplate>
</asp:Repeater>



              <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="help-btn" OnClientClick="showHelpPopupToDo(); return false;"/>

              <div class="button-row">
                  <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click"/>
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
    Ah so you wanted to see a WEIRD FEATURE HEAY?<br />
      <br />Well here is is a timeline of all your itinerary items (Sorted from start time ascending).<br />
      <br />Tehir name (and description to the left) are all displayed vertically<br />
      <br />Yep that's it you, DON'T WORRY IT BUGS ME TOO.<br />
  </p>
  <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
</div>

    </div>

</asp:Content>
