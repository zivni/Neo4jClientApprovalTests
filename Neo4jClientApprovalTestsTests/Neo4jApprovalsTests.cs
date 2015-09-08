using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jClient;
using System;
using System.IO;

namespace Neo4jClientApprovalTests.Tests
{
    [TestClass()]
    [UseReporter(typeof(Neo4jReporter))]
    public class Neo4jApprovalsTests
    {
        private const string connctionString = "http://neo4j:ziv@192.168.59.103:8475/db/data";
        private static string solutiondirecory = PathUtilities.GetDirectoryForCaller();
        private static GraphClient graph;
        private string generatedApprovedFilePath;

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

        [TestCleanup]
        public void _Cleanup()
        {
            if (generatedApprovedFilePath != null && File.Exists(generatedApprovedFilePath))
                File.Delete(generatedApprovedFilePath);
        }

        [TestMethod]
        public void VerifySimpleNewGraphTest()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            Neo4jApprovals.VerifyGraph(graph);
        }

        [TestMethod]
        public void VerifySimpleIUpdatedGraphTest()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            try { Neo4jApprovals.VerifyGraph(graph); }
            catch { }
            _deleteData();

            graph.Cypher
                .Create("(a:WRD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            Neo4jApprovals.VerifyGraph(graph);
        }

        private void SetGeneratedApprovedFilePath()
        {
            generatedApprovedFilePath = solutiondirecory + Approvals.GetDefaultNamer().Name + ".approved.txt";
        }
    }
}