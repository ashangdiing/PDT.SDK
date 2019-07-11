using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace PDT.SDK
{
    public class CallEventArgs : EventArgs
    {
        public EventType Type { get; set; }

        /// <summary>
        /// 语音线路号
        /// </summary>
        public string LineNumber { get; set; }

        /// <summary>
        /// 呼叫编号
        /// </summary>
        public string CallID { get; set; }
        /// <summary>
        /// 主叫号码
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// 被叫号码
        /// </summary>
        public string Called { get; set; }

        /// <summary>
        /// 调度坐席号
        /// </summary>
        public string SeatID { get; set; }

        /// <summary>
        /// 通话电台号码
        /// </summary>
        public string PTTNumber { get; set; }

        /// <summary>
        /// PTT状态代码：0：按下PTT开始通话，1：松开PTT结束通话；
        /// </summary>
        public string PTTStatus { get; set; }

        /// <summary>
        /// 是否是本调度台PTT：0: 不是本调度台的PTT状态，1：是本调度台PTT
        /// </summary>
        public string IsLocalPTT { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Text { get; set; }

        public string ResponseCode { get; set; }

        public override string ToString()
        {
            var js = new JavaScriptSerializer();
            var sb=new StringBuilder();
            js.Serialize(this, sb);
            return sb.ToString();
        }
    }

    public enum EventType
    {
        CALL_SUCCESS=3006,

        CALL_END=3007,

        SINGLE_INCOMING = 3008,

        GROUP_START = 3009,

        GROUP_END = 3010,

        CALL_INFO = 3011,

        TEXT_INCOMING = 3013

    }
}
