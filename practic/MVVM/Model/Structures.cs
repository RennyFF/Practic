using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.MVVM.Model
{
       public class User() {
            public int id { get; set; }
                public string firstName { get; set; }
                public string secondName { get; set; }
                public string login { get; set; }
                public string password { get; set; }
                public bool isAdmin { get; set; }

        }
       public class Ticket()
        {
            public int id { get; set; }
            public int number_ticket { get; set; }
            public string date { get; set; }
            public string causeby { get; set; }
            public string typeofcause { get; set; }
            public int client_id{ get; set; }
            public string status { get; set; }
            public string responsible { get; set; }
        }

}
