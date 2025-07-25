function closePopup() {
    document.getElementById("welcomePopup").style.display = "none";
}
//To do list page
function showHelpPopup() {
    document.getElementById("helpPopup").style.display = "flex";
}

function closeHelpPopup() {
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
