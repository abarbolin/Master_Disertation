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
            this.SuspendLayout();
            // 
            // ParseBtn
            // 
            this.ParseBtn.Location = new System.Drawing.Point(12, 25);
            this.ParseBtn.Name = "ParseBtn";
            this.ParseBtn.Size = new System.Drawing.Size(75, 23);
            this.ParseBtn.TabIndex = 0;
            this.ParseBtn.Text = "Parse";
            this.ParseBtn.UseVisualStyleBackColor = true;
            this.ParseBtn.Click += new System.EventHandler(this.ParseBtn_Click);
            // 
            // ParseBox
            // 
            this.ParseBox.Location = new System.Drawing.Point(125, 26);
            this.ParseBox.Multiline = true;
            this.ParseBox.Name = "ParseBox";
            this.ParseBox.Size = new System.Drawing.Size(843, 454);
            this.ParseBox.TabIndex = 1;
            // 
            // ParseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 492);
            this.Controls.Add(this.ParseBox);
            this.Controls.Add(this.ParseBtn);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ParseForm";
            this.Text = "Parse";
            this.Load += new System.EventHandler(this.ParseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ParseBtn;
        private System.Windows.Forms.TextBox ParseBox;
    }
}

