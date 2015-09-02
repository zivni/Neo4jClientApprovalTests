using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public class GraphRawData
    {
        public string NodeDataJsonString { get; set; }
        public long NodeId { get; set; }
        public IEnumerable<string> Labels { get; set; }
        public string EdgeType { get; set; }
        public string EdgeDataJsonString { get; set; }
        public long EdgeStartNodeId { get; set; }
        public long EdgeEndNodeId { get; set; }

        public Neo4jClient.Node<string> NodeDataJson
        {
            set
            {
                if (value != null)
                    this.NodeDataJsonString = value.Data.Replace("\r\n", " ");
            }
        }

        public Neo4jClient.Node<string> EdgeDataJson
        {
            set
            {
                if (value != null)
                    this.EdgeDataJsonString = value.Data.Replace("\r\n", " ");
            }
        }
    }
}