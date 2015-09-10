using Neo4jClient.Cypher;
using Neo4jClientApprovalTests.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public sealed class GraphEdge : IEdge
    {
        public string Data { get; set; }

        public long FromId { get; set; }

        public long ToId { get; set; }

        public string Type { get; set; }

        public static GraphEdge FromCypherResultItem(ICypherResultItem r)
        {
            return new GraphEdge
            {
                Type = r.Type(),
                Data = r.Node<string>() != null ? r.Node<string>().Data.Replace("\r\n", "") : null,
                FromId = Return.As<long>("ID(STARTNODE(r))"),
                ToId = Return.As<long>("ID(ENDNODE(r))")
            };
        }
    }
}