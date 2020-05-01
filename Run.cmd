docker stop zennoblazorserver
docker rm zennoblazorserver
docker pull alekseyaz/zennoblazorserver
docker run --name zennoblazorserver --restart=always -e "ASPNETCORE_ENVIRONMENT=Development" -d -it -p 80:80/tcp alekseyaz/zennoblazorserver