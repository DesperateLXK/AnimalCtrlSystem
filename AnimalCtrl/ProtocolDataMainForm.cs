using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;

//此文件为发送协议设置

namespace AnimalCtrl
{
    class ProtocolDataMainForm
    {

    }

    public partial class MainForm : Form
    {
        //serialSendData[0] 固定为0XAA
        public byte[] serialSendData1 = { 0xAA, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public byte[] serialSendData2 = { 0xAA, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public byte[] serialSendData3 = { 0xAA, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public byte[] serialSendData4 = { 0xAA, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        public string targetGroupChoose1 = ""; //用来显示组别选择的字符串
        public string targetGroupChoose2 = "";
        public string targetGroupChoose3 = "";
        public string targetGroupChoose4 = "";
        //更新刺激通道 serialSendData[1] 低位
        public void UpdateChannal(ComboBox StimunChannel, byte[] serialSendData)
        {
            byte temp = 0xF0;
            serialSendData[1] = Convert.ToByte(serialSendData[1] & temp);//低位清零
            if (StimunChannel.SelectedIndex == 0)//1通道
            {
                serialSendData[1] = 0x01;
            }
            else if (StimunChannel.SelectedIndex == 1)//2通道
            {
                serialSendData[1] = 0x04;
            }
            else if (StimunChannel.SelectedIndex == 2)//3通道
            {
                serialSendData[1] = 0x03;
            }
            else if (StimunChannel.SelectedIndex == 3)//4通道
            {
                serialSendData[1] = 0x06;
            }
            else if (StimunChannel.SelectedIndex == 4)//1-3
            {
                serialSendData[1] = 0x02;
            }
            else if (StimunChannel.SelectedIndex == 5)//1-4
            {
                serialSendData[1] = 0x08;
            }
            else if (StimunChannel.SelectedIndex == 6)//2-3
            {
                serialSendData[1] = 0x05;
            }
            else if (StimunChannel.SelectedIndex == 7)//2-4
            {
                serialSendData[1] = 0x09;
            }
        }
        //更新正负极serialSendData[1] 高位
        public void UpdatePosAndNeg(ComboBox PosAndNeg, byte[] serialSendData)
        {
            byte temp = 0x0F;
            serialSendData[1] = Convert.ToByte(serialSendData[1] & temp);//高位清零
            if (PosAndNeg.SelectedIndex == 0) //正脉冲
            {
                serialSendData[1] = Convert.ToByte((serialSendData[1] + 0x00));
            }
            else if (PosAndNeg.SelectedIndex == 1) //负脉冲
            {
                serialSendData[1] = Convert.ToByte((serialSendData[1] + 0x40));
            }
            else if (PosAndNeg.SelectedIndex == 2)  //双向脉冲
            {
                serialSendData[1] = Convert.ToByte((serialSendData[1] + 0x80));
            }
        }
        //更新脉冲长度  serialSendData[2]
        public void UpdatePulseLength(NumericUpDown PulseLength, byte[] serialSendData)
        {
            byte temp = 0x00;
            serialSendData[2] = Convert.ToByte(serialSendData[2] & temp);//清零
            serialSendData[2] = Convert.ToByte(PulseLength.Value);
        }
        //更新频率 serialSendData[3] 高位  serialSendData[4]低位
        public void UpdateFrequency(NumericUpDown Frequency, byte[] serialSendData)
        {
            byte temp = 0x00;
            int tempInt16 = Convert.ToInt16(Frequency.Value); //转化为16位整数型
            serialSendData[3] = Convert.ToByte(serialSendData[3] & temp);//清零
            serialSendData[4] = Convert.ToByte(serialSendData[4] & temp);//清零
            serialSendData[3] = Convert.ToByte((tempInt16 >> 8) & 0x00FF);//高位
            serialSendData[4] = Convert.ToByte(tempInt16 & 0x00FF); //低位
        }
        //更新PWM serialSendData[5]
        public void UpdatePwm(NumericUpDown Pwm, byte[] serialSendData)
        {
            byte temp = 0x00;
            serialSendData[5] = Convert.ToByte(serialSendData[5] & temp);//清零
            serialSendData[5] = Convert.ToByte(Pwm.Value);
        }

        //更新脉冲数 serialSendData[8]
        public void UpdatePulseNum(NumericUpDown PulseNum, byte[] serialSendData)
        {
            byte temp = 0x00;
            serialSendData[8] = Convert.ToByte(serialSendData[8] & temp);//清零
            serialSendData[8] = Convert.ToByte(PulseNum.Value);
        }

        //更新脉冲间隔 serialSendData[9]
        public void UpdatePulseInterval(NumericUpDown UpdatePulseInterval, byte[] serialSendData)
        {
            byte temp = 0x00;
            serialSendData[9] = Convert.ToByte(serialSendData[8] & temp);//清零
            serialSendData[9] = Convert.ToByte((UpdatePulseInterval.Value / 100)); //分度值为100
        }

        //更新刺激强度 serialSendData[6]  1-2通道高位  serialSendData[7]1-2通道低位
        //serialSendData[10]3-4通道高位     serialSendData[11]3-4通道低位
        public void UpdateStimulationIntensity(ComboBox StimunChannel, TextBox StimulationIntensity, byte[] serialSendData)
        {
            byte temp = 0x00;
            int tempInt16 = Convert.ToInt16(StimulationIntensity.Text);

            serialSendData[6] = Convert.ToByte(serialSendData[6] & temp);//清零
            serialSendData[7] = Convert.ToByte(serialSendData[7] & temp);//清零
            serialSendData[10] = Convert.ToByte(serialSendData[10] & temp);//清零
            serialSendData[11] = Convert.ToByte(serialSendData[11] & temp);//清零

            if ((StimunChannel.SelectedIndex == 0) || (StimunChannel.SelectedIndex == 1) || (StimunChannel.SelectedIndex == 2) || (StimunChannel.SelectedIndex == 3))//1通道2通道34通道
            {
                serialSendData[6] = Convert.ToByte((tempInt16 >> 8) & 0x00FF);//高位
                serialSendData[7] = Convert.ToByte(tempInt16 & 0x00FF); //低位
                serialSendData[10] = Convert.ToByte((tempInt16 >> 8) & 0x00FF);//高位
                serialSendData[11] = Convert.ToByte(tempInt16 & 0x00FF); //低位
            }
            else if ((StimunChannel.SelectedIndex == 4) || (StimunChannel.SelectedIndex == 5) || (StimunChannel.SelectedIndex == 6) || (StimunChannel.SelectedIndex == 7))//1-3 1-4 2-3 2-4
            {
                serialSendData[6] = Convert.ToByte((tempInt16 >> 8) & 0x00FF);//高位
                serialSendData[7] = Convert.ToByte(tempInt16 & 0x00FF); //低位
                serialSendData[10] = Convert.ToByte((tempInt16 >> 8) & 0x00FF);//高位
                serialSendData[11] = Convert.ToByte(tempInt16 & 0x00FF); //低位
            }
        }

        //位型转化为Int型
        public  int BitToInt(BitArray bit)
        {
            int res = 0;

            for (int i = bit.Count - 1; i >= 0; i--)
            {
                res = bit[i] ? res + (1 << i) : res;
            }
            return res;
        }

        public void UpdateTargetGroup(ComboBox StimunChannel, CheckedListBox TargetGroup, byte[] serialSendData,  ref string targetGroupChoose)
        {

            BitArray bitArray = new BitArray(8);        //创建一个8位的Bit位数组 因为群组一共有8个选项
            int tempInt16 = 0;
            byte temp = 0x00;
            targetGroupChoose = " ";//先清除
            serialSendData[12] = Convert.ToByte(serialSendData[12] & temp);// 清零
            //遍历所有选项看是否选中 选择第一组的话 0x80
            for (int i = 0; i < TargetGroup.Items.Count; i++)
            {
                if (TargetGroup.GetItemChecked(i))
                {
                    bitArray.Set(TargetGroup.Items.Count - 1 - i, true);
                    targetGroupChoose += (i+1).ToString() + "组 ";
                }
                else {
                    bitArray.Set(TargetGroup.Items.Count - 1 - i, false);
                }                
            }
            tempInt16 = BitToInt(bitArray);
            serialSendData[12] = Convert.ToByte(tempInt16 & 0x00FF); //只需要取低位
        }

        //serialSendData[14] 所有和加起来除以256的余数
        public void UpdataCheckBit(byte[] serialSendData)
        {
            int tempInt16 = 0;
            serialSendData[14] = 0x00; //先清零
            for (int i = 0; i < serialSendData.Length; i++)
            {
                tempInt16 += Convert.ToInt16( serialSendData[i] );
            }
            tempInt16 = tempInt16 % 256;
            serialSendData[14] = Convert.ToByte(tempInt16 & 0x00FF); //只需要取低位
        }
    }
}
