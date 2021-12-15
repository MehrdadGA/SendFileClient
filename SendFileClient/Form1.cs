using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//--------------------------------------------------------------
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SendFileClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Brows_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                txt_FilePath.Text = op.FileName;
            }
        }

        public void SendFile(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Length > 1024 * 50000)
            {
                MessageBox.Show("Your Size message is huge");
                return;
            }
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileInfo.Name);
            byte[] fileData = File.ReadAllBytes(fileName);
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
            byte[] dataSend = new byte[4 + fileNameByte.Length + fileData.Length];
            fileNameLen.CopyTo(dataSend, 0);
            fileNameByte.CopyTo(dataSend, 4);
            fileData.CopyTo(dataSend, 4 + fileNameByte.Length);

            try
            {
                Socket sSendFile = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(txt_IP.Text), int.Parse(txt_Port.Text));
                sSendFile.Connect(iPEndPoint);
                sSendFile.Send(dataSend);
                sSendFile.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("error => " + e.Message);
            }
        }

        private void txt_FilePath_TextChanged(object sender, EventArgs e)
        {
            btn_SendFile.Enabled = txt_FilePath.Text.Trim() != "";
        }

        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            SendFile(txt_FilePath.Text);
        }
    }
}
