using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNote.Domain.Repository.Logs
{
    public class LogsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string LogsCollectionName { get; set; } = null!;
    }
}
