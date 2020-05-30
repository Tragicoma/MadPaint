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

namespace Paint
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen;
        int x = -1;
        int y = -1;
        bool drawing = false;
        public Form1()
        {
            InitializeComponent();
            g = canvas.CreateGraphics();
            pen = new Pen(Color.Black,4);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            ClearBoxes(colorPanel);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Color = p.BackColor;

        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            x = e.X;
            y = e.Y;
            

        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(drawing && x!=-1 && y!=-1)
            {
                g.DrawLine(pen, new Point(x, y),e.Location);
                x = e.X;
                y = e.Y;
            }
            canvas.Cursor = Cursors.Cross;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
            x = -1;
            y = - 1;
            
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            ClearBoxes(toolPanel);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 1;
            

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            ClearBoxes(toolPanel);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 4;
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            ClearBoxes(toolPanel);
            p.BorderStyle = BorderStyle.Fixed3D;
            pen.Width = 7;
        }

        public void ClearBoxes(FlowLayoutPanel panel)
        {
            foreach(PictureBox pb in panel.Controls)
            {
                pb.BorderStyle = BorderStyle.FixedSingle;
            }
        }
    }
}
