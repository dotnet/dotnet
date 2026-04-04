# Run Examples.ConfluentKafka

Start the Confluent Kafka stack:

```cmd
docker run -d --name kafka -p 9092:9092 confluentinc/confluent-local
```

Start the Aspire Dashboard:

```cmd
docker run --rm -it -p 18888:18888 -p 4317:18889 -d --name aspire-dashboard mcr.microsoft.com/dotnet/nightly/aspire-dashboard:8.0.0
```
