(() => {
    setTimeout(function () {
        $('#message').fadeOut('slow');

    }, 4000)

})();

$(document).ready(function () {
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

var activitiesForCourse = document.getElementById('activitiesList');

$(document).ready(function () {
    $(".courseclick").click(function () {
        if ($(this).attr("aria-expanded") == "true") {
            activitiesForCourse.classList.remove("d-none");
            let id = $(this).attr("data-courseid");
            let params = [['courseId', id]];
            let url = new URL('https://localhost:7124/Activities/GetActionsForCourse');
            url.search = new URLSearchParams(params).toString();
            fetch(url, {
                method: 'GET'
            })
                .then(res => res.text())
                .then(data => {
                    activitiesForCourse.innerHTML = data;
                    $(".pagingbutton").click(pagingClick);
                    $(".atypes").change(typechoice);

                });
        }
        else {
            activitiesForCourse.classList.add("d-none");
        }
    });
})


function pagingClick() {
    let courseId = $(this).attr("data-courseId");
    let pageIndex = $(this).attr("data-pageIndex");
    let activityType = $('.atypes option:selected').val();
    $('#activitiesList').focus();
    let params = {
        'courseId': courseId,
        'pageIndex': pageIndex,
        'activityType': activityType
    };
    let url = new URL('https://localhost:7124/Activities/GetActionsForCourse');
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            activitiesForCourse.innerHTML = data;
            $('.atypes').val(activityType);
            $(".pagingbutton").click(pagingClick);
            $(".atypes").change(typechoice);
        });
}

function typechoice() {
    let courseId = $(this).attr("data-courseId");
    let activityType = $('.atypes option:selected').val();
    let params = {
        'courseId': courseId,
        'activityType': activityType
    };
    let url = new URL('https://localhost:7124/Activities/GetActionsForCourse');
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            activitiesForCourse.innerHTML = data;
            $('.atypes').val(activityType);
            $(".pagingbutton").click(pagingClick);
            $(".atypes").change(typechoice);
            $('#activitiesList').focus();
        });
}


