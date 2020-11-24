# Processing Cosmos DB items
Poll a Cosmos DB container for documents not marked as processed then mark them as processed and update the item.

__sample: readandupdatecosmos__

__Note - I have been unable to trigger a logic app when a new item is created so this work around is using a flag on the item and a query that finds unprocessed docs__

## Known issues 
When creating this sample __cosmoslogicapp__ I hit two issues:

1. ["The partition key supplied in x-ms-partitionkey header has fewer components than defined in the the collection".](https://marcelzehner.ch/2019/02/05/issues-with-creating-new-cosmos-db-documents-with-logic-app/).
1. ["The input content is invalid because the required properties - 'id; ' - are missing".](https://stackoverflow.com/questions/64794200/logic-apps-and-cosmosdb)


## Sample
1. If you dont already have a free tier Cosmos DB account create one.
1. Add a database and container. For mine I called it _logicappsdemo_. Make sure for this sample you choose __jobid__ as the partition key.
1. Add a recurrence trigger
1. Add a Query documents as below
![query documents](..\docs\images\QueryDocuments.PNG)
1. Add a for each using the output from the action above
1. Add an action in the loop (Cosmos) create or update documents V2
1. Update the item (see below)

There are a couple of ways to create the new document for the update. One way is to build the Json in the parse Json action like below:
![Manualy set the Json](..\docs\images\CosmosDocManual.PNG)
The other is to use the current item with the setProperty functionj as below:
```json
"body": "@setProperty(items('For_each'),'processed',true)
```
![Set property on current item](..\docs\images\CosmosUpdateWithSetProperty.PNG)
