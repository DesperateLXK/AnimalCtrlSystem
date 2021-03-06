﻿using System;
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
using System.Runtime.InteropServices;
namespace AnimalCtrl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //解决窗体最小限制
            Size newSize = new Size(1200, 810);
            this.MaximumSize = this.MinimumSize = newSize;
            this.Size = newSize;

            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //绑定串口数据接收事件函数
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedByte);
            this.ControlInit();//各种控件的初始化 包括按钮
            this.SerialPortInit();//串口出初始化
        }

        public void ControlInit()
        {
            ComboBoxInit();  //下拉列表初始化
            CheckBoxInit();    //选择列表初始化
            //ShowTimeLableInit();//时间显示初始化
            TimerInit();//各种时间初始化
            TextBoxInit();  //文本框初始化
            ButtonInit(); //按钮初始化
            GroupBoxInit();//控件组初始化
            MyLedControlInit();//LED初始化
        }

        //以下为各种控件引起的事件
        //显示时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.TimeShowLable.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            this.TimeShowLable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void label17_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("This is an EASTER EGG.\n\n" + "               Author: NIEM_LK", "GOOD LUCK ^_^");
        }


        //文本框，选项框只能输入数字
        private void StimulationIntensityText1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }
        }

        private void StimulationIntensityText2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }
        }

        private void StimulationIntensityText3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }
        }

        private void StimulationIntensityText4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }
        }

        private void IntervalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }
        }


        #region//注释的语句可以设置CheckList只能选择一项
        private void TargetGroupNum1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.CurrentValue == CheckState.Checked) return;//取消选中就不用进行以下操作
            //for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++)
            //{
            //    ((CheckedListBox)sender).SetItemChecked(i, false);//将所有选项设为不选中
            //}
            ///* //事件
            //if (TargetGroupNum4.SelectedIndex == 8)
            //{
            //    MessageBox.Show("");
            //}
            //*/
            //e.NewValue = CheckState.Checked;//刷新
        }

        private void TargetGroupNum2_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void TargetGroupNum3_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void TargetGroupNum4_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }
        #endregion


        #region  //训练日志显示以及储存
        public void TrainLogDisplayAndSave()
        {
            string trainInfoFilePath = System.Windows.Forms.Application.StartupPath + "/TrainInfo.log";
            //判断通道是否都打开
            int temp = 0;
            if (!File.Exists(trainInfoFilePath))
            {
                // 不存在则先创建文件 
                File.Create(trainInfoFilePath).Close();
            }

            StreamWriter swTrainInfo = File.AppendText(trainInfoFilePath);

            richTextBox1.AppendText("***********************************************\n");
            richTextBox1.AppendText("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
            richTextBox1.AppendText("启用选择：" + IsOpenCheckBoxNum1.Text.ToString() + "--" + IsOpenCheckBoxNum2.Text.ToString() + "--" + IsOpenCheckBoxNum3.Text.ToString() + "--" + IsOpenCheckBoxNum4.Text.ToString() + "\n");
            if (IsOpenCheckBoxNum1.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第1路参数=========\n");
                richTextBox1.AppendText("刺激编号：" + (Convert.ToInt16(serialSendData1[13]) + 1).ToString() + "\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum1.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency1.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum1.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum1.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText1.Text.ToString() + "\n");
                richTextBox1.AppendText("目标设备编号：" + targetGroupChoose1.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum2.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第2路参数=========\n");
                richTextBox1.AppendText("刺激编号：" + (Convert.ToInt16(serialSendData2[13]) + 1).ToString() + "\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum2.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency2.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum2.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum2.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText2.Text.ToString() + "\n");
                richTextBox1.AppendText("目标设备编号：" + targetGroupChoose2.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum3.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第3路参数=========\n");
                richTextBox1.AppendText("刺激编号：" + (Convert.ToInt16(serialSendData3[13]) + 1).ToString() + "\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum3.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency3.Value.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum3.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum3.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText3.Text.ToString() + "\n");
                richTextBox1.AppendText("目标设备编号：" + targetGroupChoose3.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum4.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第4路参数=========\n");
                richTextBox1.AppendText("刺激编号：" + (Convert.ToInt16(serialSendData4[13]) + 1).ToString() + "\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum4.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency4.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum4.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum4.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText4.Text.ToString() + "\n");
                richTextBox1.AppendText("目标设备编号：" + targetGroupChoose4.ToString() + "\n");
                temp += 1;
            }
            richTextBox1.AppendText("========================\n");
            //判断通道是否都打开
            if (temp > 0)
            {
                richTextBox1.AppendText("发送状态：OK\n");
                temp = 0;
            }
            else
            {
                richTextBox1.AppendText("发送状态：ERROR (所有通路均未开启)\n");
            }
            richTextBox1.AppendText("***********************************************\n");
            richTextBox1.ScrollToCaret();
            swTrainInfo.Write(richTextBox1.Text.ToString() + "\n");
            swTrainInfo.Flush();
            swTrainInfo.Close();
            swTrainInfo.Dispose();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
        }


        private void IsOpenCheckBoxNum1_CheckedChanged(object sender, EventArgs e)
        {
            if (IsOpenCheckBoxNum1.CheckState.ToString() == "Unchecked")
            {
                IsOpenCheckBoxNum1.Text = "未启用";
            }
            else if (IsOpenCheckBoxNum1.CheckState.ToString() == "Checked")
            {
                IsOpenCheckBoxNum1.Text = "启用";
            }
        }

        private void IsOpenCheckBoxNum2_CheckedChanged(object sender, EventArgs e)
        {
            if (IsOpenCheckBoxNum2.CheckState.ToString() == "Unchecked")
            {
                IsOpenCheckBoxNum2.Text = "未启用";
            }
            else if (IsOpenCheckBoxNum2.CheckState.ToString() == "Checked")
            {
                IsOpenCheckBoxNum2.Text = "启用";
            }
        }

        private void IsOpenCheckBoxNum3_CheckedChanged(object sender, EventArgs e)
        {
            if (IsOpenCheckBoxNum3.CheckState.ToString() == "Unchecked")
            {
                IsOpenCheckBoxNum3.Text = "未启用";
            }
            else if (IsOpenCheckBoxNum3.CheckState.ToString() == "Checked")
            {
                IsOpenCheckBoxNum3.Text = "启用";
            }
        }

        private void IsOpenCheckBoxNum4_CheckedChanged(object sender, EventArgs e)
        {
            if (IsOpenCheckBoxNum4.CheckState.ToString() == "Unchecked")
            {
                IsOpenCheckBoxNum4.Text = "未启用";
            }
            else if (IsOpenCheckBoxNum4.CheckState.ToString() == "Checked")
            {
                IsOpenCheckBoxNum4.Text = "启用";
            }
        }

        private void CleanSerialRecvTextBoxButton_Click(object sender, EventArgs e)
        {
            PortRecTextBox.Text = string.Empty;
            richTextBox1.ScrollToCaret();
        }
        //发送键
        private void SendDataButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
            if (SerialCheckBox.Checked == true)
            {
                if (!serialPort.IsOpen)
                {
                    MessageBox.Show("请先打开串口", "Error");
                    return;
                }
                //发送字节 使用此函数
                SerialDataSendByte();
                TrainLogDisplayAndSave();//显示
            }
            else
            {
                return;
            }
        }

        private void OpenIntervalSendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenIntervalSendCheckBox.Checked == true)
            {
                OpenClosePortButton.Enabled = false;
                SendDataButton.Enabled = false;
                IntervalTextBox.Enabled = true;
                IntervalSendStartButton.Enabled = true;
                ResetPortConfig.Enabled = false;
            }
            else
            {
                OpenClosePortButton.Enabled = true;
                ResetPortConfig.Enabled = true;
                SendDataButton.Enabled = true;
                IntervalTextBox.Enabled = false;
                IntervalSendStartButton.Enabled = false;
            }
        }

        static int intervalSendValMAX = 6000000; //定时发送间隔时间最大为6000秒
        static int intervalSendValMIN = 300;   //最小时间为0.3秒
        private void IntervalSendStartButton_Click(object sender, EventArgs e)
        {
            if (IntervalSendStartButton.Text == "开始")
            {
                if (IntervalTextBox.Text == string.Empty)
                {
                    MessageBox.Show("请输入数字\n", "ERROR");
                    return;
                }

                int intervalVal = Convert.ToInt32(IntervalTextBox.Text);

                if (intervalVal < intervalSendValMIN)
                {
                    IntervalTextBox.Text = intervalSendValMIN.ToString();
                    intervalVal = intervalSendValMIN;
                }
                else if (intervalVal > intervalSendValMAX)
                {
                    IntervalTextBox.Text = intervalSendValMAX.ToString();
                    intervalVal = intervalSendValMAX;
                }

                IntervalSendTimer.Interval = intervalVal;
                
                IntervalSendTimer.Start();
                OpenIntervalSendCheckBox.Enabled = false;
                IntervalTextBox.Enabled = false;
                IntervalSendStartButton.Text = "停止";
            }
            else if (IntervalSendStartButton.Text == "停止")
            {
                IntervalSendTimer.Stop();
                OpenIntervalSendCheckBox.Enabled = true;
                IntervalTextBox.Enabled = true;
                IntervalSendStartButton.Text = "开始";
            }
        }

        private void IntervalSendTimer_Tick(object sender, EventArgs e)
        {
            if (SerialCheckBox.Checked == true)
            {
                richTextBox1.Text = string.Empty;
                SerialDataSendByte();
                TrainLogDisplayAndSave();
            }
            else
            {
                return;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定退出？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                this.Dispose();
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        int heartTimes = 0;
        int heartTimesFlag = 0;
        int heartFlag = 1;
        //每次都发送的心跳指令
        public byte[] serialSendHeartData = { 0XBB, 0XCC, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00 };
        private void HeartTimer_Tick(object sender, EventArgs e)
        {
            //如果开启了
            if (serialPort.IsOpen)
            {

                

                if (heartTimesFlag == 1) //先发轮询送一次
                {
                    heartTimesFlag = 0;
                    serialSendHeartData[12] = 0x80;
                    serialSendHeartData[14] = 0x07;
                    serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);
                    DelayMs(50);//延时50MS
                    for (int i = 0; i < 7; i++)
                    {
                        int tempInt16 = 0;
                        serialSendHeartData[12] = (byte)(serialSendHeartData[12] / 2);
                        serialSendHeartData[14] = 0X00;//清零
                        for (int k = 0; k < serialSendHeartData.Length; k++)
                        {
                            tempInt16 += Convert.ToInt16(serialSendHeartData[k]);
                        }
                        tempInt16 = tempInt16 % 256;
                        serialSendHeartData[14] = Convert.ToByte(tempInt16 & 0x00FF); //只需要取低位
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);
                    }
                    return;
                }
                //再发送
                HeartTimer.Interval = 1000; //每隔8秒发一轮
                //CtrlMyLedStatus(false, true);
                switch (heartFlag)
                {
                    case 1:
                        serialSendHeartData[12] = 0x80;
                        //serialSendHeartData[14] = 0x3B;
                        serialSendHeartData[14] = 0x07;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 2:
                        serialSendHeartData[12] = 0x40;
                        //serialSendHeartData[14] = 0xFB;
                        serialSendHeartData[14] = 0xC7;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 3:
                        serialSendHeartData[12] = 0x20;
                        //serialSendHeartData[14] = 0xDB;
                        serialSendHeartData[14] = 0xA7;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 4:
                        serialSendHeartData[12] = 0x10;
                        //serialSendHeartData[14] = 0xCB;
                        serialSendHeartData[14] = 0x97;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 5:
                        serialSendHeartData[12] = 0x08;
                        //serialSendHeartData[14] = 0xC3;
                        serialSendHeartData[14] = 0x8F;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 6:
                        serialSendHeartData[12] = 0x04;
                        //serialSendHeartData[14] = 0xBF;
                        serialSendHeartData[14] = 0x8B;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 7:
                        serialSendHeartData[12] = 0x02;
                        //serialSendHeartData[14] = 0xBD;
                        serialSendHeartData[14] = 0x89;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                    case 8:
                        serialSendHeartData[12] = 0x01;
                        //serialSendHeartData[14] = 0xBC;
                        serialSendHeartData[14] = 0x88;
                        serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                        break;
                }


                if (heartFlag == 8)
                {
                    heartFlag = 1;
                    return;
                }
                heartFlag++;
                //serialPort.Write(serialSendHeartData, 0, serialSendHeartData.Length);//发送数据
                //serialPort.Write(DEBUG_testVal, 0, DEBUG_testVal.Length);//发送数据
                //serialPort.Write(DEBUG_testVal2, 0, DEBUG_testVal2.Length);//发送数据
                
            }
            else {
                heartTimesFlag = 1;
                CtrlMyLedStatus(false, true);
                HeartTimer.Interval = 100;
            }
        }

        public long rageHeartTimeFlag = 0;
        private void RageHeartSignal_Tick(object sender, EventArgs e)
        {
            rageHeartTimeFlag = GetTimeStamp(false);
            for (int i = 0; i < 8; i++)
            {
                if (rageHeartTimeFlag - heartDataTimeTemp[i] > 12)
                {
                    CtrlMyLedStatus(false, i + 1); //大于12秒 熄灭
                }
            }
        }
        //测试使用
        //AA EE DD 81 00 00 00 00 00 00 00 00 80 32 A8 FF
        //AA EE DD 89 00 00 00 00 00 00 00 00 40 22 60 FF
        //AA EE CC 00 00 00 00 00 00 00 00 00 80 00 E4 FF
        //AA EE CC 00 00 00 00 00 00 00 00 00 40 00 A4 FF
        //public byte[] DEBUG_testVal = { 0XAA, 0XEE, 0XCC, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X80, 0X00, 0XE4, 0XFF };
        //public byte[] DEBUG_testVal2 = { 0XAA, 0XEE, 0XCC, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X40, 0X00, 0XA4, 0XFF };
        public byte[] DEBUG_testVal3 = { 0XAA, 0XEE, 0XDD, 0X81, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X80, 0X32, 0XA8, 0XFF };

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort.Write(DEBUG_testVal3, 0, DEBUG_testVal3.Length);
        }


        private void PortRecTextBox_TextChanged(object sender, EventArgs e)
        {
            //PortRecTextBox.Refresh();
        }


    }
}
