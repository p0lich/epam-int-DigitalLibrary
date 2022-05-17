$(function () {
    $.validator.addMethod("isReleaseDateCorrect", function (value, element) {
        let date_parse = Date.parse(value);
        let publication_date = Date.parse(document.getElementById('PublicationDate').value);
        let max_date = Date.now();
        return this.optional(element) || (date_parse > publication_date && date_parse < max_date)
    }, "Date must be between 1400 year and now");
    
    $("form[name='releaseInputForm']").validate({
        rules: {
            PagesCount: {
                required: true,
                range: [1, 32000]
            },
            Number: {
                required: false,
                maxlength: 2000,
            },
            ReleaseDate: {
                required: true,
                isReleaseDateCorrect: true
            },
        },

        messages: {
            PagesCount: {
                required: "Pages count field cannot be empty"
            },
            Number: {
            },
            ReleaseDate: {
                required: "Must be filled"
            }
        },
    });
});