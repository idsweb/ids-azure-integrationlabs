# Http trigger
The sample below is for a simple HTTP service. It takes a header value for the api key and then returns a static value with a 200 OK if the api key is right or returns a 403 if it doesn't. I

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fidsweb%2Fids-azure-integrationlabs%2Fmain%2Ffundementals.logicapps%2FLab-httptrigger%2Fazuredeploy.lab-httptrigger.json)

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

In the designer in the options for the HttpAction you can enable schema validation (also in vode veiw as below). Schema validation errors will return a 400 Bad Request.
```json
    },
    "kind": "Http",
    "operationOptions": "EnableSchemaValidation",
    "type": "Request"
}
```
You can add parameters like the http method (POST) and relative paths (such as reports/{reportType}). 
```json
 "triggers": {
    "manual": {
        "inputs": {
            "method": "POST",
            "relativePath": "reports/{reportType}",
                        ..........
```
You can access headers like so
```json
"variables": [
                        {
                            "name": "apikey",
                            "type": "string",
                            "value": "@triggerOutputs()?['headers']?['x-apikey']"
                        }
                    ]
```
or access relative paths like so:
```json
    "variables": [
        {
            "name": "reportType",
            "type": "string",
            "value": "@{triggerOutputs()['relativePathParameters'].reportType}"
        }
    ]
```

You cannot run the trigger from Azure portal, instead you need to run it from a client. Open the Logic App from the designer and copy your trigger URL then run a PowerShell command like below (replacing the url with your own). 

```PowerShell
$uri = "https://prod-00.uksouth.logic.azure.com/workflows/d10cc541a6b5440888d89a2eeb4e5459/triggers/manual/paths/invoke/reports/{reportType}?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=VxMhlSWVWLGReaMt1Qi6komrUEe3pz7GjMrqkeXXcd4"
$headers = @{'x-apikey' = '1234'}
$r = Invoke-WebRequest $uri `
-Method Post `
-Headers $headers `
-Body "{'vehicle':{'vehicleReg':'yg62 byo'}}" `
-ContentType 'application/json'

$r.RawContent
```
Note - I ditched CURL and went with PowerShell as CURL seemed to buggy via CMD. For more tips on PowerShell ivr see [this blog post by David Hamman](https://davidhamann.de/2019/04/12/powershell-invoke-webrequest-by-example/)

