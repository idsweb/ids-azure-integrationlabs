# ids Azure Integration labs
You often need to refer back to notes while covering a wide range of topics. Concepts are well documented however it is when you get down to solving a particular problem you get to find the 'features' of a product set. 

This repo is here primarily for my own benefit but I hope others find it useful. It is a work in progress and comes with no warranties. 

I use a quick isolated Lab folder to look at a particular issue. Some of these contain a deployable ARM template or a console application you can run. The folders are based around:
1. Logic apps
1. Azure service bus
1. Azure functions

I plan to add further folders as I progress.

_There is a loose theme through some of these notes around reporting - in this case reporting an abandoned vehicle. This is a not uncommon type of scenario when dealing with online forms._

## Logic apps
The fudementals.logicapps folder has some logic apps demos. 
The data folder in fundementals.logicapps has sample images and JSON messages that can be uploaded to Azure Service Bus using the explorer.

__You can deploy these to your own subscription using the Deploy to Azure button in the repo.__

### Lab-CreatinganAndManipulatingJson

Without liquid transforms or XSLT (which both need integration accounts) you need to manipulate JSON.

This little sample logic app shows some basic techniques for manipuating json using and a Compose action and functions and expressions. It uses a rather contrived example of construction an array of license plate numbers (you could use this as part of test data generation).
[Creating and manipulating JSON](https://github.com/idsweb/ids-azure-integrationlabs/blob/main/fundementals.logicapps/Lab-CreatinganAndManipulatingJson/Lab-CreatingAndManipualtingJson.md)

### Lab-flowcontrol
This sample shows a simple flow control with a for loop and a switch statement to take an action based on a property value [Lab-flowcontrol](https://github.com/idsweb/ids-azure-integrationlabs/blob/main/fundementals.logicapps/Lab-flowcontrol/Lab-flowcontrol.md).

### Lab-httptrigger 
This shows how to use a http trigger, set and receive headers and set different return types. [Lab-httptrigger](https://github.com/idsweb/ids-azure-integrationlabs/tree/main/fundementals.logicapps/Lab-httptrigger)
