# Lab - runafter
In logic apps there are two types of flow control.
1. Conditional logic (if, while, case)
1. Errors in the run

To cope with error conditions in the execution of the logic app each action has a run after condition.
```json
"for_each_success": {
                "inputs": "success",
                "runAfter": {
                    "For_each": [
                        "Succeeded"
                    ]
                },
                "type": "Compose"
            }
```
The runAfter property takes the name of the action to run after and the conditions. This could be multiple conditions.
```json
 "for_each_fail": {
                "inputs": "fail",
                "runAfter": {
                    "For_each": [
                        "Failed",
                        "TimedOut",
                        "Skipped"
                    ]
                },
                "type": "Compose"
            },
```
By adding an action to run after the success and after any other outcome you build resilliance into your workflows.

The first compose action in this sample parses Json to create an array for the For each.

The second ParseJson within the For each parses the array items. One of the items has a missing property.

```json
 "schema": {
            "$schema": "http://json-schema.org/draft-04/schema#",
            "properties": {
                "property1": {
                    "type": "string"
                },
                "property2": {
                    "type": "string"
                }
            },
            "required": [
                "property1",
                "property2"
            ],
            "type": "object"
        }
    }
```
If the items in the array fail to parse one Action is taken and if it succeeds it takes another.

If the overall For each fails a subsequent action is taken and if the overall For each succeeds then another action is taken

```json
{
    "definition": {
        "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
        "actions": {
            "Compose": {
                "inputs": {
                    "anArray": [
                        {
                            "property1": "1",
                            "property2": "2"
                        },
                        {
                            "property1": "1"
                        },
                        {
                            "property1": "1",
                            "property2": "2"
                        }
                    ]
                },
                "runAfter": {},
                "type": "Compose"
            },
            "For_each": {
                "actions": {
                    "Parse_JSON_2": {
                        "inputs": {
                            "content": "@items('For_each')",
                            "schema": {
                                "$schema": "http://json-schema.org/draft-04/schema#",
                                "properties": {
                                    "property1": {
                                        "type": "string"
                                    },
                                    "property2": {
                                        "type": "string"
                                    }
                                },
                                "required": [
                                    "property1",
                                    "property2"
                                ],
                                "type": "object"
                            }
                        },
                        "runAfter": {},
                        "type": "ParseJson"
                    },
                    "Parse_JSON_failure": {
                        "inputs": "fail",
                        "runAfter": {
                            "Parse_JSON_2": [
                                "Failed",
                                "Skipped",
                                "TimedOut"
                            ]
                        },
                        "type": "Compose"
                    },
                    "Parse_JSON_success": {
                        "inputs": "success",
                        "runAfter": {
                            "Parse_JSON_2": [
                                "Succeeded"
                            ]
                        },
                        "type": "Compose"
                    }
                },
                "foreach": "@body('Parse_JSON')['anArray']",
                "runAfter": {
                    "Parse_JSON": [
                        "Succeeded"
                    ]
                },
                "type": "Foreach"
            },
            "Parse_JSON": {
                "inputs": {
                    "content": "@outputs('Compose')",
                    "schema": {
                        "$schema": "http://json-schema.org/draft-04/schema#",
                        "properties": {
                            "anArray": {
                                "items": [
                                    {}
                                ],
                                "type": "array"
                            }
                        },
                        "required": [
                            "anArray"
                        ],
                        "type": "object"
                    }
                },
                "runAfter": {
                    "Compose": [
                        "Succeeded"
                    ]
                },
                "type": "ParseJson"
            },
            "for_each_fail": {
                "inputs": "fail",
                "runAfter": {
                    "For_each": [
                        "Failed",
                        "TimedOut",
                        "Skipped"
                    ]
                },
                "type": "Compose"
            },
            "for_each_success": {
                "inputs": "success",
                "runAfter": {
                    "For_each": [
                        "Succeeded"
                    ]
                },
                "type": "Compose"
            }
        },
        "contentVersion": "1.0.0.0",
        "outputs": {},
        "parameters": {},
        "triggers": {
            "Recurrence": {
                "recurrence": {
                    "frequency": "Day",
                    "interval": 3
                },
                "type": "Recurrence"
            }
        }
    },
    "parameters": {}
}
```
