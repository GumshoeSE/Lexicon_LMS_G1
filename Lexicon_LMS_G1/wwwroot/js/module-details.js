﻿$(document).ready(function () {

    let currentDisplayed = $("#moduleDetails");

    $("#editModuleBtn").on("click", function () {
        showDiv($("#editModule"));
    });

    $("#addActivityBtn").on("click", function () {
        showDiv($("#addActivity"));

        $("#addActivtyStartDate").val($("#LastActivityEndTime").val().substr(0, 16));
        $("#addActivtyEndDate").val($("#LastActivityEndTime").val().substr(0, 16));
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

    $("#submitEditModuleBtn").on("click", function () {

        if (!validateEditModuleForm()) return;

        let dto = {
            Id : $("#editModuleId").val(),
            Name: $("#editModuleName").val().trim(),
            Description: $("#editModuleDescription").val().trim()
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
            Name: $("#editActivityName" + activityId).val().trim(),
            Description: $("#editActivityDescription" + activityId).val().trim()
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

        if (!validateAddActivityForm()) return;

        let dto = {
            Name: $("#addActivtyName").val().trim(),
            Description: $("#addActivtyDescription").val().trim(),
            StartDate: $("#addActivtyStartDate").val(),
            EndDate: $("#addActivtyEndDate").val(),
            ModuleId: $("#addActivityModuleId").val(),
            ActivityTypeId: $("#addActivityTypeId").val()
        };

        $.ajax({
            url: $("#AddActivityUrl").val(),
            type: "PUT",
            data: JSON.stringify(dto),
            contentType: "application/json",
            success: function (reply) {
                if (!reply.success) {
                    let errors = reply.errors;
                    for (var index in errors) {
                        let key = errors[index].key;
                        let message = errors[index].message;

                        let input = $("#addActivty" + key);
                        let inputValidation = $("#validationAddActivity" + key);

                        input.addClass("is-invalid");
                        inputValidation.text(message);
                    }
                }
                else {
                    window.location.href = reply.redirectToUrl;
                }
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });

    });

    $(".req-init").each(requiredFieldNotEmptyInit);

    $(".req").on("change", requiredFieldNotEmpty);
    $("#addActivtyStartDate").on("change", validateAddActivityDateRange);
    $("#addActivtyEndDate").on("change", validateAddActivityDateRange);

    function validateAddActivityDateRange() {

        let moduleStartTime = $("#ModuleStartTime");
        let startDateTime = $("#addActivtyStartDate");
        let endDateTime = $("#addActivtyEndDate");

        let startDateTimeValidation = $("#validationAddActivityStartDate");
        let endDateTimeValidation = $("#validationAddActivityEndDate");

        startDateTime.removeClass("is-valid");
        startDateTime.removeClass("is-invalid");
        endDateTime.removeClass("is-valid");
        endDateTime.removeClass("is-invalid");

        if (!startDateTime.val()) {
            startDateTimeValidation.text("Please provide a start time.");
            moduleStartTime.addClass("is-invalid");
        }
        if (!endDateTime.val()) {
            startDateTimeValidation.text("Please provide a end time.");
            endDateTime.addClass("is-invalid");
        }

        if (!requiredAfter(moduleStartTime, startDateTime)) {
            startDateTimeValidation.text("The activity can't start before the module does.");
            moduleStartTime.addClass("is-invalid");
        }
        if (!requiredAfter(moduleStartTime, endDateTime)) {
            endDateTimeValidation.text("The activity can't end before the module starts.");
            endDateTime.addClass("is-invalid");
        }

        if (!requiredAfter(startDateTime, endDateTime)) {
            startDateTimeValidation.text("The activity has to start before it ends.");
            endDateTimeValidation.text("The activity has to start before it ends.");
            startDateTime.addClass("is-invalid");
            endDateTime.addClass("is-invalid");
        }
    }

    function toggleEditActivity(activityId) {
        $("#activityDetails" + activityId).toggleClass("hidden");
        $("#editActivity" + activityId).toggleClass("hidden");
        blinkBlueBorder($("#accordion-body" + activityId))
    }

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
        $("#editModule").find('.req:not(.is-invalid)').addClass('is-valid');
        return ($("#editModule").find('.is-invalid')).length == 0;
    }

    function validateEditActivityForm(activityId) {
        $("#editActivity").find('.req:not(.is-invalid)').addClass('is-valid');
        return ($("#editActivity" + activityId).find('.is-invalid')).length == 0;
    }

    function validateAddActivityForm() {
        $("#addActivity").find('.req').each(requiredFieldNotEmpty);
        validateAddActivityDateRange();

        $("#addActivity").find('.req:not(.is-invalid)').addClass('is-valid');
        return ($("#addActivity").find('.is-invalid')).length == 0;
    }

    function requiredFieldNotEmpty() {
        if (!$(this).val() || $(this).val().trim() === "") {
            $(this).addClass("is-invalid");
            $(this).removeClass("is-valid");
        }
        else {
            $(this).removeClass("is-invalid");
            $(this).addClass("is-valid");
        }
    }

    function requiredFieldNotEmptyInit() {
        if (!$(this).val()) {
            $(this).addClass("is-invalid");
        }
    }

    function requiredAfter(beforeInput, afterInput) {
        return (new Date(beforeInput.val()) < new Date(afterInput.val()));
    }
});