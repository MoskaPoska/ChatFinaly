using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatFinaly
{
    public partial class MessageBox2 : Form
    {
        BlackList blackListForm;
        public MessageBox2()
        {
            InitializeComponent();
            blackListForm = new BlackList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                OpenNotepadWithConversation(filePath);
            }
        }
        private void OpenNotepadWithConversation(string filePath)
        {
            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (blackListForm.listBox1.SelectedItem != null)
            {
                string selectedContact = blackListForm.listBox1.SelectedItem.ToString();
                AddToBlackList(selectedContact);
            }
            else
            {
                MessageBox.Show("выберите контакт из списка", "Полундра", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AddToBlackList(string contact)
        {
            blackListForm.AddContact(contact);
            blackListForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
