using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AnimalCtrlSystem
{
    public partial class MyLedControl : UserControl
    {
        public MyLedControl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            LedTimer.Enabled = true;
            LedTimer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
    {
         
        intColorIndex++;
        if (intColorIndex >= lampColor.Length)
        {
            intColorIndex = 0;
        }
        this.Invalidate();
 
    }
 
    #region 【2】定义三个字段
 
    private Graphics g;
    private Pen p;
    private SolidBrush sb;
 
    Timer LedTimer=new Timer();
    private int intColorIndex = 0;
 
    #endregion
 
    #region 【3】添加一个设置Graphics的方法
     
    private void SetGraphics(Graphics g)
    {
        //设置画布的属性
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
 
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
    }
 
    #endregion
 
    #region 【4】根据实际控件分析的结果，创建属性
 
    private Color ledTrueColor = Color.Green;
 
    [Category("jason控件属性")]
    [Description("TRUE的时候LED指示灯颜色")]
    public Color LedTrueColor
    {
        get { return ledTrueColor; }
        set
        {
            ledTrueColor = value;
            this.Invalidate();
        }
    }
 
    private Color ledFalseColor = Color.Red;
 
    [Category("jason控件属性")]
    [Description("False的时候LED指示灯颜色")]
    public Color LedFalseColor
    {
        get { return ledFalseColor; }
        set
        {
            ledFalseColor = value;
            this.Invalidate();
        }
    }
 
    private bool ledStatus = true;
 
    [Category("jason控件属性")]
    [Description("当前的状态")]
    public bool LedStatus
    {
        get { return ledStatus; }
        set
        {
            ledStatus = value;
            this.Invalidate();
        }
    }
 
    private Color ledColor = Color.Green;
 
    [Category("jason控件属性")]
    [Description("LED指示灯演示")]
    public Color LedColor
    {
        get { return ledColor; }
        set
        {
            ledColor = value;
            this.Invalidate();
        }
    }
 
    private bool isBorder = true;
 
    [Category("jason控件属性")]
    [Description("是否有边框")]
    public bool IsBorder
    {
        get { return isBorder; }
        set
        {
            isBorder = value;
            this.Invalidate();
        }
    }
 
    private int borderWidth = 5;
 
    [Category("jason控件属性")]
    [Description("圆环的宽度")]
    public int BorderWidth
    {
        get { return borderWidth; }
        set
        {
            borderWidth = value;
            this.Invalidate();
        }
    }
 
    private int gapWidth = 5;
 
    [Category("jason控件属性")]
    [Description("间隙的宽度")]
    public int GapWidth
    {
        get { return gapWidth; }
        set
        {
            gapWidth = value;
            this.Invalidate();
        }
    }
 
    private bool isHighLight = true;
 
    [Category("jason控件属性")]
    [Description("是否高亮")]
    public bool IsHighLight
    {
        get { return isHighLight; }
        set
        {
            isHighLight = value;
            this.Invalidate();
        }
    }
 
    private Color centerColor = Color.White;
 
    [Category("jason控件属性")]
    [Description("渐变中心的颜色")]
    public Color CenterColor
    {
        get { return centerColor; }
        set
        {
            centerColor = value;
            this.Invalidate();
        }
    }
 
    private bool isFlash = true;
 
    [Category("jason控件属性")]
    [Description("是否闪烁")]
    public bool IsFlash
    {
        get { return isFlash; }
        set
        {
            isFlash = value;
            this.Invalidate();
        }
    }
 
    private int flashInterval = 500;
 
    [Category("jason控件属性")]
    [Description("闪烁的频率")]
    public int FlashInterval
    {
        get { return flashInterval; }
        set
        {
            flashInterval = value;
            LedTimer.Interval = flashInterval;//timer的时间间隔要放在这里
            this.Invalidate();
        }
    }
 
    private Color[] lampColor = new Color[] { };
 
    [Category("jason控件属性")]
    [Description("闪烁灯的几种颜色，当需要闪烁时，至少需要2个及以上颜色，不需要闪烁则至少需要1个颜色")]
    public Color[] LampColor
    {
        get { return lampColor; }
        set
        {
            if (value == null || value.Length <= 0)
                return;
            lampColor = value;
            this.Invalidate();
        }
    }
 
 
    #endregion
 
    #region 【5】创建重绘的事件
 
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        g = e.Graphics;//获取画布
        SetGraphics(g);//设置画布
 
        #region 1，画一个圆
 
        int LEDWidth = Math.Min(this.Width, this.Height);
 
        Color color = ledStatus ? ledTrueColor : ledFalseColor;
        if (isFlash)
        {
            lampColor=new Color[]{color,Color.Yellow};
            color = lampColor[intColorIndex];
        }
 
        sb = new SolidBrush(color);
        RectangleF rec = new RectangleF(1, 1, LEDWidth - 2, LEDWidth - 2);//创建矩形
        g.FillEllipse(sb, rec);//画圆
 
        #endregion
 
        #region 2,在圆里面画一个圆环
 
        //如果有边框，那就画一个圆环
        if (isBorder)//参数这里用字段或属性都可以，如果用属性，程序要都走一些判断的代码
        {
            p = new Pen(this.BackColor, borderWidth);//使用背景色
            //p = new Pen(Color.Red, borderWidth);
            float x = 1 + gapWidth + borderWidth * 0.5f;
            rec = new RectangleF(x, x, LEDWidth - 2 * x, LEDWidth - 2 * x);
            g.DrawEllipse(p, rec);//画圆环
        }
 
        #endregion
 
        #region 3，渐变色绘制，是否高亮
 
        if (isHighLight)
        {
            GraphicsPath gp = new GraphicsPath();
            float x = isBorder? 1 + gapWidth + borderWidth:1;//使用三元运算来判断，优化代码
            rec = new RectangleF(x, x, LEDWidth - 2 * x, LEDWidth - 2 * x);
            gp.AddEllipse(rec);//把矩形添加到路径
 
            //渐变色画刷，高亮
            PathGradientBrush pgb = new PathGradientBrush(gp);//把路径传入
 
            Color[] surroundColor = new Color[] {color};
 
            pgb.CenterColor = this.centerColor;
 
            //设置有多少组颜色来渐变
            pgb.SurroundColors = surroundColor;
            g.FillPath(pgb, gp);
        }
 
        #endregion
 
 
    }
 
    #endregion
    }
}
