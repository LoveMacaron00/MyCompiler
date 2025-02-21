namespace Crru
{
    partial class crru
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bRun = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.richTextBox1.Location = new System.Drawing.Point(52, 60);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(460, 428);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "input a\ninput b\n do\n  a=a+1\n  output a\nwhile a<b\n  output \"End\"\n";
            // 
            // bRun
            // 
            this.bRun.Location = new System.Drawing.Point(543, 74);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(110, 77);
            this.bRun.TabIndex = 1;
            this.bRun.Text = "Run";
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(543, 168);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(110, 66);
            this.bNew.TabIndex = 2;
            this.bNew.Text = "New";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(543, 258);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(110, 66);
            this.Close.TabIndex = 3;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(212, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "lnput";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(854, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.richTextBox2.Location = new System.Drawing.Point(684, 60);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(453, 428);
            this.richTextBox2.TabIndex = 6;
            this.richTextBox2.Text = "";
            // 
            // crru
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 528);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.richTextBox1);
            this.Name = "crru";
            this.Text = "crru";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button bRun;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}