using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace omron
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class myBrick
        {
            public int ID;
            public int TYPE;
            public double Width = 0;
            public double Height = 0;
            public PointF P1;
            public PointF P2;
            public PointF P3;
            public bool bolPlaced = false;
            public double S;

            public void Move(float x, float y)
            {
                P1.X += x;
                P1.Y += y;
                P2.X += x;
                P2.Y += y;
                P3.X += x;
                P3.Y += y;
            }

            public void Rotate()
            {
                P2 = new PointF(P2.Y, P2.X);
                P3 = new PointF(P3.Y, P3.X);
            }

            public double Square()
            {
                if (TYPE == 0)
                {
                    return Width * Height;
                }
                else
                {
                    return (P1.X * P2.Y - P1.X * P3.Y + P2.X * P3.Y - P2.X * P1.Y + P3.X * P1.Y - P2.X * P2.Y) / 2;
                }
            }
        }

        private List<myBrick> lstBricks;
        private List<myBrick> lstOrder;
        private float fltWidth = 40, fltHeight = 50;
        private Graphics g;
        private int pixStartX = 150, pixStartY = 50;

        private double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private double Distance(PointF a, PointF b)
        {
            float x1, x2, y1, y2;
            x1 = a.X;
            y1 = a.Y;
            x2 = b.X;
            y2 = b.Y;
            return Distance(x1, y1, x2, y2);
        }

        private List<double> Order(double l1, double l2, double l3)
        {
            int max = 1, min = 1;
            double dblMax = l1, dblMin = l1, dblMed = 0; ;
            if (dblMax < l2)
            {
                max = 2;
                dblMax = l2;
            }
            if (dblMin > l2)
            {
                min = 2;
                dblMin = l2;
            }
            if (dblMax < l3)
            {
                max = 3;
                dblMax = l3;
            }
            if (dblMin > l3)
            {
                min = 3;
                dblMin = l3;
            }

            if ((max == 1) || (min == 1)) l1 = 0;
            if ((max == 2) || (min == 2)) l2 = 0;
            if ((max == 3) || (min == 3)) l3 = 0;
            if (l1 > 0) dblMed = l1;
            else if (l2 > 0) dblMed = l2;
            else dblMed = l3;

            List<double> l = new List<double>();
            l.Add(dblMax);
            l.Add(dblMed);
            l.Add(dblMin);
            return l;
        }

        private double CalDistanc(PointF pointP, PointF pointA, PointF pointB)
        {
            //求直线方程
            double A = 0, B = 0, C = 0;
            A = pointA.Y - pointB.Y;
            B = pointB.X - pointA.X;
            C = pointA.X * pointB.Y - pointA.Y * pointB.X;
            //代入点到直线距离公式
            double distance = 0;

            //return (A * pointP.X + B * pointP.Y + C) / Math.Sqrt(A * A + B * B);


            if (pointP.X > (-(B * pointP.Y + C) / A))
            {//点在线右边
                distance = (Math.Abs(A * pointP.X + B * pointP.Y + C)) / (Math.Sqrt(A * A + B * B));
            }
            else
            {//点在线左边
                distance = -(Math.Abs(A * pointP.X + B * pointP.Y + C)) / (Math.Sqrt(A * A + B * B));
            }
            return distance;
        }

        private myBrick GetBrickFromDt(DataRow r)
        {
            myBrick brick = new myBrick();
            bool bolRect = (r[0].ToString().Trim() == "0");

            double x1, x2, x3, y1, y2, y3, l1, l2, l3;
            x1 = double.Parse(r[1].ToString());
            y1 = double.Parse(r[2].ToString());
            x2 = double.Parse(r[3].ToString());
            y2 = double.Parse(r[4].ToString());
            x3 = double.Parse(r[5].ToString());
            y3 = double.Parse(r[6].ToString());
            l1 = Distance(x1, y1, x2, y2);
            l2 = Distance(x1, y1, x3, y3);
            l3 = Distance(x2, y2, x3, y3);

            if (bolRect) brick.TYPE = 0;
            else brick.TYPE = 1;

            List<double> l = Order(l1, l2, l3);
            if (bolRect)
            {
                double width = l[1];
                double height = l[2];
                brick.P1 = new PointF(0, 0);
                brick.P2 = new PointF((float)width, 0);
                brick.P3 = new PointF((float)width, (float)height);
                brick.Height = height;
                brick.Width = width;
            }
            else
            {
                double width = l[0];
                PointF a, b, c;
                if (l[0] == l1)
                {
                    a = new PointF((float)x1, (float)y1);
                    b = new PointF((float)x2, (float)y2);
                    c = new PointF((float)x3, (float)y3);
                }
                else if (l[0] == l2)
                {
                    a = new PointF((float)x1, (float)y1);
                    b = new PointF((float)x3, (float)y3);
                    c = new PointF((float)x2, (float)y2);
                }
                else
                {
                    a = new PointF((float)x2, (float)y2);
                    b = new PointF((float)x3, (float)y3);
                    c = new PointF((float)x1, (float)y1);
                }

                double height = CalDistanc(c, a, b);
                brick.Height = Math.Abs(height);
                brick.Width = width;
                brick.P1 = new PointF(0, 0);
                brick.P2 = new PointF((float)width, 0);
                double dblD;
                if (height > 0) dblD = Distance(a, c);
                else dblD = Distance(b, c);
                double dblX = Math.Sqrt(dblD * dblD - height * height);
                brick.P3 = new PointF((float)dblX, (float)brick.Height);
            }
            return brick;
        }

        private bool AllPlaced()
        {
            for (int i = 0; i < lstOrder.Count; i++)
            {
                if (!lstOrder[i].bolPlaced) return false;
            }
            return true;
        }

        private double AllSquare()
        {
            double result = 0;
            for (int i = 0; i < lstOrder.Count; i++)
            {
                result += lstOrder[i].S;
            }
            return result;
        }

        private List<myBrick> CopyList(List<myBrick> lst)
        {
            List<myBrick> result = new List<myBrick>();
            for (int i = 0; i < lst.Count; i++)
            {
                myBrick brick = new myBrick();
                brick.ID = lst[i].ID;

                brick.TYPE = lst[i].TYPE;
                brick.Width = lst[i].Width;
                brick.Height = lst[i].Height;
                brick.P1 = new PointF(lst[i].P1.X, lst[i].P1.Y);
                brick.P2 = new PointF(lst[i].P2.X, lst[i].P2.Y);
                brick.P3 = new PointF(lst[i].P3.X, lst[i].P3.Y);
                brick.bolPlaced = lst[i].bolPlaced;
                brick.S = lst[i].S;
                result.Add(brick);
            }
            return result;
        }

        private double PlaceBrick(float x,float y,PointF p,List<myBrick> lst)
        {
            if ((x <= 0) || (y <= 0)) return 0;
            float fltTop = p.Y;
            double result = 0, result1 = 0, result2 = 0; 
            for (int i =0;i< lst.Count;i++)
            {
                myBrick brick = lst[i];
                if (brick.bolPlaced) continue;
                float w, h;
                PointF P1, P2;
                w = (float)brick.Width;
                h = (float)brick.Height;

                if ((w <= x) && (h <= y)) 
                {                    
                    brick.Move(p.X, p.Y);
                    brick.bolPlaced = true;
                    result += brick.S;
                    P1 = new PointF(p.X + w, p.Y);
                    P2 = new PointF(p.X, p.Y + h);
                    if (p.X==0)
                    {
                        result += PlaceBrick(x - w, h, P1, lst);
                        result += PlaceBrick(x, y - h, P2, lst);
                        return result;
                    }            
                    List<myBrick> lstNew = CopyList(lst);
                    result1 = PlaceBrick(x - w, h, P1, lstNew);
                    result1 += PlaceBrick(x, y - h, P2, lstNew);
                    lstNew = CopyList(lst);
                    result2 = PlaceBrick(x - w, y, P1, lstNew);
                    result2 += PlaceBrick(w, y - h, P2, lstNew);

                    if (result1 > result2)
                    {
                        result += PlaceBrick(x - w, h, P1, lst);
                        result += PlaceBrick(x, y - h, P2, lst);
                    }
                    else
                    {
                        result += PlaceBrick(x - w, y, P1, lst);
                        result += PlaceBrick(w, y - h, P2, lst);
                    }
                    return result;

                }
                else if ((w <= y) && (h <= x)) 
                {
                    brick.Rotate();                                       
                    brick.Move(p.X, p.Y);
                    brick.bolPlaced = true;
                    result += brick.S;
                    P1 = new PointF(p.X + h, p.Y);
                    P2 = new PointF(p.X, y - w);
                    if (p.X==0)
                    {
                        result += PlaceBrick(x - h, w, P1, lst);
                        result += PlaceBrick(x, y - w, P2, lst);
                        return result;
                    }
                    List<myBrick> lstNew = CopyList(lst);
                    result1 = PlaceBrick(x - h, w, P1,lstNew);
                    result1+= PlaceBrick(x, y - w, P2,lstNew);
                    lstNew = CopyList(lst);
                    result2 = PlaceBrick(x - h, y, P1, lstNew);
                    result2 += PlaceBrick(h, y - w, P2, lstNew);
                    if (result1 > result2)
                    {
                        result+= PlaceBrick(x - h, w, P1, lst);
                        result+= PlaceBrick(x, y - w, P2, lst);
                    }
                    else
                    {
                        result += PlaceBrick(x - h, y, P1, lst);
                        result += PlaceBrick(h, y - w, P2, lst);
                    }
                    return result;
                }
            }
            return result;
        }

        private float MaxHeight()
        {
            float result = 0;
            for (int i=0;i<lstOrder.Count;i++)
            {
                if (result < lstOrder[i].P1.Y) result = lstOrder[i].P1.Y;
                if (result < lstOrder[i].P2.Y) result = lstOrder[i].P2.Y;
                if (result < lstOrder[i].P3.Y) result = lstOrder[i].P3.Y;
            }
            return result;
        }

        private void DrawBricks()
        {
            Pen MyPen = new Pen(Color.Red, 1);
            Font font = new Font("黑体", 20f, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            for (int i=0;i<lstOrder.Count;i++)
            {
                Thread.Sleep(100);
                myBrick brick = lstOrder[i];
                int x1, y1,x2,y2, x3, y3, w, h;
                x1 = (int)(brick.P1.X * 10);
                y1 = (int)(brick.P1.Y * 10);
                x2 = (int)(brick.P2.X * 10);
                y2 = (int)(brick.P2.Y * 10);
                x3 = (int)(brick.P3.X * 10);
                y3 = (int)(brick.P3.Y * 10);

                if (brick.TYPE == 0)
                {
                    w = Math.Abs(x3 - x1);
                    h = Math.Abs(y3 - y1);
                    x1 += pixStartX;
                    y1 = pixStartY + (int)(fltHeight * 10) - y3;
                    Rectangle r = new Rectangle(x1, y1, w, h);
                    g.DrawRectangle(MyPen, r);                
                    g.DrawString(brick.ID.ToString(), font, Brushes.Aqua, r, sf);
                }
                else
                {
                    x1 += pixStartX;
                    x2 += pixStartX;
                    x3 += pixStartX;
                    y1 = pixStartY + (int)(fltHeight * 10) - y1;
                    y2 = pixStartY + (int)(fltHeight * 10) - y2;
                    y3 = pixStartY + (int)(fltHeight * 10) - y3;
                    g.DrawLine(MyPen, x1, y1, x2, y2);
                    g.DrawLine(MyPen, x2, y2, x3, y3);
                    g.DrawLine(MyPen, x3, y3, x1, y1);
                    w = Math.Max(Math.Abs(x1 - x3), Math.Abs(x1 - x2));
                    h = Math.Max(Math.Abs(y1 - y3), Math.Abs(y1 - y2));
                    Rectangle r = new Rectangle(x1, y1-h, w, h);
                    g.DrawString(brick.ID.ToString(), font, Brushes.Aqua, r, sf);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "CSV | *.csv";
            openFileDialog.CheckPathExists = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != null)
            {
                try
                {
                    string strFile = openFileDialog.FileName;
                    txbLoad.Text = strFile;
                    DataTable dt = CSVFileHelper.OpenCSV(strFile);
                    lstBricks.Clear();
                    int id = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int c = int.Parse(r[7].ToString());
                        for (int j = 1; j <= c; j++)
                        {
                            myBrick brick = GetBrickFromDt(r);
                            brick.ID = id;
                            id++;
                            brick.S = brick.Square();
                            lstBricks.Add(brick);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("读取的文件存在错误！");
                    return;
                }
            }
            lstOrder = (from s in lstBricks orderby s.Width descending select s).ToList<myBrick>();

            double dblS = AllSquare();
            //MessageBox.Show(dblS.ToString());
            PlaceBrick(fltWidth, fltHeight, new PointF(0, 0), lstOrder);
            float MaxH = MaxHeight();

            double ratio = dblS * 100.0 / (MaxH * fltWidth);
            string strRatio = ratio.ToString();
            if (strRatio.Length > 5) strRatio = strRatio.Substring(0, 5);
            strRatio += "%";
            txbRatio.Text = strRatio;
            DrawBricks();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lstBricks = new List<myBrick>();
            g = CreateGraphics();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Graphics g = CreateGraphics();
            //Pen MyPen = new Pen(Color.Blue, 2);
            //Rectangle rect = new Rectangle(150,50,400,500);
            //g.DrawRectangle(MyPen, rect);
            
            /*
            for (int i = 0; i <= 10; i++)   //绘制纵向线条
            {
                g.DrawLine(MyPen, x, 400, x, 100);
                x += 40;
            }

            Thread.Sleep(200);  //线程休眠200毫秒，便于观察绘制情况
            int y = 400;
            for (int i = 0; i < +10; i++)   //绘制横向线条 
            {
                g.DrawLine(MyPen, 100, y, 550, y);
                y -= 30;
            }
            Thread.Sleep(200);
            x = 110;
            y = 400;
            Brush MyBrush = new SolidBrush(Color.BlueViolet);
            int[] saleNum = { 120, 178, 263, 215, 99, 111, 265, 171, 136, 100, 129 };
            for (int i = 0; i < saleNum.Length; i++)
            {
                g.FillRectangle(MyBrush, x, y - saleNum[i], 20, saleNum[i]);  //绘制填充矩形
                x += 40;
            }
            */
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.csv";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "csv files|*.csv";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "物料坐标输出";

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != null)
            {
                try
                {
                    string fileName = saveFileDialog.FileName;//文件名字
                    txbSave.Text = fileName;
                    using (StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Default))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("物料代码,顶点1_X,顶点1_Y,顶点2_X,顶点2_Y,顶点3_X,顶点3_Y,");
                        streamWriter.WriteLine(sb.ToString());
                        sb.Clear();
                        for (int i = 0; i < lstOrder.Count; i++)
                        {
                            myBrick brick = lstOrder[i];
                            string s = brick.TYPE.ToString() + ",";
                            s += brick.P1.X.ToString() + "," + brick.P1.Y.ToString() + ",";
                            s += brick.P2.X.ToString() + "," + brick.P2.Y.ToString() + ",";
                            s += brick.P3.X.ToString() + "," + brick.P3.Y.ToString() + ",";
                            sb.Append(s);
                            streamWriter.WriteLine(sb.ToString());
                            sb.Clear();
                        }

                        streamWriter.WriteLine(sb.ToString());
                        sb.Clear();
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            MessageBox.Show("haha");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Pen MyPen = new Pen(Color.Blue, 2);
            Rectangle rect = new Rectangle(pixStartX, pixStartY, (int)(fltWidth * 10), (int)(fltHeight * 10));
            g.DrawRectangle(MyPen, rect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointF b = new PointF(0, 1);
            PointF a = new PointF(1, 0);
            PointF c = new PointF(0, 0);
            double d = CalDistanc(c, a, b);
            MessageBox.Show(d.ToString()); 

        }
    }

    public class CSVFileHelper
    {
        public static DataTable OpenCSV(string filePath)
        {
            //Encoding encoding = Common.GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //StreamReader sr = new StreamReader(fs, encoding);
            StreamReader sr = new StreamReader(fs);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
