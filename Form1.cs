using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenameForm
{   
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                beforImportLab.Text = dialog.SelectedPath;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                afterImportLab.Text = dialog.SelectedPath;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                beforeEportLab.Text = dialog.SelectedPath;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                afterEportLab.Text = dialog.SelectedPath;
            }
        }

        private void beforeBtn_Click(object sender, EventArgs e)
        {
            //if (beforImportLab.Text == "")
            //{
            //    MessageBox.Show("请选择要导入的文件夹路径");
            //    return;
            //}
            //if (beforeEportLab.Text == "")
            //{
            //    MessageBox.Show("请选择输出要保存的路径");
            //    return;
            //}


        }

        private void afterBtn_Click(object sender, EventArgs e)
        {
            if (afterImportLab.Text == "")
            {
                MessageBox.Show("请选择要导入的文件夹路径");
                return;
            }
            if (afterEportLab.Text == "")
            {
                MessageBox.Show("请选择输出要保存的路径");
                return;
            }
        }
    }
}
