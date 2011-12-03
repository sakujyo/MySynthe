using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace MySynthe
{
    public partial class Form1 : Form
    {
        private int[] wav;
        private const string infilename = @"D:\t\wls.wav";
        private const string outfilename = @"D:\t\out.wav";
        private WindowsMediaPlayer wm;

        public Form1()
        {
            InitializeComponent();
            wm = new WMPLib.WindowsMediaPlayer();       //TODO: wmオブジェクトの再利用やファイルの解放
            wm.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(wm_PlayStateChange);
        }

        void wm_PlayStateChange(int NewState)
        {
            //Console.WriteLine("WMP State: {0}", NewState);
            if (NewState == (int)WMPLib.WMPPlayState.wmppsStopped)
            {
                //wm.launchURL 
                wm.URL = "";
            }
        }

        static byte[] readFile(string filename)
        {
//let readFile filename =
////    let f = new IO.BufferedStream(new IO.FileStream(filename, IO.FileMode.Open, System.IO.FileAccess.Read))
//    let f = new IO.FileStream(filename, IO.FileMode.Open, System.IO.FileAccess.Read)

//    let fileSize = f.Length |> int
//    let buf = Array.create(fileSize) 0uy        // 符号なし 8 ビット自然数?? http://msdn.microsoft.com/ja-jp/library/dd233193.aspx

//    let mutable remain = fileSize;
//    let mutable bufPos = 0;
//    while remain > 0 do
//        let readSize = f.Read(buf, bufPos, System.Math.Min(1024, remain))
//        bufPos <- bufPos + readSize
//        remain <- remain - readSize

//    buf
            var f = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var fileSize = (int)f.Length;
            var buf = new byte[fileSize];
            int bufPos = 0;
            while (fileSize > 0) {
                int readSize = f.Read(buf, bufPos, System.Math.Min(1024, fileSize));
                bufPos += readSize;
                fileSize -= readSize;
            }
            return buf;
        }

        static int[] convert(byte[] buf) {
            var len = (buf.Length - 44) / 2 / 2;

            var la = new int[len];
            var ra = new int[len];
            for (int j = 0; j < len; j++)
			{
                var i = 44 + 4 * j;
                la[j] = ((int)buf[i] + 256 * (int)(buf[i + 1]));
                if (buf[i + 1] < 128) {
                } else {
                    la[j] = la[j] - 65536;
                }
                ra[j] = ((int)buf[i + 2] + 256 * (int)(buf[i + 3]));
                if (buf[i + 3] < 128) {
                } else {
                    ra[j] = ra[j] - 65536;
                }
			}

            return la;      //めんでぇので左チャネルだけ
        }

    //let len = (buf.Length - 44) / 2 / 2     // 2ch, 2byte / sample

    //let la = Array.create (len) 0
    //let ra = Array.create (len) 0
    //for j in 0..len - 1 do
    //    let i = 44 + 4 * j
    //    //注意: signed byte
    //    la.[j] <- int(buf.[i]) + 256 * int(buf.[i + 1])
    //    if buf.[i + 1] < 128uy then () else la.[j] <- la.[j] - 65536
    //    ra.[j] <- int(buf.[i + 2]) + 256 * int(buf.[i + 3])    
    //    if buf.[i + 3] < 128uy then () else ra.[j] <- ra.[j] - 65536

    //let ln = Array.create (len) 0
    //let rn = Array.create (len) 0

    //let ld = Array.create (wsize - 1) 0
    //let rd = Array.create (wsize - 1) 0

    //let lc = Array.concat [ld; la]
    //let rc = Array.concat [rd; ra]

            private void button1_Click(object sender, EventArgs e)
            {
                //const string filename = @"D:\t\レミオロメン - 粉雪.wav";
                var buf = readFile(infilename);
                //for (int i = 0; i < 44; i++)
                //{
                //    Console.WriteLine("{0:x}", buf[i]);
                //}
                var wav = convert(buf);
            }

            private int[] makefwave(double[] f)
            {
                // *5 なら5秒
                int len = 44100 * 5;        // 生成したいサンプル数
                double samplingF = 44100d;  // サンプリング周波数
                var wav = new int[len];

                double phase = 0.0;
                for (int i = 0; i < len; i++)
                {
                    int p = 0;
                    phase += 1 / samplingF * f[i];
                    p += (int)(System.Math.Sin(phase * 2.0 * System.Math.PI) * 16383.0);
                    //p += (int)(System.Math.Sin(i / samplingF * 660.0 * 2.0 * System.Math.PI) * 16383.0);
                    //p += (int)(System.Math.Sin(i / 22.5 * 2.0 * System.Math.PI) * 8191.0);
                    wav[i] = p;
                }
                return wav;
            }

            private int[] makewave()
            {
                int len = 44100;        // 生成したいサンプル数
                double samplingF = 44100d;  // サンプリング周波数
                var wav = new int[len];

                int fint;
                double f;
                if (int.TryParse(textBox1.Text, out fint) == true)
                {
                    f = (double)fint;
                }
                else
                {
                    f = 440.0;  // 440Hz
                }

                for (int i = 0; i < len; i++)
                {
                    int p = 0;
                    p += (int)(System.Math.Sin(i / samplingF * f * 2.0 * System.Math.PI) * 16383.0);
                    p += (int)(System.Math.Sin(i / samplingF * 660.0 * 2.0 * System.Math.PI) * 16383.0);
                    //p += (int)(System.Math.Sin(i / 22.5 * 2.0 * System.Math.PI) * 8191.0);
                    wav[i] = p;
                }
                return wav;
            }

            private void wmplay()
            {
                //1秒分のヘッダを付加してファイルに保存
                var header = new byte[44];
                header[0] = (byte)'R';
                header[1] = (byte)'I';
                header[2] = (byte)'F';
                header[3] = (byte)'F';
                header[4] = 0x34;       // File Size
                header[5] = 0xb1;       // 決めうちのサンプル
                header[6] = 0x02;       // ここをあとでintから変換したものに差し替える
                header[7] = 0x00;
                header[8] = (byte)'W';
                header[9] = (byte)'A';
                header[10] = (byte)'V';
                header[11] = (byte)'E';
                header[12] = 0x66;  //Subchunk 1 ID('fmt ')
                header[13] = 0x6d;
                header[14] = 0x74;
                header[15] = 0x20;
                header[16] = 0x10;  //Subchunk 1 Size
                header[17] = 0x00;
                header[18] = 0x00;
                header[19] = 0x00;
                header[20] = 0x01;  //Audio Format
                header[21] = 0x00;
                header[22] = 0x02;  //Num Channels
                header[23] = 0x00;
                header[24] = 0x44;  //Sample Rate
                header[25] = 0xac;
                header[26] = 0x00;
                header[27] = 0x00;
                header[28] = 0x10;  //Byte Rate
                header[29] = 0xb1;
                header[30] = 0x02;
                header[31] = 0x00;
                header[32] = 0x04;  //Block Align
                header[33] = 0x00;
                header[34] = 0x10;  // Bits per Sample
                header[35] = 0x00;
                header[36] = 0x64;  //'data'
                header[37] = 0x61;
                header[38] = 0x74;
                header[39] = 0x61;
                header[40] = 0x10;  //Wave Data Length
                header[41] = 0xb1;
                header[42] = 0x02;
                header[43] = 0x0;

                var a = wav.Length * 4;
                header[40] = (byte)(a % 0xff); a >>= 8;     //Wave Data Length
                header[41] = (byte)(a % 0xff); a >>= 8;
                header[42] = (byte)(a % 0xff); a >>= 8;
                header[43] = (byte)(a % 0xff);// a >>= 8;
                a = wav.Length * 4 + 44 - 8;
                header[4] = (byte)(a % 0xff); a >>= 8;     //Wave Data Length
                header[5] = (byte)(a % 0xff); a >>= 8;
                header[6] = (byte)(a % 0xff); a >>= 8;
                header[7] = (byte)(a % 0xff);// a >>= 8;
                var f = new System.IO.FileStream(outfilename, System.IO.FileMode.Create,
                    System.IO.FileAccess.Write);
                f.Write(header, 0, header.Length);
                var buf = reconv(wav);
                f.Write(buf, 0, buf.Length);
                f.Close();
                wm.settings.volume = 40;
                wm.URL = outfilename;
                wm.controls.play();
            }

            private void button2_Click(object sender, EventArgs e)
            {
                wmplay();
            }

            private byte[] reconv(int[] wav)
            {
                if (wav == null) wav = makewave();        // TODO: めんどうなので。とりまこうしてる。
                var buf = new Byte[wav.Length * 4];

                for (int i = 0; i < wav.Length; i++)
                {
                    //とりま左右チャネルで同じ波形
                    int j = i * 4;
                    buf[j] = (byte)(wav[i] % 0xff);
                    buf[j + 1] = (byte)(wav[i] / 0xff);
                    buf[j + 2] = (byte)(wav[i] % 0xff);
                    buf[j + 3] = (byte)(wav[i] / 0xff);
                }

                return buf;
            }

            private void button_makewave_Click(object sender, EventArgs e)
            {
                const double baseA = 440.0d;

                //A A# B C C# D D# E F F# G G# A A# B
                //0 1  2 3 4  5 6  7 8 9  1011 12 
                const double notea = 1.0d;                                  //A
                const double noteb = 1.1224620483093729814335330496792d;    //B
                const double notec = 1.1892071150027210667174999705605d;    //C
                const double noted = 1.3348398541700343648308318811845d;    //D
                const double notee = 1.4983070768766814987992807320298d;    //E
                const double notef = 1.5874010519681994747517056392723d;    //F
                const double noteg = 1.781797436280678609480452411181d;     //G
                const double halfnote = 1.0594630943592952645618252949463d;    //半音演算子
                const double noteas = notea * halfnote;

                //double noteg = System.Math.Pow(2.0, 10.0 / 12.0)

                //wav = makewave();
                var f = new double[44100 * 5];

                for (int i = 0; i < f.Length; i++)
                {
                    if (i >= 44100 * 0.2 * 2 && i < 44100 * 0.2 * 4)
                    {
                        //f[i] = 493.88330125612411183075454185884d;  //B
                        f[i] = baseA * notee;  //E
                    }
                    else if (i >= 44100 * 0.2 * 4 && i < 44100 * 0.2 * 10)
                    {
                        f[i] = baseA * noted;  //D
                    }
                    else if (i >= 44100 * 0.2 * 10 && i < 44100 * 0.2 * 11)
                    {
                        f[i] = baseA * notec;  //c
                    }
                    else if (i >= 44100 * 0.2 * 11 && i < 44100 * 0.2 * 12)
                    {
                        f[i] = baseA * noteb;  //b
                    }
                    else if (i >= 44100 * 0.2 * 12 && i < 44100 * 0.2 * 14)
                    {
                        f[i] = baseA * notea;  //a
                    }
                    else if (i >= 44100 * 0.2 * 14 && i < 44100 * 0.2 * 15)
                    {
                        f[i] = baseA * noteg / 2;  //g
                    }
                    else if (i >= 44100 * 0.2 * 15 && i < 44100 * 0.2 * 16)
                    {
                        f[i] = baseA * notef / 2;  //f
                    }
                    else if (i >= 44100 * 0.2 * 16 && i < 44100 * 0.2 * 17)
                    {
                        f[i] = baseA * noteg / 2;  //g
                    }
                    else if (i >= 44100 * 0.2 * 17 && i < 44100 * 0.2 * 18)
                    {
                        f[i] = baseA * notee / 2;  //e
                    }
                    else if (i >= 44100 * 0.2 * 18 && i < 44100 * 0.2 * 20)
                    {
                        f[i] = baseA * noteb;  //b
                    }
                    else if (i >= 44100 * 0.2 * 20 && i < 44100 * 0.2 * 22)
                    {
                        f[i] = baseA * notea;  //a
                    }
                    else
                    {
                        f[i] = 440.0d;  //A
                    }
                }
                var wav1 = makefwave(f);
                //wmplay();

                for (int i = 0; i < f.Length; i++)
                {
                    if (i >= 44100 * 0.2 * 0 && i < 44100 * 0.2 * 2)
                    {
                        f[i] = baseA * notee;  //E
                    }
                    else if (i >= 44100 * 0.2 * 2 && i < 44100 * 0.2 * 4)
                    {
                        f[i] = baseA * noteg;  //g
                    }
                    else if (i >= 44100 * 0.2 * 4 && i < 44100 * 0.2 * 10)
                    {
                        f[i] = baseA * notef * noteas;  //F#
                    }
                    else if (i >= 44100 * 0.2 * 10 && i < 44100 * 0.2 * 11)
                    {
                        f[i] = baseA * noted;   //D
                    }
                    else if (i >= 44100 * 0.2 * 11 && i < 44100 * 0.2 * 12)
                    {
                        f[i] = baseA * notec;   //C
                    }
                    else if (i >= 44100 * 0.2 * 12 && i < 44100 * 0.2 * 14)
                    {
                        f[i] = baseA * noteb;   //b
                    }
                    else if (i >= 44100 * 0.2 * 14 && i < 44100 * 0.2 * 15)
                    {
                        f[i] = baseA * notec;   //C
                    }
                    else if (i >= 44100 * 0.2 * 15 && i < 44100 * 0.2 * 16)
                    {
                        f[i] = baseA * noted;   //d
                    }
                    else
                    {
                        f[i] = baseA * notee;  //e
                    }
                }

                var wav2 = makefwave(f);

                //http://dobon.net/vb/dotnet/programing/arraymerge.html#section3
                wav = wav1.Concat(wav2).ToArray();
                wmplay();
            }

            private void button_showwavedata_Click(object sender, EventArgs e)
            {
                var wf = new WaveForm();
                wf.setWavL(wav);
                //wf.Visible = true;
                //ShowDialog(wf);
                wf.ShowDialog(this);
            }
    }
}
