# Working with blobs
## json blobs
You can easily grab json from the contents of a blob. Add a get blob content action then add an initialize variable action and set the input as below:
```json
{
    "inputs": {
        "variables": [
            {
                "name": "somejson",
                "type": "object",
                "value": "@json(body('Get_blob_content'))"
            }
        ]
    }
}
```


