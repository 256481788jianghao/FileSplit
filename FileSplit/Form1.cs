using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSplit
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string FileName = "";

        private void button_select_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = dialog.FileName;
                textBox1.Text = FileName;
            }
        }

        private async void button_split_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[1024*1024*100];
            //File.GetAttributes(FileName).
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead;
                    int index = 0;
                    long hasRead = 0;
                    while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        // 处理读取到的数据
                        hasRead += bytesRead;
                        index++;
                        label1.Text = (100.0* hasRead / fs.Length).ToString();
                        string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string dir = Path.GetDirectoryName(FileName);
                        File.WriteAllText(dir+"/split_" + index + ".log", text);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("文件未找到。");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"读取文件时发生错误: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生未知错误: {ex.Message}");
            }

        }
    }
}
