using Luval.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class SqlEntityAdapter : EntityAdapter
    {
        public SqlEntityAdapter(Database db, Type entityType):base(db, new SqlServerDialectProvider(SqlTableSchema.Load(entityType)))
        {

        }

        public SqlEntityAdapter(Type entityType) : this(RepositoryBase.CreateDefaultDatabaseInstance(), entityType)
        {

        }
    }
}
