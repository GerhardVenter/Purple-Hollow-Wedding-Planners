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
                 <input type="text" class="messageAdder" placeholder="Add your task" >
                 <button type="submit" class="addMessageButton">Add Task</button>
                 </div>
         </form>
       <table class="taskTable">
           <tr>
               <td>
                   <div class="checker">
                       <span><input type="checkbox" class="checkbox"/></span>My First task
                   </div>
                   <div class="actionButtons">
                       <button class="editBtn">Edit</button>
                       <button class="deleteBtn">Delete</button>
                   </div>
               </td>
           

           </tr>
       </table>
       </div>
 
      
    </section>
    <script src="Script/script.js"></script>
   
</asp:Content>
