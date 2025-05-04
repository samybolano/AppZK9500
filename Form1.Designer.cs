namespace AppZK9500
{
    partial class Form1
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
            tableLayoutPanel1 = new TableLayoutPanel();
            picFPImg = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            btnInicializarAll = new Button();
            cmbIdx = new ComboBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFPImg).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(picFPImg, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Controls.Add(btnInicializarAll, 0, 3);
            tableLayoutPanel1.Controls.Add(cmbIdx, 0, 4);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1134, 592);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // picFPImg
            // 
            picFPImg.Anchor = AnchorStyles.None;
            picFPImg.Image = Properties.Resources.fingerprint__1_;
            picFPImg.Location = new Point(452, 3);
            picFPImg.Name = "picFPImg";
            picFPImg.Size = new Size(230, 204);
            picFPImg.SizeMode = PictureBoxSizeMode.Zoom;
            picFPImg.TabIndex = 1;
            picFPImg.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(406, 210);
            label1.Name = "label1";
            label1.Size = new Size(322, 31);
            label1.TabIndex = 0;
            label1.Text = "Escaneo de huellas dactilares";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(366, 241);
            label2.Name = "label2";
            label2.Size = new Size(401, 20);
            label2.TabIndex = 2;
            label2.Text = "Coloque el dedo sobre el escáner ZK9500 cuando esté listo";
            // 
            // btnInicializarAll
            // 
            btnInicializarAll.Anchor = AnchorStyles.None;
            btnInicializarAll.BackColor = Color.RoyalBlue;
            btnInicializarAll.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInicializarAll.ForeColor = Color.White;
            btnInicializarAll.Location = new Point(475, 264);
            btnInicializarAll.Name = "btnInicializarAll";
            btnInicializarAll.Size = new Size(184, 54);
            btnInicializarAll.TabIndex = 3;
            btnInicializarAll.Text = "Iniciar";
            btnInicializarAll.UseVisualStyleBackColor = false;
            btnInicializarAll.Click += btnInicializarAll_Click;
            // 
            // cmbIdx
            // 
            cmbIdx.FormattingEnabled = true;
            cmbIdx.Location = new Point(3, 324);
            cmbIdx.Name = "cmbIdx";
            cmbIdx.Size = new Size(151, 28);
            cmbIdx.TabIndex = 4;
            cmbIdx.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1134, 592);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SecureVerify";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picFPImg).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private PictureBox picFPImg;
        private Label label2;
        private Button btnInicializarAll;
        private ComboBox cmbIdx;
    }
}
