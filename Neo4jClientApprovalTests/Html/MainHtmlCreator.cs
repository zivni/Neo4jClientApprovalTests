using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Html
{
    internal class MainHtmlCreator
    {
        public static string Create()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang = 'en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset = 'utf -8'/>");
            sb.AppendLine("");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<iframe src='/approved' style='overflow:hidden; width:45%; height:500px;'></iframe>");
            sb.AppendLine("<iframe src='/received' style='overflow:hidden; width:45%; height:500px;'></iframe>");
            sb.AppendLine("<script>");
            sb.AppendLine("$( window ).unload(function() {document.createElement('img').src='/end'; });");
            sb.AppendLine("</script>");
            sb.AppendLine("</body>");
            return sb.ToString();
        }
    }
}