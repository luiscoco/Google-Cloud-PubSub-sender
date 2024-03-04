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
        private static string projectId = "endless-set-412215"; // Replace with your Google Cloud project ID
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
