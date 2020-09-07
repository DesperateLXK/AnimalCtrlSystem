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
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;
using System.Runtime.InteropServices;   // 用 DllImport 需用此 命名空间


namespace AnimalCtrl
{
    enum SI_STATUS
    {
        SI_SUCCESS = 0x00,
        SI_INVALID_HANDLE = 0x01,
        SI_INVALID_PARAMETER = 0x02,
        SI_SYSTEM_ERROR_CODE = 0x03,
        SI_DEVICE_NOT_FOUND = 0xFF
    }

    public class PartOfUSB
    {
        [DllImport("SiUSBXp.dll", EntryPoint = "SI_GetNumDevices", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_GetNumDevices(uint lpdwNumDevices);

        [DllImport("SiUSBXp.dll", EntryPoint = "	SI_GetProductString", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_GetProductString(UInt32 dwDeviceNum, IntPtr lpvDeviceString, UInt32 dwFlags);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_Open", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_Open(UInt32 dwDevice, System.UInt32 pHandle);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_Close ", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_Close(IntPtr cyHandle);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_Read", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_Read(IntPtr cyHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, IntPtr lpdwBytesReturned, IntPtr Null);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_Write", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_Write(IntPtr cyHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, IntPtr lpdwBytesWritten, IntPtr Null);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_SetTimeouts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_SetTimeouts(ulong dwReadTimeout, ulong dwWriteTimeout);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_GetTimeouts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_GetTimeouts(IntPtr lpdwReadTimeout, IntPtr lpdwWriteTimeout);

        [DllImport("SiUSBXp.dll", EntryPoint = "SI_CheckRXQueue", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int SI_CheckRXQueue(UInt32 cyHandle, IntPtr lpdwNumBytesInQueue, IntPtr lpdwQueueStatus);


    }


    public partial class MainForm : Form
    {
        
        static bool SUCCESS = true;
        static bool FALSE = false;
        //定义静态变量
        public static UsbDevice zzuUsbDev;
        

        //全局变量类
        public  class PublicUsbVariable
        {

            public static string devInfoFilePath = System.Windows.Forms.Application.StartupPath + "/DevInfo.log";
            public static string trainInfoFilePath = System.Windows.Forms.Application.StartupPath + "/TrainInfo.log";

            /*
             * 0-0XAA 固定位
             * 1- 通道选择 ：1-1通道；2-2通道；3-3通道；4-4通道；
             * 2- 选择正负脉冲 ：0-正脉冲  1-负脉冲 2-双向
             * 3- 刺激脉冲串长度
             * 4- 刺激脉冲频率低位 
             * 5- 刺激脉冲频率高位
             * 6- 刺激脉冲的占空比
             * 7- 刺激脉冲串的间隔时间
             * 8- 刺激脉冲串的个数
             * 9- 命令序列码 无用，不需处理
             * 10- 刺激电压
             * 11- 刺激电压
             * 12- 刺激电压
             * 13- 刺激电压
             * 14- 目标组
             * 15- 17保留
             * */
            int[] sendDataOfUsb = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            int [] receiveDataOfUsb = null;
        }
        public bool UsbDevInit()
        {
            return FALSE;
        }

        public bool UsbSendData(int[] sendData)
        {

            return SUCCESS;
        }
        public bool UsbReadData(int[] sendData)
        {

            return SUCCESS;
        }

        public void UsbDevDisplayAndSaveInfoLog(bool isDisplay)
        {
            UsbRegDeviceList allDevices = UsbDevice.AllDevices;

            if (!File.Exists(PublicUsbVariable.devInfoFilePath))
            {
                // 不存在则先创建文件 
                File.Create(PublicUsbVariable.devInfoFilePath).Close();
            }
            StreamWriter swDevInfo = File.AppendText(PublicUsbVariable.devInfoFilePath);
            swDevInfo.WriteLine("****************************************************************************************************");
            swDevInfo.WriteLine("日志类型：DevInfo，时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，设备信息");
            foreach (UsbRegistry usbRegistry in allDevices)
            {
                if (usbRegistry.Open(out zzuUsbDev))
                {
                    if(isDisplay)
                        richTextBox1.AppendText("--------1-----------\n" + zzuUsbDev.Info.ToString() + "\n");
                    swDevInfo.WriteLine("--------1-----------\n" + zzuUsbDev.Info.ToString() + "\n");
                    for (int iConfig = 0; iConfig < zzuUsbDev.Configs.Count; iConfig++)
                    {
                        UsbConfigInfo configInfo = zzuUsbDev.Configs[iConfig];
                        if (isDisplay)
                            richTextBox1.AppendText("--------2-----------\n" + configInfo.ToString() + "\n");
                        swDevInfo.WriteLine("--------2-----------\n" + configInfo.ToString() + "\n");
                        ReadOnlyCollection<UsbInterfaceInfo> interfaceList = configInfo.InterfaceInfoList;
                        for (int iInterface = 0; iInterface < interfaceList.Count; iInterface++)
                        {
                            UsbInterfaceInfo interfaceInfo = interfaceList[iInterface];
                            if (isDisplay)
                                richTextBox1.AppendText("--------3-----------\n" + interfaceInfo.ToString() + "\n");
                            swDevInfo.WriteLine("--------3-----------\n" + interfaceInfo.ToString() + "\n");
                            ReadOnlyCollection<UsbEndpointInfo> endpointList = interfaceInfo.EndpointInfoList;
                            for (int iEndpoint = 0; iEndpoint < endpointList.Count; iEndpoint++)
                            {
                                if (isDisplay)
                                    richTextBox1.AppendText("--------4-----------\n" + endpointList[iEndpoint].ToString() + "\n");
                                swDevInfo.WriteLine("--------4-----------\n" + endpointList[iEndpoint].ToString() + "\n");
                            }
                        }
                    }
                }
            }
            swDevInfo.Flush();
            swDevInfo.Close();
            swDevInfo.Dispose();
        }
    }
}
