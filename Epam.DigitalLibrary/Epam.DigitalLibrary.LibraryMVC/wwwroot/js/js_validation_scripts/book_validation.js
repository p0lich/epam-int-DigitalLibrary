$(function () {
    $.validator.addMethod("isPublicationPlaceCorrect", function (value, element) {
        let regCheck = /^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+((-[A-Z])|(-[a-z]+-[A-Z]))?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+((-[А-ЯЁ])|(-[а-яё]+-[А-ЯЁ]))?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $.validator.addMethod("isISBNCorrect", function (value, element) {
        let regCheck = /^ISBN ((999[0-9]{2})|(99[4-8][0-9])|(9(([5-8][0-9])|(9[0-3])))|((8[0-9])|(9[0-4]))|[0-7])-[0-9]{1,7}-[0-9]{1,7}-[0-9X]$/g;
        return this.optional(element) || regCheck.test(value);
    }, "Wrong input. Patern mismatch");

    $("form[name='bookInputForm']").validate({
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
            PagesCount: {
                required: true,
                range: [1, 32000]
            },
            ObjectNotes: {
                required: false,
                maxlength: 2000
            },
            ISBN: {
                required: false,
                maxlength: 18,
                isISBNCorrect: true
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
            PagesCount: {
                required: "Pages count field cannot be empty",
            },
            ObjectNotes: {
                maxlength: $.validator.format("Must be shorter than {0} symbols"),
            },
            ISBN: {

            }
        },
    });
});