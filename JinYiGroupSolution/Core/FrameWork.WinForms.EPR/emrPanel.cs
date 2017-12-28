using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
namespace Neusoft.FrameWork.EPRControl
{
    [System.Drawing.ToolboxBitmap(typeof(Panel))]
    public partial class emrPanel : Panel, IContainer
    {
        public emrPanel()
        {
            InitializeComponent();
            init();
        }

        public emrPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            init();
        }
	    private bool bOnly = false;
	    #region "������"
	    [Browsable(false)]
	    public override bool AllowDrop {
		    get { return base.AllowDrop; }
		    set { base.AllowDrop = value; }
	    }
	    [Browsable(false)]
	    public new BindingsCollection DataBindings {
		    get { return base.DataBindings; }

		    set { }
	    }
	    [Browsable(false)]
	    public override System.Windows.Forms.AnchorStyles Anchor {
		    get { return base.Anchor; }
		    set { base.Anchor = value; }
	    }


	 
	    [Browsable(false)]
	    public override System.Windows.Forms.Cursor Cursor {
		    get { return base.Cursor; }
		    set { base.Cursor = value; }
	    }
	    [Browsable(false)]
	    protected override System.Windows.Forms.ImeMode DefaultImeMode {
		    get { return base.DefaultImeMode; }
	    }


	    [Browsable(false)]
	    public new string AccessibleDescription {
		    get { return base.AccessibleDescription; }
		    set { base.AccessibleDescription = value; }
	    }

	    [Browsable(false)]
	    public new string AccessibleName {
		    get { return base.AccessibleName; }
		    set { base.AccessibleName = value; }
	    }

	    [Browsable(false)]
	    public new System.Windows.Forms.AccessibleRole AccessibleRole {
		    get { return base.AccessibleRole; }
		    set { base.AccessibleRole = value; }
	    }

	    [Browsable(false)]
	    public new bool CausesValidation {
		    get { return base.CausesValidation; }
		    set { base.CausesValidation = value; }
	    }

	    [Browsable(false)]
	    public override System.Windows.Forms.DockStyle Dock {
		    get { return base.Dock; }
		    set { base.Dock = value; }
	    }
	    [Browsable(false)]
	    public new bool Enabled {
		    get { return base.Enabled; }
		    set { base.Enabled = value; }
	    }

	    [Browsable(false)]
	    public new System.Windows.Forms.ImeMode ImeMode {
		    get { return base.ImeMode; }
		    set { base.ImeMode = value; }
	    }
	    [Browsable(false)]
	    public new System.Drawing.Point Location {
		    get { return base.Location; }
		    set { base.Location = value; }
	    }

	    [Browsable(false)]
	    public new bool TabStop {
		    get { return base.TabStop; }
		    set { base.TabStop = value; }
	    }
	    [Browsable(false)]
	    public int TabIndex {
		    get { return base.TabIndex; }
		    set { base.TabIndex = value; }
	    }
	    [Browsable(false)]
	    public new object Tag {
		    get { return base.Tag; }
		    set { base.Tag = value; }
	    }
	    [Browsable(false)]
	    public new bool Visible {
		    get { return base.Visible; }
		    set { base.Visible = value; }
	    }
	    [Browsable(false)]
	    public override bool AutoScroll {
		    get { return base.AutoScroll; }
		    set { base.AutoScroll = value; }
	    }
	    [Browsable(false)]
	    public new System.Drawing.Size AutoScrollMargin {
		    get { return base.AutoScrollMargin; }
		    set { base.AutoScrollMargin = value; }
	    }
	    [Browsable(false)]
	    public new System.Drawing.Size AutoScrollMinSize {
		    get { return base.AutoScrollMinSize; }
		    set { base.AutoScrollMinSize = value; }
	    }
	    [Browsable(false)]
	    public new System.Windows.Forms.ScrollableControl.DockPaddingEdges DockPadding {
		    get { return base.DockPadding; }

		    set { }
	    }
	    [Browsable(false)]
	    public new System.Drawing.Image BackgroundImage {
		    get { return base.BackgroundImage; }
		    set { base.BackgroundImage = value; }
	    }


