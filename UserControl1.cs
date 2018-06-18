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
        public int select_start_x, select_start_y, select_end_x, select_end_y;
        List<Panel> selectedPoint = new List<Panel>();
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.KeyDown += MyControl_SelectAll;
            
            this.MouseDown += UserControl1_MouseDown;
            this.MouseDown += select_down;

            this.MouseUp += select_up;

            //this.MouseMove += select_move;

            this.KeyDown += undo_key;
            this.KeyDown += redo_key;
        }

        public void select_down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                select_start_x = e.X;
                select_start_y = e.Y;
            }
        }

        public void select_up(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                select_end_x = e.X;
                select_end_y = e.Y;
            }
            //Panel butt = (Panel)sender;
            foreach (Panel p in this.Controls)
            {
               if(p.Location.X <= select_start_x && p.Location.X >= select_end_x || p.Location.X >= select_start_x && p.Location.X <= select_end_x)
                {
                    if(p.Location.Y <= select_start_y && p.Location.Y >= select_end_y || p.Location.Y >= select_start_y && p.Location.Y <= select_end_y)
                    {
                       
                        l_x = e.X;
                        l_y = e.Y;
                        p.BackColor = Color.Yellow;
                        if (!selectedPoint.Contains(p))
                        {
                            PointHistory p_h = new PointHistory();
                            p_h.targetPoint = p;
                            p_h.BgColor = p.BackColor;
                            //p_h.command = CommandKind.BgColor;
                            p_h.X = p.Location.X;
                            p_h.Y = p.Location.Y;
                            selectedPoint.Add(p);
                            Point_his.Add(p_h);
                            //undo_point.Add(p_h);
                        }
                    }
                }
            }
        }

        public void select_move(object sender, MouseEventArgs e){
            Panel p = new Panel();
            

        }

        public void undo_key(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.Z)
            {
                undo();
            }
        }

        public void redo_key(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.X)
            {
                redo();
            }
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach(Panel p in this.Controls)
            {
                p.BackColor = Color.Blue;
            }
            selectedPoint.Clear();
        }

        public void paint_point(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Panel area = new Panel();
                area.Size = new Size(10, 10);
                area.Location = new Point(i*10, i*10);
                area.BackColor = Color.Blue;
                area.MouseDown += Button1_MouseDown;
                area.MouseUp += Button1_MouseUp;
                area.MouseMove += Button1_Move;
                area.Tag = i;
                PointHistory p_h = new PointHistory();
                p_h.targetPoint = area;
                p_h.BgColor = area.BackColor;
                p_h.X = area.Location.X;
                p_h.Y = area.Location.Y;
                Point_his.Add(p_h);

                this.Controls.Add(area);

            }
        }

        public void Button1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_move = false;

            if (ModifierKeys != Keys.Control)
            {
                foreach (Panel p in selectedPoint)
                {
                    p.BackColor = Color.Blue;
                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = p;
                    p_h.BgColor = p.BackColor;
                    //p_h.command = CommandKind.BgColor;
                    p_h.X = p.Location.X;
                    p_h.Y = p.Location.Y;

                    redo_point.Add(p_h);
                    //undo_point.Add(p_h);
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
                PointHistory p_h = new PointHistory();
                p_h.targetPoint = butt;
                p_h.BgColor = butt.BackColor;
                //p_h.command = CommandKind.BgColor;
                p_h.X = butt.Location.X;
                p_h.Y = butt.Location.Y;
                selectedPoint.Add(butt);
                Point_his.Add(p_h);
                undo_point.Add(p_h);
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
                
                Panel p = new Panel();
                foreach (Control c in selectedPoint)
                {
                    p = (Panel)c;
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
                    int i = 0;
                    while (r.BaseStream.Length != r.BaseStream.Position)
                    {
                        int position_x = r.ReadInt32();
                        int position_y = r.ReadInt32();

                        //paint_point(position_x, position_y);
                        Panel area = new Panel();
                        area.Size = new Size(10, 10);
                        area.Location = new Point(position_x, position_y);
                        area.BackColor = Color.Blue;
                        area.MouseDown += Button1_MouseDown;
                        area.MouseUp += Button1_MouseUp;
                        area.MouseMove += Button1_Move;
                        area.Tag = i;
                        PointHistory p_h = new PointHistory();
                        p_h.targetPoint = area;
                        p_h.BgColor = area.BackColor;
                        //p_h.command = CommandKind.BgColor;
                        p_h.X = area.Location.X;
                        p_h.Y = area.Location.Y;
                        //undo_point.Add(p_h);
                        Point_his.Add(p_h);
                        this.Controls.Add(area);
                        i++;
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
                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = p;
                    p_h.BgColor = p.BackColor;
                    //p_h.command = CommandKind.BgColor;
                    p_h.X = p.Location.X;
                    p_h.Y = p.Location.Y;
                    Point_his.Add(p_h);
                    undo_point.Add(p_h);
                    selectedPoint.Add(p);

                }
               // Remove_point(num_point);
            }
        }
        
        public void undo()
        {
            int j = undo_point.Count;
            int n = Point_his.Count;
            for(int i = 0; i < n; i++)
            {
                if(undo_point[j-1].targetPoint.Tag == Point_his[i].targetPoint.Tag)
                {
                    Point_his[i].targetPoint.Location = new Point(undo_point[j - 1].X, undo_point[j - 1].Y);
                }
            }

            if (!redo_point.Contains(undo_point[j - 1]))
            {
                redo_point.Add(undo_point[j - 1]);

            }
            undo_point.RemoveAt(j - 1);

        }
        public void redo()
        {
            int j = redo_point.Count;
            int n = Point_his.Count;
            for (int i = 0; i < n; i++)
            {
                if (redo_point[j - 1].targetPoint.Tag == Point_his[i].targetPoint.Tag)
                {
                    Point_his[i].targetPoint.Location = new Point(redo_point[j - 1].X, redo_point[j - 1].Y);
                }
            }
            
            if (!undo_point.Contains(redo_point[j - 1]))
            {
                undo_point.Add(redo_point[j - 1]);
            }

            redo_point.RemoveAt(j - 1);

        }

        public void remove_point()
        {
            int n = Point_his.Count;
            for (int i = 0; i < n; i++)
            {
                if (Point_his[i].BgColor == Color.Yellow)
                {
                    Point_his.RemoveAt(i);
                }
            }
        }


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
            //public CommandKind command;

            public override string ToString()
            {
                if(targetPoint.Tag != null)
                {
                    return  (int)targetPoint.Tag + "," + X + "," + Y + "," + BgColor;
                }
                else
                {
                    return  X + "," + Y + "," + BgColor;
                }
            }
            
        }
        List<PointHistory> Point_his = new List<PointHistory>();
        List<PointHistory> undo_point = new List<PointHistory>();
        List<PointHistory> redo_point = new List<PointHistory>();

        List<int> list1 = new List<int>();

        public string ShowLogs()
        {
            StringBuilder st = new StringBuilder();
            int j = Point_his.Count;
            for(int i = 0; i < j; i++)
            {
                st.AppendLine(Point_his[i].ToString());
            }
            return st.ToString();
        }
        public string ShowLogs_undo()
        {
            StringBuilder st = new StringBuilder();
            int j = undo_point.Count;
            for (int i = 0; i < j; i++)
            {
                st.AppendLine(undo_point[i].ToString());
            }
            return st.ToString();
        }
        public string ShowLogs_redo()
        {
            StringBuilder st = new StringBuilder();
            int j = redo_point.Count;
            for (int i = 0; i < j; i++)
            {
                st.AppendLine(redo_point[i].ToString());
            }
            return st.ToString();
        }

    }
}
