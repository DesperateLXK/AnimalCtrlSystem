using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
namespace AnimalCtrl
{
    //此文件为串口选项初始化
    class PartOfSerialPortMainForm
    {
    }

    public partial class MainForm : Form
    {
        //实例化串口对象
        SerialPort serialPort = new SerialPort();

        public void SerialPortInit()
        {
            if (SerialCheckBox.Checked == true)
            {

                SerialPortConfigGroupBox.Enabled = true;
                /*------串口界面参数设置------*/

                //检查是否含有串口
                string[] str = SerialPort.GetPortNames();
                
                if (str == null)
                {
                    MessageBox.Show("本机没有串口！", "Error");
                    return;
                }
                try
                {                    
                    //添加串口
                    foreach (string s in str)
                    {
                        PortChoose.Items.Add(s);
                    }
                    //设置默认串口选项
                    PortChoose.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("本机未发现COM口\n请插上串口设备或者检查驱动", "Error");
                    this.Dispose();
                    this.Close();
                }

                

                /*------波特率设置-------*/
                string[] baudRate = { "9600", "19200", "38400", "57600", "115200" };
                foreach (string s in baudRate)
                {
                    BandRate.Items.Add(s);
                }
                BandRate.SelectedIndex = 0;

                /*------数据位设置-------*/
                string[] dataBit = { "5", "6", "7", "8" };
                foreach (string s in dataBit)
                {
                    DataBit.Items.Add(s);
                }
                DataBit.SelectedIndex = 3;


                /*------校验位设置-------*/
                string[] checkBit = { "None", "Even", "Odd", "Mask", "Space" };
                foreach (string s in checkBit)
                {
                    CheckBit.Items.Add(s);
                }
                CheckBit.SelectedIndex = 0;


                /*------停止位设置-------*/
                string[] stopBit = { "1", "1.5", "2" };
                foreach (string s in stopBit)
                {
                    StopBit.Items.Add(s);
                }
                StopBit.SelectedIndex = 0;

                /*------数据格式设置-------*/
                //SendRadioButtonASCII.Checked = true;
                //RecRadioButtonASCII.Checked = true;
                SendRadioButtonHEX.Checked = true;
                RecRadioButtonHEX.Checked = true;
                /*------接受缓冲区设置-------*/

            }
            else
            {
                SerialPortConfigGroupBox.Enabled = false;
            }
        }

        //串口部分事件
        //模式选择

