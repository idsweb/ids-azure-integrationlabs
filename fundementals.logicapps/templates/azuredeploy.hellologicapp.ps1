
#Connect-AzAccount -Tenant "72325231-6924-49fe-9cb3-124d5649df9a"
#Set-AzContext -Subscription "59c33456-2122-4779-82e0-b3022b7ed84b"

$rgname = "rg-logicapps-msdn"
$rg = Get-AzResourceGroup -Name $rgname

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$templatePath = "$scriptPath\azuredeploy.hellologicapp.json"
$templateParameterPath = "$scriptPath\azuredeploy.hellologicapp.parameters.json"

New-AzResourceGroupDeployment `
-Name sampleDeployment `
-ResourceGroupName $rg.ResourceGroupName `
-TemplateFile $templatePath `
-TemplateParameterFile $templateParameterPath