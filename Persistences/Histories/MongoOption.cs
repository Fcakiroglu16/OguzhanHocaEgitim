using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistences.Histories
{
    internal class MongoOption
    {
        public string DatabaseName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
