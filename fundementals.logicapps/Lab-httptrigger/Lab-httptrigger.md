# Http trigger
One of the common types of trigger is the http trigger. 

Links:
[https://docs.microsoft.com/en-us/azure/connectors/connectors-native-reqres](https://docs.microsoft.com/en-us/azure/connectors/connectors-native-reqres)
[https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-http-endpoint#pass-parameters-through-endpoint-url](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-http-endpoint#pass-parameters-through-endpoint-url)

This sample accepts the following Json in the body of the request:
```json
{
    "vehicle": {
        "vehicleReg": "yg62 byo"
    }
}
```
The schema for the Json is like below:
```json
{
    "$schema": "http://json-schema.org/draft-07/schema",
    "$id": "http://example.com/example.json",
    "type": "object",
    "title": "Vehicle reg schema",
    "description": "The root schema comprises the entire JSON document.",
    "default": {},
    "examples": [
        {
            "vehicle": {
                "vehicleReg": "yg62 byo"
            }
        }
    ],
    "required": [
        "vehicle"
    ],
    "properties": {
        "vehicle": {
            "$id": "#/properties/vehicle",
            "type": "object",
            "title": "The vehicle schema",
            "description": "Wrapper for the vehicle object",
            "default": {},
            "examples": [
                {
                    "vehicleReg": "yg62 byo"
                }
            ],
            "required": [
                "vehicleReg"
            ],
            "properties": {
                "vehicleReg": {
                    "$id": "#/properties/vehicle/properties/vehicleReg",
                    "type": "string",
                    "title": "The vehicleReg schema",
                    "description": "The registration number of the vehicle",
                    "default": "",
                    "examples": [
                        "yg62 byo"
                    ]
                }
            },
            "additionalProperties": true
        }
    },
    "additionalProperties": true
}
```
You may see a message 'Remember to include a Content-Type header set to application/json in your request.' prompting you to include that in your calling appication if you are dealing with Json.

In the designer in the options for the HttpAction you can enable schema validation (also in vode veiw as below). Schema validation errors will return a 400 Bad Request.
```json
    },
    "kind": "Http",
    "operationOptions": "EnableSchemaValidation",
    "type": "Request"
}
```
You can add parameters like method (POST) and relative path such as reports/{reportType}. 
```json
 "triggers": {
    "manual": {
        "inputs": {
            "method": "POST",
            "relativePath": "reports/{reportType}",
            "schema": {
                "$id": "http://example.com/example.json",
                "$schema": "http://json-schema.org/draft-07/schema",
                "additionalProperties": true,
                        ..........
```
The URL below passes through the report type of stolen:
https://prod-04.uksouth.logic.azure.com/workflows/1cad499bxxxxxxxxxxxx3016/triggers/manual/paths/invoke/reports/stolen?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=12987394872kfhskdfjhsjfhs

