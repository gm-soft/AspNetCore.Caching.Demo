dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p password
dotnet dev-certs https --trust

# https://localhost:5001/
# docker-compose -f "docker-compose.yml" stop
# docker-compose -f "docker-compose.yml" rm --force
docker-compose -f "docker-compose.yml" up --build api redis