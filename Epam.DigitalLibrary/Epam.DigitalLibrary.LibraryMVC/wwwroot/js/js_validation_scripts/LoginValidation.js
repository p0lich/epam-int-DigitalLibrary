$(function () {
    jQuery.validator.addMethod("isLoginCorrect", function (value, element) {
        let regCheck = /^[A-Za-z]+((_[A-Za-z0-9]+)|([A-Za-z0-9]*))+[A-Za-z0-9]$/g;
        return this.optional(element) || regCheck.test(value);
    }, "login pattern mismatch");

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