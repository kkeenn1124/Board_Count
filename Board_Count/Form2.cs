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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = textBox2.Text;
            if("3613131"==pwd)
            {
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                label2.Visible = false;
                textBox2.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                label1.Text = "";
            }
            else
            {
                label1.Text= "密碼錯誤";
                //MessageBox.Show("密碼錯誤");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start($@"D:\PCIe_Bridge_ISP_FW_v195.01.02.07\SlotState.ini");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start($@"D:\PCIe_Bridge_ISP_FW_v195.01.02.07\MultiISP_PCIe_Bridge(Release).exe");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start($@"D:\PCIe_Bridge_ISP_FW_v195.01.02.07\PCIE_ID展延\Board ID Programmer Tool_v4.07\Board_ID_ProgrammerTool.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();    //關閉視窗
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Control)
            {
                button1_Click(sender, e);
            }
        }
    }
}
