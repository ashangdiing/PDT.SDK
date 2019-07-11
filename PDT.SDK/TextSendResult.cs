using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PDT.SDK
{
    public class TextSendResult
    {
        public string SeatID { get; set; }

        public string CallID { get; set; }

        public string Called { get; set; }

        public string Type { get; set; }

        public string ResponseCode { get; set; }

        public override string ToString()
        {
            var js = new JavaScriptSerializer();
            var sb = new StringBuilder();
            js.Serialize(this, sb);
            return sb.ToString();
        }
    }
}
