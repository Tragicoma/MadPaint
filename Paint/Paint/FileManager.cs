using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    class FileManager
    {
        public static void Save(Bitmap bm) // needs fixing, not working :(
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.ShowDialog();
            saveDlg.Filter = "PNG Files|*.png";

            if (saveDlg.FileName!="")
            {
                bm.Save(saveDlg.FileName, ImageFormat.Png);
            }
        }

        public static void Open(Bitmap bm,PictureBox p)
        {
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if(opnDlg.ShowDialog()==DialogResult.OK)
            {
                Image i = new Bitmap(opnDlg.FileName,true);
                using(Graphics g = Graphics.FromImage(bm))
                {
                    g.DrawImage(i,0,0,i.Width, i.Height);
                }
                p.Invalidate();
            }
        }
    }
}
