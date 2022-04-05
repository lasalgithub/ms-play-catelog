# Play Catalog

### Build docker image
```s
VERSION=1.0.1
export GH_OWNER="playhuborg"
export GH_PAT="[GithubPersonalToken]"
docker build --no-cache --progress=plain --secret id=GH_OWNER --secret id=GH_PAT   -t play.catalog:$VERSION .
```

### Run application locally

```s
export $AUTHORITY="http://identity:5002"
docker run -it --rm -p 5000:5000 --name catalog -e ServiceSettings__Authority=$AUTHORITY -e ServiceSettings__IdentityRequiredHttps=false  -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq --network playinfra_default play.catalog:$VERSION
```


