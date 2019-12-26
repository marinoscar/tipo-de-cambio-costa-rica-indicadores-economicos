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

        public DateTime ToWorkDate(DateTime dt)
        {
            if (dt.Date.DayOfWeek == DayOfWeek.Saturday) return dt.AddDays(-1);
            if (dt.Date.DayOfWeek == DayOfWeek.Sunday) return dt.AddDays(-2);
            return dt;
        }
    }
}
