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
    public partial class Form1 : Form            //内部（todolist item）窗口
    {
        private Point mouse_offset;
        private Form2 _form1;                    //属性类似Form2
        private int text_cont = 0;
        private int _id;
        public int f_id;         //for_GJY_改过的
        private string _title;   //标题
        public struct item       //todolist每一条的item
        {
            public int id;
            public Label _l;
            public Button _move;
            public Button _del;
            public TextBox _t;
            public void setlocation(int x, int y)
            {
                Point p = new Point(x, y);
                _l.Location = p;
                _t.Location = p;
                _move.Location = new Point(p.X + 355, p.Y-10 );
                _move.BringToFront();
                _del.Location = new Point(p.X + 355, p.Y +30);
                _del.BringToFront();

               
            }
        };
        public struct items      //item数组
        {
            public int id;
            public ArrayList _item;
        }
        public items _items;

        

        //GlobalMouseHandler 
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(Form2 form, string title, int id) : this()            //由form2生成form1时调用， for_GJY_改过的
        {
            _form1 = form;
            _title = title;
            f_id = id;
            this.folderName.Text = this._title;
            bool find = false;
            foreach (items i in form.itemList)
            {
                if (id == i.id)
                {
                    _items = i;
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                _items.id = id;
                _items._item = new ArrayList();
                form.itemList.Add(_items);
            }
            foreach (item i in _items._item)
            {
                this.panel2.Controls.Add(i._l);

                this.panel2.Controls.Add(i._t);
            }
        }
        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private item return_this_itsm(object sender,Point p)
        {
         
            foreach (item i in _items._item)
            {
                if(p.Y== i._l.Location.Y)
                {
                    return i;
                }
            }
            return new item();
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)//鼠标抬起换位置
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                bool down = false;

           
                foreach (item i in _items._item)
                {

                    item t = (item)i;
                        if (mouse_offset.X - t._l.Location.X != 0 && mouse_offset.Y - t._l.Location.Y != 0)
                        {
                            if (((Control)sender).Parent.PointToClient(mousePos).X - t._l.Location.X <= 100 && ((Control)sender).Parent.PointToClient(mousePos).X - t._l.Location.X >= -100)
                            {
                                if (((Control)sender).Parent.PointToClient(mousePos).Y - t._l.Location.Y <= 50 && ((Control)sender).Parent.PointToClient(mousePos).Y - t._l.Location.Y >= -50)
                                {
                                    item this_sander = return_this_itsm(sender, ((Control)sender).Location);
                                    int x = t._l.Location.X;
                                    int y = t._l.Location.Y;
                                    //t._t.Location = new Point(((Control)sender).Location.X, ((Control)sender).Location.Y);
                                    t.setlocation(((Control)sender).Location.X, ((Control)sender).Location.Y);
                                    //((Control)sender).Location = new Point(x, y);
                                    this_sander.setlocation(x, y);
                                    down = true;
                                }
                            }
                        }
                    
                }
            }
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
        private void ExitButton_Click(object sender, EventArgs e)    //退出
        {
            _form1.Close();
            this.Close();
        }

        private bool pinned = true;
        private void PinButton_Click(object sender, EventArgs e)     //置顶
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


        private void AddButton_Click(object sender, EventArgs e)       //添加按钮， for_HYL, 也是在这里改样式， for_GJY 改过的
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;    //滚动条
            Label l = new Label();                                               //文本label
            l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            l.Name = "新消息";
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = "新消息";
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            l.DoubleClick += new System.EventHandler(this.l_Edit_Click);
            l.Font = new Font(l.Font.FontFamily, 15, l.Font.Style);
            l.Tag = _id;

            //甘某人加的移动
            l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
            l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);

            Button del = new Button();                                                //删除button
            del.Location = new System.Drawing.Point(360, 50);
            del.Name = "D";
            del.Size = new System.Drawing.Size(25, 25);
            del.TabIndex = 0;
            del.Text = "D";
            del.UseVisualStyleBackColor = true;
            del.Click += new System.EventHandler(this.DEL_Click);
            del.Tag = _id;

            Button move = new Button();                                                //移动button
            move.Location = new System.Drawing.Point(360, 10);
            move.Name = "M";
            move.Size = new System.Drawing.Size(25, 25);
            move.TabIndex = 1;
            move.Text = "M";
            move.UseVisualStyleBackColor = true;
            move.Click += new System.EventHandler(this.move_Click);
            move.Tag = _id;

            TextBox t = new TextBox();                                                //修改时的输入文本框
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



            this.panel2.Controls.Add(l);   //添加操作
            l.Controls.Add(del);
            l.Controls.Add(move);

            this.panel2.Controls.Add(t);
            l.Visible = false;
            item i = new item();
            i._l = l;
            i._del = del;
            i._move = move;
            i._t = t;
            i.id = _id++;
            _items._item.Add(i);
            //System.Threading.Thread.Sleep(500);
            //this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
            //this.Controls.Add(tex);
        }
        public void MOVE_ADD(string text)         //添加一个文本为text的item， for_GJY_改过的， for_ZC调用这个API就可以了
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;

            Label l = new Label();
            l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            l.Name = "新消息";
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = text;
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
            move.Click += new System.EventHandler(this.move_Click);
            move.Tag = _id;

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


            l.Controls.Add(del);
            l.Controls.Add(move);

            this.panel2.Controls.Add(l);
            this.panel2.Controls.Add(t);

            item i = new item();
            i._l = l;
            i._del = del;
            i._move = move;
            i._t = t;
            i.id = _id++;
            _items._item.Add(i);
        }
        public void DEL_Click(object sender, EventArgs e)      //删除
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;
            Button btn = (Button)sender;
            int id = (int)btn.Tag;
            int top = 0;
            foreach (item i in _items._item)
            {
                if (id == i.id)
                {
                    top = i._l.Top;
                    break;
                }
            }
            foreach (item i in _items._item)
            {
                if (top < i._l.Top)
                {
                    i._l.Top -= 100;
                    i._t.Top -= 100;

                }
            }
            foreach (item i in _items._item)
            {
                if (i._l.Top == top)
                {
                    this.panel2.Controls.Remove(i._l);
                    this.panel2.Controls.Remove(i._del);
                    this.panel2.Controls.Remove(i._move);
                    this.panel2.Controls.Remove(i._t);
                    this.text_cont--;
                    _items._item.Remove(i);
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)     //返回button
        {
            this._form1.Visible = true;
            this.Visible = false;
            //this.Close();
        }


        private void TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int id = (int)t.Tag;
            foreach (item i in _items._item)
            {
                if (id == i.id)
                {
                    i._l.Text = i._t.Text;
                    break;
                }

            }
        }
        private void move_Click(object sender, EventArgs e)            //移动
        {
            Button btn = (Button)sender;
            int id = (int)btn.Tag;
            
            foreach (item i in _items._item)
            {
                if (id == i.id)
                {
                    this._form1.moveChoose(this,_items.id, i.id, i._l.Text);
                    break;
                }
            }
            this._form1.Visible = true;
            
            this.Visible = false;
        }
        private void l_Edit_Click(object sender, EventArgs e)
        {

            Label l = (Label)sender;
            int id = (int)l.Tag;
            foreach (item i in _items._item)
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
            foreach (item i in _items._item)
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
