using ApprovalTests;
using Neo4jClient;
using Neo4jClient.Cypher;
using ObjectApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public static class Neo4jApprovals
    {
        public static void VerifyGraph(GraphClient graph)
        {
            GraphRawData[] graphData = GetFraphData(graph);
            ObjectApprover.VerifyWithJson(graphData);
        }

        private static GraphRawData[] GetFraphData(GraphClient graph)
        {
            var q = graph.Cypher.Match("(n)")
               .OptionalMatch("(n)-[r]->()")
               .Return((n, r) => new GraphRawData
               {
                   NodeDataJson = n.Node<string>(),
                   Labels = n.Labels(),
                   NodeId = n.Id(),
                   EdgeType = r.Type(),
                   EdgeDataJson = r.Node<string>(),
                   EdgeStartNodeId = Return.As<long>("ID(STARTNODE(r))"),
                   EdgeEndNodeId = Return.As<long>("ID(ENDNODE(r))")
               })
               .Results;
            return q.ToArray();
        }
    }
}