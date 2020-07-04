namespace runbo
{
    partial class OverlayForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboOverLay = new System.Windows.Forms.ComboBox();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnOverlay = new System.Windows.Forms.Button();
            this.btnCannel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnOutputLayer = new System.Windows.Forms.Button();
            this.txtInputFeat = new System.Windows.Forms.TextBox();
            this.txtOverlayFeat = new System.Windows.Forms.TextBox();
            this.btnInputFeat = new System.Windows.Forms.Button();
            this.btnOverlayFeat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入要素";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "叠置要素";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "叠置方式";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "输出图层";
            // 
            // cboOverLay
            // 
            this.cboOverLay.FormattingEnabled = true;
            this.cboOverLay.Location = new System.Drawing.Point(121, 116);
            this.cboOverLay.Name = "cboOverLay";
            this.cboOverLay.Size = new System.Drawing.Size(172, 23);
            this.cboOverLay.TabIndex = 6;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(121, 158);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(130, 25);
            this.txtOutputPath.TabIndex = 7;
            // 
            // btnOverlay
            // 
            this.btnOverlay.Location = new System.Drawing.Point(66, 208);
            this.btnOverlay.Name = "btnOverlay";
            this.btnOverlay.Size = new System.Drawing.Size(75, 33);
            this.btnOverlay.TabIndex = 8;
            this.btnOverlay.Text = "分析";
            this.btnOverlay.UseVisualStyleBackColor = true;
            this.btnOverlay.Click += new System.EventHandler(this.btnOverlay_Click);
            // 
            // btnCannel
            // 
            this.btnCannel.Location = new System.Drawing.Point(176, 208);
            this.btnCannel.Name = "btnCannel";
            this.btnCannel.Size = new System.Drawing.Size(75, 33);
            this.btnCannel.TabIndex = 9;
            this.btnCannel.Text = "取消";
            this.btnCannel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Location = new System.Drawing.Point(39, 262);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 64);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "处理过程消息";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(3, 21);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(248, 25);
            this.txtMessage.TabIndex = 0;
            // 
            // btnOutputLayer
            // 
            this.btnOutputLayer.Location = new System.Drawing.Point(257, 157);
            this.btnOutputLayer.Name = "btnOutputLayer";
            this.btnOutputLayer.Size = new System.Drawing.Size(36, 23);
            this.btnOutputLayer.TabIndex = 11;
            this.btnOutputLayer.Text = "…";
            this.btnOutputLayer.UseVisualStyleBackColor = true;
            this.btnOutputLayer.Click += new System.EventHandler(this.btnOutputLayer_Click);
            // 
            // txtInputFeat
            // 
            this.txtInputFeat.Location = new System.Drawing.Point(121, 32);
            this.txtInputFeat.Name = "txtInputFeat";
            this.txtInputFeat.Size = new System.Drawing.Size(130, 25);
            this.txtInputFeat.TabIndex = 12;
            // 
            // txtOverlayFeat
            // 
            this.txtOverlayFeat.Location = new System.Drawing.Point(121, 73);
            this.txtOverlayFeat.Name = "txtOverlayFeat";
            this.txtOverlayFeat.Size = new System.Drawing.Size(130, 25);
            this.txtOverlayFeat.TabIndex = 13;
            // 
            // btnInputFeat
            // 
            this.btnInputFeat.Location = new System.Drawing.Point(257, 32);
            this.btnInputFeat.Name = "btnInputFeat";
            this.btnInputFeat.Size = new System.Drawing.Size(36, 23);
            this.btnInputFeat.TabIndex = 14;
            this.btnInputFeat.Text = "…";
            this.btnInputFeat.UseVisualStyleBackColor = true;
            this.btnInputFeat.Click += new System.EventHandler(this.btnInputFeat_Click);
            // 
            // btnOverlayFeat
            // 
            this.btnOverlayFeat.Location = new System.Drawing.Point(257, 73);
            this.btnOverlayFeat.Name = "btnOverlayFeat";
            this.btnOverlayFeat.Size = new System.Drawing.Size(36, 23);
            this.btnOverlayFeat.TabIndex = 15;
            this.btnOverlayFeat.Text = "…";
            this.btnOverlayFeat.UseVisualStyleBackColor = true;
            this.btnOverlayFeat.Click += new System.EventHandler(this.btnOverlayFeat_Click);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 338);
            this.Controls.Add(this.btnOverlayFeat);
            this.Controls.Add(this.btnInputFeat);
            this.Controls.Add(this.txtOverlayFeat);
            this.Controls.Add(this.txtInputFeat);
            this.Controls.Add(this.btnOutputLayer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCannel);
            this.Controls.Add(this.btnOverlay);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.cboOverLay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OverlayForm";
            this.Text = "叠置分析";
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboOverLay;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnOverlay;
        private System.Windows.Forms.Button btnCannel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnOutputLayer;
        private System.Windows.Forms.TextBox txtInputFeat;
        private System.Windows.Forms.TextBox txtOverlayFeat;
        private System.Windows.Forms.Button btnInputFeat;
        private System.Windows.Forms.Button btnOverlayFeat;
    }
}