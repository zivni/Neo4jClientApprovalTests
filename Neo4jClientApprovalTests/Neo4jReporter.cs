using ApprovalTests.Core;
using Neo4jClientApprovalTests.Process;
using Neo4jClientApprovalTests.Process.DataRead;
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
        public void Report(string approved, string received)
        {
            var jsonReader = JsonGraphReaderBase.Select(received);
            var recivedGraph = jsonReader.Read(received);
            var approvedGraph = jsonReader.Read(approved);

            dynamic result = RunGraphHttpServer(recivedGraph, approvedGraph);
            if (result == true)
            {
                File.Copy(received, approved, true);
            }
        }

        private static dynamic RunGraphHttpServer(Graph recivedGraph, Graph approvedGraph)
        {
            int port = PortFinder.GetFreeIpPort();
            var httpServer = new GraphHttpServer(approvedGraph, recivedGraph, port);
            Task<dynamic> t = Task<dynamic>.Run(() => httpServer.listen());
            string url = string.Format("http://localhost:{0}/test", port);
            System.Diagnostics.Process.Start(url);
            t.Wait();
            return t.Result;
        }
    }
}