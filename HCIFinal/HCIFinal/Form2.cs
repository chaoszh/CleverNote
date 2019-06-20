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
using System.IO;
using System.Text.RegularExpressions;

namespace HCIFinal
{
    public partial class Form2 : Form           //文件夹（分类、标签）窗口
    {
        private Point mouse_offset;//用作移动
        private int text_cont = 0;              //item个数
        private int _id = 0;                    //item id
        public bool is_moving = false;          //是否正处于移动状态（item在文件夹间移动）
        public bool is_show = true;             //窗口隐藏与否
       


        [DllImport("user32.dll")]
         public static extern bool ReleaseCapture();

         [DllImport("user32.dll")]
         public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

         private const int VM_NCLBUTTONDOWN = 0XA1;//定义鼠标左键按下
         private const int HTCAPTION = 2;

        public struct ditem                     //文件夹item
        {
            public int id;
            public Label _l;
            public Button _move;
            public Button _del;
            public TextBox _t;
            public void setlocation(int x, int y)//设置位置用作移动
            {
                Point p = new Point(x, y);
                _l.Location = p;
                // _del.Location = new Point(p.X+150,p.Y+6);
                _t.Location = p;
            }
        };
        public struct ditems                    //文件夹item数组
        {
            public int id;
            public ArrayList _item;
        }
        Form1 movingForm;                       //以下是处理移动状态时的传参
        int movingID;
        string movingSTR;
        int movingFID;
        public ArrayList _items = new ArrayList();
        public ArrayList itemList = new ArrayList();
        public ArrayList sonForm = new ArrayList();     //子窗口数组
                                                        //GlobalMouseHandler 
        public void Read(string path)//初始化读文件还原上次关闭
        {   StreamReader sr = new StreamReader(path);
            string[] result = Regex.Matches(File.ReadAllText(path), @"(?<=@).*?(?=#)").Cast<Match>().Select(S => S.Value.Trim()).ToArray();
            foreach(string line in result)
            {
                this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;   //滚动条
                                                                                    //System.Threading.Thread.Sleep(500);


                Label l = new Label();                                               //文本框label
                l.Location = new System.Drawing.Point(8 + (text_cont % 2) * 200, 18 + (text_cont / 2) * 100);

                l.Name = "新文件夹";
                l.Size = new System.Drawing.Size(180, 85);
                l.TabIndex = 6;
                l.BackColor = Color.FromArgb(35, 35, 35);
                l.Text = "新文件夹";
                l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                l.DoubleClick += new System.EventHandler(this.document_Click);
                l.Click += new System.EventHandler(this.move_Click);
                l.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                l.Tag = _id;
                //用于移动
                l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
                l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);
                l.Click += new System.EventHandler(this.Edit_Click_l);


                /*Button edit = new Button();                                         //编辑按钮
                edit.Location = new System.Drawing.Point(150, 6);
                edit.Name = "E";
                edit.Size = new System.Drawing.Size(25, 25);
                edit.TabIndex = 1;
                edit.Text = "E";
                edit.UseVisualStyleBackColor = true;
                edit.Click += new System.EventHandler(this.Edit_Click);
                edit.Tag = _id;*/


                TextBox t = new TextBox();                                          //重命名时的修改框
                t.Location = new System.Drawing.Point(8 + (text_cont % 2) * 200, 18 + (text_cont / 2) * 100);
                t.Name = "textBox";
                t.Size = new System.Drawing.Size(180, 85);
                t.Text = l.Text;
                t.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                t.TabIndex = 1;
                t.TextChanged += new System.EventHandler(this.TextChanged);
                t.DoubleClick += new System.EventHandler(this.t_Edit_Click);
                t.Multiline = true;
                t.Tag = _id;
                text_cont++;

                this.panel2.Controls.Add(l);                            //添加操作，不用改
                                                                        //l.Controls.Add(edit);
                this.panel2.Controls.Add(t);
                t.Visible = false;
                ditem i = new ditem();
                i._l = l;
                //i._del = edit;
                i._t = t;
                i.id = _id++;
                _items.Add(i);
            }
            
            sr.Close();
        }

        public Form2()//初始化
        {
            InitializeComponent();
            string str = System.IO.Directory.GetCurrentDirectory();
            Read(str+"\\data.txt");
            this.folderName.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ExitButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExitButton.BackgroundImage")));
            this.PinButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PinButton.BackgroundImage")));
            this.AddButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddButton.BackgroundImage")));
        }

