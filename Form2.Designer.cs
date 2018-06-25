namespace WindowsFormsApp1
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PointNum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Log = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.undo = new System.Windows.Forms.Button();
            this.redo = new System.Windows.Forms.Button();
            this.Area = new WindowsFormsApp1.UserControl1();
            this.delete = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // PointNum
            // 
            this.PointNum.Location = new System.Drawing.Point(63, 29);
            this.PointNum.Name = "PointNum";
            this.PointNum.Size = new System.Drawing.Size(185, 20);
            this.PointNum.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Show Blue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(375, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(473, 26);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Load";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Log
            // 
            this.Log.Location = new System.Drawing.Point(511, 130);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(264, 308);
            this.Log.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(567, 27);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Log";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(511, 72);
            this.undo.Name = "undo";
            this.undo.Size = new System.Drawing.Size(75, 23);
            this.undo.TabIndex = 8;
            this.undo.Text = "undo";
            this.undo.UseVisualStyleBackColor = true;
            this.undo.Click += new System.EventHandler(this.undo_Click);
            // 
            // redo
            // 
            this.redo.Location = new System.Drawing.Point(511, 101);
            this.redo.Name = "redo";
            this.redo.Size = new System.Drawing.Size(75, 23);
            this.redo.TabIndex = 9;
            this.redo.Text = "redo";
            this.redo.UseVisualStyleBackColor = true;
            this.redo.Click += new System.EventHandler(this.redo_Click);
            // 
            // Area
            // 
            this.Area.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Area.Location = new System.Drawing.Point(23, 72);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(470, 366);
            this.Area.TabIndex = 5;
            this.Area.Load += new System.EventHandler(this.userControl11_Load);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(648, 26);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 10;
            this.delete.Text = "delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(593, 72);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 17);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Animation";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 450);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.redo);
            this.Controls.Add(this.undo);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PointNum);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox PointNum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private UserControl1 Area;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button undo;
        private System.Windows.Forms.Button redo;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}