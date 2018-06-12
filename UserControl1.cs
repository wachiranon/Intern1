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
        int l_x, l_y;
        bool mouse_move = false;
        List<Panel> selectedPoint = new List<Panel>();
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.KeyDown += MyControl_SelectAll;
            
            this.MouseDown += UserControl1_MouseDown;
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach(Panel p in selectedPoint)
            {
                p.BackColor = Color.Blue;
            }
            selectedPoint.Clear();
        }

        public void paint_point(int x, int y)
        {
            Panel area = new Panel();
            area.Size = new Size(10, 10);
            area.Location = new Point(x, y);
            area.BackColor = Color.Blue;
            area.MouseDown += Button1_MouseDown;
            area.MouseUp += Button1_MouseUp;
            area.MouseMove += Button1_Move;
            this.Controls.Add(area);
        }

        public void Button1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_move = false;
            if (ModifierKeys != Keys.Control)
            {
                foreach (Panel p in selectedPoint)
                {
                    p.BackColor = Color.Blue;
                }
                selectedPoint.Clear();
            }
            
        }

        public void Button1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_move = true;
            Panel butt = (Panel)sender;
            l_x = e.X;
            l_y = e.Y;
            butt.BackColor = Color.Yellow;
            if (!selectedPoint.Contains(butt))
            {
                selectedPoint.Add(butt);
            }

            //this.MouseUp += Selected;
        }
        public void Button1_Move(object sender, MouseEventArgs e)
        {
            //this.Text = "Move";
            if(mouse_move == true)
            {
                int j = selectedPoint.Count;
                int d_x = e.X - l_x;
                int d_y = e.Y - l_y;
                foreach (Control c in selectedPoint)
                {
                    Panel p = (Panel)c;
                    p.Location = new Point(p.Left + d_x, p.Top + d_y);

                }
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
                }
            }
        }

        public void MyControl_SelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
            {
                //int num_point = this.Controls.Count;
                //Panel m = new Panel();
                foreach (Panel p in this.Controls)
                {
                    //Panel newPoint = new Panel();
                    //newPoint.BackColor = Color.Yellow;
                    //newPoint.Size = new Size(10, 10);
                    //newPoint.Location = p.Location;
                    //newPoint.MouseDown += SelectAll_MouseDown;
                    //newPoint.MouseUp += SelectAll_MouseUp;
                    //newPoint.MouseMove += SelectAll_MouseMove;

                    //this.Controls.Add(newPoint);
                    p.BackColor = Color.Yellow;
                    selectedPoint.Add(p);

                }
               // Remove_point(num_point);
            }
        }


        List<PointHistory> h_p = new List<PointHistory>();
        enum CommandKind
        {
            Unknown,
            Position,
            BgColor
        }
        class PointHistory
        {
            public Panel targetPoint;
            public int X;
            public int Y;
            public Color BgColor;
            public CommandKind command;

            public override string ToString()
            {
                if(targetPoint.Tag != null)
                {
                    return command + (int)targetPoint.Tag + "," + X + "," + Y + "," + BgColor;
                }
                else
                {
                    return command + X + "," + Y + "," + BgColor;
                }
            }
        }
    }
}
