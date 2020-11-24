# Woringin with XML

For the full sample see xmlfilesample in the portal.

Logic apps work with json. You may occasionally want to work with XML (e.g. files in a storage account) without needing an integration account.

To work with the file you will need to grab the content and parse the json representation of the XML content into your json schema.

To keep things simple we will work with a really simple XML file.

Set up a storage account and container then create a logic app with a blob trigger. You can access the content of an XML file in an initialize variable action. To grab individual properties reference them in the value of the variable as below.

![blob trigger for xml content](docs\images\XML_file_blob_trigger.PNG)

for the sample below
```xml
<person>
<name>Ian</name>
</person>
```
The code view of the Initialize variable looks like this:
```json
{
    "inputs": {
        "variables": [
            {
                "name": "person",
                "type": "string",
                "value": "@json(xml(body('Get_blob_content'))).person.name"
            }
        ]
    }
}
```
Alternatively you can just grab the whole structure and set the variable type as object and use it later (as below).

```json
@json(xml(body('Get_blob_content')))
```
This will return an object based on the XML structure. Note: if you add the declaration then it appears in the output
```json
{
  "?xml": {
    "@version": "1.0",
    "@encoding": "UTF-8"
  },
  "person": {
    "name": "Ian"
  }
}
```
You can also use the same expression in a parsejson function. If you use the above as a starter to grab your json you can paste that into the ParseJson's get schema from sample dialogue.

The parse json for our small sample looks like the code below:
```json
{
    "inputs": {
        "content": "@json(xml(body('Get_blob_content')))",
        "schema": {
            "properties": {
                "?xml": {
                    "properties": {
                        "@@encoding": {
                            "type": "string"
                        },
                        "@@version": {
                            "type": "string"
                        }
                    },
                    "type": "object"
                },
                "person": {
                    "properties": {
                        "name": {
                            "type": "string"
                        }
                    },
                    "type": "object"
                }
            },
            "type": "object"
        }
    }
}
```

IMPORTANT the ParseJson action's content takse json and parses it into the schema, NOT the raw XML.
```json
        "content": "@json(xml(body('Get_blob_content')))",
```

These properties are then available to assign to other variables.

So to summarise:
1. Add the blob trigger
1. Add the Get blob content action
1. Add the parse json
1. Set the input to "content": "@json(xml(body('Get_blob_content')))" and add your json schema

You can then use your output from ParseJson in other steps (the example below extracts the name property).

![blob trigger with get content and parse json](docs\images\XML_file_blob_trigger_2.PNG)

