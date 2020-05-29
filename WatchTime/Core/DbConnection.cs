using Neo4j.Driver.V1;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchTime.Models;

namespace WatchTime.Core
{
    public class DbConnection : IDisposable
    {
        public BoltGraphClient Neo4jClient { get; }

        public DbConnection()
        {
            Neo4jClient = new BoltGraphClient("bolt://localhost:11002", "neo4j", "watchtime");
            Neo4jClient.Connect();
        }

        public void Dispose()
        {
            Neo4jClient?.Dispose();
        }
    }
}