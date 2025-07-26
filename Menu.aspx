<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Menu
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Label ID="lblAccessMessage" runat="server" CssClass="menuValidation" />
    <section id="menuContainer">
        
        <article id="addMenuContainer">
    <table id="menuTable">
        <tr class="menuRows">
            <td class="menuLabel">
                <asp:Label ID="lblDishName" runat="server" Text="Enter the name of the dish:"></asp:Label>
            </td>
            <td class="menuInput">
                <asp:TextBox ID="txtDishName" runat="server" CssClass="menu-input" PlaceHolder="Dish name goes here, e.g. Phyllo Parcels">
                </asp:TextBox><asp:RequiredFieldValidator ID="rfvDishName" runat="server" ControlToValidate="txtDishName"
    ErrorMessage="* Dish name is required" CssClass="menuValidation" ValidationGroup="AddMenu" Display="Dynamic" />
            </td>
        </tr>

        <tr class="menuRows">
            <td class="menuLabel">
                <asp:Label ID="lblmenuCategory" runat="server" Text="Choose your dish's category:"></asp:Label>
            </td>
            <td class="menuInput">
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="menu-input">
                    <asp:ListItem Text="-- Please select a category --" Value="" Selected="True"/>
                    <asp:ListItem Text="Starter" Value="Starter" />
                    <asp:ListItem Text="Main" Value="Main" />
                    <asp:ListItem Text="Dessert" Value="Dessert" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory"
    InitialValue="" ErrorMessage="* Please select a category" CssClass="menuValidation" ValidationGroup="AddMenu"  Display="Dynamic" />
            </td>
        </tr>

        <tr class="menuRows">
            <td class="menuLabel">
                <asp:Label ID="lbldishDescription" runat="server" Text="Describe your dish in more detail:" ></asp:Label>
            </td>
            <td class="menuInput">
                <asp:TextBox ID="txtdishDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="menu-input" PlaceHolder="Describe your dish here e.g. A phyllo pastry parcel filled with a peppers and chicken drizzled with a balsamic vinegar glaze." ></asp:TextBox>
           <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ValidationGroup="AddMenu"  ControlToValidate="txtdishDescription"
    ErrorMessage="* Description is required" CssClass="menuValidation" Display="Dynamic" />

            </td>
        </tr>

         <tr class="menuRows">
           <td class="menuLabel">
            
          </td>
     <td class="menuInput">
         <asp:Button ID="btnAddMenuItem" runat="server" Text="Add item" CssClass="addMenuBtn" OnClick="btnAddMenuItem_Click"></asp:Button>
      <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="helpButton" OnClientClick="showHelpPopup(); return false;"/>

     </td>
 </tr>
    </table>
                   <asp:GridView ID="gvMenuItems" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#2C0F3D" runat="server" AutoGenerateColumns="False" CssClass="menuGrid"
    OnRowCommand="gvMenuItems_RowCommand"
    OnRowEditing="gvMenuItems_RowEditing"
    OnRowUpdating="gvMenuItems_RowUpdating"
     DataKeyNames="menuID"
    OnRowCancelingEdit="gvMenuItems_RowCancelingEdit">
    <Columns>
        <asp:TemplateField HeaderText="Dish Name" SortExpression="menuDishName">
    <ItemTemplate>
        <%# Eval("menuDishName") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtEditDishName" runat="server" Text='<%# Bind("menuDishName") %>' BackColor="White"/>
    </EditItemTemplate>
</asp:TemplateField>
       <asp:TemplateField HeaderText="Category" SortExpression="menuCategory">
    <ItemTemplate>
        <%# Eval("menuCategory") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlEditCategory" runat="server"
         SelectedValue='<%# Bind("menuCategory") %>'>
            <asp:ListItem Text="Starter" Value="Starter"></asp:ListItem>
            <asp:ListItem Text="Main" Value="Main"></asp:ListItem>
            <asp:ListItem Text="Dessert" Value="Dessert"></asp:ListItem>
        </asp:DropDownList>
    </EditItemTemplate>
</asp:TemplateField>

        <asp:BoundField DataField="menuDescription" HeaderText="Description" SortExpression="menuDescription" ControlStyle-BackColor="White" />

        <asp:TemplateField>
    <ItemTemplate>
        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="deleteBtn" Text="Edit" />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:Button ID="btnUpdate" runat="server" CommandName="Update" CssClass="deleteBtn" Text="Save" />
        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CssClass="deleteBtn" Text="Cancel" />
    </EditItemTemplate>

</asp:TemplateField>
      <asp:TemplateField> 
                         <ItemTemplate>
       <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteItem"
           CommandArgument='<%# Eval("menuID") %>' CssClass="deleteBtn"
           OnClientClick="return confirm('Delete this item?');" />
   </ItemTemplate>
          </asp:TemplateField>
    </Columns>
</asp:GridView>

            
</article>
       
        <article id="menuViewContainer">
      <img src="Images/gojoEatting.png" alt="picture of gojo eating" id="gojoEating"/>
</article>
</section>
    <!-- Help Popup -->
<div id="helpPopupMenu" class="popupOverlayToDo">
  <div class="popup-content">
    <img src="Images/helpGojo.png" alt="Gojo help" class="popup-img" />
    <p>
      To add a menu item, enter a dish name, select a dish category from the dropdown list and then enter a dish description. Once you are finished, click the Add Item button and your item will appear on the right-hand side of the screen. <br/><br/> To edit a menu item, press the edit button and click on the relevant field you would like to edit and make your changes. Press the save button to save your changes or the cancel button if you have changed your mind and wish to exit editing mode.<br/><br/> To delete a menu item, press the Delete button and follow the prompts.<br/><br/>Happy working!
    </p>
    <button onclick="closeHelpPopup()" class="close-btn">Close</button>
  </div>
</div>

    <!-- Dish Added Popup -->
<div id="dishAddedPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Dish added successfully!</p>
    <button onclick="closeDishAdded()" class="close-btn">Close</button>
  </div>
</div>

    <!-- Dish Updated Popup -->
<div id="dishUpdatedPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Dish updated successfully!</p>
    <button onclick="closeDishUpdated()" class="close-btn">Close</button>
  </div>
</div>
    <!-- Dish Deleted Popup -->
<div id="dishDeletedPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Dish deleted successfully!</p>
    <button onclick="closeDishDeleted()" class="close-btn">Close</button>
  </div>
</div>


</asp:Content>

















