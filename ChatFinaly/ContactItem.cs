using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatFinaly
{
    internal class ContactItem
    {
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public ContactItem(string n)
        {
            Name = n;
            IsFavorite = false;
        }
        public override string ToString()
        {
            if(IsFavorite)
            {
                return Name + "★";
            }
            else
            {
                return Name;
            }
            
        }
    }
}
