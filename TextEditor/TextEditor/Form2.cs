using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _4._7
{
    public partial class Form2 : Form
    {
        private RichTextBox RB;
        public Form2(RichTextBox RB)
        {
            this.RB = RB;
            InitializeComponent();
        }
        //_4._7
        private void button1_Click(object sender, EventArgs e)
        {
            int m = int.Parse(textBox1.Text);
            if (m <= RB.Lines.Length && m >= 0)
            {
                //获取目标行首字符索引
                this.RB.SelectionStart = this.RB.GetFirstCharIndexFromLine(int.Parse(textBox1.Text) - 1);
                this.Close();
            }
            else
            {
                MessageBox.Show("输入超出范围！", "提示！");
                return;
            }
        }
    }
}
