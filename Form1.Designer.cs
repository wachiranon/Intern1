namespace WindowsFormsApp1
{
    partial class Form1
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
            this.Totxt = new System.Windows.Forms.Button();
            this.InputLineNum = new System.Windows.Forms.TextBox();
            this.ShowHello = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // Totxt
            // 
            this.Totxt.Location = new System.Drawing.Point(133, 49);
            this.Totxt.Name = "Totxt";
            this.Totxt.Size = new System.Drawing.Size(75, 23);
            this.Totxt.TabIndex = 0;
            this.Totxt.Text = "button1";
            this.Totxt.UseVisualStyleBackColor = true;
            this.Totxt.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputLineNum
            // 
            this.InputLineNum.Location = new System.Drawing.Point(27, 49);
            this.InputLineNum.Name = "InputLineNum";
            this.InputLineNum.Size = new System.Drawing.Size(100, 20);
            this.InputLineNum.TabIndex = 1;
            // 
            // ShowHello
            // 
            this.ShowHello.Location = new System.Drawing.Point(27, 92);
            this.ShowHello.Multiline = true;
            this.ShowHello.Name = "ShowHello";
            this.ShowHello.Size = new System.Drawing.Size(181, 346);
            this.ShowHello.TabIndex = 2;
            this.ShowHello.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(499, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(499, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(606, 92);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(182, 342);
            this.listBox2.TabIndex = 8;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(268, 92);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(202, 346);
            this.treeView1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ShowHello);
            this.Controls.Add(this.InputLineNum);
            this.Controls.Add(this.Totxt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Totxt;
        private System.Windows.Forms.TextBox InputLineNum;
        private System.Windows.Forms.TextBox ShowHello;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TreeView treeView1;
    }
}

