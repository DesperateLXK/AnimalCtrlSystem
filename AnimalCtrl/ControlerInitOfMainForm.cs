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


//此文件进行主界面各种控件的初始化

namespace AnimalCtrl
{
   public partial class ControlerInitOfMainForm
    {
        static int StimunChannel_MAX = 4;
        /************************************************************************************/
        //对ComboBox进行数值绑定
        //刺激通道
        public static ArrayList StimunChannelList()
        {
            ArrayList StimunChannelList = new ArrayList();
            int i = 0;
            for (i = 0; i < StimunChannel_MAX; i++)
            {
                StimunChannelList.Add(new DictionaryEntry( i+1, i+1+"通道"));
            }
            StimunChannelList.Add(new DictionaryEntry(++i, "1-3通道"));
            StimunChannelList.Add(new DictionaryEntry(++i, "1-4通道"));
            StimunChannelList.Add(new DictionaryEntry(++i, "2-3通道"));
            StimunChannelList.Add(new DictionaryEntry(++i, "2-4通道"));
            return StimunChannelList;
        }
        //正负脉冲
        public static ArrayList PosNegPulseList()
        {
            ArrayList PosNegPulseList = new ArrayList();
            PosNegPulseList.Add(new DictionaryEntry(0, "正脉冲"));
            PosNegPulseList.Add(new DictionaryEntry(1, "负脉冲"));
            PosNegPulseList.Add(new DictionaryEntry(2, "双向"));
            return PosNegPulseList;
        }
        
        /************************************************************************************/

     }
    //Frequency1
    public partial class MainForm : Form
    {
        static int CHECKBOX_MAX = 8;
        public void ComboBoxInit()
        {
            //脉冲通道
            this.StimunChannelNum1.DataSource = ControlerInitOfMainForm.StimunChannelList();
            this.StimunChannelNum1.DisplayMember = "Value";
            this.StimunChannelNum1.ValueMember = "Key";
            this.StimunChannelNum1.SelectedIndex = 0;

            this.StimunChannelNum2.DataSource = ControlerInitOfMainForm.StimunChannelList();
            this.StimunChannelNum2.DisplayMember = "Value";
            this.StimunChannelNum2.ValueMember = "Key";
            this.StimunChannelNum2.SelectedIndex = 1;

            this.StimunChannelNum3.DataSource = ControlerInitOfMainForm.StimunChannelList();
            this.StimunChannelNum3.DisplayMember = "Value";
            this.StimunChannelNum3.ValueMember = "Key";
            this.StimunChannelNum3.SelectedIndex = 2;

            this.StimunChannelNum4.DataSource = ControlerInitOfMainForm.StimunChannelList();
            this.StimunChannelNum4.DisplayMember = "Value";
            this.StimunChannelNum4.ValueMember = "Key";
            this.StimunChannelNum4.SelectedIndex = 3;

            //正负脉冲
            this.PosNegPulseNum1.DataSource = ControlerInitOfMainForm.PosNegPulseList();
            this.PosNegPulseNum1.DisplayMember = "Value";
            this.PosNegPulseNum1.ValueMember = "Key";
            this.PosNegPulseNum1.SelectedIndex = 2;

            this.PosNegPulseNum2.DataSource = ControlerInitOfMainForm.PosNegPulseList();
            this.PosNegPulseNum2.DisplayMember = "Value";
            this.PosNegPulseNum2.ValueMember = "Key";
            this.PosNegPulseNum2.SelectedIndex = 2;

            this.PosNegPulseNum3.DataSource = ControlerInitOfMainForm.PosNegPulseList();
            this.PosNegPulseNum3.DisplayMember = "Value";
            this.PosNegPulseNum3.ValueMember = "Key";
            this.PosNegPulseNum3.SelectedIndex = 2;

            this.PosNegPulseNum4.DataSource = ControlerInitOfMainForm.PosNegPulseList();
            this.PosNegPulseNum4.DisplayMember = "Value";
            this.PosNegPulseNum4.ValueMember = "Key";
            this.PosNegPulseNum4.SelectedIndex = 2;
            //

        }

        //CheckBox的初始化
        public void CheckBoxInit()
        {

            for (int i = 0; i < CHECKBOX_MAX; i++)
            {
                this.TargetGroupNum1.Items.Add((i + 1) + "组");
                this.TargetGroupNum2.Items.Add((i + 1) + "组");
                this.TargetGroupNum3.Items.Add((i + 1) + "组");
                this.TargetGroupNum4.Items.Add((i + 1) + "组");
            }
            IsOpenCheckBoxNum1.Checked = true; //默认1路打开
            SerialCheckBox.Checked = true;
            OpenIntervalSendCheckBox.Checked = false;
            this.SerialCheckBox.Enabled = false;
        }
        //文本框的初始化
        public void TextBoxInit()
        {
            this.richTextBox1.AppendText("********************************************\n");
            this.richTextBox1.AppendText("Welcome to use this control system!\n");
            this.richTextBox1.AppendText("       It is designed for NIEM_ZZU!     \n");
            this.richTextBox1.AppendText("********************************************\n");

            IntervalTextBox.Enabled = false;
        }

        public void ButtonInit()
        {
            this.ResetPortConfig.Enabled = false;
            this.SendDataButton.Enabled = false;
            this.IntervalSendStartButton.Enabled = false;
        }

        public void GroupBoxInit()
        {
            this.SendGroupBox.Enabled = false;
        }
        //显示时间的LABLE
        public void ShowTimeLableInit()
        {
            DateTimer.Interval = 100; //更新间隔100毫秒
            DateTimer.Start();
        }
    }
}
