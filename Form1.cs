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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //..

            int linetxt = int.Parse(InputLineNum.Text);
            string txt = "";
            for(int i=1; i<= linetxt; i++)
            {
                string n = Convert.ToString(i);
                txt = txt + "hello" + n + "\r\n";
            }
            Writ_txt(txt);

            string show = File.ReadAllText("Hello.txt");
            ShowHello.Text = show;
        }
        void Writ_txt(string txt)
        {
            File.WriteAllText("Hello.txt", txt);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();

            curDir = "..//..";

            String[] dirs = Directory.GetDirectories(curDir);
            string[] files = Directory.GetFiles(curDir);

            int j = dirs.Length;
            for (int i = 0; i<j; i++)
            {
                listBox2.Items.Add(dirs[i]);
            }

            foreach (string file in files)
            {
                listBox2.Items.Add(file);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            curDir = "..//..";
            string[] dirs = Directory.GetDirectories(curDir);
            string[] files = Directory.GetFiles(curDir);
            int j = dirs.Length;
            for(int i = 0; i < j; i++)
            {
                LoadDirec(dirs[i]);
            }
            foreach (string file in files)
            {
                treeView1.Nodes.Add(file);
            }


        }

        void LoadDirec(string Dir)
        {
            DirectoryInfo dir = new DirectoryInfo(Dir);
            TreeNode tds = treeView1.Nodes.Add(dir.Name);
            tds.Tag = dir.FullName;
            Loadfiles(Dir, tds);
            LoadsubDirec(Dir,tds);
        }

        void LoadsubDirec(string Dir,TreeNode td)
        {
            string[] subdir = Directory.GetDirectories(Dir);
            foreach (string subdirectory in subdir)  
           {  
  
               DirectoryInfo di = new DirectoryInfo(subdirectory);  
               TreeNode tds = td.Nodes.Add(di.Name);  
               tds.Tag = di.FullName;  
               Loadfiles(subdirectory, tds);  
               LoadsubDirec(subdirectory, tds);    
           }  
        }

        void Loadfiles(string Dir,TreeNode td)
        {
            string[] files = Directory.GetFiles(Dir, "*.*");
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tds = td.Nodes.Add(fi.Name);
                tds.Tag = fi.FullName;
            }
        }
    }
}
