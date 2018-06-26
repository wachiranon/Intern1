using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class UserControl1 : UserControl
    {
        int l_x, l_y;
        bool mouse_move = false;
        public int select_start_x, select_start_y, select_end_x, select_end_y;
        List<Control> selectedPoint = new List<Control>();
        List<PointHistory> selectedPoint_anime = new List<PointHistory>();
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

            
            timer1.Tick += (s2,e2) =>
            {
                this.Invoke(new MethodInvoker(()=>
                {
                    if (multi_point.Count != 0 && undo_point.Count != 0)
                    {
                        /////
                        if (TimeCheck == 1)
                        {
                            for (int i = 0; i < multi_point[multi_point.Count - 1]; i++)
                            {
                                Control p = undo_point[undo_point.Count - 1-i].targetPoint;

                                PointHistory p_h = new PointHistory();
                                p_h.targetPoint = p;
                                p_h.X = p.Left;
                                p_h.Y = p.Top;
                                if (!redo_point.Contains(p_h))
                                {
                                    redo_point.Add(p_h);
                                }
                            }

                            TimeCheck = 2;
                        }
                        if (TimeCheck == 2)
                        {
                            int count_multi = 0;
                            int last = undo_point.Count-1;
                            while (count_multi < multi_point[multi_point.Count - 1])
                            {
                                if (undo_point[last-count_multi].targetPoint.Location.X < undo_point[last - count_multi].X)
                                {
                                    undo_point[last - count_multi].targetPoint.Left += 1;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X + 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                else if (undo_point[last - count_multi].targetPoint.Location.X > undo_point[last - count_multi].X)
                                {
                                    undo_point[last - count_multi].targetPoint.Left -= 1;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X - 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                if (undo_point[last - count_multi].targetPoint.Location.Y < undo_point[last - count_multi].Y)
                                {
                                    undo_point[last - count_multi].targetPoint.Top += 1;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y + 10);
                                }
                                else if (undo_point[last - count_multi].targetPoint.Location.Y > undo_point[last - count_multi].Y)
                                {
                                    undo_point[last - count_multi].targetPoint.Top -= 1;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y - 10);
                                }
                                count_multi++;
                            }
                            if ((undo_point[last].targetPoint.Location.X == undo_point[last].X) && (undo_point[last].targetPoint.Location.Y == undo_point[last].Y))
                            {
                                TimeCheck = 3;
                            }
                        }
                        if (TimeCheck == 3)
                        {
                            for (int i = 0; i < multi_point[multi_point.Count - 1];i++)
                            {
                                undo_point.RemoveAt(undo_point.Count - 1);
                            }

                            multi_redo.Add(multi_point[multi_point.Count - 1]);
                            multi_point.RemoveAt(multi_point.Count - 1);

                            TimeCheck = 0;
                            this.timer1.Stop();
                        }
                        /////
                    }
                    else { this.timer1.Stop(); }
                }
                ));
            };
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
//                            p_h.BgColor = p.BackColor;
                            //p_h.command = CommandKind.BgColor;
                            p_h.X = p.Location.X;
                            p_h.Y = p.Location.Y;
                            selectedPoint.Add(p);

                            //              Point_his.Add(p_h);
                            if (!undo_point.Contains(p_h))
                            {
                                undo_point.Add(p_h);
                            }
                        }
                    }
                }
            }
            //multi_point.Add(selectedPoint.Count);
        }


        public void undo_key(object sender, KeyEventArgs e)
        {
            if (undo_point.Count != 0)
            {
                if (e.Control & e.KeyCode == Keys.Z)
                {
                    undo(); 
                }
            }
        }

        public void redo_key(object sender, KeyEventArgs e)
        {
            if (redo_point.Count != 0)
            {
                if (e.Control & e.KeyCode == Keys.X)
                {
                     redo(); 
                }
            }
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach(Panel p in this.Controls)
            {
                p.BackColor = Color.Blue;
            }
            //multi_point.Add(selectedPoint.Count);
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
            //    p_h.BgColor = area.BackColor;
                p_h.X = area.Location.X;
                p_h.Y = area.Location.Y;
                //   Point_his.Add(p_h);
                //if (!undo_point.Contains(p_h))
                //{
                //    undo_point.Add(p_h);
                //}

                this.Controls.Add(area);

            }
            //multi_point.Add(num);
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
                //    p_h.BgColor = p.BackColor;
                    p_h.X = p.Location.X;
                    p_h.Y = p.Location.Y;
                    Point_his.Add(p_h);
                    //redo_point.Add(p_h);
                    //if (!undo_point.Contains(p_h) && selectedPoint.Count > 1)
                    //{
                    //    undo_point.Add(p_h);
                    //}
                }
                multi_point.Add(selectedPoint.Count);
                //multi_point.Add(selectedPoint.Count);
                if (selectedPoint.Count == 1)
                {
                    
                    selectedPoint[0].BackColor = Color.Blue;
                    selectedPoint.Clear();
                }
                
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
               // p_h.BgColor = butt.BackColor;
                p_h.X = butt.Location.X;
                p_h.Y = butt.Location.Y;
                selectedPoint.Add(butt);
                Point_his.Add(p_h);
                if (!undo_point.Contains(p_h))
                {
                    undo_point.Add(p_h);
                }
                //multi_point.Add(selectedPoint.Count);

            }

            //this.MouseUp += Selected;
        }
        public void Button1_Move(object sender, MouseEventArgs e)
        {
            //this.Text = "Move";
            if(mouse_move == true)
            {
                //multi_point.Add(selectedPoint.Count);
                int d_x = e.X - l_x;
                int d_y = e.Y - l_y;
                
                Panel p = new Panel();
                foreach (Control c in selectedPoint)
                {
                    p = (Panel)c;
                    p.Location = new Point(p.Left + d_x, p.Top + d_y);
                }
                
                redo_point.Clear();
                multi_redo.Clear();
            }
        }

        List<int> multi_point = new List<int>();
        List<int> multi_redo = new List<int>();

        public void Save_point()
        {
            List<point_json> p_d = new List<point_json>();
            ////int i = 0;
            //using (FileStream fs = new FileStream("point.bin", FileMode.Create))
            //{
            //    using (BinaryWriter w = new BinaryWriter(fs))
            //    {
            foreach (Control p in this.Controls)
            {
                point_json js = new point_json();
                //            w.Write(p.Left);
                //            w.Write(p.Top);
                js.T = (int)p.Tag;
                js.X = p.Left;
                js.Y = p.Top;
                p_d.Add(js);

            }
            //        fs.Flush();
            //        fs.Close();
            //    }
            //}
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("SavePanel.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                string json = JsonConvert.SerializeObject(p_d);
                serializer.Serialize(writer, p_d);
            }
            if (undo_point.Count > 0)
            {
                List<point_json> undo_js = new List<point_json>();
                for(int i = 0; i <= undo_point.Count-1; i++)
                {
                    point_json new_u = new point_json();
                    new_u.T = (int)undo_point[i].targetPoint.Tag;
                    new_u.X = undo_point[i].X;
                    new_u.Y = undo_point[i].Y;
                    undo_js.Add(new_u);
                }
                JsonSerializer serial2 = new JsonSerializer();
                using (StreamWriter sw2 = new StreamWriter("History.json"))
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    serial2.Serialize(writer2,undo_js );
                    //serial2.Serialize(writer2, multi_point);
                }
            }
            if (multi_point.Count > 0)
            {
                JsonSerializer serial3 = new JsonSerializer();
                using (StreamWriter sw3 = new StreamWriter("ListCountHistory.json"))
                using (JsonWriter writer3 = new JsonTextWriter(sw3))
                {
                    serial3.Serialize(writer3, multi_point);
                }
            }
        }
        
        class point_json
        {
            public int X;
            public int Y;
            public int T;
        } 
        public void Load_point()
        {
            this.Controls.Clear();
            //using (FileStream fs = new FileStream("point.bin", FileMode.Open))
            //{
            //    using (BinaryReader r = new BinaryReader(fs))
            //    {
            //        int i = 0;
            //        while (r.BaseStream.Length != r.BaseStream.Position)
            //        {
            //            int position_x = r.ReadInt32();
            //            int position_y = r.ReadInt32();

            //            //paint_point(position_x, position_y);
            //            Panel area = new Panel();
            //            area.Size = new Size(10, 10);
            //            area.Location = new Point(position_x, position_y);
            //            area.BackColor = Color.Blue;
            //            area.MouseDown += Button1_MouseDown;
            //            area.MouseUp += Button1_MouseUp;
            //            area.MouseMove += Button1_Move;
            //            area.Tag = i;
            //            PointHistory p_h = new PointHistory();
            //            p_h.targetPoint = area;
            //            //p_h.BgColor = area.BackColor;
            //            //p_h.command = CommandKind.BgColor;
            //            p_h.X = area.Location.X;
            //            p_h.Y = area.Location.Y;
            //            //if (!undo_point.Contains(p_h))
            //            //{
            //              //  undo_point.Add(p_h);
            //            //}
            //            //        Point_his.Add(p_h);
            //            this.Controls.Add(area);
            //            i++;
            //        }
            //        fs.Flush();
            //        fs.Close();
            //        //multi_point.Add(i + 1);
            //    }
            //}
            ///// paint point///////
            List<point_json> p = new List<point_json>();
            using (FileStream f_p = new FileStream("SavePanel.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    //using (JsonReader r_p = new JsonTextReader(s_p))
                    //{
                    string r_j = s_p.ReadToEnd();
                    p = JsonConvert.DeserializeObject<List<point_json>>(r_j);
                    ////
                    //}
                    //f_p.Flush();
                    f_p.Close();
                }

            }
            for (int i = 0; i < p.Count; i++)
            {
                Panel area = new Panel();
                area.Size = new Size(10, 10);
                area.Location = new Point(p[i].X, p[i].Y);
                area.BackColor = Color.Blue;
                area.MouseDown += Button1_MouseDown;
                area.MouseUp += Button1_MouseUp;
                area.MouseMove += Button1_Move;
                area.Tag = p[i].T;
                PointHistory p_h = new PointHistory();
                p_h.targetPoint = area;
                p_h.X = p[i].X;
                p_h.Y = p[i].Y;

                this.Controls.Add(area);
            }
            ///// paint point///////

            ///// Add Undo /////
            List<point_json> u = new List<point_json>();
            using (FileStream f_p = new FileStream("History.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();

                    u = JsonConvert.DeserializeObject<List<point_json>>(r_j);
                    f_p.Close();
                }
            }
            //int a = 0;
            while(u.Count>0)
            {
                //Panel u1 = new Panel();
                foreach (Panel u1 in this.Controls)
                {
                    if ((int)u1.Tag == u[0].T)
                    {
                        PointHistory u_h = new PointHistory();
                        //u1.BackColor = Color.Blue;
                        u_h.targetPoint = u1;
                        u_h.X = u[0].X;
                        u_h.Y = u[0].Y;
                        undo_point.Add(u_h);
                        u.RemoveAt(0);
                        break;
                    }
                }
                //a++;
            }
            ///// Add Undo /////

            ///// Add multi undo /////
            List<int> m_p = new List<int>();
            multi_point.Clear();
            using (FileStream f_p = new FileStream("ListCountHistory.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();

                    multi_point = JsonConvert.DeserializeObject<List<int>>(r_j);
                    f_p.Close();
                }
            }

            //for(int b =0; b < m_p.Count; b++)
            //{
            //    multi_point.Add(m_p[b]);
            //}

        }

        public void MyControl_SelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
            {
                //int num_point = this.Controls.Count;
                //Panel m = new Panel();
                foreach (Panel p in this.Controls)
                {
                    
                    p.BackColor = Color.Yellow;
                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = p;
                    //p_h.BgColor = p.BackColor;
                    p_h.X = p.Location.X;
                    p_h.Y = p.Location.Y;
                    //    Point_his.Add(p_h);

                    if (!undo_point.Contains(p_h))
                    {
                        undo_point.Add(p_h);
                    }
                    selectedPoint.Add(p);
                }
                multi_point.Add(selectedPoint.Count);
               // Remove_point(num_point);
            }
        }

        public int TimeCheck = 0;
        // 0 = null
        // 1 = add redo list
        // 2 = move point
        // 3 = remove undo list

        public void undo_anime(int choice)
        {
            if (choice == 2)
            {
                timer1.Interval = choice;
            }
            this.timer1.Start();
            TimeCheck = 1;
        }
        
        public void undo()
        {
            if (multi_point.Count != 0 && undo_point.Count != 0)
            {
                for (int k = 0; k < multi_point[multi_point.Count - 1]; k++)
                {
                    Control p = undo_point[undo_point.Count - 1].targetPoint;

                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = p;
                    p_h.X = p.Left;
                    p_h.Y = p.Top;
                    if (!redo_point.Contains(p_h))
                    {
                        redo_point.Add(p_h);   
                    }
                    undo_point[undo_point.Count - 1].targetPoint.Location = new Point(undo_point[undo_point.Count - 1].X, undo_point[undo_point.Count - 1].Y);
                    undo_point.RemoveAt(undo_point.Count - 1);

                }
                multi_redo.Add(multi_point[multi_point.Count - 1]);
                multi_point.RemoveAt(multi_point.Count-1);
                //Console.WriteLine(multi_point);
            }
        }
        public void redo()
        {
            if (multi_redo.Count != 0 && redo_point.Count != 0)
            {
                for (int k = 0; k < multi_redo[multi_redo.Count - 1]; k++)
                {
                    Control p = redo_point[redo_point.Count - 1].targetPoint;

                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = p;

                    p_h.X = p.Left;
                    p_h.Y = p.Top;
                    if (!undo_point.Contains(p_h))
                    {
                        undo_point.Add(p_h);
                        
                    }
                    redo_point[redo_point.Count - 1].targetPoint.Location = new Point(redo_point[redo_point.Count - 1].X, redo_point[redo_point.Count - 1].Y);
                    redo_point.RemoveAt(redo_point.Count - 1);
                }
                multi_point.Add(multi_redo[multi_redo.Count - 1]);
                multi_redo.RemoveAt(multi_redo.Count - 1);
            }
        }
        class PointHistory
        {
            public Control targetPoint;
            public int X;
            public int Y;
            //public Color BgColor;
            //public CommandKind command;

            public override string ToString()
            {
                if(targetPoint.Tag != null)
                {
                    return  (int)targetPoint.Tag + "," + X + "," + Y + "," ;
                }
                else
                {
                    return  X + "," + Y + ",";
                }
            }
            
        }
        List<PointHistory> Point_his = new List<PointHistory>();
        List<PointHistory> undo_point = new List<PointHistory>();
        List<PointHistory> redo_point = new List<PointHistory>();

        //List<int> list1 = new List<int>();

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
