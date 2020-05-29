using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchTime.Models;

namespace WatchTime.Core
{
    public class WatchTimeQueries : DbConnection
    {

        //User
        public User Login(string email, string password)
        {
            // MATCH(u:User) WHERE u.Email = 'bkrasicki@edu.cdv.pl' AND u.Password = 'user' 
            // RETURN(u)
            var loginQuery = Neo4jClient.Cypher.Match("(u:User)")
                .Where("u.Email = '" + email + "'")
                .AndWhere("u.Password = '" + password + "'")
                .Return((u) => new { User = u.As<User>() });
            var loginResult = loginQuery.Results;
            if (loginResult.Count() > 0)
            {
                var user = loginResult.First();
                user.User.Password = "";
                return user.User;
            }
            else
            {
                return null;
            }
        }

        public bool IsUserExists(string userLogin, string email)
        {
            // MATCH(u:User) WHERE u.Login = 'bartex102' OR u.Email = 'bkrasicki@edu.cdv.pl' 
            // RETURN(u)
            var userQuery = Neo4jClient.Cypher.Match("(u:User)")
                .Where("u.Login = '" + userLogin + "'")
                .OrWhere("u.Email = '" + email + "'")
                .Return((u) => new { User = u.As<User>() });
            var userResult = userQuery.Results;
            if (userResult.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Register(string login, string email, string password)
        {
            // CREATE (:User {Login:"bartex102", Email: "bkrasicki@edu.cdv.pl", Password: "user", Role: "user", WatchedTime: 0, ToWatchTime: 0})

            var newUser = new User { Login = login, Email = email, Password = password, Role = "user", ToWatchTime = 0, WatchedTime = 0 };
            Neo4jClient.Cypher.Create("(:User {newUser})")
                .WithParam("newUser", newUser).
                ExecuteWithoutResults();
        }

        //Series
        public List<Series> GetToWatchSeries(string userLogin)
        {
            // MATCH (s:Series)<-[r1:ToWatch]-(u:User) WHERE u.Login = "bartex102"
            var seriesQuery = Neo4jClient.Cypher
                .Match("(s:Series)<-[r1:ToWatch]-(u:User)")
                .Where("u.Login = '" + userLogin + "'")
                .Return((s) => new { Series = s.As<Series>() });
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.Series);
            }
            return seriesList;
        }

        public void AddToWatchSeries(int seriesId, string userLogin, bool createRelation)
        {
            if (createRelation)
            {
                // MATCH (s:Series), (u:User) WHERE u.Login = "bartex102" AND WHERE s.Id = 10 
                // CREATE UNIQUE (u)-[r:ToWatch]->(s)
                Neo4jClient.Cypher
                    .Match("(s:Series), (u:User)")
                    .Where("u.Login = '" + userLogin + "'")
                    .AndWhere("s.Id = " + seriesId)
                    .CreateUnique("(u)-[r:ToWatch]->(s)")
                    .ExecuteWithoutResults();
            }
            else
            {
                // MATCH (s:Series)-[r:ToWatch]-(u:User) WHERE u.Login = "bartex102" AND WHERE s.Id = 10 
                // DELETE (r)
                Neo4jClient.Cypher
                    .Match("(s:Series)-[r:ToWatch]-(u:User)")
                    .Where("u.Login = '" + userLogin + "'")
                    .AndWhere("s.Id = " + seriesId)
                    .Delete("r")
                    .ExecuteWithoutResults();
            }
        }

