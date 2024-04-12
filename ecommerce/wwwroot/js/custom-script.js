
function alertWithWarningMsg(msg) {
    //swal.fire(msg, "", "warning");
    Swal.fire({
        type: "warning",
        title: msg,
        showConfirmButton: false,
        timer: 1500
    });
}

function alertWithSuccessMsg(msg) {
    //swal.fire(msg, "", "success");
    Swal.fire({
       
        icon: "success",
        title: msg,
        showConfirmButton: false,
        timer: 1500
    });


        
           
              
}

function alertWithErrorMsg(msg) {
    /*if (timer == undefined) timer = 150000;           //time changed by kansoft-Tanvi on 19/08/2022 Ticket No - #SR-57335
    if (autoColse == undefined) autoColse = false;
    Swal.fire({
        type: "error",
        title: msg,
        showConfirmButton: autoColse,
        timer: timer
    });*/
   
    Swal.fire({
        icon: "error",
        title:msg,
        text: "Something went wrong!",
        footer: '<a href="#">Why do I have this issue?</a>'
    });
    //swal.fire(msg, "", "error");
}

function confirmSwalMaster(msg, redirectionLink, redirectionLink2) {
    var isSuccess = false;

    Swal.fire({
        title: 'Are you sure?',
        text: msg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Yes!'
    }).then((result) => {
        if (result.value) {
            isSuccess = true;
            window.location.href = redirectionLink;
        }
        else {
            window.location.href = redirectionLink2;
        }
    });
}

var confirmSwal = function (msg) {

    var isSuccess = false;

    Swal.fire({
        title: 'Are you sure?',
        text: msg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Yes!'
    }).then((result) => {
        if (result.value) {
            isSuccess = true;
        }
        return isSuccess;
    });

};

function alertWithSuccessRedirection(msg, redirectionLink) {
    Swal.fire({
        type: "success",
        title: msg,
        //timer: 1500,
        showConfirmButton: true
    }).then(function () {
        window.location.href = redirectionLink;
    });
}

function alertWithErrorRedirection(msg, redirectionLink) {

    Swal.fire({
        type: "error",
        title: msg,
        showConfirmButton: true,
        allowOutsideClick: false
    }).then(function () {
        window.location.href = redirectionLink;
    });
}

//added by kansoft-Swati Utkarsh Vendor Onboarding on 04/04/2023
function alertWithWarningRedirection(msg, func, newval) {

    Swal.fire({
        type: "warning",
        title: msg,
        showConfirmButton: true,
        allowOutsideClick: false,
        showCancelButton: true,
    }).then(function (e) {
        if (e.value == true) {
            $("#" + newval + "").val("1");
            $("#" + func + "").trigger("click");
        }
    });
}