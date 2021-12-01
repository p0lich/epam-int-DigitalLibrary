$(function () {
    $.validator.addMethod("isLoginCorrect", function (value, element) {
        let regCheck = /^[A-Za-z]+((_[A-Za-z0-9]+)|([A-Za-z0-9]*))+[A-Za-z0-9]$/g;
        return this.optional(element) || regCheck.test(value);
    }, "login pattern mismatch");

    $.validator.addMethod("isPasswordCorrect", function (value, element) {
        let login_value = element.getByName("Login").value
        return this.optional(element) || value === login_value
    }, "login cannot be part of password");

    $("form[name='loginForm']").validate({
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
        },

        messages: {
            Login: {
                required: "Login is required",
            },
            Password: {
                required: "Password is required",
            },
        },
    });
});