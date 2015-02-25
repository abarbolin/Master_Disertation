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
            this.waitingPanel = new System.Windows.Forms.Panel();
            this.waitLBL = new System.Windows.Forms.Label();
            this.errorLbl = new System.Windows.Forms.Label();
            this.infoButton = new System.Windows.Forms.Button();
            this.parseHistoryBtn = new System.Windows.Forms.Button();
            this.countOfUrls = new System.Windows.Forms.Label();
            this.testClusterBtn = new System.Windows.Forms.Button();
            this.waitingPanel.SuspendLayout();
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
            // waitingPanel
            // 
            this.waitingPanel.Controls.Add(this.waitLBL);
            this.waitingPanel.Location = new System.Drawing.Point(460, 257);
            this.waitingPanel.Name = "waitingPanel";
            this.waitingPanel.Size = new System.Drawing.Size(200, 59);
            this.waitingPanel.TabIndex = 9;
            this.waitingPanel.Visible = false;
            // 
            // waitLBL
            // 
            this.waitLBL.AutoSize = true;
            this.waitLBL.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waitLBL.Location = new System.Drawing.Point(47, 19);
            this.waitLBL.Name = "waitLBL";
            this.waitLBL.Size = new System.Drawing.Size(102, 22);
            this.waitLBL.TabIndex = 0;
            this.waitLBL.Text = "Wait..... ";
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
            // parseHistoryBtn
            // 
            this.parseHistoryBtn.Location = new System.Drawing.Point(9, 91);
            this.parseHistoryBtn.Name = "parseHistoryBtn";
            this.parseHistoryBtn.Size = new System.Drawing.Size(56, 23);
            this.parseHistoryBtn.TabIndex = 13;
            this.parseHistoryBtn.Text = "History";
            this.parseHistoryBtn.UseVisualStyleBackColor = true;
            this.parseHistoryBtn.Click += new System.EventHandler(this.parseHistoryBtn_Click);
            // 
            // countOfUrls
            // 
            this.countOfUrls.AutoSize = true;
            this.countOfUrls.Location = new System.Drawing.Point(12, 130);
            this.countOfUrls.Name = "countOfUrls";
            this.countOfUrls.Size = new System.Drawing.Size(0, 13);
            this.countOfUrls.TabIndex = 14;
            // 
            // testClusterBtn
            // 
            this.testClusterBtn.Location = new System.Drawing.Point(9, 125);
            this.testClusterBtn.Name = "testClusterBtn";
            this.testClusterBtn.Size = new System.Drawing.Size(56, 23);
            this.testClusterBtn.TabIndex = 15;
            this.testClusterBtn.Text = "Test";
            this.testClusterBtn.UseVisualStyleBackColor = true;
            this.testClusterBtn.Click += new System.EventHandler(this.testClusterBtn_Click);
            // 
            // ParseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 583);
            this.Controls.Add(this.testClusterBtn);
            this.Controls.Add(this.countOfUrls);
            this.Controls.Add(this.parseHistoryBtn);
            this.Controls.Add(this.infoButton);
            this.Controls.Add(this.errorLbl);
            this.Controls.Add(this.waitingPanel);
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
            this.waitingPanel.ResumeLayout(false);
            this.waitingPanel.PerformLayout();
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
        private System.Windows.Forms.Panel waitingPanel;
        private System.Windows.Forms.Label waitLBL;
        private System.Windows.Forms.Label errorLbl;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.Button parseHistoryBtn;
        private System.Windows.Forms.Label countOfUrls;
        private System.Windows.Forms.Button testClusterBtn;
    }
}

