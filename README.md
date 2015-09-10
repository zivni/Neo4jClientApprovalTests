# Neo4jClientApprovalTests
A [ApprovalTests.Net](https://github.com/approvals/ApprovalTests.Net) reporter and approver to be used to unit test [Neo4j](http://neo4j.com/) with the [Neo4jClient](https://github.com/Readify/Neo4jClient) .net client

This project comes to solve the problem of visual approving Neo4j graphs during unit testing.
It makes it very easy to visualize the current state of the graph and see diffrences between the approved graph and the changes made by new code. It can also be uses to visualize query results if the query returns nodes and edges (see the tests project included within this reposetory).

Usage
---
Just add `[UseReporter(typeof(Neo4jReporter)]` attribute on the test class or test method. See the [AprrovalTests documentation](http://blog.approvaltests.com/2011/12/using-reporters-in-approval-tests.html)

In your test method use `Neo4jApprover.VerifyGraph(GraphClient)` to test the graph.

Use `Neo4jApprover.VerifyQuery(IEnumerable<GraphNode> nodes, IEnumerable<GraphEdge> edges, bool leaveNodeIdsUnchanged = false)` to validate a query (see test project for an example).

The reporter will be opened within your default browser. AS vis.js uses HTMl5 canvas - so you'll need a modern browser. I tested it with Chrome v45.

Note 1: You should delete the graph before each test and use a temporery Neo4j database for testing. You can use [Neo4j Docker image](http://neo4j.com/developer/docker/) to run the temp DB

Note 2: Whenever data is inserted to Neo4j it creates a new internal ID.
Beacuse in unit tests we are always recreating the data the IDs will be diffrent each time.
To over come this, the IDs are replaced with new ids starting from 1.
Potantialy this beavior can create s false positive.

NuGet Availabilty
---
Will come soon to [Install-Package Neo4jClientApprovalTests](https://www.nuget.org/packages/Neo4jClientApprovalTests/)

Example
---
```c#
    [TestClass, UseReporter(typeof(Neo4jReporter))]
    public class Neo4jReporterTests
    {
        [TestMethod]
        public void TestGraph()
        {
            GraphClient graph = new GraphClient(new Uri("..."));
            graph.Connect();

            graph.Cypher
                .Create("(a:WORD {name:'Hello'})-[:NEXT {times: 5}]->(b:WORD {name:'world'}), (b)-[:PREV]->(a)")
                .ExecuteWithoutResults();

            Neo4jApprover.VerifyGraph(graph);
        }
    }
```

ScreenShots
---
**At first, there is nothing to compare to, so just approve it if it is the right graph**

![](https://github.com/zivni/Neo4jClientApprovalTests/blob/master/ReadmeResources/screenshoot0.png)

**Compare to approved graph**

![](https://github.com/zivni/Neo4jClientApprovalTests/blob/master/ReadmeResources/screenshoot1.png)

Dependancies
---
* The great [vis.js](http://visjs.org/) to draw the graph.
* [ObjectApproval](https://github.com/SimonCropp/ObjectApproval) to make my life easier with the approval method.
* And cannot live without [Json.net](http://www.newtonsoft.com/json)

License
---
[MIT License](https://raw.githubusercontent.com/zivni/Neo4jClientApprovalTests/master/LICENSE.md)