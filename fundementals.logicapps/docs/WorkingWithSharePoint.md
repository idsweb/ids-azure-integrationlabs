# Working with SharePoint

Most of this tutorial comes from this page [sync-sharepoint-document-library-with-azure-blob-using-logic-apps](https://medium.com/@s.c.vinod/sync-sharepoint-document-library-with-azure-blob-using-logic-apps-bb0def8a8416)

## Prerequisites
This needs a SharePoint online site. You can create a developer site as part of your MSDN benefits.

## create a folder

__sample__ sharepointfolder_logicapp.

Add a new logic app with a recurrance trigger.

Add a SharePoint Create Folder action and complete the connection information.

![SharePoint create folder connection](docs/images/SharePointCreateFolder.PNG)

Hit run and you should see your new folder.

Add a Blob action to get blob content and specify the name of a blob in your container.

Next add a sharepoint create file action and specify the path as above (including your library and new folder).

![SharePoint create file action](..\docs\images\SharePoint_create_file.PNG)

Tip: you can use concat(guid(),'.csv') as the function for the file name to generate random files.

## Create a SharePoint list item with attachments

__Intro__ this example takes a message off the Service Bus queue which also have blobs in blob storage for images.

### Preparation
Create a SharePoint site with a list with these columns:

|Item|name|
|----|----|
|Site|your SharePoint site name|
|List|abandoned vehicles|
|Columns|Title (out of the box)|
|Colums|Registration(text)|
|Columns|Lat (text)|
|Columns|Long (text)|

Create a service bus namespace with a queue called __abandonedvehicles__.

Create a storage account with a container called __abandonedvehicles__ makes sure the access level is set to __private__.

Upload the abandonedvehicle1.jpg, abandonedvehicle2.jpg, abandonedvehicle3.jpg images. Use the jobid __4f0ff9c9f2ea4ad8b4d22b64d6be676e__ from the abandonedVehicle.json message in the data folder (enter the id in the Upload to folder input under advanced options).

### Create the logic app

Create a new logic app called AzureToSharepoint, for the Trigger choose __"When a message is received on a Service Bus queue"__.

Note: for now use the root managed shared access key.

Set it to check the __abandonedvehicles queue__ for now specify every 3 weeks as we will manually trigger this from the designer for the demo.

Add a __ParseJson__ Action. Service Bus messages are base64 encoded so for the __content__ use the following expression:
```json
    base64ToString(triggerBody().ContentData)
```
For the schema select "Use sample payload to generate schema and paste in the __abandonedVehicle.json__ file from the data folder.

#### Test the progress so far ####

Navigate to the service bus explorer send screen and copy in the content of the abandonedVehicle.json file setting the content type to application/json. Run the logic app and verify the message is picked up and parsed.

### Create a SharePoint item

Add a scope action. 
Next add a parameter called sitename with the name of your SharePoint site. Give it a default parameter value of the name of your SharePoint site.
Add a SharePoint __Create Item__ action
Add a new connection if prompted and login to your SharePoint site
For the site name use the sitename parameter
For the columns add the values from the ParseJson action. Use the Jobid as the title.

You should have something like this:

![Create item](..\docs\images\CreateSPListItem.PNG)

