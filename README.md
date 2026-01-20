This branch is for usage inside a kubernetes cluster, but right now has an error with kafka not creating a consumer_offset topic, making consuming unavailable. 
This is fixed by running the following command when every deployment file has been applied to the cluster:
```
kubectl exec -it kafka-deployment-0 -- bash -c 'KAFKA_HOME=/opt/kafka; $KAFKA_HOME/bin/kafka-topics.sh --bootstrap-server kafka-service:9092 --create --topic __consumer_offsets --partitions 50 --replication-factor 1 --config cleanup.policy=compact'
```

# Messaging-code-test
This repository represents a coding test. The goal of the test is to run a producer, consumer, and message broker, and then make them work together.

The code is written as a microservice, which is run by Docker

## How to run the code
In the terminal at the code path with docker running: `docker compose up`
This runs 6 services in total:
- A .NET producer app
- A .NET consumer app
- An Apache Kafka broker
- 2 containers for a PostgreSQL database
- A Kafka UI for overview of the kafka service

### Modularity
The producer sends messages in set intervals. This interval can be changed through the 'docker-compose.yml' file.

In 'services' adjust the environment variable called 'TIMER' at the producer service, which looks like:
```
  producer:
    image: ${DOCKER_REGISTRY-}producer
    build:
      context: .
      dockerfile: Producer/Dockerfile
    depends_on:
      kafka:
        condition: service_started
    environment:
      TIMER: 1 <------ Adjust this variable to modify the timeout of the producer in seconds
```