        private void SerialCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SerialCheckBox.Checked == true)
            {
                SerialPortConfigGroupBox.Enabled = true;

            }
            else
            {
                SerialPortConfigGroupBox.Enabled = false;
            }
        }


        //打开、关闭按钮
        private void OpenClosePortButton_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)//串口处于关闭状态
            {

                try
                {

                    if (PortChoose.SelectedIndex == -1)
                    {
                        MessageBox.Show("Error: 无效的端口,请重新选择", "Error");
                        return;
                    }
                    string strSerialName = PortChoose.SelectedItem.ToString();
                    string strBaudRate = BandRate.SelectedItem.ToString();
                    string strDataBit = DataBit.SelectedItem.ToString();
                    string strCheckBit = CheckBit.SelectedItem.ToString();
                    string strStopBit = StopBit.SelectedItem.ToString();

                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDataBit = Convert.ToInt32(strDataBit);

                    serialPort.PortName = strSerialName;//串口号
                    serialPort.BaudRate = iBaudRate;//波特率
                    serialPort.DataBits = iDataBit;//数据位


                    switch (strStopBit)            //停止位
                    {
                        case "1":
                            serialPort.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            serialPort.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            serialPort.StopBits = StopBits.Two;
                            break;
                        default:
                            MessageBox.Show("Error：停止位参数不正确!", "Error");
                            break;
                    }
                    switch (strCheckBit)             //校验位
                    {
                        case "None":
                            serialPort.Parity = Parity.None;
                            break;
                        case "Odd":
                            serialPort.Parity = Parity.Odd;
                            break;
                        case "Even":
                            serialPort.Parity = Parity.Even;
                            break;
                        default:
                            MessageBox.Show("Error：校验位参数不正确!", "Error");
                            break;
                    }

                    //打开串口
                    serialPort.Open();

                    //打开串口后设置将不再有效

                    PortChoose.Enabled = false;
                    BandRate.Enabled = false;
                    DataBit.Enabled = false;
                    CheckBit.Enabled = false;
                    StopBit.Enabled = false;
                    SendRadioButtonASCII.Enabled = false;
                    SendRadioButtonHEX.Enabled = false;
                    RecRadioButtonASCII.Enabled = false;
                    RecRadioButtonHEX.Enabled = false;
                    SendDataButton.Enabled = true;
                    RrflashPortButton.Enabled = false;
                    ResetPortConfig.Enabled = true;
                    //SerialCheckBox.Enabled = false;
                    OpenClosePortButton.Text = "关闭串口";
                    SendGroupBox.Enabled = true;

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    return;
                }
            }
            else //串口处于打开状态
            {

                serialPort.Close();//关闭串口

                //串口关闭时设置有效
                PortChoose.Enabled = true;
                BandRate.Enabled = true;
                DataBit.Enabled = true;
                CheckBit.Enabled = true;
                StopBit.Enabled = true;
                SendRadioButtonASCII.Enabled = true;
                SendRadioButtonHEX.Enabled = true;
                RecRadioButtonASCII.Enabled = true;
                RecRadioButtonHEX.Enabled = true;
                SendDataButton.Enabled = false;
                RrflashPortButton.Enabled = true;
                ResetPortConfig.Enabled = false;
                SerialPortConfigGroupBox.Enabled = true;
                //SerialCheckBox.Enabled = true;
                OpenClosePortButton.Text = "打开串口";
                SendGroupBox.Enabled = false;
            }
        }


        //刷新串口
        public void ReflashPort()
        {
            this.PortChoose.Text = " ";
            this.PortChoose.Items.Clear();

            string[] str = SerialPort.GetPortNames();

            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口
            foreach (string s in str)
            {
                this.PortChoose.Items.Add(s);
            }
            //设置默认串口
            PortChoose.SelectedIndex = 0;
        }
        private void RrflashPortButton_Click(object sender, EventArgs e)
        {
            ReflashPort();
        }

        //重新设置设按钮
        private void ResetPortConfig_Click(object sender, EventArgs e)
        {

            if (OpenClosePortButton.Text == "打开串口")//串口处于关闭状态
            {

                try
                {
                    //打开串口
                    serialPort.Open();
                    //打开串口后设置将不再有效
                    //串口关闭时设置有效
                    PortChoose.Enabled = true;
                    BandRate.Enabled = true;
                    DataBit.Enabled = true;
                    CheckBit.Enabled = true;
                    StopBit.Enabled = true;
                    SendRadioButtonASCII.Enabled = true;
                    SendRadioButtonHEX.Enabled = true;
                    RecRadioButtonASCII.Enabled = true;
                    RecRadioButtonHEX.Enabled = true;
                    SendDataButton.Enabled = true;
                    RrflashPortButton.Enabled = true;
                    //ResetPortConfig.Enabled = true;
                    SerialPortConfigGroupBox.Enabled = true;
                    OpenClosePortButton.Text = "关闭串口";
                    SendGroupBox.Enabled = false;

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    return;
                }
            }
            else if(OpenClosePortButton.Text == "关闭串口")//串口处于打开状态
            {

                //serialPort.Close();//关闭串口
                serialPort.Dispose();
                //串口关闭时设置有效
                PortChoose.Enabled = true;
                BandRate.Enabled = true;
                DataBit.Enabled = true;
                CheckBit.Enabled = true;
                StopBit.Enabled = true;
                SendRadioButtonASCII.Enabled = true;
                SendRadioButtonHEX.Enabled = true;
                RecRadioButtonASCII.Enabled = true;
                RecRadioButtonHEX.Enabled = true;
                SendDataButton.Enabled = true;
                RrflashPortButton.Enabled = true;
                SerialPortConfigGroupBox.Enabled = true;
                OpenClosePortButton.Text = "打开串口";
                SendGroupBox.Enabled = false;
                ResetPortConfig.Enabled = false;
                //延时300MS进行串口刷新
                DelayMs(300);
                ReflashPort();
            }
        }
        //string 字符转化为 Byte
        public static byte[] StringToByte(string str)
        {
            byte[] bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                int btvalue = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)btvalue;
            }
            return bytes;
        }

        //Byte转化为string

        public static string ByteToString(byte[] bytes)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte bt in bytes)
            {
                strBuilder.AppendFormat("{0:X2}", bt);
            }
            return strBuilder.ToString();
        }


        //临时缓冲list
        List<byte> serialBuffer = new List<byte>(4096);
        byte[] serialFramesHeadBit = { 0XAA, 0XEE }; //数据帧头 两位
        byte serialFramesCheckBit = 0XFF; //数据帧尾
        int dataLen = 13; //数据位 共13位 从ReceiveBytes的第3位到第15位
        int serialFramesCheckNum = 3; //帧数据位数
        int serialFramesLen = 16;
        public void DataReceivedByte(object sender, SerialDataReceivedEventArgs e)
        {

            string str = " ";
            if (serialPort.IsOpen)
            {

                //输出当前时间
                DateTime dateTimeNow = DateTime.Now;
                dateTimeNow.GetDateTimeFormats();

                if (RecRadioButtonASCII.Checked == true) //接收格式为ASCII
                {
                    //防止两次回调
                    if (e.EventType == System.IO.Ports.SerialData.Eof)
                    {
                        return;
                    }

                    try
                    {
                        String input = serialPort.ReadExisting();
                        PortRecTextBox.Text += input + "\r\n";
                        PortRecTextBox.AppendText("\n");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "你波特率是不是有问题？？？");
                        return;
                    }

                    PortRecTextBox.SelectionStart = PortRecTextBox.Text.Length;
                    PortRecTextBox.ScrollToCaret();//滚动到光标处
                    serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
                }
                else //接收格式为HEX 这个是专用协议 不是通用的接收
                {
                    
                    if (e.EventType == System.IO.Ports.SerialData.Eof) //防止多次进入回调打乱缓冲区
                    {
                        return;
                    }
                    try
                    {
                        int count = serialPort.BytesToRead; //缓冲区读入数据的数量
                        byte[] buf = new byte [count];   
                        serialPort.Read(buf, 0, count);
                        serialBuffer.AddRange(buf); //将buf添加到临时list

                        while (serialBuffer.Count >= serialFramesCheckNum) //至少有3位数据 此处是判断一帧数据的完整性
                        {
                            if ((serialBuffer[0] == serialFramesHeadBit[0]) && (serialBuffer[1] == serialFramesHeadBit[1])) //帧头判断
                            {
                                
                                if (serialBuffer.Count < serialFramesLen)
                                {
                                    break;
                                }
                                byte[] ReceiveBytes = new byte[serialFramesLen];
                                serialBuffer.CopyTo(0, ReceiveBytes, 0, serialFramesLen);

                                if (serialFramesCheckBit != ReceiveBytes[serialFramesLen - 1])//判断帧尾
                                {
                                    serialBuffer.RemoveRange(0, serialFramesLen);
                                    MessageBox.Show("数据包长度不正确！");
                                    continue;
                                }

                                //判断数据校验位是否正确
                                //如果不正确 移除所有数据
                                if (!isDataCheckBitSuccess(ReceiveBytes))
                                {
                                    serialBuffer.RemoveRange(0, serialFramesLen);
                                    MessageBox.Show("数据内容不正确！");
                                    continue;
                                }

                                //receiveBytes 就是完整可用的一帧数据包
                                //接下来处理收到的数据
                                try
                                {
                                    HandleRecBuff(ReceiveBytes);
                                }
                                catch {
                                    MessageBox.Show("数据处理异常！");
                                }

                                //注释语句为调试时使用
                                //str = ByteToString(ReceiveBytes);
                                //PortRecTextBox.Text += string.Format("{0}\r\n", dateTimeNow);
                                //PortRecTextBox.AppendText(str + '\n');

                                serialBuffer.RemoveRange(0, serialFramesLen);
                            }
                            else //帧头不正确时，记得清除
                            {
                                serialBuffer.RemoveAt(0);
                            }

                        }
                        DelayMs(10);
                       
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                        PortRecTextBox.Text = "";//清空
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        //数据位中的检验位判断
        public bool isDataCheckBitSuccess(byte[] ReceiveBytes)
        {
            bool SUCCESS = true;
            bool FAILURE = false;   
            byte tempBIT = 0x00;

            int tempInt16 = 0;
            for (int i = 0; i < ReceiveBytes.Length - 2; i++)
            {
                tempInt16 += Convert.ToInt16(ReceiveBytes[i]);
            }
            tempInt16 = tempInt16 % 256;
            tempBIT = Convert.ToByte(tempInt16 & 0x00FF);
            //倒数第二位
            if (tempBIT == ReceiveBytes[14])
            {
                return SUCCESS;
            }
            else
            {
                return FAILURE;
            }
            
        }
        //判断设备编号 并且做相应的操作
        public void HeartDecideDiviceNum(byte numBit)
        {
            switch (numBit)
            {
                case 0X80:
                    myLedControl1.LedStatus = true;
                    break;
                case 0X40:
                    myLedControl2.LedStatus = true;
                    break;
                case 0X20:
                    myLedControl3.LedStatus = true;
                    break;
                case 0X10:
                    myLedControl4.LedStatus = true;
                    break;
                case 0X08:
                    myLedControl5.LedStatus = true;
                    break;
                case 0X04:
                    myLedControl6.LedStatus = true;
                    break;
                case 0X02:
                    myLedControl7.LedStatus = true;
                    break;
                case 0X01:
                    myLedControl8.LedStatus = true;
                    break;

            }
        }
        //
        //        表示通道
        //1—1通道；
        //2—1-3通道；
        //3—3通道；
        //4—2通道； 
        //5—2-3通道；
        //6—4通道；
        //8—1-4通道；
        //9—2-4通道
        public void StimulateSuccessMessageHandle(byte[] tempReceiveBytes, ref string stimuSuccessMessage)
        {
            stimuSuccessMessage = string.Empty;
            string devNumStr = "";
            string stimuNumStr = "刺激编号：" + (Convert.ToInt16(tempReceiveBytes[13]) + 1).ToString() + "\n"; //刺激编号
            string channelNumStr = "";

            byte tempChannelBit = (byte) (tempReceiveBytes[3] & 0X0F);//只取低位

            switch (tempChannelBit) //通道编号
            {
                case 0x01:
                    channelNumStr = "   1通道            ";
                    break;
                case 0x02:
                    channelNumStr = "1-3通道            ";
                    break;
                case 0x03:
                    channelNumStr = "   3通道            ";
                    break;
                case 0x04: 
                    channelNumStr = "   2通道            ";
                    break;
                case 0x05:
                    channelNumStr = "2-3通道            ";
                    break; 
                case 0x06:
                    channelNumStr = "   4通道            ";
                    break;
                case 0x08:
                    channelNumStr = "1-4通道            ";
                    break;
                case 0x09:
                    channelNumStr = "2-4通道            ";
                    break;

            }

            switch (tempReceiveBytes[12])//设备编号
            {
                //1号设备
                case 0X80:
                    devNumStr = "1号设备   ";
                    break;
                case 0X40:
                    devNumStr = "2号设备   ";
                    break;
                case 0X20:
                    devNumStr = "3号设备   ";
                    break;
                case 0X10:
                    devNumStr = "4号设备   ";
                    break;
                case 0X08:
                    devNumStr = "5号设备   ";
                    break;
                case 0X04:
                    devNumStr = "6号设备   ";
                    break;
                case 0X02:
                    devNumStr = "7号设备   ";
                    break;
                case 0X01:
                    devNumStr = "8号设备   ";
                    break;
            }
            stimuSuccessMessage = stimuNumStr + devNumStr + channelNumStr + "刺激成功" + "\n";
            //stimuSuccessMessage = stimuNumStr + devNumStr + channelNumStr;
        }

        //处理接受信息
        string  stimuSuccessMessage = " ";
        public void HandleRecBuff(byte[] ReceiveBytes)
        {
            if (ReceiveBytes[2] == 0XCC) //判断返回心跳
            {
                byte tempBit = ReceiveBytes[12];
                HeartDecideDiviceNum(tempBit);
            }
            else if (ReceiveBytes[2] == 0XDD) //返回刺激成功
            {
                StimulateSuccessMessageHandle(ReceiveBytes, ref stimuSuccessMessage);
                PortRecTextBox.AppendText(stimuSuccessMessage);
                
            }
        }

        #region//原本的数据接收函数 可以接收普通的HEX和ASCII
        //public void DataReceivedByte(object sender, SerialDataReceivedEventArgs e)
        //{

        //    if (serialPort.IsOpen)
        //    {
        //        //MessageBox.Show("sss","OK");
        //        //输出当前时间
        //        //DateTime dateTimeNow = DateTime.Now;
        //        //dateTimeNow.GetDateTimeFormats();
        //        //PortRecTextBox.Text += string.Format("{0}\r\n", dateTimeNow);
        //        //dateTimeNow.GetDateTimeFormats('f')[0].ToString() + "\r\n";
        //        PortRecTextBox.ForeColor = Color.Red;    //改变字体的颜色

        //        if (RecRadioButtonASCII.Checked == true) //接收格式为ASCII
        //        {
        //            try
        //            {
        //                String input = serialPort.ReadExisting();
        //                PortRecTextBox.Text += input + "\r\n";
        //                PortRecTextBox.AppendText("\n");
        //            }
        //            catch (System.Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "你波特率是不是有问题？？？");
        //                return;
        //            }

        //            PortRecTextBox.SelectionStart = PortRecTextBox.Text.Length;
        //            PortRecTextBox.ScrollToCaret();//滚动到光标处
        //            serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
        //        }
        //        else //接收格式为HEX
        //        {
        //            try
        //            {
        //                string input = serialPort.ReadExisting();
        //                char[] values = input.ToCharArray();
        //                foreach (char letter in values)
        //                {
        //                    // Get the integral value of the character.
        //                    int value = Convert.ToInt32(letter);
        //                    // Convert the decimal value to a hexadecimal value in string form.
        //                    string hexOutput = String.Format("{0:X}", value);
        //                    PortRecTextBox.AppendText(hexOutput + " ");
        //                    PortRecTextBox.SelectionStart = PortRecTextBox.Text.Length;
        //                    PortRecTextBox.ScrollToCaret();//滚动到光标处
        //                }
        //                PortRecTextBox.AppendText("\n");
        //                DelayMs(10);
        //            }
        //            catch (System.Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "Error");
        //                PortRecTextBox.Text = "";//清空
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("请打开某个串口", "错误提示");
        //    }
        //}
        #endregion

        public static void DelayMs(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }

        //全局变量刺激次数
        public byte stimulateTimes = 0x00;

        public void SerialDataSendByte()
        {
            #region//String strSend = serialSendData;// 
            //if (SendRadioButtonASCII.Checked == true)//以字符串 ASCII 发送
            //{
            //    serialPort.WriteLine(strSend);//发送一行数据 
            //}
            //else
            //{
            //    //16进制数据格式 HXE 发送
            //    char[] values = strSend.ToCharArray();
            //    foreach (char letter in values)
            //    {
            //        // Get the integral value of the character.
            //        int value = Convert.ToInt32(letter);
            //        // Convert the decimal value to a hexadecimal value in string form.
            //        string hexIutput = String.Format("{0:X}", value);
            //        serialPort.WriteLine(hexIutput);
            //    }
            //}
            #endregion
            if (SendRadioButtonASCII.Checked == true)//以字符串 ASCII 发送
            {
                MessageBox.Show("ASCII功能暂时不可用");
                SendRadioButtonHEX.Checked = true;
                SendRadioButtonASCII.Checked = false;
            }
            else
            {

                if (IsOpenCheckBoxNum1.Checked == true)
                {
                    UpdateChannal(StimunChannelNum1, serialSendData1);  //更新通道
                    UpdatePosAndNeg(PosNegPulseNum1, serialSendData1);//更新正负极
                    UpdatePulseLength(PulseLengthNum1, serialSendData1);//更新脉冲长度
                    UpdateFrequency(Frequency1, serialSendData1);//更新频率
                    UpdatePwm(PwmNum1, serialSendData1);//更新占空比
                    UpdatePulseNum(PulseValNum1, serialSendData1);//更新脉冲数
                    UpdatePulseInterval(PulseIntervalNum1, serialSendData1);//更新脉冲间隔
                    UpdateStimulationIntensity(StimunChannelNum1, StimulationIntensityText1, serialSendData1);//更刺激强度
                    UpdateTargetGroup(StimunChannelNum1, TargetGroupNum1, serialSendData1, ref targetGroupChoose1);//更新目标群组
                    UpdateStimTimesBit(serialSendData1, stimulateTimes);//更新发送编号
                    UpdataCheckBit(serialSendData1);//更新校验位
                    serialPort.Write(serialSendData1, 0, serialSendData1.Length);//发送数据
                }
                if (IsOpenCheckBoxNum2.Checked == true)
                {
                    UpdateChannal(StimunChannelNum2, serialSendData2);
                    UpdatePosAndNeg(PosNegPulseNum2, serialSendData2);
                    UpdatePulseLength(PulseLengthNum2, serialSendData2);
                    UpdateFrequency(Frequency2, serialSendData2);
                    UpdatePwm(PwmNum2, serialSendData2);
                    UpdatePulseNum(PulseValNum2, serialSendData2);
                    UpdatePulseInterval(PulseIntervalNum2, serialSendData2);
                    UpdateStimulationIntensity(StimunChannelNum2, StimulationIntensityText2, serialSendData2);
                    UpdateTargetGroup(StimunChannelNum2, TargetGroupNum2, serialSendData2, ref targetGroupChoose2);
                    UpdateStimTimesBit(serialSendData2, stimulateTimes);//更新发送编号
                    UpdataCheckBit(serialSendData2);
                    serialPort.Write(serialSendData2, 0, serialSendData2.Length);
                }
                if (IsOpenCheckBoxNum3.Checked == true)
                {

                    UpdateChannal(StimunChannelNum3, serialSendData3);
                    UpdatePosAndNeg(PosNegPulseNum3, serialSendData3);
                    UpdatePulseLength(PulseLengthNum3, serialSendData3);
                    UpdateFrequency(Frequency3, serialSendData3);
                    UpdatePwm(PwmNum3, serialSendData3);
                    UpdatePulseNum(PulseValNum3, serialSendData3);
                    UpdatePulseInterval(PulseIntervalNum3, serialSendData3);
                    UpdateStimulationIntensity(StimunChannelNum3, StimulationIntensityText3, serialSendData3);
                    UpdateTargetGroup(StimunChannelNum3, TargetGroupNum3, serialSendData3, ref targetGroupChoose3);
                    UpdateStimTimesBit(serialSendData3, stimulateTimes);//更新发送编号
                    UpdataCheckBit(serialSendData3);
                    serialPort.Write(serialSendData3, 0, serialSendData3.Length);
                }
                if (IsOpenCheckBoxNum4.Checked == true)
                {
                    UpdateChannal(StimunChannelNum4, serialSendData4);
                    UpdatePosAndNeg(PosNegPulseNum4, serialSendData4);
                    UpdatePulseLength(PulseLengthNum4, serialSendData4);
                    UpdateFrequency(Frequency4, serialSendData4);
                    UpdatePwm(PwmNum4, serialSendData4);
                    UpdatePulseNum(PulseValNum4, serialSendData4);
                    UpdatePulseInterval(PulseIntervalNum4, serialSendData4);
                    UpdateStimulationIntensity(StimunChannelNum4, StimulationIntensityText4, serialSendData4);
                    UpdateTargetGroup(StimunChannelNum4, TargetGroupNum4, serialSendData4, ref targetGroupChoose4);
                    UpdateStimTimesBit(serialSendData4, stimulateTimes);//更新发送编号
                    UpdataCheckBit(serialSendData4);
                    serialPort.Write(serialSendData4, 0, serialSendData4.Length);
                }

                stimulateTimes++;
                if (stimulateTimes == 0XFF)
                {
                    stimulateTimes = 0X00;
                }
            }
           
        }
        #region //初试数据接收函数
        //public string recv;
        //public void DataReceivedByte(object sender, SerialDataReceivedEventArgs e)
        //{

        //    if (serialPort.IsOpen)
        //    {
        //        DateTime dateTimeNow = DateTime.Now;
        //        PortRecTextBox.Text += string.Format("{0}\r\n", dateTimeNow);
        //        PortRecTextBox.ForeColor = Color.Red;    //改变字体的颜色
        //        recv = serialPort.ReadExisting();
        //        char[] values = recv.ToCharArray();
        //        foreach (char letter in values)
        //        {
        //            // Get the integral value of the character.
        //            int value = Convert.ToInt32(letter);
        //            // Convert the decimal value to a hexadecimal value in string form.
        //            string hexOutput = String.Format("{0:X}", value);
        //            PortRecTextBox.AppendText(hexOutput + " ");
        //            PortRecTextBox.SelectionStart = PortRecTextBox.Text.Length;
        //            PortRecTextBox.ScrollToCaret();//滚动到光标处
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("请打开某个串口", "错误提示");
        //    }
        //}
        #endregion
    }
}