        public void changeShowState()   //show or hide
        {
            if (is_show)
            {
                this.Visible = false;
                foreach (Form1 f in sonForm)
                {
                    f.Visible = false;
                }
                is_show = false;
            }
            else
            {
                is_show = true;
                this.Visible = true;
            }
        }
        public void ADD(string text)    //add item
        {
            bool success = false;
            foreach (Form1 f in sonForm)
            {
                f.MOVE_ADD(text);
                success = true;
                break;
            }
            if(!success)
            {
                Form1 frm1 = new Form1(this, Text, 0);
                frm1.StartPosition = FormStartPosition.Manual;
                frm1.Location = new Point(this.Location.X, this.Location.Y);
                //frm1.MdiParent = this;
                sonForm.Add(frm1);
            }
        }

        public void moveChoose(Form1 f,int f_id, int id, string content)   //form1进入移动状态时调用
        {
            is_moving = true;
            this.PinButton.Visible = false;
            this.ExitButton.Visible = false;
            this.AddButton.Visible = false;
            this.movingFID = f_id;
            this.movingForm = f;
            this.movingID = id;
            this.movingSTR = content;
        }
        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)//鼠标抬起换位置
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                bool down = false;
                foreach (ditem i in _items)
                {

                    {
                        ditem t = (ditem)i;
                        if (mouse_offset.X - t._l.Location.X != 0 && mouse_offset.Y - t._l.Location.Y != 0)
                        {
                            if (((Control)sender).Parent.PointToClient(mousePos).X - t._l.Location.X <= 100 && ((Control)sender).Parent.PointToClient(mousePos).X - t._l.Location.X >= -100)
                            {
                                if (((Control)sender).Parent.PointToClient(mousePos).Y - t._l.Location.Y <= 50 && ((Control)sender).Parent.PointToClient(mousePos).Y - t._l.Location.Y >= -50)
                                {
                                    int x = t._l.Location.X;
                                    int y = t._l.Location.Y;
                                    t.setlocation(((Control)sender).Location.X, ((Control)sender).Location.Y);
                                   ((Control)sender).Location = new Point(x, y);
                                    down = true;
                                }
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
        public void SaveFile()
        {
            string str = System.IO.Directory.GetCurrentDirectory();
            FileStream fs = new FileStream(str + "\\data.txt", FileMode.Open);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);
            fs.Close();
            //Directory.Delete((str + "\\data.txt"),true);
            //FileStream fs1 = new FileStream((str + "\\data.txt"), FileMode.Create, FileAccess.Write);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter((str + "\\data.txt"), true))
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    object i_ditem = _items[i];
                    ditem j_ditem = (ditem)i_ditem;
                    file.WriteLine("@" + j_ditem._l.Text + "#");// 直接追加文件末尾，换行 
                }
                foreach (Form1 s in sonForm)
                {
                    foreach (Form1.item i in s._items._item)
                    {
                        string id = Convert.ToString(s.f_id);
                        file.WriteLine("文件序号：" + id + "#文件内容：" + i._l.Text + "#");
                    }
                }
                file.Close();
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)             //退出
        {
            SaveFile(); 
            this.Close();
        }
        private void document_Click(object sender, EventArgs e)             //for_GJY_改过的
        {
            Label l = (Label)sender;
            bool find = false;
            foreach (Form1 f in sonForm)
            {
                if (f.f_id == (int)l.Tag)
                {
                    f.Visible = true;
                    find = true;
                }
            }
            if (!find)
            {
                Form1 frm1 = new Form1(this, l.Text, (int)l.Tag);
                frm1.StartPosition = FormStartPosition.Manual;
                frm1.Location = new Point(this.Location.X, this.Location.Y);

                string str = System.IO.Directory.GetCurrentDirectory();
                string path = str + "\\data.txt";
                StreamReader sr = new StreamReader(path);
                string[] id = Regex.Matches(File.ReadAllText(path), @"(?<=文件序号：).*?(?=#)").Cast<Match>().Select(S => S.Value.Trim()).ToArray();
                string[] data = Regex.Matches(File.ReadAllText(path), @"(?<=文件内容：).*?(?=#)").Cast<Match>().Select(S => S.Value.Trim()).ToArray();
                /*string[] id = Regex.Matches(File.ReadAllText(path),
                    @"\$([^\&])*\%").Cast<Match>().Select(S => S.Value.Trim()).ToArray();
                string[] data = Regex.Matches(File.ReadAllText(path),
    @"\%([^\&])*\&").Cast<Match>().Select(S => S.Value.Trim()).ToArray();*/

                int j = 0;
                foreach(string i in id)
                {
                    string tag = Convert.ToString(l.Tag);
                    if(tag==i)
                    {
                        frm1.MOVE_ADD(data[j]);
                    }
                    j++;
                }
                sr.Close();


                //frm1.MdiParent = this;
                frm1.Show();
                sonForm.Add(frm1);
            }

            this.Visible = false;
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

        private void AddButton_Click(object sender, EventArgs e)                //添加ditem, for_HYL UI主要在这里改
        {
            this.panel2.VerticalScroll.Value = panel2.VerticalScroll.Minimum;   //滚动条
            //System.Threading.Thread.Sleep(500);


            Label l = new Label();                                               //文本框label
            l.Location = new System.Drawing.Point(8 + (text_cont % 2) * 200, 18 + (text_cont / 2) * 100);

            l.Name = "新文件夹";
            l.Size = new System.Drawing.Size(180, 85);
            l.TabIndex = 6;
            l.BackColor = Color.FromArgb(35, 35, 35);
            l.Text = "新文件夹";
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            l.DoubleClick += new System.EventHandler(this.document_Click);
            l.Click += new System.EventHandler(this.move_Click);
            l.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            l.Tag = _id;
            //用于移动
            l.MouseDown += new System.Windows.Forms.MouseEventHandler(TextBox_MouseDown);
            l.MouseUp += new System.Windows.Forms.MouseEventHandler(TextBox_MouseUp);
            l.Click += new System.EventHandler(this.Edit_Click_l);


            /*Button edit = new Button();                                         //编辑按钮
            edit.Location = new System.Drawing.Point(150, 6);
            edit.Name = "E";
            edit.Size = new System.Drawing.Size(25, 25);
            edit.TabIndex = 1;
            edit.Text = "E";
            edit.UseVisualStyleBackColor = true;
            edit.Click += new System.EventHandler(this.Edit_Click);
            edit.Tag = _id;*/


            TextBox t = new TextBox();                                          //重命名时的修改框
            t.Location = new System.Drawing.Point(8 + (text_cont % 2) * 200, 18 + (text_cont / 2) * 100);
            t.Name = "textBox";
            t.Size = new System.Drawing.Size(180, 85);
            t.Text = l.Text;
            t.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            t.TabIndex = 1;
            t.TextChanged += new System.EventHandler(this.TextChanged);
            t.DoubleClick += new System.EventHandler(this.t_Edit_Click);
            t.Multiline = true;
            t.Tag = _id;
            text_cont++;

            this.panel2.Controls.Add(l);                            //添加操作，不用改
            //l.Controls.Add(edit);
            this.panel2.Controls.Add(t);
            t.Visible = false;
            ditem i = new ditem();
            i._l = l;
            //i._del = edit;
            i._t = t;
            i.id = _id++;
            _items.Add(i);
        }


        private void move_Click(object sender, EventArgs e)              //for_GJY_改过的
        {
            if (is_moving)
            {
                Label l = (Label)sender;
                bool find = false;
                foreach (Form1 f in sonForm)
                {
                    if (f.f_id == (int)l.Tag)
                    {

                        f.MOVE_ADD(movingSTR);
                        find = true;
                    }
                }
                if (!find)
                {
                    Form1 frm1 = new Form1(this, l.Text, (int)l.Tag);
                    frm1.StartPosition = FormStartPosition.Manual;
                    frm1.Location = new Point(this.Location.X, this.Location.Y);
                    sonForm.Add(frm1);
                    frm1.MOVE_ADD(movingSTR);
                }
                foreach (Form1.items i in itemList)
                {
                    if (movingFID == i.id)
                    {
                        foreach (Form1.item j in i._item)
                        {
                            if (movingID == j.id)
                            {
                                movingForm.DEL_Click(j._del, EventArgs.Empty);
                                break;
                                
                            }
                        }
                    }
                }
                this.movingForm.Visible = true;
                this.Visible = false;
                this.PinButton.Visible = true;
                this.ExitButton.Visible = true;
                this.AddButton.Visible = true;
                is_moving = false;
            }
            
        }
        private void TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int id = (int)t.Tag;
            foreach (ditem i in _items)
            {
                if (id == i.id)
                {
                    i._l.Text = i._t.Text;
                    break;
                }

            }
            foreach (Form1 f in sonForm)
            {
                if (id == f.f_id)
                {
                    f.setFolderName(t.Text);
                    break;
                }

            }
        }

        private void Edit_Click_l(object sender, EventArgs e)
        {
            MouseEventArgs mouse_e = (MouseEventArgs)e;
            if (mouse_e.Button == MouseButtons.Left)
            {
                Label l = (Label)sender;
                int id = (int)l.Tag;
                foreach (ditem i in _items)
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

        private void t_Edit_Click(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int id = (int)t.Tag;
            foreach (ditem i in _items)
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

    private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel1_MouseDown_1(object sender, MouseEventArgs e)//解决窗体不能移动
        {
            //为当前应用程序释放鼠标捕获
              ReleaseCapture();
             //发送消息 让系统误以为在标题栏上按下鼠标
            SendMessage((IntPtr)this.Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

    }

}
