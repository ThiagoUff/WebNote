using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNote.Domain.Repository.Logs
{
    public class Logs
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string LogText { get; set; } = null!;
        public DateTime LogedOn { get; set; }
    }
}
