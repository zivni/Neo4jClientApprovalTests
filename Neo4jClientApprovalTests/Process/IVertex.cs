using Newtonsoft.Json;

namespace Neo4jClientApprovalTests.Process
{
    internal interface IVertex
    {
        [JsonProperty("id")]
        long Id { get; }

        [JsonProperty("title")]
        string Popuptext { get; }

        [JsonProperty("label")]
        string Text { get; }
    }
}