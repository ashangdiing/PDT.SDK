using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PDT.SDK
{
    public class Client
    {
        public Client(string clientID,IPAddress address,int port)
        {
            this.typeID = "20";
            this.clientID = clientID;
            this.address = address;
            this.port = port;
        }
        public int Connect()
        {
            conn = new TcpClient();
            conn.Connect(new IPEndPoint(address, port));
            stream = conn.GetStream();
            var cmd = ReadCommand(stream);

            var registerCmd = new Command { ID = "2001" };
            registerCmd.Values[0] = typeID;
            registerCmd.Values[1] = clientID;
            registerCmd.Values[2] = cmd.Values[0];
            registerCmd.Values[3] = GetPassword(cmd.Values[0]);
            WriteCommand(stream, registerCmd);

            var registerReply = ReadCommand(stream);
            var code = int.Parse(registerReply.Values[2]);
            if (code == 0)
            {
                this.IsConnected = true;
                cancel = new CancellationTokenSource();
                Task.Factory.StartNew(() => this.HeartBeatDaemon(), cancel.Token);
                Task.Factory.StartNew(() => this.CallEventDaemon(), cancel.Token);
            }
            return code;
        }

        /// <summary>
        /// 系统单呼号码信息，即PDT调度系统的单呼接入号码
        /// </summary>
        /// <returns></returns>
        public List<SeatInfo> QuerySysInfoSingle()
        {
            return QuerySysInfo(true);
        }

        public List<SeatInfo> QuerySysInfoGroup()
        {
            return QuerySysInfo(false);
        }

        public bool SetGroup(IEnumerable<string> numbers)
        {
            var cmd = new Command { ID = "2004" };
            cmd.Values[0] = typeID;
            cmd.Values[1] = clientID;
            var total = numbers.Count();
            cmd.Values[2] = total.ToString();
            var index = 3;
            foreach(var n in numbers)
            {
                cmd.Values[index++] = n;
            }
            WriteCommand(stream, cmd);
            var reply = GetCommand("3004");
            return reply.Values[4] == "0";
        }

        public TextSendResult SendText(string seatID,string number,string type,string text)
        {
            var result = new TextSendResult();
            var cmd = new Command { ID = "2012" };
            cmd.Values[0] = seatID;
            textID = (textID) % 255 + 1;
            cmd.Values[1] = textID.ToString();          
            cmd.Values[2] = number;
            cmd.Values[3] = type;
            cmd.Values[4] = text;
            WriteCommand(stream,cmd);
            var reply = GetCommand("3012");
            result.SeatID = reply.Values[0];
            result.CallID = reply.Values[1];
            result.Called = reply.Values[2];
            result.Type = reply.Values[3];
            result.ResponseCode = reply.Values[4];
            return result;
        }
        public StartCallOutResult StartCallOut(string seatID,string called)
        {
            var cmd = new Command { ID = "2005" };
            cmd.Values[0] = typeID;
            cmd.Values[1] = clientID;
            cmd.Values[2] = seatID;
            cmd.Values[3] = called;
            WriteCommand(stream, cmd);
            var reply=GetCommand("3005");
            var result = new StartCallOutResult
            {
                SeatID = reply.Values[0],
                Caller = reply.Values[1],
                Called=reply.Values[2],
                CallID=reply.Values[3],
                ResponseCode=reply.Values[4]
            };
            return result;
        }

        public delegate void CallEventHandler(object sender, CallEventArgs args);

        public event CallEventHandler CallEvent;

        public bool IsConnected { get; private set; }

 


        #region internal implementaion
        [DllImport(@"NTCEnCode.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        extern static void CreateNTCCode(uint dwLinkParam,	//注册连接参数消息中的连接参数 
                             byte[] pEnCode);

        CancellationTokenSource cancel = new CancellationTokenSource();
        string typeID;
        string clientID;
        IPAddress address;
        int port = 8700;
        TcpClient conn=new TcpClient();
        NetworkStream stream;
        int maxLines;
        int maxSeats;
        List<Command> cmdBuffer = new List<Command>();
        int textID = 0;
        void CallEventDaemon()
        {
            while (!cancel.IsCancellationRequested)
            {
                var cmd = ReadCommand(stream);
                var arg = new CallEventArgs();
                switch (int.Parse(cmd.ID))
                {
                    case (int)EventType.SINGLE_INCOMING:
                        arg.Type = EventType.SINGLE_INCOMING;
                        arg.CallID = cmd.Values[0];
                        arg.LineNumber = cmd.Values[1];
                        arg.Caller = cmd.Values[2];
                        arg.Called = cmd.Values[3];
                        arg.SeatID = cmd.Values[4];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.GROUP_START:
                        arg.Type = EventType.GROUP_START;
                        arg.SeatID = cmd.Values[0];
                        arg.CallID = cmd.Values[1];
                        arg.LineNumber = cmd.Values[2];
                        arg.Caller = cmd.Values[3];
                        arg.Called = cmd.Values[4];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.GROUP_END:
                        arg.Type = EventType.GROUP_END;
                        arg.Caller = cmd.Values[0];
                        arg.Called = cmd.Values[1];
                        arg.CallID = cmd.Values[2];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.CALL_INFO:
                        arg.Type = EventType.CALL_INFO;
                        arg.CallID = cmd.Values[0];
                        arg.LineNumber = cmd.Values[1];
                        arg.Caller = cmd.Values[2];
                        arg.Called = cmd.Values[3];
                        arg.PTTNumber = cmd.Values[4];
                        arg.PTTStatus = cmd.Values[5];
                        arg.IsLocalPTT = cmd.Values[6];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.TEXT_INCOMING:
                        arg.Type = EventType.TEXT_INCOMING;
                        arg.CallID = cmd.Values[0];
                        if (cmd.Values[5] != null)  //兼容模拟器和新协议
                        {
                            arg.SeatID = cmd.Values[1];
                            arg.Caller = cmd.Values[2];
                            arg.Called = cmd.Values[3];
                            arg.Text = cmd.Values[5];
                        }
                        else
                        {
                            arg.Caller = cmd.Values[1];
                            arg.Called = cmd.Values[2];
                            arg.Text = cmd.Values[4];
                        }
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.CALL_END:
                        arg.Type = EventType.CALL_END;
                        arg.SeatID = cmd.Values[0];
                        arg.CallID = cmd.Values[1];
                        arg.LineNumber = cmd.Values[2];
                        arg.Caller = cmd.Values[3];
                        arg.Called = cmd.Values[4];
                        arg.ResponseCode = cmd.Values[5];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    case (int)EventType.CALL_SUCCESS: //调度呼出结果消息
                        arg.Type = EventType.CALL_SUCCESS;
                        arg.SeatID = cmd.Values[0];
                        arg.Caller = cmd.Values[1];
                        arg.Called = cmd.Values[2];
                        arg.CallID = cmd.Values[3];
                        arg.LineNumber = cmd.Values[4];
                        arg.ResponseCode = cmd.Values[5];
                        if (CallEvent != null)
                            CallEvent(this, arg);
                        break;
                    default:
                        DispatchCommand(cmd);
                        break;
                }
            }
        }

        void DispatchCommand(Command cmd)
        {
            lock (cmdBuffer)
            {
                cmdBuffer.Add(cmd);
            }
        }

        Command GetCommand(string ID)
        {
            Command cmd = null;

            lock (cmdBuffer)
            {
                cmd = cmdBuffer.Where(c => c.ID == ID).FirstOrDefault();
                if (cmd != null)
                    cmdBuffer.Remove(cmd);
                else if (!IsConnected)
                {
                    throw new Exception("网络中断，请尝试重新连接。");
                }
            }

            while(cmd==null)
            {
                Thread.Sleep(100);
                lock (cmdBuffer)
                {
                    cmd = cmdBuffer.Where(c => c.ID == ID).FirstOrDefault();
                    if (cmd != null)
                        cmdBuffer.Remove(cmd);
                    else if (!IsConnected)
                    {
                        throw new Exception("网络中断，请尝试重新连接。");
                    }
                }
            }

            return cmd;
        }
        void HeartBeatDaemon()
        {
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    var cmd = Command.HeartBeat;
                    cmd.Values[1] = this.clientID;
                    WriteCommand(stream, Command.HeartBeat);
                    var reply = GetCommand("3002");
                    Thread.Sleep(10000);
                }
                catch
                {

                }
            }
        }
        Command ReadCommand(NetworkStream stream)
        {
            try
            {
                Command cmd = new Command();

                List<Byte> buf = new List<byte>();
                byte b = 0;
                int currentComponent = -1;
                while (b != 0x0A)
                {
                    b = (byte)stream.ReadByte();
                    if (b == 0x3B)
                    {
                        var content = Encoding.GetEncoding("GBK").GetString(buf.ToArray());
                        if (currentComponent < 0)
                            cmd.ID = content;
                        else
                            cmd.Values[currentComponent] = content;
                        currentComponent++;
                        buf.Clear();
                    }
                    else
                    {
                        if (b != 0x0A)
                            buf.Add(b);
                    }
                }

                if (buf.Count > 0)
                {
                    var content = Encoding.GetEncoding("GBK").GetString(buf.ToArray());
                    if (currentComponent < 0)
                        cmd.ID = content;
                    else
                        cmd.Values[currentComponent] = content;
                }

                Console.WriteLine(DateTime.Now + " 接收:" + cmd.ToString());
                return cmd;
            }
            catch (Exception e)
            {
                HandleNetworkException();
               // throw e;
            }
            return null;
            
        }

        void WriteCommand(NetworkStream stream,Command cmd)
        {
            try
            {
                var str = cmd.ToString();
                Console.WriteLine(DateTime.Now + " 发送:" + str);
                var bytes = Encoding.GetEncoding("GBK").GetBytes(str);
                var bytes2 = new byte[bytes.Length + 1];
                bytes2[bytes2.Length - 1] = 0x0A;
                Array.Copy(bytes, bytes2, bytes.Length);
                stream.Write(bytes2, 0, bytes2.Length);
            }
            catch (Exception e)
            {
                HandleNetworkException();
                throw new Exception("网络中断，请尝试重新连接");
            }
        }

        string GetPassword(string code)
        {
            var bytes=new byte[200];
            Client.CreateNTCCode(uint.Parse(code), bytes);
            var str= Encoding.ASCII.GetString(bytes);
            return str.Substring(0, str.IndexOf('\u0000'));
        }

        List<SeatInfo> QuerySysInfo(bool isSingle)
        {
            var cmd = new Command { ID = "2003" };
            cmd.Values[0] = this.typeID;
            cmd.Values[1] = this.clientID;
            cmd.Values[2] = isSingle ? "1" : "2";

            WriteCommand(stream, cmd);
            List<SeatInfo> seats = new List<SeatInfo>();
            var reply = GetCommand("3003");
            for(int i=4;i<reply.Values.Length;i++)
            {
                if(reply.Values[i]!=null)
                {
                    var info = new SeatInfo { SeatID = reply.Values[i], Number = reply.Values[i + 1] };
                    seats.Add(info);
                    i += 1;
                }
                else
                {
                    break;
                }
            }

            return seats;
        }

        void HandleNetworkException()
        {
            this.IsConnected = false;
            try
            {
                if(stream!=null)
                    stream.Close();
                if (conn != null)
                    conn.Close();
            }
            finally
            {
                try
                {
                    cancel.Cancel();
                }
                finally
                {

                }
            }
        }
        #endregion
    }
}
