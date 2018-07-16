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

using System.Reflection;
using SharpConnect.WebServers;
using SharpConnect;

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
        int speed=1,speed_x,speed_y;
        //AppHost testApp;
        SharpConnect.AppHost testApp;


        private void UserControl1_Load(object sender, EventArgs e)
        {
            ///--------------Webserver
            //testApp = new AppHost(); ;
            testApp = new SharpConnect.AppHost();
            testApp.RegisterModule(new MyModule());
            testApp.RegisterModule(new MyModule2());
            testApp.RegisterModule(new MyModule3());
            testApp.RegisterModule(new MyAdvanceMathModule());
            testApp.RegisterModule(new MMath1());

            //Area.MouseDown += Area.Selected;
            WebServer webServer = new WebServer(8080, true, testApp.HandleRequest);
            //test websocket
            var webSocketServer = new WebSocketServer();
            webSocketServer.SetOnNewConnectionContext(ctx =>
            {
                ctx.SetMessageHandler(testApp.HandleWebSocket);
            });
            webServer.WebSocketServer = webSocketServer;
            webServer.Start();
            //-----------------Webserver

            this.KeyDown += MyControl_SelectAll;
            
            this.MouseDown += UserControl1_MouseDown;
            this.MouseDown += select_down;

            this.MouseUp += select_up;

            //this.MouseMove += select_move;

            this.KeyDown += undo_key;
            this.KeyDown += redo_key;

            timer2.Tick += (s2, e2) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (multi_redo.Count != 0 && redo_point.Count != 0)
                    {
                        /////
                        if (TimeCheck == 1)
                        {
                            for (int i = 0; i < multi_redo[multi_redo.Count - 1]; i++)
                            {
                                Control p = redo_point[redo_point.Count - 1 - i].targetPoint;

                                PointHistory p_h = new PointHistory();
                                p_h.targetPoint = p;
                                p_h.X = p.Left;
                                p_h.Y = p.Top;
                                if (!undo_point.Contains(p_h))
                                {
                                    undo_point.Add(p_h);
                                }
                                speed_x = speed;
                                speed_y = speed;
                            }

                            TimeCheck = 2;
                        }
                        if (TimeCheck == 2)
                        {
                            int count_multi = 0;
                            int last = redo_point.Count - 1;
                            while (count_multi < multi_redo[multi_redo.Count - 1])
                            {
                                int d_x = Math.Abs(redo_point[last - count_multi].targetPoint.Location.X - redo_point[last - count_multi].X);
                                int d_y = Math.Abs(redo_point[last - count_multi].targetPoint.Location.Y - redo_point[last - count_multi].Y);
                                if (speed > d_x) { speed_x = d_x; }
                                if (redo_point[last - count_multi].targetPoint.Location.X < redo_point[last - count_multi].X)
                                {
                                    redo_point[last - count_multi].targetPoint.Left += speed_x;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X + 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                else if (redo_point[last - count_multi].targetPoint.Location.X > redo_point[last - count_multi].X)
                                {
                                    redo_point[last - count_multi].targetPoint.Left -= speed_x;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X - 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                if(speed > d_y) { speed_y = d_y; }
                                if (redo_point[last - count_multi].targetPoint.Location.Y < redo_point[last - count_multi].Y)
                                {
                                    redo_point[last - count_multi].targetPoint.Top += speed_y;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y + 10);
                                }
                                else if (redo_point[last - count_multi].targetPoint.Location.Y > redo_point[last - count_multi].Y)
                                {
                                    redo_point[last - count_multi].targetPoint.Top -= speed_y;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y - 10);
                                }
                                count_multi++;
                            }
                            if ((redo_point[last].targetPoint.Location.X == redo_point[last].X) && (redo_point[last].targetPoint.Location.Y == redo_point[last].Y))
                            {
                                TimeCheck = 3;
                            }
                        }
                        if (TimeCheck == 3)
                        {
                            for (int i = 0; i < multi_redo[multi_redo.Count - 1]; i++)
                            {
                                redo_point.RemoveAt(redo_point.Count - 1);
                            }

                            multi_point.Add(multi_redo[multi_redo.Count - 1]);
                            multi_redo.RemoveAt(multi_redo.Count - 1);

                            TimeCheck = 0;
                            speed = 1;
                            speed_x = 0;
                            speed_y = 0;
                            this.timer2.Stop();
                        }
                        /////
                    }
                    else { this.timer2.Stop(); }
                }
                ));
            };

            

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
                                speed_x = speed;
                                speed_y = speed;
                            }

                            TimeCheck = 2;
                        }
                        if (TimeCheck == 2)
                        {
                            int count_multi = 0;
                            int last = undo_point.Count-1;
                            while (count_multi < multi_point[multi_point.Count - 1])
                            {
                                int d_x = Math.Abs(undo_point[last - count_multi].targetPoint.Location.X - undo_point[last - count_multi].X);
                                int d_y = Math.Abs(undo_point[last - count_multi].targetPoint.Location.Y - undo_point[last - count_multi].Y);
                                if (speed > d_x) { speed_x = d_x; }
                                if (undo_point[last-count_multi].targetPoint.Location.X < undo_point[last - count_multi].X)
                                {
                                    undo_point[last - count_multi].targetPoint.Left += speed_x;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X + 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                else if (undo_point[last - count_multi].targetPoint.Location.X > undo_point[last - count_multi].X)
                                {
                                    undo_point[last - count_multi].targetPoint.Left -= speed_x;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X - 10, undo_point[undo_point.Count - 1].targetPoint.Location.Y);
                                }
                                if(speed > d_y) { speed_y = d_y; }
                                if (undo_point[last - count_multi].targetPoint.Location.Y < undo_point[last - count_multi].Y)
                                {
                                    undo_point[last - count_multi].targetPoint.Top += speed_y;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y + 10);
                                }
                                else if (undo_point[last - count_multi].targetPoint.Location.Y > undo_point[last - count_multi].Y)
                                {
                                    undo_point[last - count_multi].targetPoint.Top -= speed_y;//Location = new Point(undo_point[undo_point.Count - 1].targetPoint.Location.X, undo_point[undo_point.Count - 1].targetPoint.Location.Y - 10);
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
                            speed = 1;
                            speed_x = 0;
                            speed_y = 0;
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

        public void AddTxtBox()
        {
            int count = 0;
            foreach (Panel p in this.Controls) { count++; }
            Panel area = new Panel();
            TextBox t = new TextBox();
            t.Multiline = true;
            t.Size = new Size(50, 20);
            t.Location = new Point(2, 2);
            t.Tag = count;

            area.Size = new Size(t.Size.Width + 4, t.Size.Height + 4);
            area.Location = new Point(0, count * 25);
            area.BackColor = Color.Blue;
            area.MouseDown += Button1_MouseDown;
            area.MouseUp += Button1_MouseUp;
            area.MouseMove += Button1_Move;
            area.Tag = "t";
            area.Controls.Add(t);
            
            PointHistory p_h = new PointHistory();
            p_h.targetPoint = area;
            p_h.X = area.Location.X;
            p_h.Y = area.Location.Y;
            p_h.messegetxt = t.Text;
            p_h.sizetxtbox_w = t.Size.Width;
            p_h.sizetxtbox_h = t.Size.Height;

            this.Controls.Add(area);

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
                    if ((string)p.Tag == "t")
                    {
                        foreach (TextBox t in p.Controls)
                        {
                            p_h.messegetxt = t.Text;
                            p_h.sizetxtbox_w = t.Size.Width;
                            p_h.sizetxtbox_h = t.Size.Height;
                        }
                    }
                    Point_his.Add(p_h);
                }
                multi_point.Add(selectedPoint.Count);
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
                p_h.X = butt.Location.X;
                p_h.Y = butt.Location.Y;
                if (butt.Tag == "t")
                {
                    foreach (TextBox t in butt.Controls)
                    {
                        p_h.sizetxtbox_w = t.Size.Width;
                        p_h.sizetxtbox_h = t.Size.Height;
                        p_h.messegetxt = t.Text;
                    }
                }
                selectedPoint.Add(butt);
                Point_his.Add(p_h);
                if (!undo_point.Contains(p_h))
                {
                    undo_point.Add(p_h);
                }
            }
        }

        public void Button1_Move(object sender, MouseEventArgs e)
        {
            if(mouse_move == true)
            {
                int d_x = e.X - l_x;
                int d_y = e.Y - l_y;

                Panel p = new Panel();
                TextBox t = new TextBox();
                foreach (Control c in selectedPoint)
                {
                    p = (Panel)c;
                    if(ModifierKeys == Keys.Alt)
                    {
                        p.Size = new Size(p.Size.Width + d_x,p.Size.Height + d_y);
                        foreach (object d in p.Controls)
                        {
                            t = (TextBox)d;
                            t.Size = new Size(t.Size.Width + d_x,t.Size.Height + d_y);
                        }
                        l_x = e.X;
                        l_y = e.Y;
                    }
                    else
                    {
                        p.Location = new Point(p.Left + d_x, p.Top + d_y);
                    }
                }
                redo_point.Clear();
                multi_redo.Clear();
            }
        }

        List<int> multi_point = new List<int>();
        List<int> multi_redo = new List<int>();

        public void load_form_server()
        {
            string result;
            string filepath;
            Image im;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/savejson.json");
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            List<point_json> savePanel = new List<point_json>();
            List<point_json> undoList = new List<point_json>();
            List<int> hisList = new List<int>();

            string[] data = result.Split('|');
            if(data.Length == 3)
            {
                this.Controls.Clear();
                multi_point.Clear();
                undo_point.Clear();
                savePanel = JsonConvert.DeserializeObject<List<point_json>>(data[0]);
                undoList = JsonConvert.DeserializeObject<List<point_json>>(data[1]);
                hisList = JsonConvert.DeserializeObject<List<int>>(data[2]);

                //----Add panel----//
                for (int i = 0; i < savePanel.Count; i++)
                {
                    Panel area = new Panel();
                    area.Size = new Size(10, 10);
                    area.Location = new Point(savePanel[i].X, savePanel[i].Y);
                    area.BackColor = Color.Blue;
                    area.MouseDown += Button1_MouseDown;
                    area.MouseUp += Button1_MouseUp;
                    area.MouseMove += Button1_Move;
                    area.Tag = savePanel[i].T;
                    if (savePanel[i].T == "t")
                    {
                        TextBox txt = new TextBox();
                        txt.Multiline = true;
                        txt.Size = new Size(savePanel[i].sizetxt_X, savePanel[i].sizetxt_y);
                        txt.Text = savePanel[i].path;
                        txt.Location = new Point(2, 2);
                        area.Size = new Size(savePanel[i].sizetxt_X + 4, savePanel[i].sizetxt_y + 4);
                        area.Controls.Add(txt);
                    }
                    else if (savePanel[i].T == "p")
                    {
                        PictureBox newptb = new PictureBox();
                        newptb.SizeMode = PictureBoxSizeMode.StretchImage;
                        newptb.Size = new Size(32, 32);
                        filepath = savePanel[i].path;
                        Byte[] b = Convert.FromBase64String(filepath);
                        using (var ms = new MemoryStream(b, 0, b.Length))
                        {
                            im = Image.FromStream(ms, true);
                            newptb.Image = im;
                            newptb.Location = new Point(9, 10);
                            area.Size = new Size(50, 50);
                            area.Controls.Add(newptb);
                        }
                    }

                    PointHistory p_h = new PointHistory();
                    p_h.targetPoint = area;
                    p_h.X = savePanel[i].X;
                    p_h.Y = savePanel[i].Y;

                    this.Controls.Add(area);
                }
                //----Add panel----//

                //----Add Undo-----//
                while (undoList.Count > 0)
                {
                    foreach (Panel u1 in this.Controls)
                    {
                        if ((string)u1.Tag == undoList[0].T)
                        {
                            PointHistory u_h = new PointHistory();
                            u_h.targetPoint = u1;
                            u_h.X = undoList[0].X;
                            u_h.Y = undoList[0].Y;
                            if (undoList[0].T == "t")
                            {
                                u_h.sizetxtbox_w = undoList[0].sizetxt_X;
                                u_h.sizetxtbox_h = undoList[0].sizetxt_y;
                                u_h.messegetxt = undoList[0].path;
                            }
                            undo_point.Add(u_h);
                            undoList.RemoveAt(0);
                            break;
                        }
                    }
                }
                //----Add Undo-----//
                //----Add Multipoint----//
                multi_point = hisList;
            }
        }

        public string filePhotoPath;
        public void ImportPictureBox()
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.jpg;*.png)|*.jpg;*.png ";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                PictureBox newptb = new PictureBox();
                Panel mypanel1 = new Panel();
                mypanel1.Size = new Size(50, 50);
                mypanel1.BackColor = Color.Blue;
                mypanel1.Tag = "p";
                newptb.Location = new Point(mypanel1.Left + 9, mypanel1.Top + 10);
                var image = Image.FromFile(opf.FileName);
                newptb.Image = image;
                newptb.SizeMode = PictureBoxSizeMode.StretchImage;
                newptb.Size = new Size(32, 32);
                newptb.Tag = "p";
                
                mypanel1.Controls.Add(newptb);
                mypanel1.MouseDown += Button1_MouseDown;
                mypanel1.MouseUp += Button1_MouseUp;
                mypanel1.MouseMove += Button1_Move;
                Controls.Add(mypanel1);
                filePhotoPath = opf.FileName;
                //Read_Pnl(Controls);
            }
        }
        public string base64String;



        class point_json
        {
            public int X;
            public int Y;
            public string T;
            public string path;
            public int sizetxt_X;
            public int sizetxt_y;
        }


        public void Save_point()
        {
            
            
            List<point_json> p_d = new List<point_json>();
            foreach (Control p in this.Controls)
            {
                point_json js = new point_json();
                js.T = (string)p.Tag;
                js.X = p.Left;
                js.Y = p.Top;
                if ((string)p.Tag == "t")
                {
                    foreach(TextBox t in p.Controls)
                    {
                        js.sizetxt_X = t.Size.Width;
                        js.sizetxt_y = t.Size.Height;
                        js.path = t.Text;
                    }
                }
                else if((string)p.Tag == "p")
                {
                    
                    foreach (PictureBox pb in p.Controls)
                    {
                        using (Image image = Image.FromFile(filePhotoPath))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                byte[] imageBytes = m.ToArray();

                                base64String = Convert.ToBase64String(imageBytes);
                            }
                        }
                        js.path = base64String;
                    }
                }
                p_d.Add(js);

            }
            //------Save Image----//

            

            //--------Point----//
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter("SavePanel.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                string json = JsonConvert.SerializeObject(p_d);
                serializer.Serialize(writer, p_d);
            }

            //----

            //-------Undo List-----//
            if (undo_point.Count > 0)
            {
                List<point_json> undo_js = new List<point_json>();
                for (int i = 0; i <= undo_point.Count - 1; i++)
                {
                    point_json new_u = new point_json();
                    new_u.T = (string)undo_point[i].targetPoint.Tag;
                    new_u.X = undo_point[i].X;
                    new_u.Y = undo_point[i].Y;
                    if ((string)undo_point[i].targetPoint.Tag == "t")
                    {
                        new_u.sizetxt_X = undo_point[i].sizetxtbox_w;
                        new_u.sizetxt_y = undo_point[i].sizetxtbox_h;
                        new_u.path = undo_point[i].messegetxt;
                    }
                    else if((string)undo_point[i].targetPoint.Tag == "p")
                    {
                        //new_u.path = undo_point[i].
                    }
                    undo_js.Add(new_u);
                }
                JsonSerializer serial2 = new JsonSerializer();
                using (StreamWriter sw2 = new StreamWriter("History.json"))
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    serial2.Serialize(writer2, undo_js);
                    //serial2.Serialize(writer2, multi_point);
                }
            }
            //---------Undo Count--------//
            if (multi_point.Count > 0)
            {
                JsonSerializer serial3 = new JsonSerializer();
                using (StreamWriter sw3 = new StreamWriter("ListCountHistory.json"))
                using (JsonWriter writer3 = new JsonTextWriter(sw3))
                {
                    serial3.Serialize(writer3, multi_point);
                }
            }
            ///-----------save Json-----------
            ///
            ///-----------save to other computer
        }

        public void Load_point()
        {
            this.Controls.Clear();
            ///// paint point///////
            string filepath;
            Image im;
            List<point_json> p = new List<point_json>();
            using (FileStream f_p = new FileStream("SavePanel.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();
                    p = JsonConvert.DeserializeObject<List<point_json>>(r_j);
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
                if(p[i].T == "t")
                {
                    TextBox txt = new TextBox();
                    txt.Multiline = true;
                    txt.Size = new Size(p[i].sizetxt_X, p[i].sizetxt_y);
                    txt.Text = p[i].path;
                    txt.Location = new Point(2, 2);
                    area.Size = new Size(p[i].sizetxt_X+4, p[i].sizetxt_y+4);
                    area.Controls.Add(txt);
                }else if(p[i].T == "p")
                {
                    PictureBox newptb = new PictureBox();
                    newptb.SizeMode = PictureBoxSizeMode.StretchImage;
                    newptb.Size = new Size(32, 32);
                    filepath = p[i].path;
                    Byte[] b = Convert.FromBase64String(filepath);
                    using (var ms = new MemoryStream(b, 0, b.Length))
                    {
                        im = Image.FromStream(ms, true);
                        newptb.Image = im;
                        newptb.Location = new Point(9, 10);
                        area.Size = new Size(50, 50);
                        area.Controls.Add(newptb);
                        
                    }
                }
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
                    if ((string)u1.Tag == u[0].T)
                    {
                        PointHistory u_h = new PointHistory();
                        //u1.BackColor = Color.Blue;
                        u_h.targetPoint = u1;
                        u_h.X = u[0].X;
                        u_h.Y = u[0].Y;
                        if (u[0].T == "t")
                        {
                            u_h.sizetxtbox_w = u[0].sizetxt_X;
                            u_h.sizetxtbox_h = u[0].sizetxt_y;
                            u_h.messegetxt = u[0].path;
                        }
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

        public void undo_anime(int choice,float t)
        {
            //V = S/T
            //S = V*T
            //T = S/V
            if (multi_point.Count != 0 && undo_point.Count != 0)
            {
                int dis_x = Math.Abs(undo_point[undo_point.Count - 1].targetPoint.Location.X - undo_point[undo_point.Count - 1].X);
                int dis_y = Math.Abs(undo_point[undo_point.Count - 1].targetPoint.Location.Y - undo_point[undo_point.Count - 1].Y);
                double distance = Math.Sqrt((dis_x * dis_x) + (dis_y * dis_y));
                double time;
                if (choice == 1)//Fix Speed
                {
                    //time = (int)distance;
                    //time = (int)(time/ (t));
                    //timer1.Interval = time;
                    speed = (int)t;
                }
                else if (choice == 2)//Fix Time
                {

                    time = (t+2) * 1000;
                    speed = (int)(distance / time);
                    if ((distance / time) < 1)
                    {
                        speed = 1;
                        time = time / distance;

                        timer1.Interval = (int)time;
                    }

                }
                this.timer1.Start();
                TimeCheck = 1;
            }
        }

        public void redo_anime(int choice, float t)
        {
            //V = S/T
            //S = V*T
            //T = S/V
            if (multi_redo.Count != 0 && redo_point.Count != 0)
            {
                int dis_x = Math.Abs(redo_point[redo_point.Count - 1].targetPoint.Location.X - redo_point[redo_point.Count - 1].X);
                int dis_y = Math.Abs(redo_point[redo_point.Count - 1].targetPoint.Location.Y - redo_point[redo_point.Count - 1].Y);
                double distance = Math.Sqrt((dis_x * dis_x) + (dis_y * dis_y));
                double time;
                if (choice == 1)//Fix Speed
                {
                    //time = (int)distance;
                    //time = (int)(time / (t));
                    //timer2.Interval = time;
                    speed = (int)t;
                }
                else if (choice == 2)//Fix Time
                {
                    time = (t+2) * 1000;
                    speed = (int)(distance / time);
                    if ((distance / time) < 1)
                    {
                        speed = 1;
                        time = time / distance;

                        timer2.Interval = (int)time;
                    }

                }
                this.timer2.Start();
                TimeCheck = 1;
            }
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
                    p_h.targetPoint.Tag = p.Tag;
                    if ((string)undo_point[undo_point.Count - 1].targetPoint.Tag == "t")
                    {
                        foreach (TextBox t in undo_point[undo_point.Count - 1].targetPoint.Controls)
                        {
                            p_h.messegetxt = t.Text;
                            p_h.sizetxtbox_w = t.Size.Width;
                            p_h.sizetxtbox_h = t.Size.Height;
                            TextBox newtxt = new TextBox();
                            newtxt.Multiline = true;
                            newtxt.Text = undo_point[undo_point.Count - 1].messegetxt;
                            newtxt.Size = new Size(undo_point[undo_point.Count - 1].sizetxtbox_w, undo_point[undo_point.Count - 1].sizetxtbox_h);
                            newtxt.Location = new Point(2, 2);
                            undo_point[undo_point.Count - 1].targetPoint.Controls.Clear();
                            undo_point[undo_point.Count - 1].targetPoint.Controls.Add(newtxt);
                            undo_point[undo_point.Count - 1].targetPoint.Size = new Size(newtxt.Size.Width + 4, newtxt.Size.Height + 4);
                        }
                    }
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
                    p_h.targetPoint.Tag = p.Tag;
                    if ((string)redo_point[redo_point.Count - 1].targetPoint.Tag == "t")
                    {
                        foreach (TextBox t in redo_point[redo_point.Count - 1].targetPoint.Controls)
                        {
                            p_h.messegetxt = t.Text;
                            p_h.sizetxtbox_w = t.Size.Width;
                            p_h.sizetxtbox_h = t.Size.Height;
                            TextBox newtxt = new TextBox();
                            newtxt.Multiline = true;
                            newtxt.Text = redo_point[redo_point.Count - 1].messegetxt;
                            newtxt.Size = new Size(redo_point[redo_point.Count - 1].sizetxtbox_w, redo_point[redo_point.Count - 1].sizetxtbox_h);
                            newtxt.Location = new Point(2, 2);
                            redo_point[redo_point.Count - 1].targetPoint.Controls.Clear();
                            redo_point[redo_point.Count - 1].targetPoint.Controls.Add(newtxt);
                            redo_point[redo_point.Count - 1].targetPoint.Size = new Size(newtxt.Size.Width + 4, newtxt.Size.Height + 4);
                        }
                    }
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
            public string messegetxt = "";
            public int sizetxtbox_w = 0;
            public int sizetxtbox_h = 0; 
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
