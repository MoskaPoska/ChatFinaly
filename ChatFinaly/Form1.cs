using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ChatFinaly
{
    public partial class Form1 : Form
    {
        BlackList blackListForm;
        private UdpClient udpClient;
        private IPAddress multicastIp = IPAddress.Parse("224.5.5.5");
        private int multiPort = 4569;
        private int broadPort = 8001;
        private int messageCount = 0;
        public Form1()
        {
            InitializeComponent();
            blackListForm = new BlackList();
        }
        private void списокКонтактовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContactsForm contactsForm = new ContactsForm();
            contactsForm.Show();
        }
        private void черныйСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BlackList blackList = new BlackList();
            blackList.Show();
        }
        //private void OpenNotepadWithConversation(string filePath)
        //{
        //    try
        //    {
        //        Process.Start("notepad.exe", filePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка открытия файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                udpClient = new UdpClient();
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, multiPort));
                udpClient.JoinMulticastGroup(multicastIp);

                //var broadcastAddress = IPAddress.Parse("224.5.5.11");
                //udpClient.JoinMulticastGroup(broadcastAddress);
                //while (true)
                //{
                //    var res = await udpClient.ReceiveAsync();
                //    string str = Encoding.Default.GetString(res.Buffer);
                //    textBox1.BeginInvoke(new Action<string>(AddText), str);
                //}
                //udpClient.DropMulticastGroup(broadcastAddress);
                _ = Task.Run(() =>
                {
                    _ = ReceiveMessage();
                });
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       
        private async Task ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    UdpReceiveResult receiveResult = await udpClient.ReceiveAsync();
                    string receiveMessage = Encoding.UTF8.GetString(receiveResult.Buffer);
                    AppendMessage(receiveMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AppendMessage(string message)
        {
            if(textBox2.InvokeRequired)
            {
                textBox2.BeginInvoke(new Action<string>(AppendMessage), message);
            }
            else
            {
                textBox2.Text = message;
                messageCount++;
                if (messageCount == 4) 
                {
                    MessageBox.Show("Собеседник, что надо", "Важное сообщние", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    messageCount = 0;
                }
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string message = $" {DateTime.Now.ToString("HH:mm:ss")} {textBox1.Text}: {textBox3.Text}";
                if (!blackListForm.IsContactInBlackList(textBox1.Text))
                {
                    byte[] buff = Encoding.UTF8.GetBytes(message);
                    await udpClient.SendAsync(buff, buff.Length, new IPEndPoint(multicastIp, multiPort));
                    textBox3.Text = "";
                }
                else
                {
                    MessageBox.Show("Ваш друг, больше не друг", "Конец дружбе", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить переписку";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //string contact1 = textBox1.Text;
                //string contact2 = textBox1.Text;

                string filePath = saveFileDialog.FileName;

                //string fileName = $"{contact1}_{contact2}_conversation.txt";
                //string fullFilePath=System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePath), fileName);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string message in textBox2.Lines)
                    {
                        sw.WriteLine(message);
                        sw.WriteLine();
                    }
                }
                MessageBox.Show("Переписка была успешно сохранена", "Успех", MessageBoxButtons.OK);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var broadIp = IPAddress.Parse("224.5.5.11");
                UdpClient udpClient = new UdpClient();
                string message = $" {DateTime.Now.ToString("HH:mm:ss")} {textBox1.Text}: {textBox3.Text}";
                if (!blackListForm.IsContactInBlackList(textBox1.Text)) 
                {
                    byte[] buff = Encoding.UTF8.GetBytes(message);
                    await udpClient.SendAsync(buff, new IPEndPoint(broadIp, 8001));
                    await Task.Delay(1000);
                    textBox3.Text = "";
                }
                else
                {
                    MessageBox.Show("Ваш друг, больше не друг", "Конец дружбе", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                UdpClient socket = new UdpClient();

                string message = $" {DateTime.Now.ToString("HH:mm:ss")} {textBox1.Text}: {textBox3.Text}";

                byte[] data = Encoding.UTF8.GetBytes(message);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 11002);

                socket.Send(data, data.Length, endpoint);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                      
        }
    }
}