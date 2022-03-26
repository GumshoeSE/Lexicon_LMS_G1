$(document).ready(function () {

    let currentDisplayed = $("#moduleDetails");

    $("#editModuleBtn").on("click", function () {
        showDiv($("#editModule"));
    });

    $("#addActivityBtn").on("click", function () {
        showDiv($("#addActivity"));

        $("#addActivtyStartDate").val($("#LastActivityEndTime").val().slice(0, 16));
        $("#addActivtyEndDate").val($("#LastActivityEndTime").val().slice(0, 16));
    });

    $(".editActivityBtn").on("click", function () {
        let activityId = $(this).val();
        toggleEditActivity(activityId);
    });

    $(".cancelBtn").on("click", function () {
        showDiv($("#moduleDetails"));
    });

    $(".cancelEditActivityBtn").on("click", function () {
        let activityId = $(this).val();
        toggleEditActivity(activityId);
    })

    function toggleEditActivity(activityId) {
        $("#activityDetails" + activityId).toggleClass("hidden");
        $("#editActivity" + activityId).toggleClass("hidden");
        blinkBlueBorder($("#accordion-body" + activityId))
    }

    $("#submitEditModuleBtn").on("click", function () {

        if (!validateEditModuleForm()) return;

        let dto = {
            Id : $("#editModuleId").val(),
            Name : $("#editModuleName").val(),
            Description : $("#editModuleDescription").val()
        };

        $.ajax({
            url: $("#UpdateModuleUrl").val(),
            type: "POST",
            data: JSON.stringify(dto),
            contentType: "application/json",
            success: function (reply) {
                window.location.href = reply.redirectToUrl;
            },
            error: function (req, status, error) {
                console.log(error);
                alert('error: ' + error);
            }
        });

    });

    $(".submitEditActivityBtn").on("click", function () {

        let activityId = $(this).val();

        if (!validateEditActivityForm(activityId)) return;

        let dto = {
            Id: $("#editActivityId" + activityId).val(),
            Name: $("#editActivityName" + activityId).val(),
            Description: $("#editActivityDescription" + activityId).val()
        };

        $.ajax({
            url: $("#UpdateActivityUrl").val(),
            type: "POST",
            data: JSON.stringify(dto),
            contentType: "application/json",
            success: function (reply) {
                window.location.href = reply.redirectToUrl;
            },
            error: function (req, status, error) {
                console.log(error);
                alert('error: ' + error);
            }
        });

    });

    $("#submitAddActivityBtn").on("click", function () {
        alert("add activity");
    });

    function showDiv(div) {
        currentDisplayed.addClass("hidden");
        div.removeClass("hidden");
        currentDisplayed = div;
        blinkBlueBorder($('#multi-container'));
    }

    function blinkBlueBorder(container) {
        container.addClass("border-primary");
        setTimeout(removeBorderColor.bind(null, container), 1000);
    }

    function removeBorderColor(container) {
        container.removeClass("border-primary");
    }

    function validateEditModuleForm() {
        return ($("#editModule").find('.is-invalid.req')).length == 0;
    }

    function validateEditActivityForm(activityId) {
        return ($("#editActivity" + activityId).find('.is-invalid.req')).length == 0;
    }

    $(".req").each(requiredFieldNotEmptyInit);
    $(".req").on("change", requiredFieldNotEmpty);

    function requiredFieldNotEmpty() {
        if (!$(this).val()) {
            $(this).addClass("is-invalid");
            $(this).removeClass("is-valid");
        }
        else {
            $(this).addClass("is-valid");
            $(this).removeClass("is-invalid");
        }
    }

    function requiredFieldNotEmptyInit() {
        if (!$(this).val()) {
            $(this).addClass("is-invalid");
        }
    }
});