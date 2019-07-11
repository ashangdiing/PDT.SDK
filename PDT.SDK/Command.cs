using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDT.SDK
{
    class Command
    {
        public Command()
        {
            Values = new string[256];
            
        }
    
        public string ID { get; set; }

        public string[] Values { get; set; }

        public override string ToString()
        {
            return ID + ";" + string.Join(";", Values.Where(v=>v!=null));
        }

        public static Command HeartBeat
        {
            get
            {
                if (heartBeat == null)
                {
                    heartBeat = new Command { ID = "2002" };
                    heartBeat.Values[0] = "20";
                   // heartBeat.Values[1] = "1";
                }
                return heartBeat;
            }
        }

        static Command heartBeat;
    }
}
