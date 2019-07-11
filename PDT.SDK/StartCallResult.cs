using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PDT.SDK
{
    public class StartCallOutResult
    {
        /// <summary>
        /// 调度坐席号
        /// </summary>
        public string SeatID { get; set; }
        /// <summary>
        /// 主叫号码
        /// </summary>
        public string Caller { get; set; }


        /// <summary>
        /// 被叫号码
        /// </summary>
        public string Called { get; set; }

        /// <summary>
        /// 呼叫编号：有效范围1-254，循环使用，用于同时段区分不同的呼叫，如果呼出失败，呼叫编号可能为0；
        /// </summary>
        public string CallID { get; set; }

        /// <summary>
        /// 0：正在呼出…；1：系统资源忙，失败；2：呼叫号码错误，失败；3: 调度座席号超过允许范围，失败；4：系统故障，失败;5：失败，其它错误；
        /// </summary>
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
