# Hello world logic app
This repo contains a simple demo logic app for grandma's shopping list. It is a simple recreation of the many quickstart tutorials you can access via the portal.

# steps
1. If you haven't already then create a resource group for your logic app.
1. Create a logic app via the portal. 
1. Create a service bus namespace and queue via the portal.
1. Upload the shoppinglist.json using service bus explorer via the portal.

## Upload the sample data file
Upload the sample shopping list usine service bus explorer. For type choose application-json and for custom properties add the following

|Name|Value|
|---|---|
|Store|Morrisons

Note you can send, peek and receive via the service bus explorer in the portal. If you make a mistake you can just receive your new message and it will disapear from the queue.

# Create your logic app
Open your logic app in the designer.

Choose the "when a new message is sent to the service bus" trigger.

Select the queue and for now select root managed shared access key.

img here

# Useful links
1. [Transforming JSON Objects in Logic Apps](https://platform.deloitte.com.au/articles/transforming-json-objects-in-logic-apps)
2. [Azure Logic Apps, Functions, and Service Bus](https://brentdacodemonkey.wordpress.com/2016/09/29/azure-logic-apps-functions-and-service-bus/)

## Publishing the sample api
Before publishing the api run 
``` 
dotnet publish -c Release -o ./publish
```

Then right click the __publish__ folder (if you dont you will get an access denied as aspnet core tries to serve one of the random project files).

See the link (here)[https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1]

## Custom connections
You need to use swagger V2.0
app.UseSwagger(o => o.SerializeAsV2 = true);

## Using VS Code
See this link [quickstart create logic apps visual studio code](quickstart-create-logic-apps-visual-studio-code).

## ARM templates and json
For details on arm templates and json this is a good one [https://docs.microsoft.com/en-gb/learn/modules/create-deploy-logic-apps-using-arm-templates](https://docs.microsoft.com/en-gb/learn/modules/create-deploy-logic-apps-using-arm-templates).

## Samples
blobdemo gets a sample json file and initializes a variable (json)
xmlfilesample gets an xml file and parses it into a json object
 