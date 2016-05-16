// POSTs a message to your Azure Function.

(function () {
    // Get this from the Azure Dashboard in the Functions blade.
    var functionUrl = 'https://YOUR_APP_URL.azurewebsites.net/api/YOUR_AZURE_FUNCTION';

    $(document).ready(function () {

        $('#contactForm').submit(function (event) {

            event.preventDefault();
            $(':submit').prop('disabled', true);
            $.ajax({
                url: functionUrl,
                type: 'POST',
                data: JSON.stringify(getFormData($('#contactForm'))),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: postSuccess,
                error: postFailure
            });
        });

    });

    function postSuccess(data) {
        $(':submit').prop('disabled', false);
        $(':submit').after('<p>Message received, thanks!</p>');
    }

    function postFailure(data) {
        $(':submit').prop('disabled', false);
        if (data.responseText)
            $(':submit').after('<p>Something went wrong: ' + data.responseText + '</p>');
        else
            $(':submit').after('<p>Something went wrong. Make sure all fields are filled out.</p>');
    }

    function getFormData($form) {
        var data = {};
        $form.serializeArray().map(function (x) { data[x.name] = x.value; });
        return data;
    }
})();