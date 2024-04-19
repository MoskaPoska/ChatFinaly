using System.Diagnostics;
using System.Windows.Forms;

namespace ChatFinaly
{
    public partial class ContactsForm : Form
    {
        BlackList blackListForm;
        private List<ContactItem> contacts;
        public ContactsForm()
        {
            InitializeComponent();
            contacts = new List<ContactItem>();
            contacts.Add(new ContactItem("Олег"));
            contacts.Add(new ContactItem("Алина"));
            contacts.Add(new ContactItem("Jozef"));
            DisplayCon();
            blackListForm = new BlackList();
        }
        private void DisplayCon()
        {
            listBox1.Items.Clear();
            foreach (var item in contacts)
            {
                //ListViewItem contact = new ListViewItem(item);
                listBox1.Items.Add(item);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        private void AddToBlackList(string contact)
        {
            blackListForm.AddContact(contact);
            blackListForm.Show();
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }

            string selectedContact = listBox1.SelectedItem.ToString();

            //if (blackListForm == null || blackListForm.IsDisposed)
            //{
            //    blackListForm = new BlackList();
            //}

            if (blackListForm.IsContactInBlackList(selectedContact))
            {
                MessageBox.Show("Контакт в черном списке", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //MessageBox2 messageBox = new MessageBox2();
            //DialogResult result = messageBox.ShowDialog();
            DialogResult result = MessageBox.Show("Выберите действие", "Выбор действия", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    OpenNotepadWithConversation(filePath);
                }
            }
            else if (result == DialogResult.No)
            {
                AddToBlackList(selectedContact);
            }
            else if(result == DialogResult.Cancel)
            {
                if (listBox1.SelectedIndex != null) 
                {
                    ContactItem contact = (ContactItem)listBox1.SelectedItem;
                    AddToFavorite(contact);
                }
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
        private void AddToFavorite(ContactItem contact)
        {
            if(!contact.IsFavorite)
            {
                contact.IsFavorite = true;               
                listBox1.Items.Remove(contact);
                listBox1.Items.Insert(0, contact);
                //listBox1.SelectedIndex = 0;
                listBox1.Refresh();
            }
        }
    }
}
