# AzureFunctionsContactForm
A simple "drop-in" contact form for any website using Azure Functions.

#### Instructions (in no particular order)
* Create a new HTTP Trigger Azure Function and paste the code from Server/SendEmailFunction.csx
* Set "Authorization level" to "Anonymous" from "Integrate" tab in your Function dashboard
* Enable CORS on your Function App
* See: http://stackoverflow.com/questions/36411536/how-can-i-use-nuget-packages-in-my-azure-functions on how to add the Server/Project.json file to your Function
* Drop the form found in client.html into a web-page
* Add a reference to the javascript in sendEmail.js
* Some other things I'm sure to have forgotten...
* Complete the contact form and see your email!
