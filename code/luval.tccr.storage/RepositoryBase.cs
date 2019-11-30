using luval.tccr.config;
using Luval.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace luval.tccr.storage
{
    public class RepositoryBase
    {

        public RepositoryBase() : this(CreateDefaultDatabaseInstance())
        {

        }
        public RepositoryBase(Database database)
        {
            Database = database;
        }
        public virtual Database Database { get; private set; }

        internal static Database CreateDefaultDatabaseInstance()
        {
            return new Database(() => new SqlConnection(ConfigManager.Setting["ConnectionString"]));
        }
    }
}
