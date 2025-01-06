# StayScanner

## StayScanner API

StayScanner is a .NET-based API project designed to interact with a variety of services, including PostgreSQL, RabbitMQ, Elasticsearch, Kibana, and Logstash. It allows users to interact with hotel data, reports, and related functionalities.

## Requirements

Before running the project, make sure you have the following tools installed:

- Docker
- Docker Compose

## Setup

### Clone the repository:

```bash
git clone https://github.com/cihanelibol/StayScanner.git
cd stayscanner
```

Run the application:
To run the entire project with Docker Compose, execute the following command:

```bash
docker-compose up --build
```

### Accessing the services:

- The swagger documentation available at [http://localhost:7146/swagger](http://localhost:7146/swagger).
- The API will be available at [http://localhost:7146](http://localhost:7146).
- RabbitMQ Management UI can be accessed at [http://localhost:15672](http://localhost:15672) with the default credentials (`guest`/`guest`).
- PostgreSQL is available on port `5432`.
- Elasticsearch can be accessed at [http://localhost:9200](http://localhost:9200).
- Kibana's UI can be accessed at [http://localhost:5601](http://localhost:5601).
- Logstash can be accessed at [http://localhost:9600](http://localhost:9600).


## Docker Compose Configuration

This project uses Docker Compose to orchestrate the following services:

- **stayscanner.api**: The main .NET API service.
- **rabbitmq**: Message queue for asynchronous communication.
- **postgres**: PostgreSQL database for storing hotel and report data.
- **elasticsearch**: For searching and analyzing data.
- **kibana**: UI for interacting with Elasticsearch data.
- **logstash**: Log ingestion and processing service.

