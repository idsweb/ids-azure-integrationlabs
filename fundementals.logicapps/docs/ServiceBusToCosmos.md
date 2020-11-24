# Creating Cosmos DB items from Service bus queue messages
This sample takes a JSON document off the service bus queue and creates a Cosmos DB item.

__Sample: Cosmoslogicapp__


## Known issues 
When creating this sample __cosmoslogicapp__ I hit two issues:

1. ["The partition key supplied in x-ms-partitionkey header has fewer components than defined in the the collection".](https://marcelzehner.ch/2019/02/05/issues-with-creating-new-cosmos-db-documents-with-logic-app/).
1. ["The input content is invalid because the required properties - 'id; ' - are missing".](https://stackoverflow.com/questions/64794200/logic-apps-and-cosmosdb)

In the end adding the correct partition and an id solved the issue. Note the approach I took in the end is to parse the Service bus queue message data into a string variable like so:
```json
{
    "inputs": {
        "variables": [
            {
                "name": "sbMessage",
                "type": "string",
                "value": "@{base64ToString(triggerBody().ContentData)}"
            }
        ]
    }
}
```
From there I parsed the Json string above using my service bus queue message content string above.

## Sample
1. If you dont already have a free tier Cosmos DB account create one.
1. Add a database and container. For mine I called it _logicappsdemo_. Make sure for this sample you choose __jobid__ as the partition key.
1. Create a service bus namespace if you dont have one already and add a queue. I called mine abandoned vehicles.
1. Create a logic app - I called mine __cosmoslogicapp__. 
1. For the trigger choose service bus queue and as described above add a initialize variable of type string action.
1. Add a parse Json action using the string above. 
1. Note in the Cosmos DB create or update action the partition key value doesnt seem to appear. In the end it still worked without but many blog posts refer to this key. Your logic should look like this:
![servicebustologicapp](..\docs\images\ServiceBusToCosmos.PNG)
The Cosmos action looks like this:
![cosmosconnection](..\docs\images\cosmosconnection.PNG)

