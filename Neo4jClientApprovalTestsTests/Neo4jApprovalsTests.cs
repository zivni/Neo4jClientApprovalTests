using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.IO;
using System.Linq;

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

            Neo4jApprover.VerifyGraph(graph);
        }

        [TestMethod]
        public void VerifyGraphWithoutDataTest()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a)-[:NEXT]->(b), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            Neo4jApprover.VerifyGraph(graph);
        }

        [TestMethod]
        public void VerifySimpleIUpdatedGraphTest()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            try { Neo4jApprover.VerifyGraph(graph); }
            catch { }
            _deleteData();

            graph.Cypher
                .Create("(a:WRD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a), (a)-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            Neo4jApprover.VerifyGraph(graph);
        }

        [TestMethod]
        public void VeryfiySimpleQuery()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NEXT]->(b)")
                .ExecuteWithoutResults();

            Validate3partQuery("(a)-[rl]->(b)");
        }

        [TestMethod]
        public void VeryfiySimpleQueryUpdate()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NXT]->(b)")
                .ExecuteWithoutResults();

            try { Validate3partQuery("(a)-[rl]->(b)"); }
            catch { }

            Validate3partQuery("(a)-[rl:NXT]->(b)");
        }

        [TestMethod]
        public void VeryfiySimpleQueryUpdateWithIdRenumberingOff()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NXT]->(b)")
                .ExecuteWithoutResults();

            try { Validate3partQuery("(a)-[rl]->(b)"); }
            catch { }

            _deleteData();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NXT]->(b)")
                .ExecuteWithoutResults();

            Validate3partQuery("(a)-[rl]->(b)", true);
        }

        [TestMethod]
        public void VeryfiySimpleQueryUpdateWithIdRenumberingOn()
        {
            SetGeneratedApprovedFilePath();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NXT]->(b)")
                .ExecuteWithoutResults();

            try { Validate3partQuery("(a)-[rl]->(b)"); }
            catch { }

            _deleteData();
            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT]->(b:WORD {name:'world'}), (:WORD {name:'Goodby'})-[:NXT]->(b)")
                .ExecuteWithoutResults();

            Validate3partQuery("(a)-[rl]->(b)", false);
        }

        private static void Validate3partQuery(string queryString, bool leaveNodeIdsUnchanged = false)
        {
            var q = graph.Cypher.Match(queryString)
                .Return((a, rl, b) => new
                {
                    aId = a.Id(),
                    aLabels = a.Labels(),
                    aData = a.Node<string>(),
                    bId = b.Id(),
                    bLabels = b.Labels(),
                    bData = b.Node<string>(),
                    rType = rl.Type(),
                    rFrom = Return.As<long>("ID(STARTNODE(rl))"),
                    rTo = Return.As<long>("ID(ENDNODE(rl))"),
                    rData = rl.Node<string>(),
                });

            var qr = q.Results
                .Select(c => new
                {
                    a = new GraphNode { Id = c.aId, Text = string.Join(",", c.aLabels), Popuptext = c.aData.Data },
                    b = new GraphNode { Id = c.bId, Text = string.Join(",", c.bLabels), Popuptext = c.bData.Data },
                    r = new GraphEdge { Type = c.rType, FromId = c.rFrom, ToId = c.rTo, Data = c.rData.Data }
                })
                .ToArray();
            Neo4jApprover.VerifyQuery(qr.Select(c => c.a).Union(qr.Select(c => c.b)).ToArray(), qr.Select(c => c.r).ToArray(), leaveNodeIdsUnchanged);
        }

        private void SetGeneratedApprovedFilePath()
        {
            generatedApprovedFilePath = solutiondirecory + Approvals.GetDefaultNamer().Name + ".approved.txt";
        }
    }
}