<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary_Share.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary_Share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Itinerary Share
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

             <div class="guest-wrapper">
        <h2 class="guest-title">Itinerary List</h2>

        <div class="guest-section">
            <div class="guest-container">
                <h3 class="guest-subtitle">Sharing Itinerary</h3>

                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Recipients email"></asp:Label>
                        </td>

                        <td class="delte-input-guest">
                            <input id="Text1" type="text" runat="server" placeholder="Please enter your recipients email here..."/>
                        </td>

                        <td class="btnRemove-guest">
                            <asp:Button ID="btnRemoveGUest" runat="server" Text="Share" CssClass="action-btn" OnClick="btnRemoveGUest_Click" OnClientClick="return confirm('Are you sure you want to send to this email?');"/>
                        </td>
                      
                    </tr>
                </table>

                <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="help-btn" OnClientClick="showHelpPopupToDo(); return false;"/>

                <div class="button-row">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="action-btn" OnClick="btnView_Click"/>
                    <asp:Button ID="btnTimeLine" runat="server" Text="Time Line" CssClass="action-btn" OnClick="btnTimeLine_Click"/>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="action-btn" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="action-btn" OnClick="btnEdit_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="action-btn" OnClick="btnDelete_Click" />
                </div>
            </div>
        </div>
    </div>

        <%-- Help pop-up massage --%>
<div id="helpPopup" class="popupOverlayToDo">
          <div class="popup-content">
              <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
    <p>
     *Sniffs variously*<br />
        <br />Ah dattebayo you want to send your itinerary list to someone in an email form. Sounds good!<br />
        <br />Enter the recipient's email in the input box and then hit that SHARE BUTTON!!!!!!!<br />
        <br />Please ensure the email is correct and that your internet is not slow like the algorithm that was used to send the list:).<br />
        <br />Happy sharing!<br />
    </p>
    <button onclick="closeHelpPopupToDo()" class="close-btn">Close</button>
  </div>

      </div>

    <%-- Delete confirmation --%>
    <div id="DeletedSuccessPopupGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Sucessfully sent an email address to the email your email recipient. (Unless the email you sent it to does not exist).</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

    <%-- Delete Error No Match value --%>
    <div id="DeletedErrorNoMatchGuest" class="popupOverlayToDo">
  <div class="popup-content">
    <p>It seems there was a problem sending your email. Please ensure you entered the correct email or check yor network connection.</p>
    <button onclick="closeDeleteSuccessGuest()" class="close-btn">Close</button>
  </div>
</div>

</asp:Content>
