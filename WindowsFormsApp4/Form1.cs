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

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        int port = 777;
        public Form1()
        {
            InitializeComponent();
        }

        //включить
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start(10);
                Task.Run(() => { Listening(listener); });
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            port = (int)numericUpDown1.Value;
        }

        //постоянное прослушивание
        private void Listening(TcpListener t)
        {
            try
            {
                while (true)
                {
                    if (t.Pending())
                    {
                        TcpClient client = t.AcceptTcpClient(); // полчучаем клиентское подключение
                        this.Invoke(new Action(() => { richTextBox1.Text+=$"{client.Client.RemoteEndPoint}\n"; }));
                        //client.Close();
                        Task.Run(() => { ReadClient(client);});
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                t.Stop();   
            }
            this.Invoke(new Action(() => { button1.Enabled = true; }));
        }

        private void ReadClient(TcpClient client)
        {
            try
            {
                string d = "";
                StreamReader sr = new StreamReader(client.GetStream(), Encoding.Unicode);
                while (d!="exit")
                {
                    d = sr.ReadLine();
                    if (d.Trim()!="")
                    {
                        this.Invoke(new Action(() => { richTextBox1.Text += $"{client.Client.RemoteEndPoint} - {d}\n"; }));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally 
            { 
                client.Close();
                richTextBox1.Text +=$"Соединение {client.Client.RemoteEndPoint} закрыто\n";
            }
        }
    }
}
