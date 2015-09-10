using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientApprovalTests.Process.DataRead
{
    internal abstract class JsonGraphReaderBase
    {
        private static JsonSerializer myJsonSerializer;

        static JsonGraphReaderBase()
        {
            myJsonSerializer = JsonSerializer.CreateDefault();
        }

        public static JsonGraphReaderBase Select(string filename)
        {
            char[] chars = new char[1];
            using (TextReader sr = new StreamReader(filename))
            {
                sr.Read(chars, 0, 1);
            }
            if (chars[0] == '[')
                return new RawDataJsonReader();
            return new GraphDataJsonReader();
        }

        public abstract Graph Read(string filename);

        protected T LoadGraphRawDataFromJsonfile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default(T);

            using (TextReader tr = new StreamReader(filePath))
            using (JsonReader jr = new JsonTextReader(tr))
            {
                return myJsonSerializer.Deserialize<T>(jr);
            }
        }
    }
}