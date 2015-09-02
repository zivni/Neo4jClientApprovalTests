using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jClient;
using System;

namespace Neo4jClientApprovalTests.Tests
{
    [TestClass()]
    [UseReporter(typeof(Neo4jReporter))]
    public class Neo4jApprovalsTests
    {
        private const string connctionString = "http://neo4j:ziv@192.168.59.103:8475/db/data";
        private static GraphClient graph;

        [ClassInitialize]
        public static void _allTestsInit(TestContext context)
        {
            graph = new GraphClient(new Uri("http://neo4j:ziv@192.168.59.103:8475/db/data"));
            graph.Connect();
        }

        [TestInitialize]
        public void _deleteData()
        {
            graph.Cypher
                .Match("(n)")
                .OptionalMatch("(n)-[r]-()")
                .Delete("n, r")
                .ExecuteWithoutResults();
        }

        [TestMethod()]
        public void VerifyGraphTest()
        {
            graph.Cypher
                .Create("(:WORD {name:'Hello'})-[:NEXT {times: 5}]->(:WORD {name:'world'})")
                .ExecuteWithoutResults();

            Neo4jApprovals.VerifyGraph(graph);
        }
    }
}