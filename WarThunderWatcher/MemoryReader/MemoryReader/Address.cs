using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryReader
{
    public class Address
    {
        public Int64 Add { get; set; }
        public uint[] Chain { get; set; }
        public string Type { get; set; }

        public Address(string type, Int64 add, uint[] chain)
        {
            Type = type;
            Add = add;
            Chain = chain;
        }

        public Address(string type, Int64 add)
        {
            Type = type;
            Add = add;
        }
    }
}
