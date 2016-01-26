using System;
using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace UsersVoice.DbInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAll(CreateAdmin());
            Task.WaitAll(InitAdminRoles());
            Task.WaitAll(AddUserWithRole());

        }
        private static async Task CreateAdmin()
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["Admin_1"].ConnectionString;
                var client = new MongoClient(connString);

                await AddUserWithRole(client, "admin", "admin", "admin", "userAdminAnyDatabase");
            }
            catch (Exception ex)
            {
               Console.WriteLine("cannot create admin user: {0}", ex.Message);
            }
          
        }

        private static async Task InitAdminRoles()
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["Admin_2"].ConnectionString;
                var client = new MongoClient(connString);
                var db = client.GetDatabase("admin");

                await GrantUserRole(db, "admin", "dbOwner", "admin");
                await GrantUserRole(db, "admin", "clusterAdmin", "admin");
                await GrantUserRole(db, "admin", "userAdmin", "usersvoice_commands");
                await GrantUserRole(db, "admin", "userAdmin", "usersvoice_queries");
            }
            catch (Exception ex)
            {
                Console.WriteLine("cannot init roles for admin user: {0}", ex.Message);
            }
        }

        private static async Task GrantUserRole(IMongoDatabase db, string userName, string role, string dbName)
        {
            var command = new BsonDocument
            {
                {"grantRolesToUser", userName},
                {"roles", new BsonArray {new BsonDocument {{"role", role}, {"db", dbName}}}}
            };
            await db.RunCommandAsync<BsonDocument>(command);
        }

        private static async Task AddUserWithRole()
        {
            var connString = ConfigurationManager.ConnectionStrings["Admin_2"].ConnectionString;
            var client = new MongoClient(connString);

            try
            {
                await AddUserWithRole(client, "usersvoice_commands", "web_access", "web", "dbOwner");
            }
            catch (Exception ex)
            {
                Console.WriteLine("cannot add user: {0}", ex.Message);
            }

            try
            {
                await AddUserWithRole(client, "usersvoice_queries", "web_access", "web", "dbOwner");
            }
            catch (Exception ex)
            {
                Console.WriteLine("cannot add user: {0}", ex.Message);
            }
        }

        private static async Task AddUserWithRole(MongoClient client, string dbName, string userName, string password,string role)
        {
            var db = client.GetDatabase(dbName);
            var command = new BsonDocument
            {
                {"createUser", userName},
                {"pwd", password},
                {"roles", new BsonArray {new BsonDocument {{"role", role}, {"db", dbName}}}}
            };
            await db.RunCommandAsync<BsonDocument>(command);
        }
    }
}
