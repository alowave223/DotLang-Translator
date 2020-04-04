using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotLangTranslator
{

    public partial class Form2 : Form
    {
        public List<string> selectedItems = new List<string>();
        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            object selectedItem1 = comboBox1.SelectedItem;
            object selectedItem2 = comboBox2.SelectedItem;
            selectedItems.Add(selectedItem1.ToString().ToLower());
            selectedItems.Add(selectedItem2.ToString().ToLower());
            Close();
        }
    }
}
