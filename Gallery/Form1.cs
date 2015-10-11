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
using System.IO;

namespace Gallery
{
    public partial class Form1 : Form
    {
        List<Bitmap> bMaps = new List<Bitmap>();
        List<TextureBrush> tBrushes = new List<TextureBrush>();
        Graphics graphics;
        List<FileSystemInfo> fSI;

        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                DirectoryInfo dI = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                //MessageBox.Show(path);
                fSI = new List<FileSystemInfo>();
                fSI.AddRange(dI.GetFiles("*.png"));
                fSI.AddRange(dI.GetFiles("*.jpg"));
                fSI.AddRange(dI.GetFiles("*.jpeg"));
                fSI.AddRange(dI.GetFiles("*.bmp"));

                graphics = panel1.CreateGraphics();
                Bitmap bmpTmp;
                float index;

                for (int i = 0; i < fSI.Count; i++)
                {
                    bmpTmp = new Bitmap(Image.FromFile(fSI[i].FullName));
                    index = (float)bmpTmp.Size.Height / (float)bmpTmp.Size.Width;
                    //index = (float)1 /(float)2;
                    if (index >= 1)
                        bMaps.Add(new Bitmap(bmpTmp, ((int)(80 / index)), 80));
                    else
                        bMaps.Add(new Bitmap(bmpTmp, 80, ((int)(80 * index))));
                    //panel1.BackgroundImage = bMaps[i];
                }
                for (int i = 0; i < bMaps.Count; i++)
                    tBrushes.Add(new TextureBrush(bMaps[i], WrapMode.Clamp));
                panel1.Width = 80 * tBrushes.Count + 5 * (tBrushes.Count + 1);
                //hScrollBar1.Maximum = panel1.Width / this.Width;
                hScrollBar1.Maximum = panel1.Width - this.Width + 30;
                //HScrollProperties hSP = panel1.HorizontalScroll;
                for (int i = 0; i < tBrushes.Count; i++)
                    graphics.FillRectangle(tBrushes[i], new Rectangle(i * 80 + 5 * (i + 1), 0, 80, 80));

                //MessageBox.Show(fSI[0].FullName.ToString());
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = e.X / 85;
            if (!(e.X % 85 >= 0 && e.X % 85 <= 5))
            {

                float ind;
                Bitmap bmp2;
                Bitmap bmp = new Bitmap(Image.FromFile(fSI[index].FullName));
                ind = (float)bmp.Size.Height / (float)bmp.Size.Width;
                if (ind >= 1)
                    bmp2 = new Bitmap(bmp, 480 * bmp.Width / bmp.Height, 480);
                else
                    bmp2 = new Bitmap(bmp, 756, 756 * bmp.Height / bmp.Width);
                TextureBrush tB = new TextureBrush(bmp2, WrapMode.Clamp);
                Graphics gr2 = panel2.CreateGraphics();
                gr2.Clear(panel2.BackColor);
                gr2.FillRectangle(tB, 0, 0, panel2.Width, panel2.Height); 
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //MessageBox.Show(hScrollBar1.Value.ToString());
            panel1.Location = new Point(-1 * hScrollBar1.Value , panel1.Location.Y);
            for (int i = 0; i < tBrushes.Count; i++)
                graphics.FillRectangle(tBrushes[i], new Rectangle(i * 80 + 5 * (i + 1), 0, 80, 80));
        }
    }
}
