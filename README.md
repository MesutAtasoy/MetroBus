# MetroBus

![Diagram](https://github.com/MesutAtasoy/MetroBus/blob/master/Diagram.png?raw=true "Metrobus")

- Data capture service listens to a specific local folder specified path in startup file and retrieve documents of some specific format (i.e., PDF) and sends the files with chunks (1MB) to handle message limit size
- Main processing service : The service creates new queue, listens the queue, stores all incoming messages in a local folder (/bin/Debug/net6.0/temp) 

### Project Setup
 
1. Clone the repository 

`git clone https://github.com/MesutAtasoy/MetroBus.git` 

2. Create Docker Network

`sudo docker network create metrobus` 

4. Run the insfrastructure containters

`sudo docker-compose up -d` 

## Configuration 

RabbitMQ is used as message queue but it can be supported another message queue system using IEventBus interface. 

```csharp
public interface IEventBus
{
    void Publish(IntegrationEvent @event);

    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}
```

### Application settings

```csharp
services.AddApplicationSettings(x =>
    {
        x.FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // listening specific folder 
        x.AllowedExtensions = new[] { ".pdf", ".jpg", ".epub" }; // allow just pdf format to transmission.
    })
```

- FolderPath : Path which will Listen in Data
- AllowedExtensions : Extensions which allow to transfer


### RabbitMQ settings

```csharp
  serviceCollection.AddRabbitMQEventBus(x =>
        {
            x.Connection = new MetroBusRabbitMQConnectionOptions
            {
                Host = "localhost",
                Password = "guest",
                Username = "guest"
            };
            x.ExchangeName = "metrobus";
            x.QueueName = "mainprocessing";
        });
```
- Host : Url of RabbitMQ
- Password : the user's password of rabbitmq
- Username : the user's username of rabbitmq
- ExchangeName : the name of exchange 
- Passowrd : the name of queue

### Output's of Console

![Diagram](https://github.com/MesutAtasoy/MetroBus/blob/master/data%20capture%20service.png "Metrobus")

![Diagram](https://github.com/MesutAtasoy/MetroBus/blob/master/main%20process.png "Metrobus")


