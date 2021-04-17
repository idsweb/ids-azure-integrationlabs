# Lab-WorkflowDefinitionSchema 

There is no sample for this however you can follow along with the readme in VS Code to recreate the sample. This readme highlights the main components of the workflow definition. [For more informaiton see the docs](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-author-definitions). You will need the ARM and Logic Apps extensions (not the preview extension).

The overall Logic App resource in an ARM template is a resource of type _Microsoft.Logic/workflows_. You can author and deploy Logic Apps in VS Code then add them to your ARM template. Your actual workflow is defined as a _workflow definition_. The _workflow definition_ sits within the ARM template like so:
```json
{
    "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
    },
    "variables": {},
    "resources": [
        {
            "type": "Microsoft.Logic/workflows",
            "apiVersion": "2017-07-01",
            "name": "lab_flowcontrol",
            "location": "uksouth",
            "properties": {
                "state": "Enabled",
                "definition": {
                    ...
                },
                "parameters": {}
            }
        }
    ]
}
```
## Authoring a new Logic App in VS Code
Step 1: Create a new Logic App in VS Code, entering your Subscription, Resource Group, Region and name.

_If you are not familiar with creating Logic Apps in VS Code follow this Microsoft tutorial [Quickstart: Create and manage logic app workflow definitions by using Visual Studio Code](https://docs.microsoft.com/en-us/azure/logic-apps/quickstart-create-logic-apps-visual-studio-code)_

## Basic workflow (definition) structure
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
### The contentVersion section
This can be used to version your workflows, for now leave it as is.

### Triggers
Add a recurrence trigger (press Ctrl+Spacebar and select recurrence). Change the frequency to week and leave the defaults.
```json
    "triggers": {
        "Recurrence": {
            "recurrence": {
                "frequency": "Week",
                "interval": 1,
                "startTime": "",
                "endTime": "",
                "timeZone": "Dateline Standard Time"
            },
            "metadata": {},
            "type": "Recurrence",
            "description": "recurrence trigger"
        }
    },
```

### The Parameters section
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
Save your logic app, VS Code will deploy the Logic App for you.

Parameters in your workflow can be passed in from an ARM template file. There is an example of that later.__TIP__:Always give your parameters default values so you can edit them in the portal and test run them. 

[Overview: Automate deployment for Azure Logic Apps by using Azure Resource Manager templates](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-azure-resource-manager-templates-overview).

### Actions
IN VS Code place your cursor in the actions and press Ctrl + Spacebar and type compose. Replace the inputs with the JSON below:
```json
            "inputs": {
                "filePaths": [
                    "@concat(parameters('blobUrl'),'abandonedvehicle1.jpg')",
                    "@concat(parameters('blobUrl'),'abandonedvehicle2.jpg')",
                    "@concat(parameters('blobUrl'),'abandonedvehicle3.jpg')",
                    "@concat(parameters('blobUrl'),'abandonedvehicle4.jpg')",
                    "@concat(parameters('blobUrl'),'abandonedvehicle5.jpg')",
                    "@concat(parameters('blobUrl'),'abandonedvehicle6.jpg')"
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
            "runAfter": {
                "Compose": [
                    "Succeeded"
                ]
            },
            "trackedProperties": {},
            "metadata": {},
            "type": "InitializeVariable",
            "inputs": {
                "variables": [
                    {
                        "name": "ArFilePaths",
                        "type": "Array",
                        "value": "@outputs('Compose').filePaths"
                    }
                ]
            },
            "description": "Sets up the array of file paths"
        }
```
Run the workflow. The value uses the expression below to access the output from the named action.
```json
"@outputs('Compose').filePaths"
```
### Expressions
Expressions result in JSON values at runtime and are prefixed with an @ symbol. Add a foreach action (Ctrl + Spacebar) and set the runafter to 'initialize variables' and success. Set its _foreach_ property to the variable you initialized in the previous step. In the _actions_ of the for each nest a __compose__ action and for the items add _"@item()"_. The full JSON should look like below:
```json
    "Foreach": {
        "actions": {
            "Compose": {
                "description": "",
                "inputs": "@item()",
                "metadata": {},
                "runAfter": {},
                "trackedProperties": {},
                "type": "Compose"
            }
        },
        "description": "",
        "foreach": "@variables('arFilePaths')",
        "metadata": {},
        "runAfter": {
            "IntializeVariables":[
                "Succeeded"
            ]
        },
        "trackedProperties": {},
        "type": "Foreach"
}
```
### Functions
[Functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#item) can be used in expressions. 

The item() function when used inside a repeating action over an array, return the current item in the array during the action's current iteration. 
```json
@item()
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