# Working with Key Vault
Logic apps has an action for retreiving a Key Vault secret however this requires a client secret and id.

However by using the REST api for key vault and a HTTP action you can make use of a Managed Identity. It is a little more work but has the advantage of not needed to store secrets.

```json
{
    "definition": {
        "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
        "actions": {
            "HTTP": {
                "inputs": {
                    "authentication": {
                        "audience": "https://vault.azure.net",
                        "type": "ManagedServiceIdentity"
                    },
                    "headers": {
                        "Content-Type": "application/json"
                    },
                    "method": "GET",
                    "queries": {
                        "api-version": "7.1"
                    },
                    "uri": "@{concat(parameters('keyvaultbaseurl'),'supersecret/')}"
                },
                "runAfter": {},
                "type": "Http"
            }
        },
        "contentVersion": "1.0.0.0",
        "outputs": {},
        "parameters": {
            "keyvaultbaseurl": {
                "defaultValue": "https://kvidslogicapps001.vault.azure.net/secrets/",
                "type": "String"
            }
        },
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
