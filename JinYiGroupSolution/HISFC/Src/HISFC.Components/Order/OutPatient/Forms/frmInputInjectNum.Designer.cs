namespace Neusoft.HISFC.Components.Order.OutPatient.Forms
{
    partial class frmInputInjectNum
    {
        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        /// <param name="disposing">���Ӧ�ͷ��й���Դ��Ϊ true������Ϊ false��</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���

        /// <summary>
        /// �����֧������ķ��� - ��Ҫ
        /// ʹ�ô���༭���޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.neuLabel1 = new Neusoft.FrameWork.WinForms.Controls.NeuLabel();
            this.neuNumericUpDown1 = new Neusoft.FrameWork.WinForms.Controls.NeuNumericUpDown();
            this.lblDoseOnce = new Neusoft.FrameWork.WinForms.Controls.NeuLabel();
            this.neuLabel3 = new Neusoft.FrameWork.WinForms.Controls.NeuLabel();
            this.txtTimes = new Neusoft.FrameWork.WinForms.Controls.NeuTextBox();
            this.btnOK = new Neusoft.FrameWork.WinForms.Controls.NeuButton();
            this.lblInjectDays = new Neusoft.FrameWork.WinForms.Controls.NeuLabel();
            ((System.ComponentModel.ISupportInitialize)(this.neuNumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // neuLabel1
            // 
            this.neuLabel1.AutoSize = true;
            this.neuLabel1.Location = new System.Drawing.Point(12, 14);
            this.neuLabel1.Name = "neuLabel1";
            this.neuLabel1.Size = new System.Drawing.Size(41, 12);
            this.neuLabel1.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.neuLabel1.TabIndex = 0;
            this.neuLabel1.Text = "Ժע��";
            // 
            // neuNumericUpDown1
            // 
            this.neuNumericUpDown1.Location = new System.Drawing.Point(47, 10);
            this.neuNumericUpDown1.Name = "neuNumericUpDown1";
            this.neuNumericUpDown1.Size = new System.Drawing.Size(50, 21);
            this.neuNumericUpDown1.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.neuNumericUpDown1.TabIndex = 1;
            this.neuNumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.neuNumericUpDown1.ValueChanged += new System.EventHandler(this.neuNumericUpDown1_ValueChanged);
            this.neuNumericUpDown1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.neuNumericUpDown1_KeyUp);
            // 
            // lblDoseOnce
            // 
            this.lblDoseOnce.ForeColor = System.Drawing.Color.Red;
            this.lblDoseOnce.Location = new System.Drawing.Point(97, 14);
            this.lblDoseOnce.Name = "lblDoseOnce";
            this.lblDoseOnce.Size = new System.Drawing.Size(77, 12);
            this.lblDoseOnce.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.lblDoseOnce.TabIndex = 2;
            this.lblDoseOnce.Text = "ÿ��";
            // 
            // neuLabel3
            // 
            this.neuLabel3.AutoSize = true;
            this.neuLabel3.Location = new System.Drawing.Point(185, 14);
            this.neuLabel3.Name = "neuLabel3";
            this.neuLabel3.Size = new System.Drawing.Size(95, 12);
            this.neuLabel3.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.neuLabel3.TabIndex = 3;
            this.neuLabel3.Text = "��       ƿע��";
            // 
            // txtTimes
            // 
            this.txtTimes.IsEnter2Tab = false;
            this.txtTimes.Location = new System.Drawing.Point(201, 10);
            this.txtTimes.Name = "txtTimes";
            this.txtTimes.Size = new System.Drawing.Size(36, 21);
            this.txtTimes.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.txtTimes.TabIndex = 4;
            this.txtTimes.TextChanged += new System.EventHandler(this.txtTimes_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(181, 37);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "ȷ ��";
            this.btnOK.Type = Neusoft.FrameWork.WinForms.Controls.General.ButtonType.None;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblInjectDays
            // 
            this.lblInjectDays.AutoSize = true;
            this.lblInjectDays.Location = new System.Drawing.Point(12, 42);
            this.lblInjectDays.Name = "lblInjectDays";
            this.lblInjectDays.Size = new System.Drawing.Size(65, 12);
            this.lblInjectDays.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.lblInjectDays.TabIndex = 0;
            this.lblInjectDays.Text = "Ժע������";
            // 
            // frmInputInjectNum
            // 
            this.ClientSize = new System.Drawing.Size(292, 65);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtTimes);
            this.Controls.Add(this.neuLabel3);
            this.Controls.Add(this.neuNumericUpDown1);
            this.Controls.Add(this.lblInjectDays);
            this.Controls.Add(this.neuLabel1);
            this.Controls.Add(this.lblDoseOnce);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputInjectNum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "����Ժע������ע�����";
            this.Load += new System.EventHandler(this.frmInputInjectNum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.neuNumericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Neusoft.FrameWork.WinForms.Controls.NeuLabel neuLabel1;
        private Neusoft.FrameWork.WinForms.Controls.NeuNumericUpDown neuNumericUpDown1;
        private Neusoft.FrameWork.WinForms.Controls.NeuLabel lblDoseOnce;
        private Neusoft.FrameWork.WinForms.Controls.NeuLabel neuLabel3;
        private Neusoft.FrameWork.WinForms.Controls.NeuTextBox txtTimes;
        private Neusoft.FrameWork.WinForms.Controls.NeuButton btnOK;
        private Neusoft.FrameWork.WinForms.Controls.NeuLabel lblInjectDays;
    }
}
