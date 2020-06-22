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
        List<Bitmap> OldStack = new List<Bitmap>();
        List<Bitmap> RedoStack = new List<Bitmap>();
        Pen pen;
        int x = -1;
        int y = -1;
        int width = 4;
        int height = 4;
        bool isDown = false;
        bool flood = false;
        bool sample = false;
        Point pDown;
        Point pUp;

        public Form()
        {
            InitializeComponent();
          
            bm = new Bitmap(Canvas.ClientSize.Width,Canvas.ClientSize.Height);  //Initial bitmap(drawn as white rec and pushed to undo stack)
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.FillRectangle(Brushes.White, 0, 0, bm.Width, bm.Height);
            }
            OldStack.Add((Bitmap)bm.Clone());


            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.Paint += Canvas_Paint;

            pen = new Pen(Color.Black,4);            
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            Canvas.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
            
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to save the changes?", "Reminder :)", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FileManager.Save(bm);
            }
        }
           
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            bm = FileManager.ResizeBitmap(bm, Canvas.ClientSize.Width, Canvas.ClientSize.Height);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        }


                    //////Drawing/////
                    //////////////////
        private void Canvas_MouseDown(object sender, MouseEventArgs e)  
        {
            isDown = true;
            x = e.X;
            y = e.Y;
            pDown = e.Location;
            FileManager.NewOldBitmap(bm, OldStack,RedoStack);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDown && x!=-1 && y!=-1 && !flood && !sample)
            {
                using (g = Graphics.FromImage(bm))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawLine(pen, new Point(x, y), e.Location);
                }
            }
            
            x = e.X;
            y = e.Y;
            
            Canvas.Invalidate();
            Canvas.Cursor = Cursors.Cross;
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            pUp = e.Location;

            if (sample)
            {
                pen.Color = bm.GetPixel(e.X, e.Y);
                ColorWheelBox.BackColor = pen.Color;
                
            }
            else if (flood && pen.Color != bm.GetPixel(e.X, e.Y))
            {
                Image.FloodFiller(bm,Canvas, e.Location, bm.GetPixel(e.X, e.Y), pen.Color);
            }
            else if (pDown == pUp && !flood && !sample)
            {
                using (g = Graphics.FromImage(bm))
                {
                    SolidBrush brush = new SolidBrush(pen.Color);   // might adjust, works fine atm
                    g.FillEllipse(brush, x, y, width, height);
                    brush.Dispose();
                }
            }
            isDown = false;
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
            ColorWheelBox.BackColor = p.BackColor;

        }

        private void ColorWheelBox_DoubleClick(object sender, EventArgs e)
        {
            if(colorDialog.ShowDialog()==DialogResult.OK)
            {
                pen.Color = colorDialog.Color;
            }
            ColorWheelBox.BackColor = colorDialog.Color;
        }

        private void SizeBox_TextChanged(object sender, EventArgs e)
        {
            try
            { pen.Width = Convert.ToInt32(SizeBox.Text); }
            catch
            { pen.Width = pen.Width; }
        }

        private void Pic_Small_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            pen.Width = 1;
            width = height = 1;
            SizeBox.Text = 1.ToString();
            flood = false;
            sample = false;
        }

        private void Pic_Medium_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            pen.Width = 4;
            width = height = 4;
            SizeBox.Text = 4.ToString();
            flood = false;
            sample = false;
        }

        private void Pic_Big_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            pen.Width = 7;
            width = height = 7;
            SizeBox.Text = 7.ToString();
            flood = false;
            sample = false;
        }

        private void Pic_Erase_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            pen.Color = System.Drawing.Color.White;
            flood = false;
            sample = false;
        }

        private void Pic_Flood_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            sample = false;
            flood = true;
        }
        private void Pic_Sample_Click(object sender, EventArgs e)
        {
            UI.SelectedTool(sender, ToolSize);
            sample = true;
            flood = false;
            Canvas.Cursor = Cursors.PanNW;
        }

        private void Pic_GrayScale_Click(object sender, EventArgs e)
        {
            FileManager.NewOldBitmap(bm, OldStack, RedoStack);
            UI.ClickOnce(sender);
            bm = Image.GrayScale(bm);
        }
        private void Pic_Negative_Click(object sender, EventArgs e)
        {
            FileManager.NewOldBitmap(bm, OldStack, RedoStack);
            UI.ClickOnce(sender);
            bm = Image.Negative(bm);
        }
        private void Pic_Blue_Click(object sender, EventArgs e)
        {
            FileManager.NewOldBitmap(bm, OldStack, RedoStack);
            UI.ClickOnce(sender);
            bm = Image.Blue(bm);
        }

        ///File ToolBar///
        //////////////////
        private void Pic_New_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);

            if (MessageBox.Show("Do you want to save the changes?", "Reminder :)", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FileManager.Save(bm); // to be fixed
            }
            using (g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
            }
        }

        private void Pic_Save_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
            FileManager.Save(bm); // close enough
        }

        private void Pic_Open_Click(object sender, EventArgs e)
        {
            UI.ClickOnce(sender);
            FileManager.Open(bm,Canvas);
            
        }

        private void Pic_Undo_Click(object sender, EventArgs e)
        {
            FileManager.Undo(OldStack, RedoStack, bm,Canvas);
            UI.ClickOnce(sender);
        }

        private void Pic_Redo_Click(object sender, EventArgs e)
        {
            FileManager.Redo(RedoStack,OldStack,bm,Canvas);
            UI.ClickOnce(sender);
        }

       
    }
}
