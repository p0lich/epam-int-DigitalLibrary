$(function () {
    $.validator.addMethod("isCountryCorrect", function (value, element) {
        let regCheck = /^(([A-Z][a-z]+|[A-Z]{2,})|([А-ЯЁ][а-яё]+|[А-ЯЁ]{2,}))$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $.validator.addMethod("isRegistrationNumberCorrect", function (value, element) {
        let regCheck = /^[0-9]{1,9}$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $("form[name='patentInputForm']").validate({
        rules: {
            Name: {
                required: true,
                maxlength: 300,
            },
            Country: {
                required: true,
                maxlength: 200,
                isCountryCorrect: true
            },
            RegistrationNumber: {
                required: true,
                maxlength: 9,
                isRegistrationNumberCorrect: true
            },
            ApplicationDate: {
                required: false,

            },
            PublicationDate: {
                required: true,
            },
            PagesCount: {
                required: true,
                range: [1, 32000]
            },
            ObjectNotes: {
                required: false,
                maxlength: 2000,
            },
        },

        messages: {
            Name: {
                required: "Name field cannot be empty",
            },
            Country: {
                required: "Publication field place cannot be empty",
            },
            RegistrationNumber: {
                required: "Publisher field cannot be empty",
            },
            ApplicationDate: {
                
            },
            PublicationDate: {
                required: "Choose date of publication",
            },
            PagesCount: {
                required: "Pages count field cannot be empty",
            },
            ObjectNotes: {
                maxlength: $.validator.format("Must be shorter than {0} symbols"),
            },
        },
    });
});