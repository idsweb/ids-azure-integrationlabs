# Http trigger
The sample below is for a simple HTTP service. It takes a report type as a path which it echoes out.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fidsweb%2Fids-azure-integrationlabs%2Fmain%2Ffundementals.logicapps%2FLab-httptrigger%2Ftemplate.json)

MSDN Links:
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
You cannot run the trigger from Azure portal, instead you need to run it from a client. Open the Logic App from the designer and copy your trigger URL then run a CURL command like below (replacing the url with your own):
```
curl -i -X POST https://prod-19.uksouth.logic.azure.com/workflows/2edff6e8948e4ebc87244e11e914d555/triggers/manual/paths/invoke/reports/{reportType}?api-version=2016-10-01"&"sp=%2Ftriggers%2Fmanual%2Frun"&"sv=1.0"&"sig=oEB09N4xnkB0i5b2cggZfMhVbBSL0osvoscGWi51ezw -H "Content-Type: application/json" -H  "x-apikey:1234" -d "{\"vehicle\":{\"vehicleReg\":\"yg62 byo\"}}"
```
_note: this is all one line and the & character from the Logic apps designer needs wrapping in double quotes. 

