


(() => {
    setTimeout(function () {
        $('#message').fadeOut('slow');

    }, 4000)

})();

$(document).ready(function (){
    $(".deleter").click(function () {
        $("#deleteId").val(
            $(this).attr("data-ref")
        );
        var name = $(this).attr("data-name");
        $(".modal-title").text("Do you really want to remove this " + name + " course");
    });
});

$(document).ready(function () {
    $(".toast").toast('show');
});