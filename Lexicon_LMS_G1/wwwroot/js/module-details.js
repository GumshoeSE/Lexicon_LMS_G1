$(document).ready(function () {

    let currentDisplayed = $("#moduleDetails");

    $("#addActivityBtn").on("click", function (e) {
        currentDisplayed.addClass("hidden");
        $("#addActivity").removeClass("hidden");
        currentDisplayed = $("#addActivity");

        $('.collapse').collapse('hide');

        $('#multi-container').addClass("border-primary");
        setTimeout(removeBlueBorder, 1000);

        $('html, body').animate({ scrollTop: 0 }, 'fast');

    });
    $(".editActivityBtn").on("click", function (e) {

        let activityId = $(this).val();

        $.ajax({
            type: "GET",
            url: '@Url.Action("FindById", "Activities")?id=' + activityId,
            data: { "id": activityId },
            contentType: "application/json",
            success: function (reply) {
                let activity = JSON.parse(reply);
                $("#editActivityName").val(activity.Name);
                $("#editActivityDescription").val(activity.Description);

                document.querySelector("#editActivtyStartDate").value = activity.StartDate;
                $("#editActivtyStartDate").val(activity.StartDate.slice(0, 16));
                $("#editActivtyEndDate").val(activity.EndDate.slice(0, 16));

                currentDisplayed.addClass("hidden");
                $("#editActivity").removeClass("hidden");
                currentDisplayed = $("#editActivity");

                $('.collapse').collapse('hide');

                $('#multi-container').addClass("border-primary");
                setTimeout(removeBlueBorder, 1000);

                $('html, body').animate({ scrollTop: 0 }, 'fast');

            },
            error: function (req, status, error) {
                console.log(error);
                alert('error: ' + error);
            }
        });
    });
    $(".cancelBtn").on("click", function (e) {
        currentDisplayed.addClass("hidden");
        $("#moduleDetails").removeClass("hidden");
        currentDisplayed = $("#moduleDetails");

        $('#multi-container').addClass("border-primary");
        setTimeout(removeBlueBorder, 1000);
    });

    function removeBlueBorder() {
        $('#multi-container').removeClass("border-primary");
    }
});