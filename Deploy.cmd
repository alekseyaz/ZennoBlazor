 
dotnet dev-certs https -ep %APPDATA%\ASP.NET\https\ZennoBlazor.Server.pfx -p crypticpassword
dotnet dev-certs https --trust
dotnet user-secrets -p ZennoBlazor\Server\ZennoBlazor.Server.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword"
SETX APPDATA %AppData%
docker-compose -f docker-compose.yml -f docker-compose.override.dev.yml up --build --force-recreate
REM docker-compose -f docker-compose.yml -f docker-compose.override.prod.yml up --build --force-recreate