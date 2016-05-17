// POSTs a message to your Azure Function.

(function () {
    // Get this from the Azure Dashboard in the Functions blade.
    var functionUrl = 'https://YOUR_APP_URL.azurewebsites.net/api/YOUR_AZURE_FUNCTION';

    $(document).ready(function () {

        $('#contactForm').submit(function (event) {

            event.preventDefault();

            var $submitButton = $('#submitContactFormButton');
            $submitButton.prop('value', "Please wait...");
            $submitButton.prop('disabled', true);

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
        var $submitButton = $('#submitContactFormButton');
        $submitButton.prop('value', "Contact Me");
        $submitButton.prop('disabled', false);
        $('#submitContactFormButton').after('<p>Message received, thanks!</p>');
    }

    function postFailure(data) {
        var $submitButton = $('#submitContactFormButton');
        $submitButton.prop('value', "Contact Me");
        $submitButton.prop('disabled', false);
        if (data.responseText)
            $submitButton.after('<p>Something went wrong: ' + data.responseText + '</p>');
        else
            $submitButton.after('<p>Something went wrong. Make sure all fields are populated.</p>');
    }

    function getFormData($form) {
        var data = {};
        $form.serializeArray().map(function (x) { data[x.name] = x.value; });
        return data;
    }
})();