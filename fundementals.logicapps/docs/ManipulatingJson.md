# Manipulating JSON

Great link [Transforming JSON Objects in Logic Apps](https://platform.deloitte.com.au/articles/transforming-json-objects-in-logic-apps) Posted by Paco de la Cruz on 18 May 2017.

## Options
Integration accounts have support for transformations and of course you can link out to function apps. However for simple tasks you can use built in functions.

## Using compose
You can use a simple compose and feed it a parseJson as below:
```json
"Transform_Participant_by_Using_Compose": {
   "inputs": {
      "country": "@{body('Parse_Input_Employee_Details')?['country']}",
      "department": "@{body('Parse_Input_Employee_Details')?['department']}",
      "email": "@{body('Parse_Input_Employee_Details')?['email']}",
      "familyName": "@{body('Parse_Input_Employee_Details')?['lastName']}",
      "givenName": "@{body('Parse_Input_Employee_Details')?['firstName']}",
      "office": "@{body('Parse_Input_Employee_Details')?['location']}"
   },
   "runAfter": {
      "Parse_Input_Steps": [
         "Succeeded"
       ]
   },
   "type": "Compose"
}
```
__TIP: A lot of the functions that manipulate JSON are also PowerAutomate functions__

You can use a subset of your data within ParseJson by cutting down the schema:
```json
{
    "type": "object",
    "properties": {
        "vehicle": {
            "type": "object",
            "properties": {
                "vehicleReg": {
                    "type": "string"
                }
            }
        },
        "location": {
            "type": "object",
            "properties": {
                "coordinates": {
                    "type": "string"
                }
            }
        }
    }
}
```