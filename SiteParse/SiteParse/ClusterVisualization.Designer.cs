namespace SiteParse
{
    partial class ClusterVisualization
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
            this.forelBtn = new System.Windows.Forms.Button();
            this.aglomerativeBtn = new System.Windows.Forms.Button();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // forelBtn
            // 
            this.forelBtn.Location = new System.Drawing.Point(9, 2);
            this.forelBtn.Name = "forelBtn";
            this.forelBtn.Size = new System.Drawing.Size(75, 41);
            this.forelBtn.TabIndex = 0;
            this.forelBtn.Text = "Метод фореля";
            this.forelBtn.UseVisualStyleBackColor = true;
            this.forelBtn.Click += new System.EventHandler(this.forelBtn_Click);
            // 
            // aglomerativeBtn
            // 
            this.aglomerativeBtn.Location = new System.Drawing.Point(90, 2);
            this.aglomerativeBtn.Name = "aglomerativeBtn";
            this.aglomerativeBtn.Size = new System.Drawing.Size(109, 41);
            this.aglomerativeBtn.TabIndex = 1;
            this.aglomerativeBtn.Text = "Агломеративный метод";
            this.aglomerativeBtn.UseVisualStyleBackColor = true;
            this.aglomerativeBtn.Click += new System.EventHandler(this.aglomerativeBtn_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.Location = new System.Drawing.Point(9, 52);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(598, 261);
            this.zedGraphControl1.TabIndex = 2;
            // 
            // ClusterVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 313);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.aglomerativeBtn);
            this.Controls.Add(this.forelBtn);
            this.Name = "ClusterVisualization";
            this.Text = "ClusterVisualization";
            this.Load += new System.EventHandler(this.ClusterVisualization_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button forelBtn;
        private System.Windows.Forms.Button aglomerativeBtn;
        private ZedGraph.ZedGraphControl zedGraphControl1;
    }
}