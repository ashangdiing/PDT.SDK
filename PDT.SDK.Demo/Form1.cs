using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDT.SDK;
using System.Net;

namespace PDT.SDK.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            //监听通话事件
            client.CallEvent += client_CallEvent;
            
        }

        /// <summary>
        /// 处理来电，短信等推送消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void client_CallEvent(object sender, CallEventArgs args)
        {
            this.Invoke(new Action(() =>
            {
                switch (args.Type)
                {
                    case EventType.SINGLE_INCOMING:
                        break;
                    case EventType.TEXT_INCOMING:
                        textBox1.Text += "收到短信:" + args.Text + "\r\n";
                        break;
                    case EventType.GROUP_START:
                        textBox1.Text += "监听通话开始：" + args.ToString() + "\r\n";
                        break;
                    case EventType.GROUP_END:
                        textBox1.Text += "监听通话结束：" + args.ToString() + "\r\n";
                        break;
                    case EventType.CALL_INFO:
                        textBox1.Text += "PTT通话信息:" + args.ToString() + "\r\n";
                        break;
                    case EventType.CALL_END:
                        textBox1.Text += "通话结束:" + args.ToString() + "\r\n";
                        break;
                    case EventType.CALL_SUCCESS:
                        textBox1.Text += "电话拨出结果：" + args.ToString() + "\r\n";
                        break;
                }

            }));

        }

        private void button1_Click(object sender, EventArgs e)
        {           
            var code=client.Connect();
            textBox1.Text += "连接返回：" + code+"\r\n";           
        
        }
        Client client = new Client("1", IPAddress.Parse("127.0.0.1"),8700);
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var result = client.QuerySysInfoSingle();
                var text = string.Join(";", result.Select(r => r.SeatID + " " + r.Number));
                textBox1.Text += "查询单呼信息:" + text + "\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络中断");
            }
        }

        private void buttonCall_Click(object sender, EventArgs e)
        {
           var res= client.StartCallOut("3",textBoxNumber.Text);
           textBox1.Text += "呼叫请求结果:" + res.ToString() + "\r\n"; 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var res = client.SendText("3", textBoxNumber.Text, "1", textBoxSMS.Text);
            textBox1.Text += "短信发送结果:" + res.ToString() + "\r\n";
        }

        private void textBoxNumber_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