        public List<Series> GetWatchedSeries(string userLogin)
        {
            // MATCH (s:Series)<-[r1:Watched]-(u:User) WHERE u.Login = "bartex102" 
            // RETURN (s)
            var seriesQuery = Neo4jClient.Cypher
                .Match("(s:Series)<-[r1:Watched]-(u:User)")
                .Where("u.Login = '" + userLogin + "'")
                .Return((s) => new { Series = s.As<Series>() });
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.Series);
            }
            return seriesList;
        }

        public void AddWatchedSeries(int seriesId, string userLogin, bool createRelation)
        {
            if (createRelation)
            {
                // MATCH (s:Series), (u:User) WHERE u.Login = "bartex102" AND WHERE s.Id = 10 
                // CREATE UNIQUE (u)-[r:Watched]->(s)
                Neo4jClient.Cypher
                    .Match("(s:Series), (u:User)")
                    .Where("u.Login = '" + userLogin + "'")
                    .AndWhere("s.Id = " + seriesId)
                    .CreateUnique("(u)-[r:Watched]->(s)")
                    .ExecuteWithoutResults();
            }
            else
            {
                // MATCH (s:Series)-[r:Watched]-(u:User) WHERE u.Login = "bartex102" AND WHERE s.Id = 10 
                // DELETE (r)
                Neo4jClient.Cypher
                    .Match("(s:Series)-[r:Watched]-(u:User)")
                    .Where("u.Login = '" + userLogin + "'")
                    .AndWhere("s.Id = " + seriesId)
                    .Delete("r")
                    .ExecuteWithoutResults();
            }
        }

        public List<Series> GetSeries()
        {
            // MATCH (s:Series) 
            // RETURN (s)
            var seriesQuery = Neo4jClient.Cypher.Match("(s:Series)")
                .Return((s) => new { User = s.As<Series>() });
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.User);
            }
            return seriesList;
        }

        public List<Series> GetSuggestedSeries(string userLogin, int numberOfSeries)
        {
            // MATCH(u: User) WHERE u.Login = "bartex102"
            // MATCH(a: User) WHERE NOT a.Login = "bartex102"
            // MATCH d = shortestPath((u) -[*..2] - (a))
            // UNWIND nodes(d) AS n
            // MATCH(n: User) WHERE NOT n.Login = "bartex102"
            // MATCH(n) -[:Watched]->(s: Series)
            // RETURN DISTINCT(s) LIMIT 5

            //MATCH(a: Point) WHERE ID(a) = 1
            //MATCH(b: Point) WHERE ID(b) = 3
            //CALL algo.shortestPath.stream(a, b, "dist")
            //YIELD nodeId, dist
            //MATCH(other: Point) WHERE id(other) = nodeId
            //RETURN other.lat AS lat, other.lng AS lng, dist
            
            var seriesQuery = Neo4jClient.Cypher
                .Match("(a:User)")
                .Where("NOT a.Login = '" + userLogin + "'")
                .Match("(u:User)")
                .Where("u.Login = '" + userLogin + "'")
                .Match("d = shortestPath((u)-[*..2]-(a))")
                .Unwind("nodes(d)", "n")
                .Match("(n:User)")
                .Where("NOT n.Login = '" + userLogin + "'")
                .Match("(n)-[:Watched]->(s:Series)")
                .ReturnDistinct((s) => new { Series = s.As<Series>() }).Limit(5);
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.Series);
            }
            return seriesList;
        }

        public List<Series> GetMostPopularSeriesByTag(string userLogin, int tagId, int numberOfSeries)
        {
            // MATCH(t:Tag) WHERE t.Id = 5
            // MATCH(a:User) WHERE NOT a.Login = "bartex102"
            // MATCH d = shortestPath((t)<-[*..2]-(a))
            // UNWIND nodes(d) AS n
            // MATCH(n:User)-[r1]->(s:Series)-[r2:FromGenre]->(t)
            // RETURN DISTINCT s, count(r1) ORDER BY count(r1) DESC LIMIT 3
            var seriesQuery = Neo4jClient.Cypher
                .Match("(t:Tag)")
                .Where("t.Id = " + tagId)
                .Match("(a:User)")
                .Where("NOT a.Login = '" + userLogin + "'")
                .Match("d = shortestPath((t)<-[*..2]-(a))")
                .Unwind("nodes(d)", "n")
                .Match("(n:User)-[r1]->(s:Series)-[r2:FromGenre]->(t)")
                .ReturnDistinct((s, r1) => new { Series = s.As<Series>(), r1 = r1.Count() })
                .OrderByDescending("count(r1)")
                .Limit(numberOfSeries);
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.Series);
            }
            return seriesList;
        }

        public List<Series> GetToWatchSeriesByProducer(int producerId, int limitNumber)
        {
            // MATCH (p:Producer)-[r1:Directed]->(s:Series)<-[r2:ToWatch]-(u:User) WHERE p.Id = 1 
            // RETURN (s), count(r2) 
            // ORDER BY count(r2) DESC 
            // LIMIT 3
            var seriesQuery = Neo4jClient.Cypher
                .Match("(p:Producer)-[r1:Directed]->(s:Series)<-[r2:ToWatch]-(u:User)")
                .Where("p.Id = " + producerId)
                .Return((s, r2) => new { Series = s.As<Series>(), CountR2 = r2.Count() })
                .OrderBy("count(r2)")
                .Limit(limitNumber);
            List<Series> seriesList = new List<Series>();
            foreach (var series in seriesQuery.Results)
            {
                seriesList.Add(series.Series);
            }
            return seriesList;
        }

        public bool CreateSeries(string userRole, int id, string name, int seasonsNumber, int yearOfProduction, TimeSpan time, int tagId)
        {
            if (userRole == "admin")
            {
                var newSeries = new Series { Id = id, Name = name, SeasonsNumber = seasonsNumber, YearOfProduction = yearOfProduction, Time = time };
                Neo4jClient.Cypher
                    .Match("(t:Tag)")
                    .Where("t.Id = " + tagId)
                    .Create("(:Series {newSeries})-[:FromGenre]->(t)")
                    .WithParam("newSeries", newSeries)
                    .ExecuteWithoutResults();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Producer
        public List<Producer> GetProducers()
        {
            // MATCH (p:Producer) 
            // RETURN (p)
            var producersQuery = Neo4jClient.Cypher.Match("(p:Producer)")
                .Return((p) => new { User = p.As<Producer>() });
            List<Producer> producersList = new List<Producer>();
            foreach (var producer in producersQuery.Results)
            {
                producersList.Add(producer.User);
            }
            return producersList;
        }

        public List<Tag> GetTags()
        {
            // MATCH (t:Tag) 
            // RETURN (t)
            var tagQuery = Neo4jClient.Cypher.Match("(t:Tag)")
                .Return((t) => new { Tag = t.As<Tag>() });
            List<Tag> tagsList = new List<Tag>();
            foreach (var tag in tagQuery.Results)
            {
                tagsList.Add(tag.Tag);
            }
            return tagsList;
        }
    }
}