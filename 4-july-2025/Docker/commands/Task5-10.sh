docker swarm init
docker service create --name nginx-web --replicas 3 -p 8080:80 nginx

docker service update --image nginx:alpine nginx-web
docker service scale nginx-web=5

docker service create --name webapp --replicas 3 --update-delay 10s httpd
docker service update --image httpd:alpine webapp

docker service create \
  --name viz \
  --publish 8081:8080 \
  --constraint node.role==manager \
  --mount type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
  dockersamples/visualizer
