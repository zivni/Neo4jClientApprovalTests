using Newtonsoft.Json;

namespace Neo4jClientApprovalTests.Process
{
    internal interface IEdge
    {
        [JsonProperty("title")]
        string Data { get; }

        [JsonProperty("from")]
        long FromId { get; }

        [JsonProperty("to")]
        long ToId { get; }

        [JsonProperty("label")]
        string Type { get; }
    }
}