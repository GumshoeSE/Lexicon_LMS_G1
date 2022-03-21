// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


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
