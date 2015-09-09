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
        public Graph()
        {
        }

        [JsonConstructor]
        public Graph(IEnumerable<GraphNode> nodes, IEnumerable<GraphEdge> edges)
        {
            this.Vertices = nodes;
            this.Edges = edges;
        }

        [JsonProperty("nodes")]
        public IEnumerable<IVertex> Vertices { get; set; }

        [JsonProperty("edges")]
        public IEnumerable<IEdge> Edges { get; set; }
    }
}