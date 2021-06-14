// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
Site = (() => {
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    (function setNavBarItem() {
        if (getCookie("AdminId") != null) {
            $("#navbar-list").append($(`
                <li style="margin-left:20px" class="nav-item">
                    <a href="/admin" class="btn btn-outline-primary my-2 my-sm-0 login-color">Games</a>
                    <a href="/admin/gamemaster" class="btn btn-outline-primary my-2 my-sm-0 login-color">Game masters</a>
                </li>
                <li style="margin-left:auto" class="nav-item">
                    <a href="/home/logout" class="btn btn-outline-primary my-2 my-sm-0 login-color">Log out</a>
                </li>
            `));
        }
        else if (getCookie("GameMasterId") != null) {
            $("#navbar-list").append($(`
                <li style="margin-left:20px" class="nav-item">
                    <a href="/gamemaster" class="btn btn-outline-primary my-2 my-sm-0 login-color">Games</a>
                </li>
                <li style="margin-left:auto" class="nav-item">
                    <a href="/home/logout" class="btn btn-outline-primary my-2 my-sm-0 login-color">Log out</a>
                </li>
            `));
        }
        else {
            $("#navbar-list").append($(`
                <li style="margin-left:auto" class="nav-item">
                    <a href="/home/login" class="btn btn-outline-primary my-2 my-sm-0 login-color">Login</a>
                </li>
            `));
        }
    } )();

    return {
        getCookie: getCookie
    }
})();