using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process.DataRead
{
    internal class GraphDataJsonReader : JsonGraphReaderBase
    {
        public override Graph Read(string filename)
        {
            return LoadGraphRawDataFromJsonfile<Graph>(filename);
        }
    }
}