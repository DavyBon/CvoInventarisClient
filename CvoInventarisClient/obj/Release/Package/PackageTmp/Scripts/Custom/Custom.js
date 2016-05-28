
$(document).ready(function () {
    (function ($) {
        $('#filter').keyup(function () {
            var rex = new RegExp($(this).val(), 'i');
            $('.search tr').hide();
            $('.search tr').filter(function () {
                return rex.test($(this).text());
            }).show();
        })
    }(jQuery));
});

