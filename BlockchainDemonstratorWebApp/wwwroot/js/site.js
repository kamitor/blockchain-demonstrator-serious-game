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
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/admin'">Games</button>
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/admin/gamemaster'">Game masters</button>
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/factors'">Game tuning</button>
                </li>
                <li style="margin-left:auto" class="nav-item">
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/home/logout'">Logout</button>
                </li>
            `));
        }
        else if (getCookie("GameMasterId") != null) {
            $("#navbar-list").append($(`
                <li style="margin-left:20px" class="nav-item">
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/gamemaster'">Games</button>
                </li>
                <li style="margin-left:auto" class="nav-item">
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/home/logout'">Logout</button>
                </li>
            `));
        }
        else {
            $("#navbar-list").append($(`
                <li style="margin-left:auto" class="nav-item">
                    <button style="border:none; outline:none;" class="nav-button gradient tertiary-text" onclick="window.location.href='/home/login'">Login</button>
                </li>
            `));
        }
    } )();

    return {
        getCookie: getCookie
    }
})();