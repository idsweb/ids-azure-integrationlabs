# Creating and manipulating JSON

## Sample
This little sample uses creating a license plate number to explore a few basic concepts around creating objects.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fidsweb%2Fids-azure-integrationlabs%2Fmain%2Ffundementals.logicapps%2FLab-CreatinganAndManipulatingJson%2Ftemplate.json)

Logic apps use JSON. Integration accounts have support for transformations and of course you can link out to function apps, however integration accounts have a cost attached and function apps involves calling out to compiled services. For in app manipulation you need to work with JSON.

## Workflow definition language
Logic apps are defined as resources in an ARM template using the [Schema reference guide for the Workflow Definition Language in Azure Logic Apps](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-workflow-definition-language). This defines the structure of the overall logic app. This includes the triggers and actions. In actions you can create and manipulate JSON. You can use __expressions__ in your definition.

## Expressions
In your workflow definition, you can use an expression anywhere in a JSON string value by prefixing the expression with the at-sign (@). When evaluating an expression that represents a JSON value, the expression body is extracted by removing the @ character, and always results in another JSON value. You can use {} to interpolate strings with an expression.
```json
"someproperty": "@variables('variableName').variableProperty"
```
The @variables part of the expression is a function.

## Operators
|operator|task|
|--------|----|
|'       | strings as input in expressions or functions |
|[] | array index |
|. | property |
| ? | reference null properties in an object safely |

The following expression will access the log date if that property exists. WIthout the question mark it would fail.
```json
"@outputs('Compose').logs[0]?.logdate"
```

## Functions
You use functions with the @ symbol [Reference guide to using functions in expressions for Azure Logic Apps and Power Automate](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference). Within your expression.

A list of function categories below:
1. [String functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#string-functions)
1. [Collection functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#collection-functions)
1. [Logical comparison functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#logical-comparison-functions)
1. [Conversion functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#conversion-functions)
1. [Math functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#math-functions)
1. [Date and time functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#date-and-time-functions)
1. [Workflow functions](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#workflow-functions)
1. [Xanipulation functions json xml](https://docs.microsoft.com/en-us/azure/logic-apps/workflow-definition-language-functions-reference#manipulation-functions-json--xml)

## Variables
You create variables with an action. When creating variables you can add expressions and functions that evalauate to JSON strings.

The code below uses a function to create an array:
```json
    "actions": {
        "initializeLettersArray": {
            "runAfter": {},
            "trackedProperties": {},
            "metadata": {},
            "type": "InitializeVariable",
            "inputs": {
                "variables": [
                    {
                        "name": "lettersArray",
                        "type": "Array",
                        "value": "@split('A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,R,S,T,U,V,W,X,Y', ',')"
                    }
                ]
            },
            "description": "Turns the letters into an array"
        }
```
You can combine other functions to create more complex expressions
```json
"value": "Y@{variables('lettersArray')[rand(1,23)]}@{rand(10,29)}@{variables('lettersArray')[rand(1,23)]}@{variables('lettersArray')[rand(1,23)]}@{variables('lettersArray')[rand(1,23)]}"
```

You can create variables and then set or add properties however the functions that add/set properties do not alter the original variable but instead return a copy.

You can use them as the input to declaring a new variable or alternatively use them in a compose action.

```json
        "ComposeNewVehicle": {
            "runAfter": {
                "AddProperty": [
                    "Succeeded"
                ]
            },
            "trackedProperties": {},
            "metadata": {},
            "type": "Compose",
            "inputs": {
                "newVehicle": {
                    "reg": "@variables('plateNumber')",
                    "make": "vaxhaul"
                }
            },
            "description": "Creates a new vehicle, the expressions are used to build the new object"
        }
```
The sample in this folder shows a few examples of building objects using expressions/functions.

Great link [Transforming JSON Objects in Logic Apps](https://platform.deloitte.com.au/articles/transforming-json-objects-in-logic-apps)

Nothing to do with logic apps but an explanation of license plates :) [https://www.thecarexpert.co.uk/how-does-the-uk-number-plate-system-work/](https://www.thecarexpert.co.uk/how-does-the-uk-number-plate-system-work/)