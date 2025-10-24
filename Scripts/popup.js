function closePopup() {
    document.getElementById("welcomePopup").style.display = "none";
}
//To do list page
function showHelpPopupToDo() {
    document.getElementById("helpPopup").style.display = "flex";
}

function closeHelpPopupToDo() {
    document.getElementById("helpPopup").style.display = "none";
}

function showTaskPopup() {
    document.getElementById("taskSuccessPopup").style.display = "flex";
}

function closeTaskPopup() {
    document.getElementById("taskSuccessPopup").style.display = "none";
}

function showUpdatedPopup() {
    document.getElementById("taskUpdatedPopup").style.display = "flex";
}

function closeUpdatedPopup() {
    document.getElementById("taskUpdatedPopup").style.display = "none";
}
let selectedTaskID = null;



function showDeletePopup(taskID) {
    taskToDeleteID = taskID;
    document.getElementById("deleteConfirmPopup").style.display = "flex";
}

function closeDeletePopup() {
    document.getElementById("deleteConfirmPopup").style.display = "none";
    taskToDeleteID = null;
}

function triggerServerDelete() {
    closeDeletePopup();
    __doPostBack('hiddenDeleteBtn', taskToDeleteID); 
}
function showDeletedPopup() {
    document.getElementById("deleteSuccessPopup").style.display = "flex";
}

function closeDeleteSuccess() {
    document.getElementById("deleteSuccessPopup").style.display = "none";
}
function toggleStrike(checkbox) {
    const taskText = checkbox.nextElementSibling;
    if (checkbox.checked) {
        taskText.classList.add("strike");
    } else {
        taskText.classList.remove("strike");
    }
}


// Menu
function showHelpPopupMenu() {
    document.getElementById("helpPopupMenu").style.display = "flex";
}

function closeHelpPopupMenu() {
    document.getElementById("helpPopupMenu").style.display = "none";
}
function showDishAdded()
{
    document.getElementById("dishAddedPopup").style.display = "flex";
}
function closeDishAdded() {
        document.getElementById("dishAddedPopup").style.display = "none";
}

function showDishUpdated() {
    document.getElementById("dishUpdatedPopup").style.display = "flex";
}

function closeDishUpdated() {
    document.getElementById("dishUpdatedPopup").style.display = "none";
}

function showDishDeleted() {
    document.getElementById("dishDeletedPopup").style.display = "flex";
}

function closeDishDeleted() {
    document.getElementById("dishDeletedPopup").style.display = "none";
}


//Guest list
function showAddedSuccessPopup() {
    document.getElementById("AddedSuccessPopupGuest").style.display = "flex";
}

function showErrorPopupGuest() {
    document.getElementById("AddedErrorPopupGuest").style.display = "flex";
}

function closeDeleteSuccessGuest() {
    document.getElementById("AddedSuccessPopup").style.display = "none";
}

function showDeleteSuccessPopupGuest() {
    document.getElementById("DeletedSuccessPopupGuest").style.display = "flex";
}

function showDeleteErrorNullEntryPopupGuest() {
    document.getElementById("DeletedErrorNullGuest").style.display = "flex";
}
function showDeleteErrorNoMatchEntryPopupGuest() {
    document.getElementById("DeletedErrorNoMatchGuest").style.display = "flex";
}

function showEditedNullError() {
    document.getElementById("EditedNullErrorGuest").style.display = "flex";
}

function showItiNotNumPopup() {
    document.getElementById("ItiNotNumPopup").style.display = "flex";
}

function showItiTooLongPopup() {
    document.getElementById("ItiTooLongPopup").style.display = "flex";
}

