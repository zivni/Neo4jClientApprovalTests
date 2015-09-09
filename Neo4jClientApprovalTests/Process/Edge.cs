using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class Edge : IEquatable<Edge>, IEdge
    {
        public long EdgeId { get; set; }

        public long FromId { get; set; }

        public long ToId { get; set; }

        public string Type { get; set; }

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