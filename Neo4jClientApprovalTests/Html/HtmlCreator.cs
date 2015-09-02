﻿using Neo4jClientApprovalTests.Process;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Html
{
    internal class HtmlCreator
    {
        public static string Create(Graph graph)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang = 'en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset = 'utf -8'/>");
            sb.AppendLine("<link rel='stylesheet' href='http://cdn.graphalchemist.com/alchemy.min.css'>");
            sb.AppendLine("<script type='text/javascript' src='http://cdn.graphalchemist.com/alchemy.min.js'></script>");
            sb.AppendLine("<script src='https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js'></script>");
            sb.AppendLine("");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div id='alchemy' class='alchemy'></div>");
            sb.AppendLine("<script>");
            sb.AppendFormat("var some_data = {0}\r\n", GetGraphJson(graph));
            sb.AppendLine("alchemy.begin({'dataSource': some_data})");
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