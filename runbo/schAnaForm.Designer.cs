namespace runbo
{
    partial class schAnaForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSch = new System.Windows.Forms.ComboBox();
            this.cboField = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.分析结果 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.分析结果.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboSch);
            this.groupBox1.Controls.Add(this.cboField);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(22, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(719, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "学校类型";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "区县";
            // 
            // cboSch
            // 
            this.cboSch.FormattingEnabled = true;
            this.cboSch.Location = new System.Drawing.Point(343, 74);
            this.cboSch.Name = "cboSch";
            this.cboSch.Size = new System.Drawing.Size(121, 26);
            this.cboSch.TabIndex = 2;
            // 
            // cboField
            // 
            this.cboField.FormattingEnabled = true;
            this.cboField.Location = new System.Drawing.Point(107, 74);
            this.cboField.Name = "cboField";
            this.cboField.Size = new System.Drawing.Size(121, 26);
            this.cboField.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(568, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "分析";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 分析结果
            // 
            this.分析结果.Controls.Add(this.label4);
            this.分析结果.Controls.Add(this.label3);
            this.分析结果.Location = new System.Drawing.Point(22, 188);
            this.分析结果.Name = "分析结果";
            this.分析结果.Size = new System.Drawing.Size(719, 109);
            this.分析结果.TabIndex = 1;
            this.分析结果.TabStop = false;
            this.分析结果.Text = "分析结果";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(578, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "图中已经显示出学校范围覆盖不到的区域，新建学校可以参考实际情况在指定区域实施";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(353, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "学校覆盖范围重合较大的学校可以考虑迁移或者合并";
            // 
            // schAnaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 313);
            this.Controls.Add(this.分析结果);
            this.Controls.Add(this.groupBox1);
            this.Name = "schAnaForm";
            this.Text = "schAnaForm";
            this.Load += new System.EventHandler(this.schAnaForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.分析结果.ResumeLayout(false);
            this.分析结果.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSch;
        private System.Windows.Forms.ComboBox cboField;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox 分析结果;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}