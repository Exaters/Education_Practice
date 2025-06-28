namespace Education_practice
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            R = new TextBox();
            X0 = new TextBox();
            Y0 = new TextBox();
            C = new TextBox();
            Сalculate = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            Line = new ComboBox();
            pictureBox = new PictureBox();
            Dots_Count = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            toolStrip1 = new ToolStrip();
            Exit = new ToolStripLabel();
            Statistics = new ToolStripLabel();
            Help = new ToolStripLabel();
            Information_about_programm = new ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // R
            // 
            R.BackColor = Color.DarkSlateBlue;
            R.BorderStyle = BorderStyle.FixedSingle;
            R.ForeColor = Color.Gold;
            R.Location = new Point(286, 51);
            R.Name = "R";
            R.Size = new Size(51, 27);
            R.TabIndex = 0;
            // 
            // X0
            // 
            X0.BackColor = Color.DarkSlateBlue;
            X0.BorderStyle = BorderStyle.FixedSingle;
            X0.ForeColor = Color.Gold;
            X0.Location = new Point(114, 50);
            X0.Name = "X0";
            X0.Size = new Size(51, 27);
            X0.TabIndex = 1;
            // 
            // Y0
            // 
            Y0.BackColor = Color.DarkSlateBlue;
            Y0.BorderStyle = BorderStyle.FixedSingle;
            Y0.ForeColor = Color.Gold;
            Y0.Location = new Point(203, 51);
            Y0.Name = "Y0";
            Y0.Size = new Size(51, 27);
            Y0.TabIndex = 2;
            // 
            // C
            // 
            C.BackColor = Color.DarkSlateBlue;
            C.BorderStyle = BorderStyle.FixedSingle;
            C.ForeColor = Color.Gold;
            C.Location = new Point(544, 50);
            C.Name = "C";
            C.Size = new Size(51, 27);
            C.TabIndex = 3;
            // 
            // Сalculate
            // 
            Сalculate.BackColor = Color.DarkSlateBlue;
            Сalculate.FlatAppearance.BorderColor = Color.White;
            Сalculate.FlatAppearance.BorderSize = 0;
            Сalculate.FlatStyle = FlatStyle.Flat;
            Сalculate.ForeColor = Color.Gold;
            Сalculate.Location = new Point(758, 50);
            Сalculate.Name = "Сalculate";
            Сalculate.Size = new Size(106, 28);
            Сalculate.TabIndex = 4;
            Сalculate.Text = "Расчитать";
            Сalculate.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(260, 55);
            label1.Name = "label1";
            label1.Size = new Size(20, 19);
            label1.TabIndex = 5;
            label1.Text = "R";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(82, 55);
            label2.Name = "label2";
            label2.Size = new Size(26, 19);
            label2.TabIndex = 6;
            label2.Text = "x0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(171, 54);
            label3.Name = "label3";
            label3.Size = new Size(26, 19);
            label3.TabIndex = 7;
            label3.Text = "y0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(518, 55);
            label4.Name = "label4";
            label4.Size = new Size(20, 19);
            label4.TabIndex = 8;
            label4.Text = "C";
            // 
            // Line
            // 
            Line.BackColor = Color.DarkSlateBlue;
            Line.FlatStyle = FlatStyle.Flat;
            Line.ForeColor = Color.Gold;
            Line.FormattingEnabled = true;
            Line.Items.AddRange(new object[] { "Вертикальная", "Горизонтальная" });
            Line.Location = new Point(343, 51);
            Line.Name = "Line";
            Line.Size = new Size(169, 27);
            Line.TabIndex = 9;
            Line.Text = "Линия";
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(31, 151);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(897, 478);
            pictureBox.TabIndex = 10;
            pictureBox.TabStop = false;
            // 
            // Dots_Count
            // 
            Dots_Count.BackColor = Color.DarkSlateBlue;
            Dots_Count.BorderStyle = BorderStyle.FixedSingle;
            Dots_Count.ForeColor = Color.Gold;
            Dots_Count.Location = new Point(658, 50);
            Dots_Count.Name = "Dots_Count";
            Dots_Count.Size = new Size(94, 27);
            Dots_Count.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(601, 53);
            label5.Name = "label5";
            label5.Size = new Size(51, 19);
            label5.TabIndex = 12;
            label5.Text = "Точки";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(219, 104);
            label6.Name = "label6";
            label6.Size = new Size(101, 19);
            label6.TabIndex = 13;
            label6.Text = "По формуле:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(568, 104);
            label7.Name = "label7";
            label7.Size = new Size(91, 19);
            label7.TabIndex = 14;
            label7.Text = "По методу:";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.DarkSlateBlue;
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { Exit, Statistics, Help, Information_about_programm });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0, 0, 2, 0);
            toolStrip1.Size = new Size(958, 25);
            toolStrip1.TabIndex = 16;
            toolStrip1.Text = "toolStrip1";
            // 
            // Exit
            // 
            Exit.ActiveLinkColor = Color.White;
            Exit.ForeColor = Color.Gold;
            Exit.Name = "Exit";
            Exit.Size = new Size(53, 22);
            Exit.Text = "Выйти";
            // 
            // Statistics
            // 
            Statistics.ActiveLinkColor = Color.White;
            Statistics.ForeColor = Color.Gold;
            Statistics.Name = "Statistics";
            Statistics.Size = new Size(84, 22);
            Statistics.Text = "Статистика";
            // 
            // Help
            // 
            Help.ForeColor = Color.Gold;
            Help.Name = "Help";
            Help.Size = new Size(67, 22);
            Help.Text = "Справка";
            // 
            // Information_about_programm
            // 
            Information_about_programm.ForeColor = Color.Gold;
            Information_about_programm.Name = "Information_about_programm";
            Information_about_programm.Size = new Size(199, 22);
            Information_about_programm.Text = "Информация о программе";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            ClientSize = new Size(958, 687);
            Controls.Add(toolStrip1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(Dots_Count);
            Controls.Add(pictureBox);
            Controls.Add(Line);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Сalculate);
            Controls.Add(C);
            Controls.Add(Y0);
            Controls.Add(X0);
            Controls.Add(R);
            Font = new Font("Times New Roman", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ForeColor = Color.DarkSlateGray;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            ImeMode = ImeMode.On;
            Name = "MainWindow";
            Text = "Monte-Carlo";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox R;
        private TextBox X0;
        private TextBox Y0;
        private TextBox C;
        private Button Сalculate;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox Line;
        private PictureBox pictureBox;
        private TextBox Dots_Count;
        private Label label5;
        private Label label6;
        private Label label7;
        private ToolStrip toolStrip1;
        private ToolStripLabel Statistics;
        private ToolStripLabel Exit;
        private ToolStripLabel Help;
        private ToolStripLabel Information_about_programm;
    }
}
