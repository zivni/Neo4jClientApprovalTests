using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class Vertex : IEquatable<Vertex>
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("caption")]
        public string Labels { get; set; }

        public string Data { get; set; }

        public bool Equals(Vertex other)
        {
            return this.Id == other.Id;
        }
    }
}