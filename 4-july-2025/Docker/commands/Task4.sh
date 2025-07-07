docker network create appnet

docker run -d --name backend --network appnet my-backend-image
docker run -d --name frontend --network appnet -e API_URL=http://backend:5000 my-frontend-image
