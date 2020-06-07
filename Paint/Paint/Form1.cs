using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Paint
{
    public partial class Form : System.Windows.Forms.Form
    {
        Graphics g;
        Bitmap bm;
        Pen pen;
        int x = -1;
        int y = -1;
        int width = 4;
        int height = 4;
        bool drawing = false;
        Point pDown;
        Point pUp;
        public Form()
        {
            InitializeComponent();
            bm = new Bitmap(Canvas.ClientSize.Width,Canvas.ClientSize.Height);
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.Paint += Canvas_Paint;

            pen = new Pen(Color.Black,4);            
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            Canvas.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top); // will this work? dunno...
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        }
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            x = e.X;
            y = e.Y;
            pDown = e.Location;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(drawing && x!=-1 && y!=-1)
            {
                using (g = Graphics.FromImage(bm))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawLine(pen, new Point(x, y), e.Location);
                }
                    
                x = e.X;
                y = e.Y;
            }
            Canvas.Invalidate();
            Canvas.Cursor = Cursors.Cross;
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            pUp = e.Location;
            if (pDown == pUp)
            {
                using (g = Graphics.FromImage(bm))
                {
                    SolidBrush brush = new SolidBrush(pen.Color);   // might adjust, works fine atm
                    g.FillEllipse(brush, x, y, width, height);
                    brush.Dispose();
                }
            }
            drawing = false;
            x = -1;
            y = - 1;
            
        }
                    ///Drawing ToolBar///
                    ////////////////////
        private void Color_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            UI.ClearBoxes(colorPanel);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Color = p.BackColor;

        }

        private void Pic_Small_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            UI.ClearBoxes(ToolSize);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 1;
            width = height = 1;

        }

        private void Pic_Medium_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            UI.ClearBoxes(ToolSize);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 4;
            width = height = 4;
        }

        private void Pic_Big_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            UI.ClearBoxes(ToolSize);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 7;
            width = height = 7;
        }

        private void Pic_Erase_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
        }

                    ///File ToolBar///
                    //////////////////
        private void Pic_New_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
            using (g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
            }
            FileManager.Save(bm); // to be fixed
        }

        private void Pic_Save_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
            FileManager.Save(bm); // to be fixed
        }

        private void Pic_Open_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
            FileManager.Open(bm,Canvas);
        }

        private void Pic_Undo_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
        }

        private void Pic_Redo_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
        }

        
    }
}
