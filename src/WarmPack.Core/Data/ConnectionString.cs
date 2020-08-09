using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarmPack.Data
{
    public sealed class  ConnectionString
    {
        public ConnectionString()
        {

        }

        public ConnectionString(string server, string database, string user, string password)
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
        }

        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ToMsqlConnectionString()
        {
            return $"data source = {Server}; initial catalog = {Database}; user id = {User}; password = {Password}";
        }
        
    }
}
