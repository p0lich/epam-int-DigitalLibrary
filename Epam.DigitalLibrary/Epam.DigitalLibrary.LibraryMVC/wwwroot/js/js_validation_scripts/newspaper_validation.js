$(function () {
    $.validator.addMethod("isPublicationPlaceCorrect", function (value, element) {
        let regCheck = /^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+(-[A-Z])?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+(-[А-ЯЁ])?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $.validator.addMethod("isISSNCorrect", function (value, element) {
        let regCheck = /^ISSN[0-9]{4}-[0-9]{4}$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $("form[name='newspaperInputForm']").validate({
        rules: {
            Name: {
                required: true,
                maxlength: 300,
            },
            PublicationPlace: {
                required: true,
                maxlength: 200,
                isPublicationPlaceCorrect: true
            },
            Publisher: {
                required: true,
                maxlength: 300,
            },
            PublicationDate: {
                required: true,
            },
            ObjectNotes: {
                required: false,
                maxlength: 2000,
            },
            ISSN: {
                required: false,
                maxlength: 13,
                isISSNCorrect: true
            }
        },

        messages: {
            Name: {
                required: "Name field cannot be empty",
            },
            PublicationPlace: {
                required: "Publication field place cannot be empty",
            },
            Publisher: {
                required: "Publisher field cannot be empty",
            },
            PublicationDate: {
                required: "Choose date of publication",
            },
            ObjectNotes: {
                maxlength: $.validator.format("Must be shorter than {0} symbols"),
            },
            ISSN: {

            }
        },
    });
});