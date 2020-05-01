REM 
SETX APPDATA %AppData%
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build --force-recreate