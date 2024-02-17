$grp = "TestResourceGroup"
$loc = "westeurope"
$env= "findpet-env"
# 
# az group create  --name $grp --location $loc
# 
# az containerapp env create --name $env --resource-group $grp --internal-only false -- location $loc

az containerapp create `
--name authservice `
--resource-group $grp `
--environment $env `
--image findpetregistry.azurecr.io/authservice:latest `
--target-port 80 `
--ingress 'external' `
--secrets key1=123456 key2="another secret"

# az containerapp revision restart -n authservice -g $grp --revision 