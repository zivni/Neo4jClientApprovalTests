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
            NormalizeGraphData(graphData);
            ObjectApprover.VerifyWithJson(graphData);
        }

        private static void NormalizeGraphData(GraphRawData[] graphData)
        {
            //Because on each insert the Graph DB sets new node ids,
            //then when comparing to the old version there should be the same ids

            SortedSet<long> nodes = new SortedSet<long>(graphData.Select(g => g.NodeId));
            long i = 0;
            foreach (long nodeId in nodes)
            {
                i++;
                foreach (var rawData in graphData)
                {
                    if (rawData.NodeId == nodeId)
                        rawData.NodeId = i;
                    if (rawData.EdgeStartNodeId == nodeId)
                        rawData.EdgeStartNodeId = i;
                    if (rawData.EdgeEndNodeId == nodeId)
                        rawData.EdgeEndNodeId = i;
                }
            }
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
                   EdgeId = r.Id(),
                   EdgeType = r.Type(),
                   EdgeDataJson = r.Node<string>(),
                   EdgeStartNodeId = Return.As<long>("ID(STARTNODE(r))"),
                   EdgeEndNodeId = Return.As<long>("ID(ENDNODE(r))")
               });
            return q.Results.ToArray();
        }
    }
}