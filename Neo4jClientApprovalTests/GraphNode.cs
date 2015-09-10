using Neo4jClient.Cypher;
using Neo4jClientApprovalTests.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public sealed class GraphNode : IVertex, IEquatable<GraphNode>
    {
        public long Id { get; set; }

        public string Popuptext { get; set; }

        public string Text { get; set; }

        public bool Equals(GraphNode other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}