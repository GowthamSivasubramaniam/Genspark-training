docker volume create mydbdata
docker run -d --name mysql-db -e MYSQL_ROOT_PASSWORD=root \
  -v mydbdata:/var/lib/mysql mysql:latest
