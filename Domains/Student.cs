using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual List<Teacher>? Teachers { get; set; }
    }
}
