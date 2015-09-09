using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process
{
    internal class Vertex : IEquatable<Vertex>, IVertex
    {
        public long Id { get; set; }

        public string Labels { get; set; }

        public string Data { get; set; }
        public string firstLabel { get { return string.Join(", ", Labels); } }

        public string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(Data))
                {
                    var jObj = JObject.Parse(Data);
                    if (jObj.First is JProperty)
                        return ((JProperty)jObj.First).Value.ToString();
                }
                if (!string.IsNullOrEmpty(firstLabel))
                    return firstLabel;
                return Id.ToString();
            }
        }

        public string Popuptext
        {
            get
            {
                string seperator = "";
                if (!string.IsNullOrEmpty(Labels) && !string.IsNullOrEmpty(Data))
                    seperator = ": ";
                return string.Format("{0}{1}{2}", Labels, seperator, Data);
            }
        }

        public bool Equals(Vertex other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}