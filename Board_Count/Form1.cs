using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace Board_Count
{
    public partial class Form1 : Form
    {

        // public string _connectionString = $@"Data Source=U:\apache-tomcat-9.0.31\webapps\phison\maintain.db;";
        //指向W槽db 要用cmd鎖定磁碟位置
        //public string _connectionString2 = $@"Data Source=D:\PCIe_Bridge_ISP_FW_v195.01.02.07\maintain.db;";
        //string A = GetMAC();
        Form2 form2 = new Form2();

        public Form1()
        {
            InitializeComponent();
            ControlBox = false;
            Timer_Start(); //啟動計時器
        }

        public void timer1_Tick(object sender, EventArgs e)  //每60秒更新 無限迴圈
        {
            File.Copy($@"\\172.16.22.177\Tomcat\apache-tomcat-9.0.31\webapps\phison\maintain.db", $@"D:\PCIe_Bridge_ISP_FW_v195.01.02.07\maintain.db",true);    //將檔案從U:複製到D:

            string _connectionString2 = $@"Data Source=D:\PCIe_Bridge_ISP_FW_v195.01.02.07\maintain.db;";

            string Board_Setting = @"D:\PCIe_Bridge_ISP_FW_v195.01.02.07\SlotState.ini";
            string HostName = System.Net.Dns.GetHostName();
            //string Date = $"{DateTime.Now:yyyy-MM-dd}";
            double CanchangeBoard=0;
            int alarmcount=0, textposition=0;


            string[] file = File.ReadAllLines(Board_Setting, Encoding.Default);

            using (var connection = new SQLiteConnection(_connectionString2))
            {
                connection.Open();  //開啟db
                var command1 = connection.CreateCommand();  //定義連線

                for (int i = 0; i <= file.GetUpperBound(0); i++)
                {
                    if ("[Alarm]"==file[i])
                    {
                        textposition = file[i+1].IndexOf("=") + 1;
                        alarmcount = int.Parse(file[i+1].Substring(textposition));
                        CanchangeBoard = alarmcount * 0.8;
                    }
                }
                int outrange = 10100;


            try
            {
                string MAC = GetMAC();
                int alarm = 0;
                string[,] Board_Name_ary = new string[12,2];
                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[2]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox1.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read()==true)
                    {
                        string board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox1.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label1.Text = board1;
                        label13.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[0, 0] = board1;
                        Board_Name_ary[0, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox1.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox1.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox1.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox1.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox1.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label1.Text = "無此編號";
                        label13.Text = "";
                        groupBox1.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[4]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox13.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read()==true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox13.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label2.Text = board1;
                        label15.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[1, 0] = Board_Name;
                        Board_Name_ary[1, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox13.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox13.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox13.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox13.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox13.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label2.Text = "無此編號";
                        label15.Text = "";
                        groupBox13.BackColor = Color.Red;
                    }
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[6]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox12.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read())
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox12.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label3.Text = board1;
                        label16.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[2, 0] = Board_Name;
                        Board_Name_ary[2, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox12.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox12.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox12.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox12.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox12.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label3.Text = "無此編號";
                        label16.Text = "";
                        groupBox12.BackColor = Color.Red;
                    }

                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[8]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox2.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox2.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label4.Text = board1;
                        label14.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[3, 0] = Board_Name;
                        Board_Name_ary[3, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox2.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox2.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox2.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox2.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox2.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label4.Text = "無此編號";
                        label14.Text = "";
                        groupBox2.BackColor = Color.Red;
                    }
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[10]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox7.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox7.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label5.Text = board1;
                        label18.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[4, 0] = Board_Name;
                        Board_Name_ary[4, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox7.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox7.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox7.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox7.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox7.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label5.Text = "無此編號";
                        label18.Text = "";
                        groupBox7.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[12]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox4.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox4.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label6.Text = board1;
                        label19.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[5, 0] = Board_Name;
                        Board_Name_ary[5, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox4.BackColor = Color.DarkOrange;
                            connection.Close();
                            BoardIDLength_Message(groupBox4.Text);
                            connection.Open();
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox4.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox4.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox4.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label6.Text = "無此編號";
                        label19.Text = "";
                        groupBox4.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[14]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox3.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox3.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label7.Text = board1;
                        label20.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[6, 0] = Board_Name;
                        Board_Name_ary[6, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox3.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox3.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox3.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox3.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox3.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label7.Text = "無此編號";
                        label20.Text = "";
                        groupBox3.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[16]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox11.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox11.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label8.Text = board1;
                        label21.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[7, 0] = Board_Name;
                        Board_Name_ary[7, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox11.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox11.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox11.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox11.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox11.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label8.Text = "無此編號";
                        label21.Text = "";
                        groupBox11.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[18]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox8.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox8.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label9.Text = board1;
                        label25.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[8, 0] = Board_Name;
                        Board_Name_ary[8, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox8.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox8.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox8.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox8.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox8.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label9.Text = "無此編號";
                        label25.Text = "";
                        groupBox8.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[20]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox9.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox9.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label10.Text = board1;
                        label24.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[9, 0] = Board_Name;
                        Board_Name_ary[9, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox9.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox9.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox9.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox9.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox9.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label10.Text = "無此編號";
                        label24.Text = "";
                        groupBox9.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[22]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox6.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox6.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label11.Text = board1;
                        label23.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[10, 0] = Board_Name;
                        Board_Name_ary[10, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox6.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox6.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox6.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox6.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox6.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label11.Text = "無此編號";
                        label23.Text = "";
                        groupBox6.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                command1.CommandText = $@"SELECT PORT_ID,PORT_ID_CNT from MAINTAIN_INFO where PORT_ID='{file[24]}'";//針對各BoardID寫SQL
                using (var reader = command1.ExecuteReader())
                {
                    groupBox10.BackColor = Color.FromArgb(0, 0, 0, 0);
                    if (reader.Read() == true)
                    {
                        var board1 = reader.GetString(0);
                        var Boardcount1 = reader.GetInt32(1);
                        groupBox10.BackColor = Color.FromArgb(0, 0, 0, 0);
                        label12.Text = board1;
                        label22.Text = Boardcount1.ToString();
                        string Board_Name = board1;
                        string Board_Count = Boardcount1.ToString();
                        Board_Name_ary[11, 0] = Board_Name;
                        Board_Name_ary[11, 1] = Board_Count;
                        if (Boardcount1 >= outrange)
                        {
                            groupBox10.BackColor = Color.DarkOrange;
                            BoardIDLength_Message(groupBox10.Text);
                        }
                        else if (Boardcount1 >= alarmcount)
                        {
                            groupBox10.BackColor = Color.Red;
                            alarm++;
                            ChangeBoard_Message(groupBox10.Text);
                        }
                        else if (Boardcount1 >= Convert.ToInt32(CanchangeBoard))
                        {
                            groupBox10.BackColor = Color.Yellow;
                            alarm++;
                        }
                    }
                    else
                    {
                        label12.Text = "無此編號";
                        label22.Text = "";
                        groupBox10.BackColor = Color.Red;
                    }
                    
                    reader.Close();
                }

                string failfilename = $@"\\172.16.22.176\data\Phison商規\12.Board_ID_Record\轉板須更換_{HostName}_BoardID.txt";
                string safefilename = $@"\\172.16.22.176\data\Phison商規\12.Board_ID_Record\{HostName}_BoardID.txt";
                string filename;
                File.Delete(safefilename);
                File.Delete(failfilename);
                if (alarm > 0)
                    filename = failfilename;
                else
                {
                    filename = safefilename;
                }
                
                File.WriteAllText(filename, "");

                File.AppendAllText(filename, $"MAC：{MAC}\n");
                for (int i = 0; i < 12; i++)
                {
                    File.AppendAllText(filename, $"PORT{i+1}：");
                    for (int j = 0; j < 2; j++)
                    {
                        File.AppendAllText(filename, $"{Board_Name_ary[i,j]}  ");
                    }
                    File.AppendAllText(filename, "\n");
                    
                }
                
            }
            catch (Exception)
            {
                connection.Close();
                timer1.Enabled = false;
                groupBox1.BackColor = Color.Red;
                groupBox13.BackColor = Color.Red;
                groupBox12.BackColor = Color.Red;
                groupBox2.BackColor = Color.Red;
                groupBox7.BackColor = Color.Red;
                groupBox4.BackColor = Color.Red;
                groupBox3.BackColor = Color.Red;
                groupBox11.BackColor = Color.Red;
                groupBox8.BackColor = Color.Red;
                groupBox9.BackColor = Color.Red;
                groupBox6.BackColor = Color.Red;
                groupBox10.BackColor = Color.Red;
                MessageBox.Show("找不到BoardID\n請確認SlotState.ini", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);  //跳錯誤訊息
            }
            connection.Close();
            }
            timer1.Interval = 60000; //第二圈開始改為60秒更新
            timer1.Enabled = true;
        }

        private string ChangeBoard_Message(string portnum) //次數到8000跳通知
        {

            timer1.Enabled = false;
            MessageBox.Show($"PORT{portnum}請更換轉板\n請通知隨線工程更換轉板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return portnum;
        }

        public string BoardIDLength_Message(string portnum) //次數超過10000需展延
        {
            timer1.Enabled = false;
            MessageBox.Show($"PORT{portnum}請進行ID展延\n通知隨線工程進行處理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return portnum; 
        }
        public void Timer_Start()  //計時器
        {
            timer1.Interval = 3000; //起初3秒後刷新畫面
            timer1.Enabled = true;  //自動刷新
        }

        private static string GetMAC()      //抓MAC
        {
            try
            {
                //抓取電腦MAC
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        mo.Dispose();//釋放記憶體
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Timer_Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (form2.IsDisposed)
            {
                form2 = new Form2();
                form2.Show();
                form2.Focus();
            }
            else
            {
                form2.Show();
                form2.Focus();
            }
        }
    }
}
