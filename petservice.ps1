$grp = "TestResourceGroup"
$loc = "westeurope"

az group create  --name $grp --location $loc

az deployment group create --resource-group $grp --template-file 'petservice.json'
