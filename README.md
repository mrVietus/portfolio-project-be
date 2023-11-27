# SuperCrawler Backend App

## Created with AzureFunctions

To run the application just start the application in VisualStudio

AzureFunction should start on [localhost:7180](http://localhost:7180) and endpoint `http://localhost:7180/api/pagedata` should be available to hit from Postman or Front-End app.

## Configuring connection to the frontend app

In the `FunctionHandler` project we have `local.settings.json` file.

- Configure the `Crawler:AllowedOrigin` property with the frontend url like this:

```json
   {
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "Crawler:AllowedOrigin": "http://localhost:5173",
        "Crawler:CacheItemsTimeSpanInDays": 1,
        "Crawler:CountOfTopWordsThatWillBeReturned": 15
    },
    "Host": {
        "LocalHttpPort": 7180,
        "CORS": "*"
    }
}
```
