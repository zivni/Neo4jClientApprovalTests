using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class Edge : IEquatable<Edge>
    {
        public long EdgeId { get; set; }

        [JsonProperty("from")]
        public long SourceVertex { get; set; }

        [JsonProperty("to")]
        public long TargetVertex { get; set; }

        [JsonProperty("label")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Data { get; set; }

        public bool Equals(Edge other)
        {
            return this.EdgeId == other.EdgeId;
        }

        public override int GetHashCode()
        {
            return this.EdgeId.GetHashCode();
        }
    }
}