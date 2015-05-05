namespace SiteParse
{
    partial class ParseForm
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
            this.ParseBtn = new System.Windows.Forms.Button();
            this.ParseBox = new System.Windows.Forms.TextBox();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.lemmaBox = new System.Windows.Forms.TextBox();
            this.parseTextLbl = new System.Windows.Forms.Label();
            this.lemmaLbl = new System.Windows.Forms.Label();
            this.findWordLbl = new System.Windows.Forms.Label();
            this.findWordTB = new System.Windows.Forms.TextBox();
            this.errorLbl = new System.Windows.Forms.Label();
            this.infoButton = new System.Windows.Forms.Button();
            this.visualBtn = new System.Windows.Forms.Button();
            this.userParseBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ParseBtn
            // 
            this.ParseBtn.Location = new System.Drawing.Point(9, 20);
            this.ParseBtn.Margin = new System.Windows.Forms.Padding(2);
            this.ParseBtn.Name = "ParseBtn";
            this.ParseBtn.Size = new System.Drawing.Size(56, 19);
            this.ParseBtn.TabIndex = 0;
            this.ParseBtn.Text = "Parse";
            this.ParseBtn.UseVisualStyleBackColor = true;
            this.ParseBtn.Click += new System.EventHandler(this.ParseBtn_Click);
            // 
            // ParseBox
            // 
            this.ParseBox.Location = new System.Drawing.Point(94, 72);
            this.ParseBox.Margin = new System.Windows.Forms.Padding(2);
            this.ParseBox.Multiline = true;
            this.ParseBox.Name = "ParseBox";
            this.ParseBox.Size = new System.Drawing.Size(958, 180);
            this.ParseBox.TabIndex = 1;
            this.ParseBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParseBox_KeyDown);
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(137, 19);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(590, 20);
            this.urlBox.TabIndex = 2;
            this.urlBox.Text = "http://top.rbc.ru/politics/05/10/2014/5431511fcbb20f3858d57d15";
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(96, 22);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(26, 13);
            this.urlLabel.TabIndex = 3;
            this.urlLabel.Text = "Url :";
            // 
            // lemmaBox
            // 
            this.lemmaBox.Location = new System.Drawing.Point(94, 414);
            this.lemmaBox.Margin = new System.Windows.Forms.Padding(2);
            this.lemmaBox.Multiline = true;
            this.lemmaBox.Name = "lemmaBox";
            this.lemmaBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lemmaBox.Size = new System.Drawing.Size(958, 154);
            this.lemmaBox.TabIndex = 4;
            // 
            // parseTextLbl
            // 
            this.parseTextLbl.AutoSize = true;
            this.parseTextLbl.Location = new System.Drawing.Point(94, 57);
            this.parseTextLbl.Name = "parseTextLbl";
            this.parseTextLbl.Size = new System.Drawing.Size(120, 13);
            this.parseTextLbl.TabIndex = 5;
            this.parseTextLbl.Text = "Информация с сайта :";
            // 
            // lemmaLbl
            // 
            this.lemmaLbl.AutoSize = true;
            this.lemmaLbl.Location = new System.Drawing.Point(96, 397);
            this.lemmaLbl.Name = "lemmaLbl";
            this.lemmaLbl.Size = new System.Drawing.Size(49, 13);
            this.lemmaLbl.TabIndex = 6;
            this.lemmaLbl.Text = "Лемма :";
            // 
            // findWordLbl
            // 
            this.findWordLbl.AutoSize = true;
            this.findWordLbl.Location = new System.Drawing.Point(94, 255);
            this.findWordLbl.Name = "findWordLbl";
            this.findWordLbl.Size = new System.Drawing.Size(104, 13);
            this.findWordLbl.TabIndex = 8;
            this.findWordLbl.Text = "Найденные слова :";
            // 
            // findWordTB
            // 
            this.findWordTB.Location = new System.Drawing.Point(97, 270);
            this.findWordTB.Margin = new System.Windows.Forms.Padding(2);
            this.findWordTB.Multiline = true;
            this.findWordTB.Name = "findWordTB";
            this.findWordTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.findWordTB.Size = new System.Drawing.Size(958, 121);
            this.findWordTB.TabIndex = 7;
            // 
            // errorLbl
            // 
            this.errorLbl.AutoSize = true;
            this.errorLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLbl.ForeColor = System.Drawing.Color.Red;
            this.errorLbl.Location = new System.Drawing.Point(746, 17);
            this.errorLbl.MaximumSize = new System.Drawing.Size(300, 50);
            this.errorLbl.Name = "errorLbl";
            this.errorLbl.Size = new System.Drawing.Size(57, 20);
            this.errorLbl.TabIndex = 10;
            this.errorLbl.Text = "label1";
            // 
            // infoButton
            // 
            this.infoButton.Location = new System.Drawing.Point(9, 47);
            this.infoButton.Name = "infoButton";
            this.infoButton.Size = new System.Drawing.Size(56, 23);
            this.infoButton.TabIndex = 12;
            this.infoButton.Text = "Info";
            this.infoButton.UseVisualStyleBackColor = true;
            this.infoButton.Click += new System.EventHandler(this.infoButton_Click);
            // 
            // visualBtn
            // 
            this.visualBtn.Location = new System.Drawing.Point(9, 76);
            this.visualBtn.Name = "visualBtn";
            this.visualBtn.Size = new System.Drawing.Size(56, 23);
            this.visualBtn.TabIndex = 13;
            this.visualBtn.Text = "Visual";
            this.visualBtn.UseVisualStyleBackColor = true;
            this.visualBtn.Click += new System.EventHandler(this.visualBtn_Click);
            // 
            // userParseBtn
            // 
            this.userParseBtn.Location = new System.Drawing.Point(9, 118);
            this.userParseBtn.Margin = new System.Windows.Forms.Padding(2);
            this.userParseBtn.Name = "userParseBtn";
            this.userParseBtn.Size = new System.Drawing.Size(56, 38);
            this.userParseBtn.TabIndex = 14;
            this.userParseBtn.Text = "User Parse";
            this.userParseBtn.UseVisualStyleBackColor = true;
            this.userParseBtn.Click += new System.EventHandler(this.userParseBtn_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(750, 44);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(80, 23);
            this.progressBar1.TabIndex = 15;
            // 
            // ParseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 583);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.userParseBtn);
            this.Controls.Add(this.visualBtn);
            this.Controls.Add(this.infoButton);
            this.Controls.Add(this.errorLbl);
            this.Controls.Add(this.findWordLbl);
            this.Controls.Add(this.findWordTB);
            this.Controls.Add(this.lemmaLbl);
            this.Controls.Add(this.parseTextLbl);
            this.Controls.Add(this.lemmaBox);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.urlBox);
            this.Controls.Add(this.ParseBox);
            this.Controls.Add(this.ParseBtn);
            this.Name = "ParseForm";
            this.Text = "Parse";
            this.Load += new System.EventHandler(this.ParseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ParseBtn;
        private System.Windows.Forms.TextBox ParseBox;
        private System.Windows.Forms.TextBox urlBox;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.TextBox lemmaBox;
        private System.Windows.Forms.Label parseTextLbl;
        private System.Windows.Forms.Label lemmaLbl;
        private System.Windows.Forms.Label findWordLbl;
        private System.Windows.Forms.TextBox findWordTB;
        private System.Windows.Forms.Label errorLbl;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.Button visualBtn;
        private System.Windows.Forms.Button userParseBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