	    [Browsable(false)]
	    public override System.Drawing.Color BackColor {
		    get { return base.BackColor; }
		    set { base.BackColor = value; }
	    }
	    #endregion

	    #region "������"
	    [CategoryAttribute("��ɫ"), Description("�޸����ò���ҳ�ı�����ɫ��")]
	    public System.Drawing.Color ������ɫ {
		    get { return base.BackColor; }
		    set { base.BackColor = value; }
	    }

	    [Browsable(false)]
	    public override System.Drawing.Color ForeColor {
		    get { return base.ForeColor; }
		    set { }
	    }
	    

	    [Browsable(false)]
	    public override System.Drawing.Font Font {
		    get { return base.Font; }
            set { base.Font = value; }
	    }
	    
	    [Browsable(false)]
	    public new System.Drawing.Size Size {
		    get { return base.Size; }
		    set { base.Size = value; }
	    }
	    [Browsable(false)]
	    public new BorderStyle BorderStyle {
		    get { return base.BorderStyle; }
		    set { base.BorderStyle = value; }
	    }
	    [CategoryAttribute("���"), Description("�޸����ò���ҳ�ĳߴ��С��")]
	    public System.Drawing.Size ����ҳ�ߴ� {
		    get { return base.Size; }
		    set { base.Size = value; }
	    }
	    [CategoryAttribute("����"), Description("�޸����ò���ҳ�ļ��������Ƿ���Ψһ��")]
	    public bool Ψһ {
		    get { return bOnly; }
		    set { bOnly = value; }
	    }
	    string ControlName="";
	    [CategoryAttribute("��������"), Browsable(true), DescriptionAttribute("���ò������ƣ�Ϊ���������ã����ܰ���'�ո�\\,-,(,),,.%�������ַ�'")]
	    public new string ���� {
		    get
            {
			    if (string.IsNullOrEmpty (ControlName)) ControlName = this.Name; 
			    return ControlName;
		    }
		    set { ControlName = value; }
	    }
	    string strSQLSelect="";
	    [CategoryAttribute("����"), Description("���ó�ʼ�����ݣ�")]
	    public string SQLSelect {
		    get { return strSQLSelect; }
		    set { strSQLSelect = value; }
	    }
	    string strSQLUpdate="";
	    [CategoryAttribute("����"), Description("����ʱ��Ҫ���µ����ݣ�")]
	    public string SQLUpdate {
		    get { return strSQLUpdate; }
		    set { strSQLUpdate = value; }
	    }
	    protected bool bAutoPage;
	    [CategoryAttribute("����"), Description("���ݶ����ı��򼰱�������ݵ����������Զ���ҳ����")]
	    public bool �Զ���ҳ {
		    get { return bAutoPage; }
		    set { bAutoPage = value; }
	    }

