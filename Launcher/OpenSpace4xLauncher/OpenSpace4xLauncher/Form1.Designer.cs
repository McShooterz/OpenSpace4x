namespace OpenSpace4xLauncher
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.modVersionBox = new System.Windows.Forms.TextBox();
            this.ModDescriptionBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ModList = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(438, 453);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.modVersionBox);
            this.tabPage1.Controls.Add(this.ModDescriptionBox);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.ModList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(430, 427);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.modVersionBox.Location = new System.Drawing.Point(6, 321);
            this.modVersionBox.Name = "textBox1";
            this.modVersionBox.ReadOnly = true;
            this.modVersionBox.Size = new System.Drawing.Size(157, 20);
            this.modVersionBox.TabIndex = 5;
            // 
            // ModDescriptionBox
            // 
            this.ModDescriptionBox.Location = new System.Drawing.Point(6, 347);
            this.ModDescriptionBox.Multiline = true;
            this.ModDescriptionBox.Name = "ModDescriptionBox";
            this.ModDescriptionBox.ReadOnly = true;
            this.ModDescriptionBox.Size = new System.Drawing.Size(418, 74);
            this.ModDescriptionBox.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(258, 286);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Move Down";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.MoveModDownButton);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(64, 286);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Move Up";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.MoveModUpButton);
            // 
            // ModList
            // 
            this.ModList.FormattingEnabled = true;
            this.ModList.Location = new System.Drawing.Point(6, 6);
            this.ModList.Name = "ModList";
            this.ModList.Size = new System.Drawing.Size(418, 274);
            this.ModList.TabIndex = 1;
            this.ModList.SelectedIndexChanged += new System.EventHandler(this.ChangeSelectedMod);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(430, 427);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graphics";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(161, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 64);
            this.button1.TabIndex = 1;
            this.button1.Text = "Launch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.LaunchButton);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 543);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Open Space 4x Launcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox ModList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox ModDescriptionBox;
        private System.Windows.Forms.TextBox modVersionBox;
    }
}

