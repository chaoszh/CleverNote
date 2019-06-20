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
using System.Runtime.InteropServices;


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

        [DllImport("user32.dll")]//这块用来移动这个窗体
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int VM_NCLBUTTONDOWN = 0XA1;//定义鼠标左键按下
        private const int HTCAPTION = 2;

        public struct item       //todolist每一条的item
        {
            public int id;
            public Label _l;
            public Button _move;
            public Button _del;
            public TextBox _t;
            public void setlocation(int x, int y)
            {
                Point p = new Point(_l.Location.X, y);
                if (_l != null)
                {
                    _l.Location = p;
                }
                if (_t != null)
                {
                    _t.Location = p;
                }
                //_move.Location = new Point(p.X + 355, p.Y-10 );
                //_move.BringToFront();
                //_del.Location = new Point(p.X + 355, p.Y +30);
                //_del.BringToFront();


            }
        };
        public struct items      //item数组
        {
            public int id;
            public ArrayList _item;
        }
        public items _items;
        private item movingItem;
        public bool is_down = false;
        public Point delta;
        public item moving;
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
            movingItem = createMovingItem();
        }
        private item createMovingItem()
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;    //滚动条
            Label l = new Label();                                               //文本label
            //l.BackColor = Color.LightGray;
            //l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            l.Name = "新消息";
            l.BackColor = Color.FromArgb(35, 35, 35);
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = "新消息";
            l.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            l.Click += new System.EventHandler(this.l_Edit_Click);
            l.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            l.Tag = _id;
            l.Padding = new Padding(10, 10, 80, 10);
            l.AutoSize = false;

            //甘某人加的移动
            l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
            l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);
            l.MouseMove += new System.Windows.Forms.MouseEventHandler(TextBox_MouseMove);

            Button del = new Button();                                                //删除button
            del.Location = new System.Drawing.Point(360, 50);
            del.Name = "D";
            del.Size = new System.Drawing.Size(22, 22);
            del.TabIndex = 0;
            //del.Text = "D";
            del.UseVisualStyleBackColor = true;
            del.Click += new System.EventHandler(this.DEL_Click);
            del.Tag = _id;
            del.BackColor = Color.FromArgb(35, 35, 35);
            del.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            del.FlatAppearance.BorderSize = 0;
            Image img1 = Image.FromFile(@"..\..\\Resources\del.png");
            del.BackgroundImage = img1;
            del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            Button move = new Button();                                                //移动button
            move.Location = new System.Drawing.Point(360, 10);
            move.Name = "M";
            move.Size = new System.Drawing.Size(22, 22);
            move.TabIndex = 0;
            //move.Text = "M";

            // img1.Size = move.Size;
            move.UseVisualStyleBackColor = true;
            move.BackColor = Color.FromArgb(35, 35, 35);
            move.Click += new System.EventHandler(this.move_Click);
            move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            move.FlatAppearance.BorderSize = 0;
            move.Tag = _id;
            Image img2 = Image.FromFile(@"..\..\\Resources\move.png");
            move.BackgroundImage = img2;
            move.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;





            this.panel2.Controls.Add(l);   //添加操作
            l.Controls.Add(del);
            l.Controls.Add(move);

            l.Visible = false;
            item i = new item();
            i._l = l;
            i._del = del;
            i._move = move;
            i.id = -1;
            return i;
        }


        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouse_offset = new Point(-e.X, -e.Y);
                Label movingLabel = (Label)sender;
                foreach (item i in _items._item)
                {
                    if (i.id == (int)movingLabel.Tag)
                    {
                        moving = i;
                        moving._l.Visible = false;
                        moving._t.Visible = false;
                        movingItem._l.Visible = true;
                        movingItem._l.Text = moving._l.Text;

                    }
                }
                delta.X = movingItem._l.Location.X - mouse_offset.X;
                delta.Y = movingItem._l.Location.Y - mouse_offset.Y;

                if (is_down)
                {
                    Point mousePos = Control.MousePosition;
                    movingItem.setlocation(((Control)sender).Parent.PointToClient(mousePos).X + delta.X, ((Control)sender).Parent.PointToClient(mousePos).Y);
                }



                is_down = true;
            }

        }

        private item return_this_itsm(object sender, Point p)
        {

            foreach (item i in _items._item)
            {
                if (p.Y == i._l.Location.Y)
                {
                    return i;
                }
            }
            return new item();
        }
        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = Control.MousePosition;
            if (is_down)
            {
                movingItem.setlocation(((Control)sender).Parent.PointToClient(mousePos).X + delta.X, ((Control)sender).Parent.PointToClient(mousePos).Y);


                foreach (item i in _items._item)
                {
                    item t = (item)i;
                    if (mouse_offset.Y - t._l.Location.Y != 0)
                    {
                        if (movingItem._l.Location.Y - t._l.Location.Y <= 50 && movingItem._l.Location.Y - t._l.Location.Y >= -50)
                        {
                            item this_sander = return_this_itsm(sender, ((Control)sender).Location);

                            int x = t._l.Location.X;
                            int y = t._l.Location.Y;
                            t.setlocation(moving._l.Location.X, moving._l.Location.Y);
                            moving.setlocation(x, y);
                        }
                    }

                }
            }

        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)//鼠标抬起换位置
        {
            is_down = false;
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

                                t.setlocation(((Control)sender).Location.X, ((Control)sender).Location.Y);
                                ((Control)sender).Location = new Point(x, y);
                                this_sander.setlocation(x, y);
                                this_sander._l.Visible = true;
                                this_sander._t.Visible = true;
                                movingItem._l.Visible = false;
                                down = true;
                            }
                        }
                    }

                }

                if (!down)
                {
                    item this_sander = return_this_itsm(sender, ((Control)sender).Location);
                    movingItem.setlocation(mouse_offset.X, mouse_offset.Y);
                    this_sander._l.Visible = true;
                    this_sander._t.Visible = true;
                    movingItem._l.Visible = false;
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
                case 0x0312://热键
                    HotKeyReact(m.WParam.ToInt32());
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
            _form1.SaveFile();
            _form1.Close();
            this.Close();
        }

        private bool pinned = true;
        private void PinButton_Click(object sender, EventArgs e)              //置顶
        {
            if (pinned == true)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                this.TopMost = false;
                Image img4 = Image.FromFile("../../Resources/move.png");
                this.PinButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PinButton.BackgroundImage")));
                this.PinButton.Size = new System.Drawing.Size(16, 16);
                //this.PinButton.BackgroundImage = img4;
            }
            else
            {
                this.TopMost = true;
                this.PinButton.Size = new System.Drawing.Size(20, 20);
                Image img3 = Image.FromFile("../../Resources/Pinned.png");
                this.PinButton.BackgroundImage = img3;
            }
            pinned = !pinned;
        }
        #endregion


        private void AddButton_Click(object sender, EventArgs e)       //添加按钮， for_HYL, 也是在这里改样式， for_GJY 改过的
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;    //滚动条
            Label l = new Label();                                               //文本label
            //l.BackColor = Color.LightGray;
            //l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            l.Name = "新消息";
            l.BackColor = Color.FromArgb(35, 35, 35);
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = "新消息";
            l.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            l.Click += new System.EventHandler(this.l_Edit_Click);
            l.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            l.Tag = _id;
            l.Padding = new Padding(10, 10, 80, 10);
            l.AutoSize = false;



            //甘某人加的移动
            l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
            l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);
            l.MouseMove += new System.Windows.Forms.MouseEventHandler(TextBox_MouseMove);

            Button del = new Button();                                                //删除button
            del.Location = new System.Drawing.Point(360, 50);
            del.Name = "D";
            del.Size = new System.Drawing.Size(22, 22);
            del.TabIndex = 0;
            //del.Text = "D";
            del.UseVisualStyleBackColor = true;
            del.Click += new System.EventHandler(this.DEL_Click);
            del.Tag = _id;
            del.BackColor = Color.FromArgb(35, 35, 35);
            del.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            del.FlatAppearance.BorderSize = 0;
            Image img1 = Image.FromFile(@"..\..\\Resources\del.png");
            del.BackgroundImage = img1;
            del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            Button move = new Button();                                                //移动button
            move.Location = new System.Drawing.Point(360, 10);
            move.Name = "M";
            move.Size = new System.Drawing.Size(22, 22);
            move.TabIndex = 0;
            //move.Text = "M";

            // img1.Size = move.Size;
            move.UseVisualStyleBackColor = true;
            move.BackColor = Color.FromArgb(35, 35, 35);
            move.Click += new System.EventHandler(this.move_Click);
            move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            move.FlatAppearance.BorderSize = 0;
            move.Tag = _id;
            Image img2 = Image.FromFile(@"..\..\\Resources\move.png");
            move.BackgroundImage = img2;
            move.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;



            TextBox t = new TextBox();                                                //修改时的输入文本框
            t.Location = new System.Drawing.Point(8, 18 + (text_cont++) * 100);
            t.Name = "textBox";
            t.Size = new System.Drawing.Size(400, 85);
            t.Text = l.Text;
            t.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            t.TabIndex = 1;
            t.DoubleClick += new System.EventHandler(this.t_Edit_Click);
            t.TextChanged += new System.EventHandler(this.TextChanged);
            t.Multiline = true;
            t.Tag = _id;
            t.BorderStyle = System.Windows.Forms.BorderStyle.None;


            this.panel2.Controls.Add(l);   //添加操作
            l.Controls.Add(del);
            l.Controls.Add(move);

            this.panel2.Controls.Add(t);
            t.Visible = false;
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

            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;    //滚动条
            Label l = new Label();                                               //文本label
            //l.BackColor = Color.LightGray;
            //l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            l.Location = new System.Drawing.Point(8, 18 + (text_cont) * 100);
            l.Name = "新消息";
            l.BackColor = Color.FromArgb(35, 35, 35);
            l.Size = new System.Drawing.Size(400, 85);
            l.TabIndex = 6;
            l.Text = "新消息";
            l.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            l.Padding = new Padding(10, 10, 80, 10);
            l.AutoSize = false;


            l.Click += new System.EventHandler(this.l_Edit_Click);
            l.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            l.Tag = _id;

            //甘某人加的移动
            l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
            l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);
            l.MouseMove += new System.Windows.Forms.MouseEventHandler(TextBox_MouseMove);

            Button del = new Button();                                                //删除button
            del.Location = new System.Drawing.Point(360, 50);
            del.Name = "D";
            del.Size = new System.Drawing.Size(22, 22);
            del.TabIndex = 0;
            //del.Text = "D";
            del.UseVisualStyleBackColor = true;
            del.Click += new System.EventHandler(this.DEL_Click);
            del.Tag = _id;
            del.BackColor = Color.FromArgb(35, 35, 35);
            del.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            del.FlatAppearance.BorderSize = 0;
            Image img1 = Image.FromFile(@"..\..\\Resources\del.png");
            del.BackgroundImage = img1;
            del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            Button move = new Button();                                                //移动button
            move.Location = new System.Drawing.Point(360, 10);
            move.Name = "M";
            move.Size = new System.Drawing.Size(22, 22);
            move.TabIndex = 0;
            //move.Text = "M";

            // img1.Size = move.Size;
            move.UseVisualStyleBackColor = true;
            move.BackColor = Color.FromArgb(35, 35, 35);
            move.Click += new System.EventHandler(this.move_Click);
            move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            move.FlatAppearance.BorderSize = 0;
            move.Tag = _id;
            Image img2 = Image.FromFile(@"..\..\\Resources\move.png");
            move.BackgroundImage = img2;
            move.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;



            TextBox t = new TextBox();                                                //修改时的输入文本框
            t.Location = new System.Drawing.Point(8, 18 + (text_cont++) * 100);
            t.Name = "textBox";
            t.Size = new System.Drawing.Size(400, 85);
            t.Text = l.Text;
            t.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            t.TabIndex = 1;
            t.DoubleClick += new System.EventHandler(this.t_Edit_Click);
            t.TextChanged += new System.EventHandler(this.TextChanged);
            t.Multiline = true;
            t.Tag = _id;
            t.BorderStyle = System.Windows.Forms.BorderStyle.None;

            t.Visible = false;
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
                    this._form1.moveChoose(this, _items.id, i.id, i._l.Text);
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

        private void Panel1_MouseDown(object sender, MouseEventArgs e)//解决窗体不能移动
        {
            //为当前应用程序释放鼠标捕获
            ReleaseCapture();
            //发送消息 让系统误以为在标题栏上按下鼠标
            SendMessage((IntPtr)this.Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        #region 热键
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        //热键统一注册
        private void ShortcutKeyActivate()
        {
            //F2
            RegisterHotKey(Handle, 990316, 0, Keys.F2);
            //F3
            RegisterHotKey(Handle, 990803, 0, Keys.Q);
        }

        //热键统一注销
        private void ShortcutKeyUnactivate()
        {
            //注销
            UnregisterHotKey(Handle, 990316);
        }

        //窗口Activate时注册热键
        private void Form1_Activated(object sender, EventArgs e)
        {
            ShortcutKeyActivate();
        }

        //窗口Leave时注销热键
        private void Form1_Leave(object sender, EventArgs e)
        {
            ShortcutKeyUnactivate();
        }

        //快捷键响应，在WndProc()中调用的函数
        private void HotKeyReact(int m)
        {
            switch (m)
            {
                //热键F2
                case 990316:
                    MOVE_ADD(getClipMsg());
                    break;
                //热键F3
                case 990803:
                    if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
                    else this.WindowState = FormWindowState.Minimized;
                    break;
            }
        }

        //从剪贴板拿取消息
        private string getClipMsg()
        {
            // Declares an IDataObject to hold the data returned from the clipboard.
            // Retrieves the data from the clipboard.
            IDataObject iData = Clipboard.GetDataObject();

            // Determines whether the data is in a format you can use.
            if (iData.GetDataPresent(DataFormats.Text))
            {
                // Yes it is, so display it in a text box.
                return (String)iData.GetData(DataFormats.Text);
            }
            else
            {
                return "无法从剪贴板获取数据";
            }
        }
        #endregion

    }
}
