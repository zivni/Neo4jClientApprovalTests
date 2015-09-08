using Neo4jClientApprovalTests.Process;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Html
{
    internal class MainHtmlCreator
    {
        public static string Create(Graph approved, Graph recived)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang = 'en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset = 'utf -8'/>");
            sb.AppendLine("<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/vis/4.8.0/vis.min.css'>");
            sb.AppendLine("<script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/vis/4.8.0/vis.min.js'></script>");
            sb.AppendLine("<script src='https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js'></script>");
            sb.AppendLine("");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div style='margin-bottom: 10px;'>");
            sb.AppendLine("<button id='approve_button' style='width: 100px; margin - right: 5px;'>Approve</button>");
            sb.AppendLine("<button id='reject_button'  style='width: 100px;'>Reject</button>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div style='display: table; width: 100%;position: absolute;'>");
            sb.AppendLine("<div style='display: table-cell; width: 50%; padding: 5px; '>Recived</div>");
            sb.AppendLine("<div style='display: table-cell; width: 50%; padding: 5px; '>Approved</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div id='graph_container' style='display: table; width: 100%;height: 100%;position: absolute;'>");
            sb.AppendLine("<div id='recived' style='display: table-cell; width: 50%; border: 1px solid; '></div>");
            sb.AppendLine("<div id='approved' style='display: table-cell; width: 50%; border: 1px solid;'></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<script>");
            sb.AppendLine("$(window).unload(function() {document.createElement('img').src='/end'; });");
            sb.AppendFormat("var data_recived = {0}\r\n", GetGraphJson(recived));
            sb.AppendLine("var graph_recived = new vis.Network(document.getElementById('recived'), data_recived, {});");
            sb.AppendFormat("var data_approved = {0}\r\n", GetGraphJson(approved));
            sb.AppendLine("var op={layout:{randomSeed:graph_recived.getSeed()}, edges:{arrows: 'to'}};");
            sb.AppendLine("var graph_approved = new vis.Network(document.getElementById('approved'), data_approved, op);");
            sb.AppendLine("graph_recived.setSize($('#recived').width(),$('#recived').height());");
            sb.AppendLine("graph_approved.setSize($('#approved').width(),$('#approved').height());");
            sb.AppendLine("$('#reject_button').click(function(){$.ajax('/end').done(function(){window.close();});});");
            sb.AppendLine("$('#approve_button').click(function(){$.ajax('/approve').done(function(){window.close();});});");
            sb.AppendLine("</script>");
            sb.AppendLine("</body>");
            return sb.ToString();
        }

        private static string GetGraphJson(Graph graph)
        {
            if (graph == null)
            {
                return @"{'nodes': [], 'edges': []};";
            }

            string json = JsonConvert.SerializeObject(graph);
            return json;
        }
    }
}