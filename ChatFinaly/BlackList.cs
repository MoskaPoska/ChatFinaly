using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatFinaly
{
    public partial class BlackList : Form
    {
        BlackList blackKistForm;
        List<string> blackList;
        public BlackList()
        {
            InitializeComponent();
            blackList = new List<string>();
        }
        public void AddContact(string contact)
        {
            blackList.Add(contact);
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            foreach (var item in blackList)
            {
                listBox1.Items.Add(item);
            }
        }
        public bool IsContactInBlackList(string contact)
        {
            return blackList.Contains(contact);
        }
    }
}
