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
using Newtonsoft.Json.Serialization;
using SharpConnect.WebServers;
using SharpConnect;



namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        //bool mouse_move = false;
        //int x, y;
        public Form2()
        {
            
            InitializeComponent();
            //this.button1.MouseDown += Button1_MouseDown;
            //this.button1.MouseUp += Button1_MouseUp;
        }

        //private void Button1_MouseUp(object sender, MouseEventArgs e)
        //{
          //  mouse_move = false;

          //  Control butt = (Control)sender;
          //  butt.BackColor = Color.Blue;
        //}

        //private void Button1_MouseDown(object sender, MouseEventArgs e)
        //{
           // mouse_move = true;
           // Control butt = (Control)sender;
           // x = e.X;
           // y = e.Y;
           // butt.BackColor = Color.Yellow;
        //}

        //private void Button1_Move(object sender, MouseEventArgs e)
        //{
            //this.Text = "Move";
          //  if(mouse_move == true)
          //  {
          //      Control p = (Control)sender;
          //      p.Location = new Point(e.X + p.Left - x, e.Y + p.Top - y);
          //  }
        //}


        private void Form2_Load(object sender, EventArgs e)
        {
            //AppHost testApp = new AppHost(); ;

            ////Area.MouseDown += Area.Selected;
            //WebServer webServer = new WebServer(8080, true, testApp.HandleRequest);
            ////test websocket 
            //var webSocketServer = new WebSocketServer();
            //webSocketServer.SetOnNewConnectionContext(ctx =>
            //{
            //    ctx.SetMessageHandler(testApp.HandleWebSocket);
            //});
            //webServer.WebSocketServer = webSocketServer;
            //webServer.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PointNum.Text != null)
            {
                int p_num = int.Parse(PointNum.Text);

                Area.paint_point(p_num);
            }
            
            
            //Area.point_move();

        }

        //private void point_move(object panel)
        //{
          //  foreach(Control p in this.Area.Controls)
          //  {
          //      p.MouseUp += Button1_MouseUp;
          //      p.MouseDown += Button1_MouseDown;
          //      p.MouseMove += Button1_Move;

          //  }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            this.Area.Controls.Clear();
            Area.Load_point();
            
            //Area.point_move();
      //      using (FileStream fs = new FileStream("point.bin", FileMode.Open))
      //      {
      //          using (BinaryReader r = new BinaryReader(fs))
      //          {
      //              while (r.BaseStream.Length != r.BaseStream.Position)
      //              {
      //                  int position_x = r.ReadInt32();
      //                  int position_y = r.ReadInt32();

      //                  paint_point(position_x, position_y);
      //              }
      //              fs.Flush();
      //              fs.Close();
      //              point_move(this.Area.Controls);
      //          }
      //      }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //..

            //..
            Area.Save_point();
        //    using (FileStream fs = new FileStream("point.bin", FileMode.Create))
        //    {
        //        using (BinaryWriter w = new BinaryWriter(fs))
        //        {
        //            foreach (Control p in this.Area.Controls)
        //            {
        //                w.Write(p.Left);
        //                w.Write(p.Top);
        //            }
        //            fs.Flush();
        //            fs.Close();
        //        }
        //    }
        }

        private void userControl11_Load(object sender, EventArgs e)
        {
            //Area.point_move();
        }

        private void Log_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Log.Text += "P\r\n"+Area.ShowLogs();
            //Log.Text += "----\r\n";
            Log.Text += "Undo\r\n" + Area.ShowLogs_undo();
            Log.Text += "----\r\n";
            Log.Text += "Redo\r\n" + Area.ShowLogs_redo();
            Log.Text += "----\r\n";
            
        }

        private void Index_Click(object sender, EventArgs e)
        {
            
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true || this.checkBox2.Checked == true)
            {
                if (this.checkBox1.Checked == true)
                {
                    float speed = int.Parse(InputSpeed.Text);
                    Area.undo_anime(1,speed);
                }if (this.checkBox2.Checked == true)
                {
                    float time = int.Parse(InputTime.Text);
                    Area.undo_anime(2,time);
                }
            }
            else
            {
                Area.undo();
            }
        }

        private void redo_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true || this.checkBox2.Checked == true)
            {
                if (this.checkBox1.Checked == true)
                {
                    float speed = int.Parse(InputSpeed.Text);
                    Area.redo_anime(1, speed);
                }
                if (this.checkBox2.Checked == true)
                {
                    float time = int.Parse(InputTime.Text);
                    Area.redo_anime(2, time);
                }
            }
            else
            {
                Area.redo();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            //Area.remove_point();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Area.Controls.Clear();
            Area.load_form_server();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Area.AddTxtBox();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Area.ImportPictureBox();
        }
    }
}
