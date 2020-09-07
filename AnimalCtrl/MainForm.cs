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
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;
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
            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //绑定串口数据接收事件函数
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedTextLine);
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedByte);
            this.ControlInit();//各种控件的初始化 包括按钮
            this.SerialPortInit();//串口出初始化
            //this.UsbDevDisplayAndSaveInfoLog(false);
        }

        public void ControlInit()
        {
            ComboBoxInit();  //下拉列表初始化
            CheckBoxInit();    //选择列表初始化
            ShowTimeLableInit();//时间显示初始化
            TextBoxInit();  //文本框初始化
            ButtonInit(); //按钮初始化
            GroupBoxInit();//控件组初始化
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
            MessageBox.Show("This is an EASTER EGG.\n\n" + "               Author: Lxk_NIEM", "GOOD LUCK ^_^");
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


        //注释的语句可以设置CheckList只能选择一项
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
        
        #region  //训练日志显示以及储存
        public void TrainLogDisplayAndSave()
        {
            //判断通道是否都打开
            int temp = 0;
            if (!File.Exists(PublicUsbVariable.trainInfoFilePath))
            {
                // 不存在则先创建文件 
                File.Create(PublicUsbVariable.trainInfoFilePath).Close();
            }

            StreamWriter swTrainInfo = File.AppendText(PublicUsbVariable.trainInfoFilePath);

            richTextBox1.AppendText("***********************************************\n");
            richTextBox1.AppendText("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
            richTextBox1.AppendText("启用选择：" + IsOpenCheckBoxNum1.Text.ToString() + "--" + IsOpenCheckBoxNum2.Text.ToString() + "--" + IsOpenCheckBoxNum3.Text.ToString() + "--" + IsOpenCheckBoxNum4.Text.ToString() + "\n");
            if (IsOpenCheckBoxNum1.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第1路参数=========\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum1.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency1.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum1.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum1.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum1.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText1.Text.ToString() + "\n");
                richTextBox1.AppendText("目标组：" + targetGroupChoose1.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum2.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第2路参数=========\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum2.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency2.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum2.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum2.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum2.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText2.Text.ToString() + "\n");
                richTextBox1.AppendText("目标组：" + targetGroupChoose2.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum3.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第3路参数=========\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum3.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency3.Value.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum3.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum3.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum3.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText3.Text.ToString() + "\n");
                richTextBox1.AppendText("目标组：" + targetGroupChoose3.ToString() + "\n");
                temp += 1;
            }
            if (IsOpenCheckBoxNum4.CheckState.ToString() == "Checked")
            {
                richTextBox1.AppendText("=========第4路参数=========\n");
                richTextBox1.AppendText("刺激通道：" + StimunChannelNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("脉冲选择：" + PosNegPulseNum4.Text.ToString() + "\n");
                richTextBox1.AppendText("脉冲长度：" + PulseLengthNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("频率：" + Frequency4.Text.ToString() + "Hz\n");
                richTextBox1.AppendText("占空比：" + PwmNum4.Text.ToString() + "%" + "\t");
                richTextBox1.AppendText("脉冲间隔：" + PulseIntervalNum4.Text.ToString() + "ms\n");
                richTextBox1.AppendText("脉冲串数：" + PulseValNum4.Text.ToString() + "\t");
                richTextBox1.AppendText("刺激强度：" + StimulationIntensityText4.Text.ToString() + "\n");
                richTextBox1.AppendText("目标组：" + targetGroupChoose4.ToString() + "\n");
                temp += 1;
            }
            richTextBox1.AppendText("===================\n");
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

            }
        }

        private void OpenIntervalSendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenIntervalSendCheckBox.Checked == true)
            {
                SendDataButton.Enabled = false;
                IntervalTextBox.Enabled = true;
                IntervalSendStartButton.Enabled = true;
            }
            else
            {
                SendDataButton.Enabled = true;
                IntervalTextBox.Enabled = false;
                IntervalSendStartButton.Enabled = false;
            }
        }

        private void IntervalSendStartButton_Click(object sender, EventArgs e)
        {
            if (IntervalSendStartButton.Text == "开始")
            {
                IntervalSendTimer.Interval = Convert.ToInt32(IntervalTextBox.Text);
                IntervalSendTimer.Start();
                IntervalSendStartButton.Text = "停止";
            }
            else if (IntervalSendStartButton.Text == "停止")
            {
                IntervalSendTimer.Stop();
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
    }
}
