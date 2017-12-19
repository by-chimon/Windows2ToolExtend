using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Windows2ToolExtend
{
    public partial class Form1 : Form
    {
        bool wintype = Environment.Is64BitOperatingSystem;//判断32位或64位
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        bool beginMove = false;//初始化鼠标位置  
        int currentXPosition;
        int currentYPosition;
        public void Waiting()
        {
            toolStripStatusLabel1.Text = "执行中";
            statusStrip1.BackColor = Color.Red;
        }

        public void IsOver()
        {
            toolStripStatusLabel1.Text = "就绪";
            statusStrip1.BackColor = Color.OrangeRed;
        }

        public static void ReFileManager()
        {
            string cmd = @"gpupdate /force
            taskkill /f /im explorer.exe
            start %systemroot%\explorer";
            CmdHelper.RunCmd(cmd);
        }//强制更新组策略并重启资源管理器，执行此项可能会造成系统不稳定

        public static void WipeIconCache()
        {
            string cmd = @"del %userprofile%\AppData\Local\iconcache.db /f /q
            taskkill /f /im explorer.exe
            start %systemroot%\explorer";
            CmdHelper.RunCmd(cmd);
        }//清除图标缓存

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label13.Text = "NT" + " " + Convert.ToString(Environment.OSVersion.Version);
            if (wintype == true)
            {
                label15.Text = "64 bit";
            }
            if (wintype == false)
            {
                label15.Text = "32 bit";
            }
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 企业版KMS激活ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Waiting);
            th.Start();
            string cmd = @"echo.&echo  连接KMS服务器...
            ping www.33161000.com | findstr TTL > nul && goto yes || goto no

            :yes
            cls
            echo.&echo  成功连接KMS服务器！尝试激活...
            slmgr.vbs /ipk NPPR9-FWDCX-D2C8J-H872K-2YT43
            slmgr.vbs /skms www.33161000.com
            slmgr.vbs /ato
            goto ex

            :no
            cls
            echo  无法连接KMS服务器，请检查电脑是否联网！
            goto ex

            :ex
            echo OVER";
            CmdHelper.RunCmd(cmd);
            IsOver();
        }

        private void 本机密钥查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wintype == true)
            {
                string info = "";
                RegistryKey Key;
                Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                //读取64位操作系统注册表，32位不可用
                RegistryKey myreg;
                myreg = Key.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SoftwareProtectionPlatform");
                info = Convert.ToString(myreg.GetValue("BackupProductKeyDefault"));
                myreg.Close();
                if (info == null)
                {
                    MessageBox.Show("没有找到产品密钥！");
                }
                else
                {
                    MessageBox.Show(info, "您的产品密钥是");
                }
            }
            else
            {
                string info = "";
                RegistryKey Key;
                Key = Registry.LocalMachine;
                //读取32位操作系统注册表，64位不可用，OEM设备可能不支持
                RegistryKey myreg;
                myreg = Key.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SoftwareProtectionPlatform");
                info = Convert.ToString(myreg.GetValue("BackupProductKeyDefault"));
                myreg.Close();
                if (info == null)
                {
                    MessageBox.Show("没有找到产品密钥！");
                }
                else
                {
                    MessageBox.Show(info, "您的产品密钥是");
                }
            }
        }

        private void 关闭程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 软件信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.ShowDialog();
        }

        private void 专业版KMS激活实验性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Waiting);
            th.Start();
            string cmd = @"echo.&echo  连接KMS服务器...
            ping www.33161000.com | findstr TTL > nul && goto yes || goto no

            :yes
            cls
            echo.&echo  成功连接KMS服务器！尝试激活...
            slmgr.vbs /ipk W269N-WFGWX-YVC9B-4J6C9-T83GX
            slmgr.vbs /skms www.33161000.com
            slmgr.vbs /ato
            goto ex

            :no
            cls
            echo  无法连接KMS服务器，请检查电脑是否联网！
            goto ex

            :ex
            echo OVER";
            CmdHelper.RunCmd(cmd);
            IsOver();
        }

        private void Win10教育版KMS激活实验性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Waiting);
            th.Start();
            string cmd = @"echo.&echo  连接KMS服务器...
            ping www.33161000.com | findstr TTL > nul && goto yes || goto no

            :yes
            cls
            echo.&echo  成功连接KMS服务器！尝试激活...
            slmgr.vbs /ipk NW6C2-QMPVW-D7KKK-3GKT6-VCFB2
            slmgr.vbs /skms www.33161000.com
            slmgr.vbs /ato
            goto ex

            :no
            cls
            echo  无法连接KMS服务器，请检查电脑是否联网！
            goto ex

            :ex
            echo OVER";
            CmdHelper.RunCmd(cmd);
            IsOver();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey("Control Panel\\Desktop", true);
                    myreg.SetValue("PaintDesktopVersion", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey("Control Panel\\Desktop", true);
                    myreg.SetValue("PaintDesktopVersion", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启生效");
                    IsOver();
                }
            }
            if (comboBox1.Text == "不显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey("Control Panel\\Desktop", true);
                    myreg.SetValue("PaintDesktopVersion", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey("Control Panel\\Desktop", true);
                    myreg.SetValue("PaintDesktopVersion", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启生效");
                    IsOver();
                }
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Control Panel\International", true);
                    myreg.SetValue("sLongDate", "yyyy'年'M'月'd'日',dddd");
                    myreg.SetValue("sShortDate", "yyyy/M/d/ddd");
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Control Panel\International", true);
                    myreg.SetValue("sLongDate", "yyyy'年'M'月'd'日',dddd");
                    myreg.SetValue("sShortDate", "yyyy/M/d/ddd");
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
            if (comboBox2.Text == "不显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Control Panel\International", true);
                    myreg.SetValue("sLongDate", "yyyy'年'M'月'd'日'");
                    myreg.SetValue("sShortDate", "yyyy/M/d");
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Control Panel\International", true);
                    myreg.SetValue("sLongDate", "yyyy'年'M'月'd'日'");
                    myreg.SetValue("sShortDate", "yyyy/M/d");
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "执行")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                    myreg.SetValue("DisableAntiSpyware", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                    myreg.SetValue("DisableAntiSpyware", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
            if (comboBox3.Text == "恢复")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                    myreg.SetValue("DisableAntiSpyware", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                    myreg.SetValue("DisableAntiSpyware", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text == "显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced\Folder\Hidden\SHOWALL", true);
                    myreg.SetValue("CheckedValue", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced\Folder\Hidden\SHOWALL", true);
                    myreg.SetValue("CheckedValue", "1", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
            if (comboBox5.Text == "不显示")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced\Folder\Hidden\SHOWALL", true);
                    myreg.SetValue("CheckedValue", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced\Folder\Hidden\SHOWALL", true);
                    myreg.SetValue("CheckedValue", "0", RegistryValueKind.DWord);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        ReFileManager();
                    }
                    IsOver();
                }
            }
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            if (comboBox4.Text == "启用")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
                    myreg.SetValue("Isolation64Bit", "1");
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
                    myreg.SetValue("Isolation", "1");
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
            }
            if (comboBox4.Text == "禁用")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
                    myreg.SetValue("Isolation64Bit", "0");
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.CurrentUser;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
                    myreg.SetValue("Isolation", "0");
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text == "执行")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\TCPIP6\Parameters", true);
                    myreg.SetValue("DisabledComponents", "255", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\TCPIP6\Parameters", true);
                    myreg.SetValue("DisabledComponents", "255", RegistryValueKind.DWord);
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
            }
            if (comboBox6.Text == "恢复")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\TCPIP6\Parameters", true);
                    string[] keyValueNames = myreg.GetValueNames();
                    bool GetvalueResult = false;
                    foreach (string keyValueName in keyValueNames)
                    {
                        if (keyValueName == "DisabledComponents")
                        {
                            GetvalueResult = true;
                            break;
                        }
                        else
                        {
                            GetvalueResult = false;
                        }
                    }
                    if (GetvalueResult == true)
                    {
                        myreg.DeleteValue("DisabledComponents");
                    }
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\TCPIP6\Parameters", true);
                    myreg.SetValue("Isolation", "0");
                    myreg.Close();
                    MessageBox.Show("重启电脑生效");
                    IsOver();
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (wintype == true)
            {
                Thread th = new Thread(Waiting);
                th.Start();
                RegistryKey Key;
                Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey myreg;
                myreg = Key.OpenSubKey(@"Control Panel\Desktop", true);
                myreg.SetValue("FontSmoothing", "2");
                myreg.Close();
                DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                if (Warn == DialogResult.OK)
                {
                    ReFileManager();
                }
                IsOver();
            }
            else
            {
                Thread th = new Thread(Waiting);
                th.Start();
                RegistryKey Key;
                Key = Registry.CurrentUser;
                RegistryKey myreg;
                myreg = Key.OpenSubKey(@"Control Panel\Desktop", true);
                myreg.SetValue("FontSmoothing", "2");
                myreg.Close();
                DialogResult Warn = MessageBox.Show("是否刷新注册表使其生效，为了避免导致系统不稳定我们建议您重启", "是否刷新注册表", MessageBoxButtons.OKCancel);
                if (Warn == DialogResult.OK)
                {
                    ReFileManager();
                }
                IsOver();
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (comboBox7.Text == "执行")
            {
                Thread th = new Thread(Waiting);
                th.Start();
                string cmd = @"sc stop WdiSystemHost
                sc stop WdiServiceHost
                sc stop DPS
                sc config DPS start= disabled
                sc config WdiServiceHost start= disabled
                sc config WdiSystemHost start= disabled";
                CmdHelper.RunCmd(cmd);
                MessageBox.Show("执行成功");
                IsOver();
            }
            if (comboBox7.Text == "恢复")
            {
                Thread th = new Thread(Waiting);
                th.Start();
                string cmd = @"sc config DPS start= auto
                sc config WdiServiceHost start= demand
                sc config WdiSystemHost start= demand
                sc start DPS";
                CmdHelper.RunCmd(cmd);
                MessageBox.Show("恢复成功");
                IsOver();
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text == "执行")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    //Icon已解决
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons");
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons", true);
                    myreg.SetValue("29", @"%systemroot%\system32\imageres.dll,197", RegistryValueKind.String);
                    myreg.Close();
                    /*
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    myreg = Key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer", true);
                    //异常标记点
                    myreg.SetValue("link", "00000000", RegistryValueKind.Binary);
                    myreg.Close();
                    */
                    DialogResult Warn = MessageBox.Show("是否清除图标缓存使其生效，你可以选择稍后手动操作", "是否清除图标缓存", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        WipeIconCache();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons");
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons", true);
                    myreg.SetValue("29", @"%systemroot%\system32\imageres.dll,197", RegistryValueKind.String);
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否清除图标缓存使其生效，你可以选择稍后手动操作", "是否清除图标缓存", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        WipeIconCache();
                    }
                    IsOver();
                }
            }
            if (comboBox8.Text == "恢复")
            {
                if (wintype == true)
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer");
                    string[] keyNames = myreg.GetSubKeyNames();
                    bool GetkeyResult = false;
                    foreach (string keyName in keyNames)
                    {
                        if (keyName == "Shell Icons")
                        {
                            GetkeyResult = true;
                            break;
                        }
                        else
                        {
                            GetkeyResult = false;
                        }
                    }
                    if (GetkeyResult == true)
                    {
                        myreg.DeleteSubKeyTree("Shell Icons");
                    }
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否清除图标缓存使其生效，你可以选择稍后手动操作", "是否清除图标缓存", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        WipeIconCache();
                    }
                    IsOver();
                }
                else
                {
                    Thread th = new Thread(Waiting);
                    th.Start();
                    RegistryKey Key;
                    Key = Registry.LocalMachine;
                    RegistryKey myreg;
                    myreg = Key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer");
                    string[] keyNames = myreg.GetSubKeyNames();
                    bool GetkeyResult = false;
                    foreach (string keyName in keyNames)
                    {
                        if (keyName == "Shell Icons")
                        {
                            GetkeyResult = true;
                            break;
                        }
                        else
                        {
                            GetkeyResult = false;
                        }
                    }
                    if (GetkeyResult == true)
                    {
                        myreg.DeleteSubKeyTree("Shell Icons");
                    }
                    myreg.Close();
                    DialogResult Warn = MessageBox.Show("是否清除图标缓存使其生效，你可以选择稍后手动操作", "是否清除图标缓存", MessageBoxButtons.OKCancel);
                    if (Warn == DialogResult.OK)
                    {
                        WipeIconCache();
                    }
                    IsOver();
                }
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Waiting);
            th.Start();
            ReFileManager();
            IsOver();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Waiting);
            th.Start();
            WipeIconCache();
            IsOver();
        }

        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updatelog updatelogthing = new updatelog();
            updatelogthing.ShowDialog();
        }
    }
}
