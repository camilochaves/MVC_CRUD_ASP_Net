docker-compose -f docker-compose.yml -f docker-compose-cache.yml -f docker-compose-rabbitmq.yml up -d Cache
dotnet test Tests