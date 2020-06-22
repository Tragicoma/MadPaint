using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    class UI
    {

        public static void SelectedTool(object sender,FlowLayoutPanel panel)
        {
            PictureBox p = (PictureBox)sender;
            ClearBoxes(panel);
            p.BorderStyle = BorderStyle.Fixed3D;
        }

        public static void ClickOnce(object sender)
        {
            PictureBox p = (PictureBox)sender;
            p.BorderStyle = BorderStyle.Fixed3D;
            Thread.Sleep(100);
            p.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void ClearBoxes(FlowLayoutPanel panel) // clears "clicked" boxes from a panel
        {
            foreach (PictureBox pb in panel.Controls)
            {
                pb.BorderStyle = BorderStyle.FixedSingle;
            }
        }

    }
}
