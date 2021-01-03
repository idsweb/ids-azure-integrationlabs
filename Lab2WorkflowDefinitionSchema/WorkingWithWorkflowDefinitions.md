# Create, edit, or extend JSON for logic app workflow definitions in Azure Logic Apps 

[See the docs](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-author-definitions)

## Basic structure
The basic structure looks like this:
```json
{
    "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {},
    "triggers": {},
    "actions": {},
    "outputs": {}
}
```
## contentVersion
This can be used to version your workflows.

## Parameters
Parameters are passed in to your workflow. Add a parameter using the json below to your code:
```json
    "parameters": {
        "blobUrl": {
            "defaultValue": "http://somebloburi/",
            "type": "String",
            "metadata": {
                "description": "the URI of blob storage"
            }
        }
    },
```
__TIP__:Always give your parameters default values so you can edit them in the portal and test run them. 

### Parameters and deployment
Parameters in your workflow can be passed in from an ARM template file. There is an example of that later.

[Overview: Automate deployment for Azure Logic Apps by using Azure Resource Manager templates](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-azure-resource-manager-templates-overview).

## Triggers
In Visual Studio Code create a new logic app. Add a recurrence trigger (press Ctrl+Spacebar and select recurrence). Change the frequency to week and leave the defaults.

## Actions
IN VS Code place your cursor in the actions and press Ctrl + Spacebar and type compose. Add a simple array as below:
```json
            "inputs": {
                "sampleArray": [
                    "http://www.someurl.co.uk/files/file1.jpg",
                    "http://www.someurl.co.uk/files/file2.jpg",
                    "http://www.someurl.co.uk/files/file3.jpg",
                    "http://www.someurl.co.uk/files/file4.jpg"
                ]
            }
```
next add another action this time an initialize variable.
1. For the name enter a name
1. For the type enter ```Array
1. For the value enter '''"@outputs('Compose')"
1. For the runafter "Compose" and then in the array of actions "Succeed" 

You should have something like below:
```json
        "InitializeVariable": {
            "description": "",
            "inputs": {
                "variables": [
                    {
                        "name": "myVariable",
                        "type": "Array",
                        "value": "@outputs('Compose').sampleArray"
                    }
                ]
            },
            "metadata": {},
            "runAfter": {
                "Compose" : [
                    "Succeeded"
                ]
            },
            "trackedProperties": {},
            "type": "InitializeVariable"
        }
```
Run the workflow. The value uses the expression below to access the output from the named action.
```json
"@outputs('Compose').sampleArray"
```
### Expressions
Expressions result in JSON values at runtime and are prefixed with an @ symbol.

Add a foreach action set to runafter initialize variables and use the output from that as below:
```json
            "foreach": "@variables('myVariable')",
```
The variable myVariable is the variable you set above. Set its run after to be when initializeVariables succeeded.

In the actions of the for each nest a __compose__ action (call it get subsite) and for the items add the full text below:
```json
    "inputs": "@item()",
```
### Functions
[Functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#item) can be used in expressions. 

The item() function When used inside a repeating action over an array, return the current item in the array during the action's current iteration. You can also get the values from that item's properties.
```json
item().body
```

Now modify this to use string functions to find the filename only. The {} brackets are used in string interpolation and always return a string. Notice the parameters function. The output of both functions need to be treated as a string so both are wrapped in curly brackets {}. The concat() function could also have been used.

```json
"getSubsite": {
    "runAfter": {},
    "trackedProperties": {},
    "metadata": {},
    "type": "Compose",
    "inputs": "@{parameters('blobUrl')}@{substring(item(), lastIndexOf(item(), '/'), sub(length(item()), lastIndexOf(item(), '/') ))}",
    "description": ""
}
```
TIP: You can see more data operations here [Perform data operations in Azure Logic Apps](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-perform-data-operations).

__Note on strings:__
Given a variable p below:
```json
"person":{
    "age":42
}
```
@variables('p').age would return 42. @{ @variables('p').age } would return "42".

## Outputs 
Outputs are like standard ARM template outputs. The example below will output the the object (as JSON) in the run details of the run. Outputs are NOT returned to the caller. For that you would use a Response action.
```json
    "outputs": {
        "myOutPut": {
            "type": "Object",
            "value": {
                "name": "myobject",
                "somevalue": "@outputs('Compose').sampleArray[0]"
            }
        }
    }
```
## Static parameters
You can use static parameters to mock results. For example to mock results. For example for the http action below static results named "HTTP0" have been enabled as defined in the static results object further in the template. Static results have been enabled. In this case a 200 OK will be mocked in the workflow runs.

```json
 "HTTP": {
        "inputs": {
            "method": "GET",
            "uri": "https://mysite.com?id=1"
        },
        "runAfter": {
            "getSubsite": [
                "Succeeded"
            ]
        },
        "runtimeConfiguration": {
            "staticResult": {
                "name": "HTTP0",
                "staticResultOptions": "Enabled"
            }
        },
        "type": "Http"
    },
..... ommitted for clarity
,
"staticResults": {
    "HTTP0": {
        "outputs": {
            "headers": {},
            "statusCode": "OK"
        },
        "status": "Succeeded"
    }
},
```
For more info on mocking see [Test logic apps with mock data by setting up static results](https://docs.microsoft.com/en-us/azure/logic-apps/test-logic-apps-mock-data-static-results)

This was a simple overview of workflow definitions but it introduced a number of key concepts.