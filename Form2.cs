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

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int p_num = int.Parse(PointNum.Text);
            for(int i = 0; i < p_num; i++)
            {
               Area.paint_point(i*10,i*10);
            }
            Area.point_move(this.Area.Controls);


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

        }

        
    }
}
