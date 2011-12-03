using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySynthe
{
    public partial class WaveForm : Form
    {
        private int[] wavl;
        private int[] wavr;
        private int dx;
        private int dy;
        private int scale = 256;
        private Bitmap b = new Bitmap(640, 480);
        private Graphics g;
        private int location = 0;   // WAVの先頭からの位置
        //private int pos = 0;

        public void setWavL(int[] wav)
        {
            wavl = wav;
            draw();
        }

        public void draw()
        {
            var halfHeight = pictureBox1.Height / 2;
            g.Clear(Color.White);
            int i = location;
            //while (wavl[i] == 0)
            //{
            //    i++;
            //    if (i > wavl.Length) return;
            //}

            //var y = wavl[i];
            g.DrawLine(Pens.Black, 0, halfHeight, 640, halfHeight);
            var y = wavl[i] / scale;
            //g.DrawLine(Pens.PaleVioletRed, 0, y + 240, 0, y + 240);
            g.DrawRectangle(Pens.PaleVioletRed, 0, y + 240, 1, 1);
            //b.SetPixel(0, y, Color.OrangeRed);
            for (int x = 0; x < 639; x++)
            {
                if (i + 1 > wavl.Length) return;
                
                //var nexty = wavl[i + 1];

                var nexty = wavl[i + 1] / scale;
                //g.DrawLine(Pens.Aquamarine, x, y + 240, x + 1, nexty + 240);
                //b.SetPixel(x, nexty, Color.OrangeRed);
                //g.DrawLine(Pens.PaleVioletRed, x, nexty + 240, x + 1, nexty + 240);
                g.DrawRectangle(Pens.PaleVioletRed, x, nexty + halfHeight, 1, 1);
                i++;
            }
            g.DrawLine(Pens.Black, 0, halfHeight, 640, halfHeight);
            pictureBox1.Image = b;
        }

        public WaveForm()
        {
            InitializeComponent();
            g = Graphics.FromImage(b);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            MouseWheel += new System.Windows.Forms.MouseEventHandler(this.WaveForm_MouseWheel);
            //pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.WaveForm_MouseWheel);
            //this.Width -= pictureBox1.Width - 640;
            //this.Height -= pictureBox1.Height - hScrollBar1.Height - 480; //Fillの場合
        }

        private void WaveForm_MouseWheel(object sender, MouseEventArgs e)
        {
            scale -= (e.Delta / 120);
            if (scale < 1) scale = 1;
            //Console.WriteLine(e.Delta);
            draw();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                location -= e.X - dx;
                if (location < 0)
                {
                    location = 0;
                }
                //Console.WriteLine(e.X);
                dx = e.X;
                dy = e.Y;
                draw();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dx = e.X;
            dy = e.Y;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //var et = e.Type;
            //Console.WriteLine(et);  //ThumbTrack, LargeSmallIncrementDecrement, EndScroll
            //var env = e.NewValue;
            //Console.WriteLine(env);
            if (e.Type == ScrollEventType.EndScroll)
            {
                var rpos = e.NewValue / hScrollBar1.Maximum; // 値をみてみたいだけで使う気なし
                //pos = e.NewValue;
                location = (wavl.Length - pictureBox1.Width) * e.NewValue / hScrollBar1.Maximum;
                draw();
            }
        }
    }
}
