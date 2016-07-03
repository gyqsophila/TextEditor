using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace _4._7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.printdoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printdoc_PrintPage);
            //this.FormClosing+=new FormClosingEventHandler(Form1_FormClosing);
        }
        string startText = "";//设置初始内容
        string str = "";//设置剪贴板容器
        string[] old = new string[20];//设置撤销栈

        PrintDocument printdoc = new PrintDocument();//打印程序
        PageSetupDialog setup = new PageSetupDialog();//页面设置
        PrintPreviewDialog preview = new PrintPreviewDialog();//打印预览
        FontDialog font = new FontDialog();//字体设置
        PrintDialog print = new PrintDialog();//打印对话框

        //_4._7
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != startText)
            {
                DialogResult result = MessageBox.Show("是否保存更改？", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                {
                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                            richTextBox1.Text = "";
                            startText = "";
                        }
                    }
                    if (result == DialogResult.No)
                    {
                        richTextBox1.Text = "";
                        startText = "";
                    }
                }
            }
            else
            {
                richTextBox1.Text = "";
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //是否保存原文本
            if (richTextBox1.Text != startText)
            {
                DialogResult result = MessageBox.Show("是否保存更改？", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                {
                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                        }
                    }
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            //打开对话框
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
                startText = richTextBox1.Text;
            }
            richTextBox1_TextChanged(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startText != richTextBox1.Text)
            {
                DialogResult result = MessageBox.Show("是否保存更改？", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                {
                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveFileDialog save = new SaveFileDialog();
                            if (save.ShowDialog() == DialogResult.OK)
                            {
                                richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                                this.Close();
                            }
                            break;
                        case DialogResult.No:
                            this.Close();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            else
            {
                this.Close();
            }
        }


        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            print.Document = printdoc;
            if (print.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printdoc.Print();
                    TextBox tb = new TextBox();
                    tb.Text = "print.UseEXDialog:" + print.UseEXDialog.ToString() + "\r\n"
                            + "PrintToFile:" + print.PrintToFile.ToString() + "\r\n"
                            + "AllowSelection:" + print.AllowSelection.ToString() + "\r\n"
                            + "AllowCurrentPage:" + print.AllowCurrentPage.ToString() + "\r\n"
                            + "setup.AllowMargins:" + setup.AllowMargins.ToString() + "\r\n"
                            + "AllowPaper.ToString:" + setup.AllowOrientation.ToString() + "\r\n"
                            + "AllowPaper:" + setup.AllowPaper.ToString() + "\r\n"
                            + "AllowPrinter:" + setup.AllowPrinter.ToString() + "\r\n"
                            + "MinMargins:" + setup.MinMargins.ToString() + "\r\n"
                            + "ShowHelp:" + setup.ShowHelp.ToString();
                    System.IO.File.WriteAllText("PrintINF.txt", tb.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "打印出错");
                }
            }
        }
        private void printdoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            string[] str = richTextBox1.Text.Split('\n');
            int i = 0;
            foreach (string s in str)
            {
                g.DrawString(str[i], font.Font, new SolidBrush(richTextBox1.ForeColor), new PointF(100, 80 + richTextBox1.Font.Height * 1));
            }
        }

        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setup.Document = printdoc;
            try
            {
                setup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印出错");
            }
        }

        private void 预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preview.Document = printdoc;
            try
            {
                preview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印出错");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (startText != richTextBox1.Text)
            {
                DialogResult result = MessageBox.Show("是否保存更改？", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                {
                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                            this.Dispose();
                            e.Cancel = false;
                        }
                    }
                    if (result == DialogResult.No)
                    {
                        this.Dispose();
                        e.Cancel = false;
                    }
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    //方法二
                    //switch (result)
                    //{
                    //    case DialogResult.Yes:
                    //        SaveFileDialog save = new SaveFileDialog();
                    //        if (save.ShowDialog() == DialogResult.OK)
                    //        {
                    //            richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                    //            this.Dispose();
                    //            e.Cancel = false;
                    //        }
                    //        break;
                    //    case DialogResult.No:
                    //        this.Dispose();
                    //        e.Cancel = false;
                    //        break;
                    //    case DialogResult.Cancel:
                    //        this.Dispose();
                    //        e.Cancel = true;
                    //        break;
                    //}
                }
            }
            else
            {
                this.Dispose();
                e.Cancel = false;
            }
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            str = richTextBox1.SelectedText;
            richTextBox1.SelectedText = "";
            粘贴ToolStripMenuItem.Enabled = true;
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            str = richTextBox1.SelectedText;
            粘贴ToolStripMenuItem.Enabled = true;
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int t = richTextBox1.SelectionStart;
            int l = richTextBox1.Text.Length;
            string str1 = richTextBox1.Text.Substring(0, t);
            string str2 = richTextBox1.Text.Substring(t, l - t);
            richTextBox1.Text = str1 + str + str2;
        }

        private void 左对齐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            toolStripButton8.Checked = true;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            左对齐ToolStripMenuItem.Checked = true;
            右对齐ToolStripMenuItem.Checked = false;
            居中ToolStripMenuItem.Checked = false;
        }

        private void 右对齐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = true;
            toolStripButton10.Checked = false;
            右对齐ToolStripMenuItem.Checked = true;
            左对齐ToolStripMenuItem.Checked = false;
            居中ToolStripMenuItem.Checked = false;
        }

        private void 居中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = true;
            居中ToolStripMenuItem.Checked = true;
            左对齐ToolStripMenuItem.Checked = false;
            右对齐ToolStripMenuItem.Checked = false;
        }

        private void 加粗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            加粗ToolStripMenuItem.Checked = !加粗ToolStripMenuItem.Checked;
            toolStripButton11.Checked = !toolStripButton11.Checked;
            FontStyle style = richTextBox1.Font.Style;
            if (加粗ToolStripMenuItem.Checked == false)
            {
                style &= FontStyle.Bold;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, style);
            }
            else
            {
                style |= FontStyle.Bold;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, style);
            }
        }
        //4.7
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = System.DateTime.Now.ToString();
        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            coordinate();
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            coordinate();
        }

        //4.7光标位置
        private void coordinate()
        {
            int index = richTextBox1.GetFirstCharIndexOfCurrentLine();//获取当前行第一个字符的索引
            int line = richTextBox1.GetLineFromCharIndex(index) + 1;//获取当前行数
            int col = richTextBox1.SelectionStart - index;//获取当前列数
            toolStripStatusLabel3.Text = "(" + line + "," + col + ")";
        }

        //统计汉字、字母、空格、行数
        string chexiao = "";
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string str_ing = richTextBox1.Text;
            //统计汉字、字母、空格、行数
            int space = 0, alpha = 0;
            int HanNum = Encoding.Default.GetByteCount(str_ing) - str_ing.Length;//获取汉字个数
            int totallines = richTextBox1.GetLineFromCharIndex(richTextBox1.TextLength) + 1;//获取总行数
            for (int i = 0; i < str_ing.Length; i++)
            {
                old[9] = str_ing;
                string str = str_ing.Substring(i, 1);
                if (str == " ")
                    space++;
                else if (char.IsUpper(str, 0) || char.IsLower(str, 0))
                    alpha++;
            }
            toolStripStatusLabel2.Text = "汉字:" + HanNum + " 字母:" + alpha + " 空格:" + space + " 行数:" + totallines;
            coordinate();
            if (str_ing != chexiao)//避免连续记录重复值
            {
                for (int i = 0; i < 9; i++)
                {
                    old[i] = old[i + 1];//记录前移
                }
                old[9] = str_ing;//添加最后一条记录
            }
            chexiao = str_ing;//记录上次值
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            coordinate();
            richTextBox1_TextChanged(sender, e);
        }

        //4.4至多允许连续撤销七次(在连续编辑10步以上的情况下)
        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (old[7] != old[9])//判断撤销是否出错
            {
                richTextBox1.Text = old[7];
                for (int i = 9; i >= 2; i--)
                {
                    old[i] = old[i - 2];
                }
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                string str = richTextBox1.Text;
                int m = richTextBox1.SelectionStart;
                int n = richTextBox1.SelectionLength;
                richTextBox1.Text = str.Substring(0, m) + str.Substring(m + n);
                richTextBox1.SelectionStart = m;
            }
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //_4._7
        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            if (font.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = font.Font;
            }
        }

        //_4._7
        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = color.Color;
            }
        }

        private void 换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = richTextBox1.Text;
            int m = richTextBox1.SelectionStart;
            string str1 = str.Substring(0, m);
            string str2 = str.Substring(m);
            richTextBox1.Text = str1 + Environment.NewLine + str2;
            richTextBox1.SelectionStart = m + 1;
        }
        //_4._7
        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
        //_4._7
        private void 日期ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = richTextBox1.Text;
            int m = richTextBox1.SelectionStart;
            string n = System.DateTime.Today.ToShortDateString();
            richTextBox1.Text = str.Substring(0, m) + n + str.Substring(m);
            richTextBox1.SelectionStart = m + n.Length;
        }
        //_4._7
        private void 时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = richTextBox1.Text;
            int m = richTextBox1.SelectionStart;
            string n = System.DateTime.Now.ToShortTimeString();
            richTextBox1.Text = str.Substring(0, m) + n + str.Substring(m);
            richTextBox1.SelectionStart = m + n.Length;
        }
        //_4._7
        private void 转到ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text;
            Form2 form2 = new Form2(richTextBox1);
            form2.ShowDialog();
            coordinate();
        }
    }
}
