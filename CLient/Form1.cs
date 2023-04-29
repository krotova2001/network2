using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CLient
{
    public partial class Form1 : Form
    {
        TcpClient client = null;
        public Form1()
        {
            InitializeComponent();
        }

        //start
        private void button1_Click(object sender, EventArgs e)
        {
           client = new TcpClient();
            try
            {
                client.Connect(textBox1.Text.Trim(), (int)numericUpDown1.Value);
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client = null;
            }
        }

        //send
        private void button2_Click(object sender, EventArgs e)
        {
            if (client!=null)
            {
                NetworkStream sm = client.GetStream();
                string message = richTextBox2.Text.Trim();
                byte[] data = Encoding.Unicode.GetBytes(message);
                sm.Write(data, 0, data.Length);
                //richTextBox2.Clear();
            }
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button2_Click(sender, e); }
        }
    }
}
