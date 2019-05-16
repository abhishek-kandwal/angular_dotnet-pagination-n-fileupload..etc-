using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPro.Models
{
    public class DBDataModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public Int64 PhoneNo { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public Boolean Approved { get; set; }

    }
}
