namespace Redlocon
{
    partial class SupportedTest
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
            this.dataGridViewSupportedTests = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSupportedTests)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSupportedTests
            // 
            this.dataGridViewSupportedTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSupportedTests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSupportedTests.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewSupportedTests.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewSupportedTests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSupportedTests.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewSupportedTests.Name = "dataGridViewSupportedTests";
            this.dataGridViewSupportedTests.ReadOnly = true;
            this.dataGridViewSupportedTests.RowTemplate.Height = 33;
            this.dataGridViewSupportedTests.Size = new System.Drawing.Size(946, 728);
            this.dataGridViewSupportedTests.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new System.Drawing.Point(12, 759);
            this.btnClose.MaximumSize = new System.Drawing.Size(235, 49);
            this.btnClose.MinimumSize = new System.Drawing.Size(235, 49);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(235, 49);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "btn.close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SupportedTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 825);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataGridViewSupportedTests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximumSize = new System.Drawing.Size(1000, 900);
            this.MinimumSize = new System.Drawing.Size(1000, 900);
            this.Name = "SupportedTest";
            this.Text = "SupportedTest";
            this.Load += new System.EventHandler(this.SupportedTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSupportedTests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSupportedTests;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnClose;
    }
}