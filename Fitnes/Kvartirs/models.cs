using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvartirs
{
    class models
    {
        public class Buy
        {
            public int ID_buy { get; set; }
            public DateTime dateofbuy { get; set; }
            public int ID_abonement { get; set; }
            public string abonement_name { get; set; }
            public int ID_customer{ get; set; }
            public string customer_name { get; set; }
            public Double price { get; set; }
            public int count_hour { get; set; }
        }

        public class Visit
        {
            public int ID_visit { get; set; }
            public DateTime visitdate { get; set; }
            public DateTime timein { get; set; }
            public DateTime timeleft { get; set; }
            public int ID_customer { get; set; }
            public string customer_name { get; set; }
            public int ID_worker { get; set; }
            public string worker_name { get; set; }
        }


        public class Abonement
        {
            public int ID_abonement { get; set; }
            public string abonement_name { get; set; }
            public double price { get; set; }
            public int count_hour { get; set; }
           
        }
    }
}
