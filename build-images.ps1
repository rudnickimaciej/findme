docker build -t apigateway -f .\ApiGateway\Dockerfile .

docker build -t petservice -f .\PetService.API\Dockerfile .

docker build -t authservice -f .\Identity\Dockerfile .



