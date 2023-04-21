using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace RenameForm
{   
    public partial class Form1 : Form
    {
        static int picCount = 0;
        static int revertCount = 0;

        public Dictionary<string,string> saveInfo = new Dictionary<string,string>();
        public Dictionary<string, int> nameCount = new Dictionary<string, int>();
        public Form1()
        {
            InitializeComponent();
            InitViewLab();
        }
        private void InitViewLab()
        {
            beforeDesc.Text = "";
            afterDesc.Text = "";
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择重命名导出文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                beforImportLab.Text = dialog.SelectedPath;
                DirectoryInfo path = new DirectoryInfo(dialog.SelectedPath);

                beforeEportLab.Text = path.Parent.FullName + "\\" + "art_rename";
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择归档导入文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                afterImportLab.Text = dialog.SelectedPath;
                DirectoryInfo path = new DirectoryInfo(dialog.SelectedPath);

                afterEportLab.Text = path.Parent.FullName + "\\" + "art_revert";
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
            string searchPattern = "*";  // 匹配所有文件
            picCount = 0;
            if (beforImportLab.Text == "")
            {
                MessageBox.Show("请选择要导入的文件夹路径");
                return;
            }
            if (beforeEportLab.Text == "")
            {
                MessageBox.Show("请选择输出要保存的路径");
                return;
            }
            string folderPath = beforImportLab.Text;
            string beforePath = beforeEportLab.Text;
            if (!Directory.Exists(beforePath))
            {
                Directory.CreateDirectory(beforePath);
            }
            UpdateBtnEnable(true, true);
            nameCount.Clear();
            saveInfo.Clear();
            RenameFiles(folderPath, beforePath, searchPattern);
        }
        private void RenameFiles(string folderPath, string beforePath, string searchPattern)
        {
            // 获取文件夹下的所有文件夹
            string[] subFolders = Directory.GetDirectories(folderPath);
            // 遍历每个子文件夹
            foreach (string subFolder in subFolders)
            {
                // 获取子文件夹下的所有文件
                string[] files = Directory.GetFiles(subFolder, searchPattern);

                // 遍历每个文件
                foreach (string file in files)
                {
                    // 获取文件名和扩展名
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string fileExtension = Path.GetExtension(file);
                    string directoryPath = Path.GetDirectoryName(file);
                    string[] pathLevels = directoryPath.Split('\\');
                    int len = pathLevels.Length;
                    // 修改文件名
                    string newFileName = pathLevels[len-2] + "-" + pathLevels[len-1] + "-" + fileName + fileExtension;
                    string copyFilePath = beforePath + "\\" + pathLevels[len - 1];
                    if (!Directory.Exists(copyFilePath))
                    {
                        Directory.CreateDirectory(copyFilePath);
                    }
                    string newFilePath = Path.Combine(copyFilePath, newFileName);
                    int newCount;
                    string aa =  pathLevels[len - 1];
                    if (nameCount.ContainsKey(aa))
                    {
                        newCount = nameCount[aa] + 1;
                        nameCount.Remove(aa);
                        nameCount.Add(aa, newCount);
                    }
                    else
                    {
                        newCount = 10000;
                        nameCount.Add(aa, newCount);
                    }
                    string newName = aa + newCount;

                    saveInfo.Add(newName, newFileName);
                    //File.Move(file, newFilePath);
                    string saveFileName = Path.Combine(copyFilePath, newName + fileExtension);
                    File.Copy(file, saveFileName, true);
                    picCount = picCount + 1;
                }

                // 递归处理子文件夹
                RenameFiles(subFolder, beforePath, searchPattern);
            }
            UpdateBtnEnable(false, true);
            beforeDesc.Text = "当前导出" + picCount + "张图片";
        }

        private void UpdateBtnEnable(Boolean isActive, Boolean isBefore)
        {
            if (isActive)
            {
                if (isBefore) {
                    beforeBtn.Text =  "运行中";
                }
                else{
                    afterBtn.Text = "运行中";
                }
                beforeBtn.Enabled = false;
                afterBtn.Enabled = false;
            }
            else
            {
                beforeBtn.Text = "重命名";
                afterBtn.Text = "归档";
                beforeBtn.Enabled = true;
                afterBtn.Enabled = true;
            }
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
            revertCount = 0;
            string folderPath = afterImportLab.Text;
            string afterPath = afterEportLab.Text;
            if (!Directory.Exists(afterPath))
            {
                Directory.CreateDirectory(afterPath);
            }
            string searchPattern = "*";
            RevertFiles(folderPath, afterPath, searchPattern);
        }

        private void RevertFiles(string folderPath, string afterPath, string searchPattern)
        {
            UpdateBtnEnable(true, false);
            // 获取文件夹下的所有文件夹
            string[] subFolders = Directory.GetDirectories(folderPath);
            // 遍历每个子文件夹
            foreach (string subFolder in subFolders)
            {
                // 获取子文件夹下的所有文件
                string[] files = Directory.GetFiles(subFolder, searchPattern);

                // 遍历每个文件
                foreach (string file in files)
                {
                    // 获取文件名和扩展名
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string directoryPath = Path.GetDirectoryName(file);
                    //这里的操作加文件所在路径上一层的前缀是因为美术那边工具输出之后是把前缀给删了
                    //比如保存的是“all\all10000”美术输出变成了“all\10000”
                    string[] pathLevels_ = directoryPath.Split('\\');
                    int len_ = pathLevels_.Length;
                    fileName = pathLevels_[len_ - 1] + fileName;

                    if (saveInfo.ContainsKey(fileName))
                    {
                        string reverVaule = saveInfo[fileName];
                        string[] pathLevels = reverVaule.Split('-');

                        int len = pathLevels.Length;
                        // 修改文件名
                        string newFileName = pathLevels[len - 1];
                        string copyFilePath = afterPath + "\\" + pathLevels[0]+ "\\" + pathLevels[1];
                        if (!Directory.Exists(copyFilePath))
                        {
                            Directory.CreateDirectory(copyFilePath);
                        }
                        string newFilePath = Path.Combine(copyFilePath, newFileName);
                    
                        File.Copy(file, newFilePath, true);
                        revertCount = revertCount + 1;
                    }
                }

                // 递归处理子文件夹
                RenameFiles(subFolder, afterPath, searchPattern);
            }
            UpdateBtnEnable(false, false);
            afterDesc.Text = "当前还原" + revertCount + "张图片";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
