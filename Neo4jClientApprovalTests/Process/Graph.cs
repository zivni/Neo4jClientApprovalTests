using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class Graph
    {
        [JsonProperty("nodes")]
        public IEnumerable<Vertex> Vertices { get; set; }

        [JsonProperty("edges")]
        public IEnumerable<Edge> Edges { get; set; }
    }
}