# Lab flow control
Logic apps has Flow actions to control flow in the logic app. This sample uses iteration, condition and switch style flow control.

The compose action contains an array of messages. The Json contains a contact property with an email and or phone number. There is also a preferred contact. The receive email summary boolean decides if a receipt email is to be sent.

```json
{
  "messages": [
    {
     ...
        },
        "contact": {
          "email": "someguy@googlemail.com",
          "phoneNumber": "",
          "preferredContact": "email"
        },
        ...
        },
        "requireReceipt": true
      },
      "receiveEmailSummary": true
    }
  ]
}
```
The flow control is as follows:

1. Iterate over the messages.
1. If the receipt is required teh contact method is evaluated
1. A switch statement is used to send the right receipt (not implemented).

```json
{
    "definition": {
        "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
        "actions": {
            "CreateJsonArray": {
                "description": "Creates some test data for the demo",
                "inputs": {
                    "messages": [
                        {
                            "id": "53aa43cc-4472-4af2-8a4b-46bfa76b6c06",
                            "personalDetails": {
                                "address": {
                                    "address": "13 Some Street",
                                    "postCode": "LS5 3PQ"
                                },
                                "contact": {
                                    "email": "someguy@googlemail.com",
                                    "phoneNumber": "",
                                    "preferredContact": "email"
                                },
                                "name": {
                                    "firstName": "Hue",
                                    "lastName": "Jass"
                                },
                                "requireReceipt": true
                            },
                            "receiveEmailSummary": true
                        },
                        {
                            "id": "99cc43cc-4472-4af2-8a4b-46bfa76b6c06",
                            "personalDetails": {
                                "address": {
                                    "address": "14 Some Street",
                                    "postCode": "LS12 3XF"
                                },
                                "contact": {
                                    "email": "someguy@googlemail.com",
                                    "phoneNumber": "123455678",
                                    "preferredContact": "phone"
                                },
                                "name": {
                                    "firstName": "George",
                                    "lastName": "Washington"
                                },
                                "requireReceipt": true
                            },
                            "receiveEmailSummary": true
                        },
                        {
                            "id": "99cc43cc-4472-4af2-8a4b-46bfa76b6c06",
                            "personalDetails": {
                                "address": {
                                    "address": "33 Some Street",
                                    "postCode": "LS5 3XS"
                                },
                                "contact": {
                                    "email": "someotherguy@googlemail.com",
                                    "phoneNumber": "077777777777",
                                    "preferredContact": "email"
                                },
                                "name": {
                                    "firstName": "Ben",
                                    "lastName": "Stiller"
                                },
                                "requireReceipt": false
                            },
                            "receiveEmailSummary": true
                        }
                    ]
                },
                "metadata": {},
                "runAfter": {},
                "trackedProperties": {},
                "type": "Compose"
            },
            "IterateThroughMessages": {
                "actions": {
                    "receiptCondition": {
                        "actions": {
                            "Switch": {
                                "cases": {
                                    "Case": {
                                        "actions": {},
                                        "case": "email"
                                    },
                                    "Case_2": {
                                        "actions": {},
                                        "case": "phone"
                                    }
                                },
                                "default": {
                                    "actions": {}
                                },
                                "expression": "@items('IterateThroughMessages').personalDetails.contact.preferredContact",
                                "runAfter": {},
                                "type": "Switch"
                            }
                        },
                        "expression": {
                            "and": [
                                {
                                    "equals": [
                                        "@items('IterateThroughMessages').personalDetails.requireReceipt",
                                        false
                                    ]
                                }
                            ]
                        },
                        "runAfter": {},
                        "type": "If"
                    }
                },
                "foreach": "@outputs('CreateJsonArray').messages",
                "metadata": {},
                "runAfter": {
                    "CreateJsonArray": [
                        "Succeeded"
                    ]
                },
                "trackedProperties": {},
                "type": "Foreach"
            }
        },
        "contentVersion": "1.0.0.0",
        "outputs": {},
        "parameters": {},
        "triggers": {
            "Recurrence": {
                "metadata": {},
                "recurrence": {
                    "endTime": "",
                    "frequency": "Day",
                    "interval": 3,
                    "timeZone": "Dateline Standard Time"
                },
                "type": "Recurrence"
            }
        }
    },
    "parameters": {}
}
```