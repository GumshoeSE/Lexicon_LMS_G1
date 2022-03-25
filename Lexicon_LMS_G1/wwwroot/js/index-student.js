
$(document).ready(function () {
    //const uploadButtons = document.querySelectorAll("[id^=uploadAssignment]");
    $(".uploadAssignment").on("click", function () {
        let activityId = $(this).attr("data-activity-id");

        $("#-R��������Ξft").val(activityId);

    });
});