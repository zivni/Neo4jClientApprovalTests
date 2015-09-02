using Neo4jClientApprovalTests.Html;
using Neo4jClientApprovalTests.Process;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.WebServer
{
    internal class GraphHttpServer : HttpServer
    {
        private readonly Graph graph;

        public GraphHttpServer(Graph graph, int port) : base(port)
        {
            this.graph = graph;
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            string html;
            switch (p.http_url)
            {
                case "/end":
                    p.writeSuccess();
                    is_active = false;
                    return;

                case "/approved":
                case "/received":
                    html = HtmlCreator.Create(graph);
                    break;

                default:
                    html = MainHtmlCreator.Create();
                    break;
            }

            p.writeSuccess();
            p.outputStream.Write(html);
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            throw new NotImplementedException();
        }
    }
}