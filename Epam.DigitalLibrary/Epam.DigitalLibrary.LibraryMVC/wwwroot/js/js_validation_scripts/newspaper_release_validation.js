$(function () {
    
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
            },
        },

        messages: {
            PagesCount: {
                required: "Pagec count field cannot be empty"
            },
            Number: {
                required: false,
            },
            ReleaseDate: {
                required: false,
            }
        },
    });
});