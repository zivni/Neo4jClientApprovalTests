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
        private readonly Graph approvedGraph;
        private readonly Graph recivedGraph;

        public GraphHttpServer(Graph approvedGraph, Graph recivedGraph, int port) : base(port)
        {
            this.approvedGraph = approvedGraph;
            this.recivedGraph = recivedGraph;
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            result = false;
            string html;
            switch (p.http_url)
            {
                case "/approve":
                    result = true;
                    p.writeSuccess();
                    is_active = false;
                    return;

                case "/end":
                    p.writeSuccess();
                    is_active = false;
                    return;

                case "/favicon.ico":
                    html = null;
                    break;

                default:
                    html = MainHtmlCreator.Create(approvedGraph, recivedGraph);
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