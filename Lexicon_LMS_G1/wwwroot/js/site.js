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
        var type = $(this).attr("data-type");
        $(".modal-title").text("Do you really want to remove " + name + " " + type);
    });

    $(".toast").toast('show');

    reloadModule();
});

const target = document.getElementById("moduleview");

const breadcrumbModule = document.getElementById("breadcrumbModule");           //li element
const breadcrumbActivity = document.getElementById("breadcrumbActivity");       //li element
const breadcrumbDetail = document.getElementById("breadcrumbDetail");           //li element

let latestModuleId;

breadcrumbModule?.addEventListener("click", function () {
    
    breadcrumbModule.classList.add("active");

    breadcrumbActivity.classList.remove("active");
    breadcrumbActivity.classList.add("hidden");

    breadcrumbDetail.classList.add("hidden");
    breadcrumbDetail.classList.remove("active");
    
    reloadCourse();
});

breadcrumbActivity?.addEventListener("click", function () {
    let params = [['moduleId', latestModuleId]];
    let url = new URL('https://localhost:7124/Courses/GetActionsForModule');
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            target.innerHTML = data;
            reloadActivity();

        });
    breadcrumbDetail.classList.add("hidden");
    breadcrumbDetail.classList.remove("active");
    breadcrumbActivity.classList.add("active");
});

function reloadCourse() {
    let courseId = $("#breadcrumbModule").attr("data-course");
    console.log(courseId);
    let params = [['courseId', courseId]];
    let url = new URL('https://localhost:7124/Courses/GetModulesForCourse');
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            target.innerHTML = data;
            reloadModule();
        });
}

function reloadModule() {
    $(".moduleSelectButton").click(function () {
        breadcrumbActivity.classList.remove("hidden");
        breadcrumbActivity.classList.add("active");
        breadcrumbModule.classList.remove("active");
        let id = $(this).attr("data-module");
        latestModuleId = id;
        let params = [['moduleId', id]];
        let url = new URL('https://localhost:7124/Courses/GetActionsForModule');
        url.search = new URLSearchParams(params).toString();
        fetch(url, {
            method: 'GET'
        })
            .then(res => res.text())
            .then(data => {
                target.innerHTML = data;

                reloadActivity();
            });
    });
}

function reloadActivity() {
    $(".activitySelectButton").click(function () {
        breadcrumbDetail.classList.remove("hidden");
        breadcrumbDetail.classList.add("active");
        breadcrumbActivity.classList.remove("active");
        let id = $(this).attr("data-activity");
        let params = [['activityId', id]];
        let url = new URL('https://localhost:7124/Courses/GetActionsForActivity');
        url.search = new URLSearchParams(params).toString();
        fetch(url, {
            method: 'GET'
        })
            .then(res => res.text())
            .then(data => {
                target.innerHTML = data;
            });
    });
}