docker build -t findpetregistry.azurecr.io/apigateway:latest -f .\ApiGateway\Dockerfile .

docker build -t findpetregistry.azurecr.io/petservice:latest -f .\PetService.API\Dockerfile .
 docker build -t findpetregistry.azurecr.io/authservice:latest -f .\Identity\Dockerfile .



