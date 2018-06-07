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
    public partial class UserControl1 : UserControl
    {
        int x, y;
        bool mouse_move = false;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        public void paint_point(int x, int y)
        {
            UserControl1 area = new UserControl1();
            //Panel area = new Panel();
            area.Size = new Size(10, 10);
            area.Location = new Point(x, y);
            area.BackColor = Color.Blue;

            this.Controls.Add(area);
        }

        private void Button1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_move = false;

            Control butt = (Control)sender;
            butt.BackColor = Color.Blue;
        }

        private void Button1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_move = true;
            Control butt = (Control)sender;
            x = e.X;
            y = e.Y;
            butt.BackColor = Color.Yellow;
        }

        private void Button1_Move(object sender, MouseEventArgs e)
        {
            //this.Text = "Move";
            if(mouse_move == true)
            {
                Control p = (Control)sender;
                p.Location = new Point(e.X + p.Left - x, e.Y + p.Top - y);
            }
        }

        public void point_move(object panel)
        {
            foreach (Control p in this.Controls)
            {
                p.MouseUp += Button1_MouseUp;
                p.MouseDown += Button1_MouseDown;
                p.MouseMove += Button1_Move;

            }
        }

        public void Save_point()
        {
            using (FileStream fs = new FileStream("point.bin", FileMode.Create))
            {
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    foreach (Control p in this.Controls)
                    {
                        w.Write(p.Left);
                        w.Write(p.Top);
                    }
                    fs.Flush();
                    fs.Close();
                }
            }
        }

        public void Load_point()
        {
            using (FileStream fs = new FileStream("point.bin", FileMode.Open))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    while (r.BaseStream.Length != r.BaseStream.Position)
                    {
                        int position_x = r.ReadInt32();
                        int position_y = r.ReadInt32();

                        paint_point(position_x, position_y);
                    }
                    fs.Flush();
                    fs.Close();
                    point_move(this.Controls);
                }
            }
        }
    }
}
