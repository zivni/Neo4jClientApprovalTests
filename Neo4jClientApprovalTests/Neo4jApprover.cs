using ApprovalTests;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClientApprovalTests.Process;
using ObjectApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public static class Neo4jApprover
    {
        public static void VerifyGraph(GraphClient graph)
        {
            GraphRawData[] graphData = GetFraphData(graph);
            NormalizeGraphData(graphData);
            ObjectApprover.VerifyWithJson(graphData);
        }

        public static void VerifyQuery(IEnumerable<GraphNode> nodes, IEnumerable<GraphEdge> edges, bool leaveNodeIdsUnchanged = false)
        {
            Graph graph = new Graph { Edges = edges, Vertices = nodes };
            if (!leaveNodeIdsUnchanged)
                NormlizeQueryData(graph);
            ObjectApprover.VerifyWithJson(graph);
        }

        private static void NormlizeQueryData(Graph graph)
        {
            SortedSet<long> nodes = new SortedSet<long>(graph.Vertices.Select(g => g.Id));
            long i = 0;
            foreach (long nodeId in nodes)
            {
                i++;
                var v = graph.Vertices.Cast<GraphNode>().First(n => n.Id == nodeId);
                v.Id = i;
                var e1 = graph.Edges.Cast<GraphEdge>().Where(e => e.FromId == nodeId);
                foreach (var e in e1)
                {
                    e.FromId = i;
                }

                var e2 = graph.Edges.Cast<GraphEdge>().Where(e => e.ToId == nodeId);
                foreach (var e in e2)
                {
                    e.ToId = i;
                }
            }
        }

        private static void NormalizeGraphData(GraphRawData[] graphData)
        {
            //Because on each insert the Graph DB sets new node ids,
            //then when comparing to the old version there should be the same ids

            SortedSet<long> nodes = new SortedSet<long>(graphData.Select(g => g.NodeId));
            Dictionary<long, long> edgeIds = new Dictionary<long, long>();
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

            i = 0;
            foreach (var rawData in graphData)
            {
                if (!edgeIds.ContainsKey(rawData.EdgeId))
                {
                    edgeIds.Add(rawData.EdgeId, ++i);
                }
                rawData.EdgeId = edgeIds[rawData.EdgeId];
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
            return q.Results
                .OrderBy(r => r.NodeId)
                .ThenBy(r => r.EdgeId)
                .ToArray();
        }
    }
}