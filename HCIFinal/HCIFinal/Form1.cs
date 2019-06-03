using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCIFinal
{
    public partial class Form1 : Form
    {
        private Point mouse_offset;
        private bool is_writing = false;
        private int text_cont = 0;
        private int _id = 0;
        private struct item
        {
            public int id;
            public Label _l;
            public Button _move;
            public Button _del;
            public TextBox _t;
        };
        private ArrayList items = new ArrayList();
        //GlobalMouseHandler 
        public Form1()
        {
            InitializeComponent();
        }

        #region 窗口拖动、大小
        /// <summary>
        /// FormBorderStyle.None时，支持拖动、改变窗体大小
        /// </summary>
        /// <param name="m"></param>
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else
                            m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else
                            m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201://鼠标左键按下的消息
                    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标
                    m.LParam = IntPtr.Zero; //默认值
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region navigator bar
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool pinned = true;
        private void PinButton_Click(object sender, EventArgs e)
        {
            if (pinned == true)
            {
                this.TopMost = false;
            }
            else
            {
                this.TopMost = true;
            }
            pinned = !pinned;
        }
        #endregion

        
        private void AddButton_Click(object sender, EventArgs e)
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;
            //System.Threading.Thread.Sleep(500);
            Label pos = new Label();
            pos.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            pos.Size = new System.Drawing.Size(400, 85);
            Label l = new Label();
            l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont)*100);
            l.Name = "新消息";
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = "新消息";
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            l.DoubleClick += new System.EventHandler(this.l_Edit_Click);
            l.Font = new Font(l.Font.FontFamily, 15, l.Font.Style);
            l.Tag = _id;

            Button del = new Button();
            del.Location = new System.Drawing.Point(360, 50);
            del.Name = "D";
            del.Size = new System.Drawing.Size(25, 25);
            del.TabIndex = 0;
            del.Text = "D";
            del.UseVisualStyleBackColor = true;
            del.Click += new System.EventHandler(this.DEL_Click);
            del.Tag = _id;

            Button move = new Button();
            move.Location = new System.Drawing.Point(360, 10);
            move.Name = "M";
            move.Size = new System.Drawing.Size(25, 25);
            move.TabIndex = 1;
            move.Text = "M";
            move.UseVisualStyleBackColor = true;

            TextBox t = new TextBox();
            t.Location = new System.Drawing.Point(8, 18 + (text_cont++) * 100);
            t.Name = "textBox";
            t.Size = new System.Drawing.Size(400, 85);
            t.Text = l.Text;
            t.Font = new Font(t.Font.FontFamily, 15, t.Font.Style);
            t.TabIndex = 1;
            t.DoubleClick += new System.EventHandler(this.t_Edit_Click);
            t.TextChanged += new System.EventHandler(this.TextChanged);
            t.Multiline = true;
            t.Tag = _id;

            
            is_writing = true;
            this.panel2.Controls.Add(pos);
            
            this.panel2.Controls.Add(l);
            l.Controls.Add(del);
            l.Controls.Add(move);

            this.panel2.Controls.Add(t);
            this.panel2.Controls.Remove(pos);
            l.Visible = false;
            item i = new item();
            i._l = l;
            i._del = del;
            i._move = move;
            i._t = t;
            i.id = _id++;
            items.Add(i);
            //System.Threading.Thread.Sleep(500);
            //this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
            //this.Controls.Add(tex);
        }

        private void DEL_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = (int)btn.Tag;
            int top=0;
            foreach (item i in items)
            {
                if (id == i.id)
                {
                    top = i._l.Top; 
                    break;
                }
            }
            foreach (item i in items)
            {
                if (top < i._l.Top)
                {
                    i._l.Top-=100;
                    i._t.Top -= 100;

                }
            }
            foreach (item i in items)
            {
                Console.WriteLine(top==i._l.Top);
                if (i._l.Top == top)
                {
                    this.panel2.Controls.Remove(i._l);
                    this.panel2.Controls.Remove(i._del);
                    this.panel2.Controls.Remove(i._move);
                    this.panel2.Controls.Remove(i._t);
                    this.text_cont--;
                    items.Remove(i);
                    break;
                }
                Console.WriteLine(i._l.Top);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (item i in items)
            {
                if (i._l.Visible)
                    i._l.Visible = false;
                else i._l.Visible = true;
                break;
            }
        }


        private void TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int id = (int)t.Tag;
            foreach (item i in items)
            {
                if (id == i.id)
                {
                    i._l.Text = i._t.Text;
                    break;
                }

            }
        }
        private void l_Edit_Click(object sender, EventArgs e)
        {
            
            Label l = (Label)sender;
            int id = (int)l.Tag;
            Console.WriteLine(l.Top);
            foreach (item i in items)
            {
                if (id == i.id)
                {
                    if (i._l.Visible)
                    {
                        i._l.Visible = false;
                        i._t.Visible = true;
                    }

                    else
                    {
                        i._l.Visible = true;
                        i._t.Visible = false;
                    }
                    break;
                }

            }
        }
        private void t_Edit_Click(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int id = (int)t.Tag;
            Console.WriteLine(t.Top);
            foreach (item i in items)
            {
                if (id == i.id)
                {
                    if (i._l.Visible)
                    {
                        i._l.Visible = false;
                        i._t.Visible = true;
                    }

                    else
                    {
                        i._l.Visible = true;
                        i._t.Visible = false;
                    }
                    break;
                }

            }
        }
    }
}
