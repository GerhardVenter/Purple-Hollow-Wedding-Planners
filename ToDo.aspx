<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ToDo.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.ToDo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">To-Do List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
<h2 id="toDoHeading">To-Do List <img src="Images/clipboard.png" alt="picture of clipboard" id="clipboard" /></h2>

<section class="todo-container">
   <div class="myList">
     <form action="">

             <div class="actions">
                 <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="messageAdder" Placeholder="Add your task"></asp:TextBox>
                 <asp:Button ID="btnAddTask" runat="server" CssClass="addMessageButton" Text="Add Task" OnClick="btnAddTask_Click" />
                 <asp:Label ID="lblMsg" runat="server"></asp:Label>
                 </div>
         </form>
       <asp:Table ID="taskTable" runat="server" CssClass="taskTable">
     
       </asp:Table>

     <asp:Button ID="btnHelpToDo" runat="server" Text="Need help?" CssClass="helpButton" OnClientClick="showHelpPopup(); return false;" />

       <div id="helpPopup" class="popupOverlayToDo">
          <div class="popup-content">
              <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
    <p>
      To add a new task, enter it into the task description bar.<br/> Once complete click the Add Task button and your new task will appear for you to see.<br/><br/> To delete a task, click on the Delete button and that task will disappear.<br/><br/>  To edit a task , click on the Edit button and make your changes. Do not forget to press the Save button to save your changes or press the Cancel button if you want to exit Edit mode.<br/><br/>  Happy working!
    </p>
    <button onclick="closeHelpPopup()" class="close-btn">Close</button>
  </div>
</div>

       <div id="taskSuccessPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Task added successfully!</p>
    <button onclick="closeTaskPopup()" class="close-btn">Close</button>
  </div>
</div>

       <div id="taskUpdatedPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Task updated successfully!</p>
    <button onclick="closeUpdatedPopup()" class="close-btn">Close</button>
  </div>
</div>


<!-- Custom Delete Confirmation Popup -->
<div id="deleteConfirmPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Are you sure you want to delete this task?</p>
    <button onclick="triggerServerDelete()" class="confirm-btn">Yes</button>
    <button onclick="closeDeletePopup()" class="close-btn">No</button>
  </div>
</div>


<!-- Task Deleted Popup -->
<div id="deleteSuccessPopup" class="popupOverlayToDo">
  <div class="popup-content">
    <p>Task deleted successfully!</p>
    <button onclick="closeDeleteSuccess()" class="close-btn">Cancel</button>
  </div>
</div>

<!-- Hidden Button to trigger server-side delete -->
<asp:Button ID="hiddenDeleteBtn" runat="server" OnClick="DeleteTask_Click" Style="display:none;" />

 
      
    </section>
       


</asp:Content>
