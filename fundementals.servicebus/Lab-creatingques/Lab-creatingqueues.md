# Creating queues with ARM
To deploy the ARM template open PowerShell ISE and connect to Azure and run


```PowerShell
New-AzResourceGroupDeployment -ResourceGroupName rg-logicapps-msdn -TemplateFile azuredeploy.servicebusqueues.json