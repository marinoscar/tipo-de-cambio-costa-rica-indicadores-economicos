using Luval.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class RepositoryBase
    {
        public RepositoryBase(Database database)
        {
            Database = database;
        }
        public virtual Database Database { get; private set; }
    }
}
