# Working with SharePoint

Most of this tutorial comes from this page [sync-sharepoint-document-library-with-azure-blob-using-logic-apps](https://medium.com/@s.c.vinod/sync-sharepoint-document-library-with-azure-blob-using-logic-apps-bb0def8a8416)

## Prerequisites
This needs a SharePoint online site. You can create a developer site as part of your MSDN benefits.

## create a folder
Add a new logic app with a recurrance trigger.

Add a SharePoint Create Folder action and complete the connection information.

![SharePoint create folder connection](docs/images/SharePointCreateFolder.PNG)

Hit run and you should see your new folder.

Add a Blob action to get blob content and specify the name of a blob in your container.

Next add a sharepoint create file action and specify the path as above (including your library and new folder).

![SharePoint create file action](docs\images\SharePoint_create_file.PNG)

Tip: you can use concat(guid(),'.csv') as the function for the file name to generate random files.