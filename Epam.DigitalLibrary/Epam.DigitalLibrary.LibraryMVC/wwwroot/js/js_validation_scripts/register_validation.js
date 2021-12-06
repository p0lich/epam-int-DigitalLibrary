$(function () {
    $.validator.addMethod("isLoginCorrect", function (value, element) {
        let regCheck = /^[A-Za-z]+((_[A-Za-z0-9]+)|([A-Za-z0-9]*))+[A-Za-z0-9]$/g;
        return this.optional(element) || regCheck.test(value);
    }, "login pattern mismatch");

    $.validator.addMethod("isPasswordCorrect", function (value, element) {
        let login_reg = new RegExp(document.getElementById('Login').value, "g");
        return this.optional(element) || !login_reg.test(value)
    }, "login cannot be part of password");

    $("form[name='registerForm']").validate({
        rules: {
            Login: {
                required: true,
                minlength: 3,
                maxlength: 20,
                isLoginCorrect: true,
            },

            Password: {
                required: true,
                minlength: 3,
                maxlength: 20,
                isPasswordCorrect: true
            },

            PasswordConfirm: {
                required: true,
                minlength: 3,
                maxlength: 20,
                equalTo: '#PasswordConfirm'
            }
        },

        messages: {
            Login: {
                required: "Login is required",
            },
            Password: {
                required: "Password is required",
            },
            PasswordConfirm: {
                required: "Repeat password",
                equalTo: "Password mismatch"
            }
        },
    });
});