	    private System.ComponentModel.ComponentCollection coms;
	    private IComponent[] c;
	    private ArrayList al;
	    public System.ComponentModel.ComponentCollection Components {
		    get {
			    if (this.coms == null)
			    {
				    al = new ArrayList();
				    al.Add(this);
				    GetControlList(this);
                    c = new IComponent[al.Count];
				    int i;
				    for (i = 0; i <= al.Count - 1; i++) {
                        try
                        {
                            c[i] = al[i] as IComponent;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
				    }
				    coms = new System.ComponentModel.ComponentCollection(c);
			    }
			    return coms;
		    }
	    }
	    private void GetControlList(Control pc)
	    {
             foreach (Control c in pc.Controls)
             {
			    if (c.Controls.Count > 0)
			    {
				    GetControlList(c);
			    }
			    al.Add(c);
		    }
	    }


        private System.Drawing.Size validSize = new System.Drawing.Size(-1, 0);

        /// <summary>
        /// ҳ����Ч����
        /// </summary>
        [CategoryAttribute("���"), Description("��ʾҳ����Ч����")]
        public System.Drawing.Size ��Ч����
        {
            get { return validSize; }
            set { validSize = value;
            iWidth = validSize.Width;
            }
        }
        private System.Drawing.Image firstPagePrintingBackImage;

	    [CategoryAttribute("��ӡ"), Description("��ҳ��ӡ����"), Browsable(true)]
        public System.Drawing.Image ��ҳ��ӡ����
        {
            get { return firstPagePrintingBackImage; }
            set { firstPagePrintingBackImage = value; }
        }
        private System.Drawing.Image printingBackImage;
	    
	    [CategoryAttribute("��ӡ"), Description("��ӡ����"), Browsable(true)]
        public System.Drawing.Image ��ӡ����
        {
          get { return printingBackImage; }
          set { printingBackImage = value; }
        }
        private bool isFirstPageHavePageHead;

        [CategoryAttribute("��ӡ"), Description("��ҳ�Ƿ��ӡҳü"), Browsable(true)]
        public bool ��ҳ��ӡҳü
        {
            get { return isFirstPageHavePageHead; }
            set { isFirstPageHavePageHead = value; }
        }
        private bool isHavePageHead;

        [CategoryAttribute("��ӡ"), Description("�Ƿ��ӡҳü"), Browsable(true)]
        public bool ��ӡҳü
        {
            get { return isHavePageHead; }
            set { isHavePageHead = value; }
        }
        private bool isHavePageFoot;

        [CategoryAttribute("��ӡ"), Description("�Ƿ��ӡҳ��"), Browsable(true)]
        public bool ��ӡҳ��
        {
            get { return isHavePageFoot; }
            set { isHavePageFoot = value; }
        }

        private string pageName;
        [CategoryAttribute("��ӡ"), Description("��ӡֽ��"), Browsable(true)]
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        private bool landscape;
        [CategoryAttribute("��ӡ"), Description("�Ƿ�����ӡ"), Browsable(true)]
        public bool Landscape
        {
            get { return landscape; }
            set { landscape = value; }
        }

        #region pantiejun add at 2008-3-28 begin
        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        private bool isShowGrid;
        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        [Category("���"), Description("�Ƿ���ʾ����"), Browsable(true)]
        public bool ��ʾ����
        {
            get { return this.isShowGrid; }
            set {
                this.isShowGrid = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// �Ƿ���ʾ���
        /// </summary>
        private bool isShowRuler;
        /// <summary>
        /// �Ƿ���ʾ���
        /// </summary>
        [Category("���"), Description("�Ƿ���ʾ���"), Browsable(true)]
        public bool ��ʾ���
        {
            get { return this.isShowRuler; }
            set 
            {
                this.isShowRuler = value;
                this.Invalidate();
            }
        }
        #endregion pantiejun add at 2008-3-28 end

        #endregion


        [CategoryAttribute("��ӡ"), Description("ѡ��ҳ��ؼ���"), Browsable(false)]
	    public Control PageNumberControl {
		    get
            {
			    int i = 0;
                Control page = null;
                foreach (System.ComponentModel.Component c in this.Components)
                {
				    if (c.GetType().ToString() == "Neusoft.FrameWork.EPRControl.emrPage")
				    {
					    i = i + 1;
					    page = c as Control;
				    }
			    }
			    if (i > 1)
			    {
				    return null;
			    }
			    else
			    {
                    return page;
			    }
		    }
        }

        #region ��ǰ����
        protected int iWidth = -1;
	    protected int iHeight = 1145;

	    //����ҳ���С
	    public void SetPageSize(int iHeight,    int iWidth)
	    {
		    this.iHeight = iHeight;
		    this.iWidth = iWidth;
	    }

        //���»�
        private PictureBox pbGridRuler = new PictureBox();//add by pantiejun 2008-4-2
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	    {
		    System.Drawing.Graphics g = this.CreateGraphics();
            #region pantiejun add at 2008-3-28 begin
            if ((this.isShowGrid || this.isShowRuler) && this.DesignMode)
            {
                if (this.��Ч����.Width > 0 && this.��Ч����.Height > 0)
                {
                    drawGrid(this.��Ч����);
                }
                else
                {
                    drawGrid(this.Size);
                }
            }
            else
            {
                if (this.Controls.Contains(pbGridRuler))
                {
                    this.Controls.Remove(pbGridRuler);
                }
            }
            #endregion pantiejun add at 2008-3-28 end

            int i = 0;
		    System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Blue);
		    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;


		    if (iWidth == -1)
		    {
			    //ֻ������
			    for (i = 1; i <= 10; i++) {
				    g.DrawLine(pen, 0, iHeight * i, this.Width, iHeight * i);
			    }
		    }
		    else
		    {
                System.Drawing.Pen pen1 = new System.Drawing.Pen(System.Drawing.Color.Red);
                pen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			    //����
			    for (i = 1; i <= 10; i++) {
                    g.DrawRectangle(pen1, 1, iHeight * (i - 1)+1, validSize.Width, validSize.Height);
			    }
                //ֻ������
                for (i = 1; i <= 10; i++)
                {
                    g.DrawLine(pen, 0, iHeight * i, this.Width, iHeight * i);
                }
		    }

            if (this.DesignMode == false)
            {
                g.DrawRectangle(new Pen(new SolidBrush(Color.Yellow)), 0, 0, 5, base.Height);
            }

	    }

        private void drawGrid(System.Drawing.Size mysize)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[] { new System.Drawing.Point(mysize) };
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(mysize.Width, mysize.Height);
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(image);//this.CreateGraphics();
            newG.PageUnit = System.Drawing.GraphicsUnit.Millimeter;
            newG.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.Page, System.Drawing.Drawing2D.CoordinateSpace.Device, points);
            float gridUnit = 10;
            if (this.isShowGrid)
            {
                System.Drawing.Pen gridPen = new System.Drawing.Pen(System.Drawing.Color.Black, 0.1f);
                gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                for (float x = 0; x < points[0].Y; x += gridUnit)
                {
                    //����
                    newG.DrawLine(gridPen, new System.Drawing.PointF(0, x), new System.Drawing.PointF(points[0].X, x));
                }
                for (float x = 0; x < points[0].X; x += gridUnit)
                {
                    //����
                    newG.DrawLine(gridPen, new System.Drawing.PointF(x, 0), new System.Drawing.PointF(x, points[0].Y));
                }
            }
            if (this.isShowRuler)
            {
                //����Y����
                float numberY = 2.5f;
                //���ֵĴ�С
                float fontSize = 6;
                //����Y����
                float lineY = numberY + 3f;
                //���߸߶�
                float lineHeigth = 1.2f;
                //��ʾ������
                int number = 0;
                //���ֲ��ֵĵ�ɫ
                System.Drawing.Color numberBackground = System.Drawing.Color.White;
                //���߲��ֵĵ�ɫ
                System.Drawing.Color lineBackground = System.Drawing.Color.FromArgb(216, 231, 252);
                //�����ֲ��ֵĵ�ɫ
                newG.DrawRectangle(new System.Drawing.Pen(numberBackground, lineY + 0.5f), new System.Drawing.Rectangle(0, 0, points[0].X - 3, (int)numberY));
                //�����߲��ֵĵ�ɫ
                newG.DrawRectangle(new System.Drawing.Pen(lineBackground, lineHeigth), 0, lineY, points[0].X, lineHeigth);
                for (float x = gridUnit; x < points[0].X; x += gridUnit)
                {
                    newG.DrawString(number.ToString(), new System.Drawing.Font("Times New Roman", fontSize), System.Drawing.Brushes.Black, new System.Drawing.PointF((number > 9 ? x - 1f : x - 0.5f), numberY));
                    newG.DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Black, 0.1f), new System.Drawing.PointF(x, lineY), new System.Drawing.PointF(x, lineY + lineHeigth));
                    number++;

                }
            }
            newG.Dispose();
            pbGridRuler.Size = image.Size;
            pbGridRuler.Location = new System.Drawing.Point(0, 0);
            pbGridRuler.Image = image;
            pbGridRuler.SendToBack();
            this.Controls.Add(pbGridRuler);
        }


	    private System.Drawing.Point p = new System.Drawing.Point();
	    private System.Drawing.Point op = new System.Drawing.Point();
	    private bool b = false;
	    private int os;
	    private System.Drawing.Point tp;
        private System.Drawing.Point tt;
        
	    protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
	    {

            if (isCanEdit && this.DesignMode == false && bHasDrag == false)
            {
                if (e.Button == MouseButtons.Right && e.Clicks == 1 && isDrawing == false)
                {
                    isDrawing = true;
                    if (this.GetChildAtPoint(new Point(e.X, e.Y)) == null)
                    {
                        foreach (Control c in this.Controls)
                        {

                            if (c.Text.Trim() == "" &&
                                c.Tag != null &&
                                c.Tag.ToString() == "Auto")
                            {
                                this.Controls.Remove(c);
                            }
                        }
                        EmrAddTextBox text = new EmrAddTextBox();
                        text.Name = System.DateTime.Now.Ticks.ToString();
                        text.Font = this.Font;
                        text.Width = text.InitWidth;
                        text.Visible = true;
                        text.Tag = "Auto";
                        text.Location = new Point(e.X, e.Y);
                        this.Controls.Add(text);
                        text.Focus();
                        text.LostFocus += new EventHandler(text_LostFocus);
                    }

                }
            }
            if (isMovingControls)
            {
                base.Refresh();
            }
            isDrawing = false;
            isMovingControls = false;
            b = false;
            bHasDrag = false;
            this.Cursor = Cursors.Default;
	    }
        private bool bHasDrag = false;
	    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
	    {
            if (isMovingControls == false)
            {
                tt.X = e.X;
                tt.Y = e.Y;

                if (b)
                {
                    tp.Y = e.Y;
                    tp.X = e.X;
                    p.Y = this.op.Y - this.PointToScreen(tp).Y - os;
                    try
                    {
                        ScrollableControl c = (ScrollableControl)this.Parent;
                        c.AutoScrollPosition = p;
                    }
                    catch (Exception ex)
                    {

                    }
                    bHasDrag = true;

                }
            }
            DrawMoveArea(e);
	    }

	    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
	    {


            if (e.X <= 5 && this.DesignMode == false) //����
            {
                base.Refresh();
                myMovingControl.BeginMoving(this, e.Y);
                Graphics g = this.CreateGraphics();
                g.DrawLine(new Pen(new SolidBrush(Color.Blue)), 0, e.Y, base.Width, e.Y);
                iLastMovingY = e.Y;
                isMovingControls = true;
                isDrawing = true;
            }
            else if(e.Button == MouseButtons.Left)
            {

                tp.Y = e.Y;
                tp.X = e.X;
                op.Y = this.PointToScreen(tp).Y;
                b = true;
                try
                {
                    ScrollableControl c = (ScrollableControl)this.Parent;
                    os = c.AutoScrollPosition.Y;
                }
                catch (Exception ex)
                {

                }
                this.Cursor = Cursors.Hand;
            }

	    }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F4)
            {
                System.Drawing.Point p = Control.MousePosition;
                Control ctr = this.GetChildAtPoint(this.PointToClient(p));
                if(ctr != null)
                {
                    while(ctr.HasChildren)
                    {
                        Control ctrTemp = ctr.GetChildAtPoint(ctr.PointToClient(p));
                        if(ctrTemp != null)
                        {
                            ctr = ctrTemp;
                        }
                    }
                    PropertyGrid myPropGrid = new PropertyGrid();
                    myPropGrid.Dock = DockStyle.Fill;
                    myPropGrid.SelectedObject = ctr;
                    Form frmPropGrid = new Form();
                    frmPropGrid.Size = new System.Drawing.Size(250,450);
                    frmPropGrid.Controls.Add(myPropGrid);
                    frmPropGrid.ShowDialog();
                    myPropGrid.Dispose();
                    frmPropGrid.Dispose();
                }
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #region IContainer ��Ա

        public void Add(IComponent component, string name)
        {
            return;
        }

        public void Add(IComponent component)
        {
            return;
        }

        public void Remove(IComponent component)
        {
            return;
        }

        #endregion
        #endregion

        #region ������
        private bool isCanEdit = false;
        /// <summary>
        /// �Ƿ�����Ctrl���޸Ŀؼ�λ��
        /// </summary>
        [Description("�Ƿ��������������Ϣ�ڰװ��ϣ��Ƿ�����Ctrl�޸Ŀؼ�λ��")]
        public bool IsCanEdit
        {
            get
            {
                return isCanEdit;
            }
            set
            {
                isCanEdit = value;
            }
        }
        #endregion

        #region �¹���

        protected override void OnControlAdded(ControlEventArgs e)
        {
            
            {
                e.Control.MouseHover += new EventHandler(Control_MouseHover);
                e.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
                e.Control.MouseLeave += new EventHandler(Control_MouseLeave);
                this.addhadle(e.Control);
            }
            base.OnControlAdded(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (isCanEdit && this.DesignMode == false && bHasDrag == false)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control && isDrawing == false)
                {
                    isDrawing = true;
                    if (this.GetChildAtPoint(this.PointToClient(Control.MousePosition)) == null)
                    {
                        foreach (Control c in this.Controls)
                        {

                            if (c.Text.Trim() == "" &&
                                c.Tag != null &&
                                c.Tag.ToString() == "Auto")
                            {
                                this.Controls.Remove(c);
                            }
                        }
                        EmrAddTextBox text = new EmrAddTextBox();
                        text.Name = System.DateTime.Now.Ticks.ToString();
                        text.Font = this.Font;
                        text.Width = text.InitWidth;
                        text.Visible = true;
                        text.Tag = "Auto";
                        text.Location = this.PointToClient(Control.MousePosition);
                        this.Controls.Add(text);
                        text.Focus();
                        text.LostFocus += new EventHandler(text_LostFocus);
                    }

                }
            }
            base.OnClick(e);
        }

      

        void text_LostFocus(object sender, EventArgs e)
        {
            if (isCanEdit)
            {
                if (((Control)sender).Text.Trim() == "")
                {
                    this.Controls.Remove(((Control)sender));
                }
            }

        }
        void Control_MouseLeave(object sender, EventArgs e)
        {
            //this.Refresh();
        }

        void Control_MouseMove(object sender, MouseEventArgs e)
        {

        }

        void Control_MouseHover(object sender, EventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Default;
        }
        private Point pBeginPoint = new Point(0, 0);
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        private bool isDrawing = false;




        #region �ƶ�
        DesignControl design = null;
        private void init()
        {
            design = new DesignControl((ScrollableControl)this);
            design.IsShowPropertyForm = false;
            design.IsDrawGrid = false;
            design.IsShowTip = false;
        }
        private void addhadle(Control c)
        {
            c.KeyDown += new KeyEventHandler(c_KeyDown);
            c.KeyUp += new KeyEventHandler(c_KeyUp);
        }

        void c_KeyUp(object sender, KeyEventArgs e)
        {
            if (isCanEdit)
                design.IsDesignMode = false;
        }

        void c_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && isCanEdit)
            {
                design.IsDesignMode = true;
            }

        }
        #endregion


        #region  �ƶ�����
        private MovingControl myMovingControl = new MovingControl();
        private int iLastMovingY = 0;
        private void DrawMoveArea(MouseEventArgs e)
        {
            if (isMovingControls)
            {
                base.Refresh();
                Graphics g = this.CreateGraphics();
                g.DrawLine(new Pen(new SolidBrush(Color.Blue)), 0, e.Y, base.Width, e.Y);
                myMovingControl.EndMoving(iLastMovingY, e.Y);
                iLastMovingY = e.Y;
            }
        }

        /// <summary>
        /// �Ƿ������ƶ��ؼ�
        /// </summary>
        private bool isMovingControls = false;
       
        #endregion
        #endregion
    }

}
