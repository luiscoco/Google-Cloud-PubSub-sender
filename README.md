# GoogleCloud Pub/Sub with dotNET8 WebAPI producer

## 1. Create Google Cloud Pub/Sub service

**Google Cloud Project**: You'll need a Google Cloud Project, if you don't have one, create one at https://console.cloud.google.com

**Pub/Sub API Enabled**: Make sure the Pub/Sub API is enabled in your project. You can check this or enable it in the "APIs & Services" section of the Google Cloud Console

We first have to log in to Google Cloud Service 

![image](https://github.com/luiscoco/GoogleCloud_Pub_Sub_with_dotNET8_WebAPI_producer/assets/32194879/434e7c37-a5af-46f3-9039-333c7155da53)

We have to search for Pub/Sub service 

![image](https://github.com/luiscoco/GoogleCloud_Pub_Sub_with_dotNET8_WebAPI_producer/assets/32194879/bbc6178e-f2d5-4323-9fd7-3c84800d5c46)

### 1.1. Create a Topic

We press **CREATE TOPIC** button 

![image](https://github.com/luiscoco/GoogleCloud_Pub_Sub_with_dotNET8_WebAPI_producer/assets/32194879/1bda8bf5-9b43-453c-97f6-17dc858c1f39)

We input the topic name

![image](https://github.com/luiscoco/GoogleCloud_Pub_Sub_with_dotNET8_WebAPI_producer/assets/32194879/f22cf987-2885-45a5-b6f3-050cf24eef42)



### 1.2. Create a Service Account



### 1.3. Set Environment Variable

We run the application to edit and set the environmental variables



## 2. Create a .NET8 WebAPI with VSCode

Creating a .NET 8 Web API using Visual Studio Code (VSCode) and the .NET CLI is a straightforward process

This guide assumes you have .NET 8 SDK, VSCode, and the C# extension for VSCode installed. If not, you'll need to install these first

**Step 1**: Install .NET 8 SDK

Ensure you have the .NET 8 SDK installed on your machine: https://dotnet.microsoft.com/es-es/download/dotnet/8.0

You can check your installed .NET versions by opening a terminal and running:

```
dotnet --list-sdks
```

If you don't have .NET 8 SDK installed, download and install it from the official .NET download page

**Step 2**: Create a New Web API Project

Open a terminal or command prompt

Navigate to the directory where you want to create your new project

Run the following command to create a new Web API project:

```
dotnet new webapi -n GooglePubSubSenderApi
```

This command creates a new directory with the project name, sets up a basic Web API project structure, and restores any necessary packages

**Step 3**: Open the Project in VSCode

Once the project is created, you can open it in VSCode by navigating into the project directory and running:

```
code .
```

This command opens VSCode in the current directory, where . represents the current directory

## 3. Load project dependencies



## 4. Create the project structure



## 5. Create the Controller

```csharp
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Mvc;

namespace PubSubSenderApi.Controllers
{
    public class MessageDto
    {
        public string? Body { get; set; }
        public string? Priority { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PubSubController : ControllerBase
    {
        private static string projectId = "XXXXXXXXXX"; // Replace with your Google Cloud project ID
        private static string topicId = "mytopic"; // Replace with your topic ID

        private static PublisherServiceApiClient publisher;

        static PubSubController()
        {
            publisher = PublisherServiceApiClient.Create();
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            TopicName topicName = new TopicName(projectId, topicId);

            PubsubMessage pubsubMessage = new PubsubMessage
            {
                Data = Google.Protobuf.ByteString.CopyFromUtf8(messageDto.Body),
                Attributes =
                {
                    { "priority", messageDto.Priority }
                }
            };

            await publisher.PublishAsync(topicName, new[] { pubsubMessage });

            return Ok($"Sent message: {messageDto.Body}, Priority: {messageDto.Priority}");
        }
    }
}
```

## 6. Modify the application middleware(program.cs)

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceBusSenderApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceBusSenderApi v1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
```


## 7. Run and Test the application


