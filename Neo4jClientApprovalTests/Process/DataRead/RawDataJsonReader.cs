using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process.DataRead
{
    internal class RawDataJsonReader : JsonGraphReaderBase
    {
        private static RawToGraphElementsConverter graphConverter = new RawToGraphElementsConverter();

        public override Graph Read(string filename)
        {
            var rawData = LoadGraphRawDataFromJsonfile<IEnumerable<GraphRawData>>(filename);
            if (rawData == null)
                rawData = new GraphRawData[0];

            return graphConverter.Convert(rawData);
        }
    }
}