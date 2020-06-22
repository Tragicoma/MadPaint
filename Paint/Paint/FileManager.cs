using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Paint
{
    class FileManager
    {
        

        public static void Redo(List<Bitmap> redoStack, List<Bitmap> oldStack, Bitmap bm, PictureBox canvas)
        {
            if (redoStack.Count > 0)
            {
                oldStack.Clear();
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.DrawImage(redoStack[redoStack.Count - 1], 0, 0,bm.Width,bm.Height);
                }

                redoStack.Remove(redoStack[redoStack.Count - 1]);

                canvas.Invalidate();
            }
        }

        public static void Undo(List<Bitmap> oldStack,List<Bitmap> redoStack, Bitmap bm, PictureBox canvas)
        {
            if (oldStack.Count >  0)
            {
                redoStack.Add((Bitmap)bm.Clone());

                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        g.DrawImage(oldStack[oldStack.Count - 1], 0, 0, bm.Width, bm.Height);
                    }
                    oldStack.Remove(oldStack[oldStack.Count - 1]);

                if(redoStack.Count>50)
                {
                    redoStack.RemoveAt(0);
                }
                    canvas.Invalidate();
            }
        }

        public static void NewOldBitmap(Bitmap bm, List<Bitmap> oldStack, List<Bitmap> redoStack)
        {

            Bitmap oldBm = new Bitmap(bm.Width, bm.Height);
            using (Graphics g = Graphics.FromImage(oldBm))
            {
                g.DrawImage(bm, 0, 0, bm.Width,bm.Height);
            }
            oldStack.Add((Bitmap)oldBm.Clone());
            redoStack.Clear();
            if (oldStack.Count > 50)
            {
                oldStack.RemoveAt(0);
            }

        }


        public static void Save(Bitmap bm) 
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.ShowDialog();
            saveDlg.Filter = "PNG Files|*.png";


            if (saveDlg.FileName!="")
            {
                bm.Save(saveDlg.FileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public static void Open(Bitmap bm,PictureBox p) // issues with bitmap size when opening file
        {
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)| *.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if(opnDlg.ShowDialog()==DialogResult.OK)
            {
                Bitmap i = new Bitmap(opnDlg.FileName);
                using(Graphics g = Graphics.FromImage(bm))
                {
                    
                    g.Clear(Color.White);
                    
                    g.DrawImage(i,0,0);
                    
                }
                p.Invalidate();
                i.Dispose();
            }
        }

        public static Bitmap ResizeBitmap(Bitmap bm,int width, int height) // is quality really important here? 
        {
            Bitmap newBitMap = new Bitmap(width, height);
            using(Graphics g = Graphics.FromImage(newBitMap))
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bm, 0, 0, bm.Width, bm.Height);
                
            }
            return newBitMap;
        }
    }
}
