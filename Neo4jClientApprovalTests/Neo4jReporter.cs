using ApprovalTests.Core;
using Neo4jClientApprovalTests.Process;
using Neo4jClientApprovalTests.WebServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests
{
    public class Neo4jReporter : IApprovalFailureReporter
    {
        private static JsonSerializer myJsonSerializer;
        private static RawToGraphElementsConverter graphConverter = new RawToGraphElementsConverter();

        static Neo4jReporter()
        {
            myJsonSerializer = JsonSerializer.CreateDefault();
        }

        public void Report(string approved, string received)
        {
            var approvedRawData = LoadGraphRawDataFromJsonfile(approved);
            var receivedRawDate = LoadGraphRawDataFromJsonfile(received);

            var recivedGraph = graphConverter.Convert(receivedRawDate);

            int port = PortFinder.GetFreeIpPort();
            var httpServer = new GraphHttpServer(recivedGraph, port);
            Task t = Task.Run(() => httpServer.listen());
            string url = string.Format("http://localhost:{0}/test", port);
            System.Diagnostics.Process.Start(url);
            t.Wait();
        }

        private IEnumerable<GraphRawData> LoadGraphRawDataFromJsonfile(string filePath)
        {
            using (TextReader tr = new StreamReader(filePath))
            using (JsonReader jr = new JsonTextReader(tr))
            {
                return myJsonSerializer.Deserialize<IEnumerable<GraphRawData>>(jr);
            }
        }
    }
}