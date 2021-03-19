using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.Utility
{
    public class ExceptionEntry
    {
        public ExceptionEntry(string EntryID, string Data)
        {
            entryId = EntryID;
            data = Data;
        }

        public string entryId { get; set; }
        public string data { get; set; }
    }
}
