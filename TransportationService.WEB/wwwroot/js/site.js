// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var loginBtn;
var authenticateButton;
var errorLoginSpan;
var loginFormDiv;
var loginForm;
var ResetPasswordLogSpan;
var resetPasswordForm;
var resetPasswordButton;
$(document).ready(() => {
    loginBtn = $('#loginButton');
    authenticateButton = $('#authButton');
    errorLoginSpan = $('#errorLoginSpan');
    loginFormDiv = $('#loginFormDiv');
    loginForm = $('#loginForm')
    resetPasswordForm = $('#resetPasswordForm');
    ResetPasswordLogSpan = $('#resetPasswordLogSpan');
    resetPasswordButton = $('#resetPasswordButton');
    resetPasswordButton.on('click', tryFindUser)
    loginForm.on('submit', tryAuthenticate);
    loginBtn.on('click', showLoginForm);
});

function hideLoginForm() {
    errorLoginSpan.empty();
    loginFormDiv.css('display', 'none');
    loginBtn.click(showLoginForm);
}
function showLoginForm() {
    loginFormDiv.css('display', 'block');
    loginBtn.click(hideLoginForm);
}
function tryAuthenticate(e) {
    e.preventDefault();
    errorLoginSpan.empty();
    $.ajax({
        type: "get",
        url: "/Account/Authenticate",
        data: loginForm.serialize(), // serializes the form's elements.
        success: function () {
            location.reload();
        },
        error: function (xhr) {
            errorLoginSpan.append("<p>" + xhr.responseText + "</p>")
        }
    });
}
function tryFindUser(e) {
    e.preventDefault();
    ResetPasswordLogSpan.empty();
    $.ajax({
        type: "get",
        url: "/Account/FindSuggestedUser",
        data: resetPasswordForm.serialize(),
        success: function () {
            resetPasswordForm.submit();
        },
        error: function (xhr) {
            ResetPasswordLogSpan.append("<p>" + xhr.responseText + "</p>")
        }
    });
}