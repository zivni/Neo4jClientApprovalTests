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
        [JsonProperty("from")]
        public long SourceVertex { get; set; }

        [JsonProperty("to")]
        public long TargetVertex { get; set; }

        [JsonProperty("label")]
        public string Type { get; set; }

        public bool Equals(Edge other)
        {
            return this.SourceVertex == other.SourceVertex
                && this.TargetVertex == other.TargetVertex
                && this.Type == other.Type;
        }
    }
}