using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class RawToGraphElementsConverter
    {
        public Graph Convert(IEnumerable<GraphRawData> rawData)
        {
            if (rawData == null)
            {
                return null;
            }

            var v = rawData.Select(r => new Vertex
            {
                Id = r.NodeId,
                Labels = GetLabelString(r.Labels),
                Data = r.NodeDataJsonString
            })
            .Distinct()
            .ToArray();

            var nodeIds = new HashSet<long>(v.Select(vr => vr.Id));

            var e = rawData
                .Where(r => nodeIds.Contains(r.EdgeStartNodeId) && nodeIds.Contains(r.EdgeEndNodeId))
                .Select(r => new Edge
                {
                    EdgeId = r.EdgeId,
                    FromId = r.EdgeStartNodeId,
                    ToId = r.EdgeEndNodeId,
                    Type = r.EdgeType,
                    Data = r.EdgeDataJsonString
                })
                .Distinct()
                .ToArray();
            return new Graph
            {
                Vertices = v,
                Edges = e,
            };
        }

        private string GetLabelString(IEnumerable<string> labels)
        {
            return string.Join(", ", labels);
        }
    }
}