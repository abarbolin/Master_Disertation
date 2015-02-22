namespace SiteParse
{
    partial class InfoForm
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
            this.siteCompareLb1 = new System.Windows.Forms.ListBox();
            this.siteCompareLb2 = new System.Windows.Forms.ListBox();
            this.distanceBtn = new System.Windows.Forms.Button();
            this.distanceLbl = new System.Windows.Forms.Label();
            this.forelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // siteCompareLb1
            // 
            this.siteCompareLb1.FormattingEnabled = true;
            this.siteCompareLb1.Location = new System.Drawing.Point(12, 12);
            this.siteCompareLb1.Name = "siteCompareLb1";
            this.siteCompareLb1.Size = new System.Drawing.Size(420, 433);
            this.siteCompareLb1.TabIndex = 0;
            // 
            // siteCompareLb2
            // 
            this.siteCompareLb2.FormattingEnabled = true;
            this.siteCompareLb2.Location = new System.Drawing.Point(470, 12);
            this.siteCompareLb2.Name = "siteCompareLb2";
            this.siteCompareLb2.Size = new System.Drawing.Size(364, 433);
            this.siteCompareLb2.TabIndex = 1;
            // 
            // distanceBtn
            // 
            this.distanceBtn.Location = new System.Drawing.Point(759, 464);
            this.distanceBtn.Name = "distanceBtn";
            this.distanceBtn.Size = new System.Drawing.Size(75, 23);
            this.distanceBtn.TabIndex = 2;
            this.distanceBtn.Text = "Distance";
            this.distanceBtn.UseVisualStyleBackColor = true;
            this.distanceBtn.Click += new System.EventHandler(this.distanceBtn_Click);
            // 
            // distanceLbl
            // 
            this.distanceLbl.AutoSize = true;
            this.distanceLbl.Location = new System.Drawing.Point(432, 474);
            this.distanceLbl.Name = "distanceLbl";
            this.distanceLbl.Size = new System.Drawing.Size(0, 13);
            this.distanceLbl.TabIndex = 3;
            // 
            // forelBtn
            // 
            this.forelBtn.Location = new System.Drawing.Point(12, 464);
            this.forelBtn.Name = "forelBtn";
            this.forelBtn.Size = new System.Drawing.Size(52, 23);
            this.forelBtn.TabIndex = 16;
            this.forelBtn.Text = "Forel";
            this.forelBtn.UseVisualStyleBackColor = true;
            this.forelBtn.Click += new System.EventHandler(this.forelBtn_Click);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 505);
            this.Controls.Add(this.forelBtn);
            this.Controls.Add(this.distanceLbl);
            this.Controls.Add(this.distanceBtn);
            this.Controls.Add(this.siteCompareLb2);
            this.Controls.Add(this.siteCompareLb1);
            this.Name = "InfoForm";
            this.Text = "InfoForm";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox siteCompareLb1;
        private System.Windows.Forms.ListBox siteCompareLb2;
        private System.Windows.Forms.Button distanceBtn;
        private System.Windows.Forms.Label distanceLbl;
        private System.Windows.Forms.Button forelBtn;
    }
}