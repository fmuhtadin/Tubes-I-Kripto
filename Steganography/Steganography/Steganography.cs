using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using AviFile;

namespace WindowsFormsApplication1
{
    public partial class Steganography : Form
    {

        private int framesize;
        private string extension;
        private List<double> rms = new List<double>();

        public Steganography()
        {
            InitializeComponent();
        }

        private void btnBrowseCover_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Avi Files (*.avi)|*.avi";
            dialog.ShowDialog();
            txtCover.Text = dialog.FileName;
        }

        private void btnBrowseHidden_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All types (*.*)|*.*";
            dialog.ShowDialog();
            txtHidden.Text = dialog.FileName;
        }

        private String getExtension(String text)
        {
            int last = text.LastIndexOf(".");
            return text.Substring(last + 1);
        }

        private byte SetBit(byte b, int position, bool newBitValue)
        {
            byte mask = (byte)(1 << position);
            if (newBitValue)
            {
                return (byte)(b | mask);
            }
            else
            {
                return (byte)(b & ~mask);
            }
        }

        private int getKey(String input)
        {
            int hasil = 0;
            char[] temporary = input.ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                int rem = i % 3;
                if (rem == 0)
                {
                    hasil -= (int)(temporary[i]);
                }
                else if (rem == 1)
                {
                    hasil *= (int)(temporary[i]);
                }
                else
                {
                    hasil += (int)(temporary[i]);
                }
            }
            return hasil;
        }

        private bool GetBit(byte b, int position)
        {
            return ((b & (byte)(1 << position)) != 0);
        }

        private double calculatePSNR(Bitmap bmp1, Bitmap bmp2)
        {
            double sum = 0;
            for (int i = 0; i < bmp1.Width; i++)
            {
                for (int j = 0; j < bmp1.Height; j++)
                {
                    sum += Math.Pow(calculatePixelDiff(bmp1.GetPixel(i, j), bmp2.GetPixel(i, j)),2);
                }
            }
             return Math.Sqrt(sum / (bmp1.Height * bmp1.Width));
        }

        private int calculatePixelDiff(Color color1, Color color2)
        {
            Byte[] byteRes = new Byte[4];

            BitArray Ra = new BitArray(new byte[] { color1.R });
            BitArray Ga = new BitArray(new byte[] { color1.G });
            BitArray Ba = new BitArray(new byte[] { color1.B });
            BitArray Aa = new BitArray(new byte[] { color1.A });

            BitArray Rb = new BitArray(new byte[] { color2.R });
            BitArray Gb = new BitArray(new byte[] { color2.G });
            BitArray Bb = new BitArray(new byte[] { color2.B });
            BitArray Ab = new BitArray(new byte[] { color2.A });

            BitArray Rx = Ra.Xor(Rb);
            BitArray Gx = Ga.Xor(Gb);
            BitArray Bx = Ba.Xor(Bb);
            BitArray Ax = Aa.Xor(Ab);
            int res = 0;
            for (int i=0; i<8; i++)
            {
                byteRes[0] = SetBit(byteRes[0], i, Rx.Get(i));
            }
            res += BitConverter.ToInt32(byteRes, 0);
            for (int i = 0; i < 8; i++)
            {
                byteRes[0] = SetBit(byteRes[0], i, Gx.Get(i));
            }
            res += BitConverter.ToInt32(byteRes, 0);
            for (int i = 0; i < 8; i++)
            {
                byteRes[0] = SetBit(byteRes[0], i, Bx.Get(i));
            }
            res += BitConverter.ToInt32(byteRes, 0);
            for (int i = 0; i < 8; i++)
            {
                byteRes[0] = SetBit(byteRes[0], i, Ax.Get(i));
            }
            res += BitConverter.ToInt32(byteRes, 0);
            //int res = BitConverter.ToInt32(byteRes, 0);
            return res;
        }

        public Bitmap HideByteIntoBitmap2bit(byte[] myArray, string key, Bitmap bmp)
        {
            int fileSize = myArray.Length;

            int bytes = bmp.Width * bmp.Height;
            byte[] extension = System.Text.Encoding.Default.GetBytes(getExtension(txtHidden.Text));
            byte[] size = BitConverter.GetBytes(fileSize);
            Bitmap bmpCompare = (Bitmap)bmp.Clone();


            if (myArray.Length <= (6 * bytes) / 8 - 7)
            {
                Color pixelColor;
                byte r, g, b;

                BitArray bits = new BitArray(myArray);
                BitArray sizeBits = new BitArray(size);
                BitArray extBits = new BitArray(extension);

                int maxIdx = ((bits.Count + 1) / 6);

                int[] idx = new int[bytes];

                int choose, maxSize = bytes, a;

                for (int ctr = 0; ctr < bytes; ctr++)
                {
                    idx[ctr] = ctr;
                }

                Random randomer = new Random(getKey(txtKey.Text));

                for (int i = 0; i < 5; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    r = SetBit(r, 0, sizeBits.Get(6 * i));
                    g = SetBit(g, 0, sizeBits.Get(6 * i + 1));
                    b = SetBit(b, 0, sizeBits.Get(6 * i + 2));
                    r = SetBit(r, 1, sizeBits.Get(6 * i + 3));
                    g = SetBit(g, 1, sizeBits.Get(6 * i + 4));
                    b = SetBit(b, 1, sizeBits.Get(6 * i + 5));
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }

                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                r = SetBit(r, 0, sizeBits.Get(30));
                g = SetBit(g, 0, sizeBits.Get(31));
                b = SetBit(b, 0, extBits.Get(0));
                r = SetBit(r, 1, extBits.Get(1));
                g = SetBit(g, 1, extBits.Get(2));
                b = SetBit(b, 1, extBits.Get(3));
                bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));

                for (int i = 0; i < 3; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    r = SetBit(r, 0, extBits.Get(6 * i + 4));
                    g = SetBit(g, 0, extBits.Get(6 * i + 5));
                    b = SetBit(b, 0, extBits.Get(6 * i + 6));
                    r = SetBit(r, 1, extBits.Get(6 * i + 7));
                    g = SetBit(g, 1, extBits.Get(6 * i + 8));
                    b = SetBit(b, 1, extBits.Get(6 * i + 9));
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }

                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                r = SetBit(r, 0, extBits.Get(22));
                g = SetBit(g, 0, extBits.Get(23));
                b = SetBit(b, 0, bits.Get(0));
                r = SetBit(r, 1, bits.Get(1));
                g = SetBit(g, 1, bits.Get(2));
                b = SetBit(b, 1, bits.Get(3));
                bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));

                for (int i = 0; i < maxIdx; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    if (6 * i + 4 < bits.Count)
                    {
                        r = SetBit(r, 0, bits.Get(6 * i + 4));
                    }
                    if (6 * i + 5 < bits.Count)
                    {
                        g = SetBit(g, 0, bits.Get(6 * i + 5));
                    }
                    if (6 * i + 6 < bits.Count)
                    {
                        b = SetBit(b, 0, bits.Get(6 * i + 6));
                    }
                    if (6 * i + 7 < bits.Count)
                    {
                        r = SetBit(r, 1, bits.Get(6 * i + 7));
                    }
                    if (6 * i + 8 < bits.Count)
                    {
                        g = SetBit(g, 1, bits.Get(6 * i + 8));
                    }
                    if (6 * i + 9 < bits.Count)
                    {
                        b = SetBit(b, 1, bits.Get(6 * i + 9));
                    }
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }
            }
            rms.Add(calculatePSNR(bmp,bmpCompare));
            return bmp;
        }


        public Bitmap HideByteIntoBitmap(byte[] myArray, string key, Bitmap bmp)
        {
            int fileSize = myArray.Length;
            int bytes = bmp.Width * bmp.Height;
            byte[] extension = System.Text.Encoding.Default.GetBytes(getExtension(txtHidden.Text));
            byte[] size = BitConverter.GetBytes(fileSize);
            Bitmap bmpCompare = (Bitmap)bmp.Clone();

            if (myArray.Length <= (3 * bytes) / 8 - 7)
            {
                Color pixelColor;
                byte r, g, b;

                BitArray bits = new BitArray(myArray);
                BitArray sizeBits = new BitArray(size);
                BitArray extBits = new BitArray(extension);

                int maxIdx = ((bits.Count + 1) / 3);

                int[] idx = new int[bytes];

                int choose, maxSize = bytes, a;

                for (int ctr = 0; ctr < bytes; ctr++)
                {
                    idx[ctr] = ctr;
                }

                Random randomer = new Random(getKey(txtKey.Text));

                for (int i = 0; i < 10; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    r = SetBit(r, 0, sizeBits.Get(3 * i));
                    g = SetBit(g, 0, sizeBits.Get(3 * i + 1));
                    b = SetBit(b, 0, sizeBits.Get(3 * i + 2));
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }

                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                r = SetBit(r, 0, sizeBits.Get(30));
                g = SetBit(g, 0, sizeBits.Get(31));
                b = SetBit(b, 0, extBits.Get(0));
                bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));

                for (int i = 0; i < 7; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    r = SetBit(r, 0, extBits.Get(3 * i + 1));
                    g = SetBit(g, 0, extBits.Get(3 * i + 2));
                    b = SetBit(b, 0, extBits.Get(3 * i + 3));
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }

                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                r = SetBit(r, 0, extBits.Get(22));
                g = SetBit(g, 0, extBits.Get(23));
                b = SetBit(b, 0, bits.Get(0));
                bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));

                for (int i = 0; i < maxIdx; i++)
                {
                    a = randomer.Next(maxSize);
                    choose = idx[a];
                    idx[a] = idx[--maxSize];

                    pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;

                    if (3 * i + 1 < bits.Count)
                    {
                        r = SetBit(r, 0, bits.Get(3 * i + 1));
                    }
                    if (3 * i + 2 < bits.Count)
                    {
                        g = SetBit(g, 0, bits.Get(3 * i + 2));
                    }
                    if (3 * i + 3 < bits.Count)
                    {
                        b = SetBit(b, 0, bits.Get(3 * i + 3));
                    }
                    bmp.SetPixel(choose % bmp.Width, choose / bmp.Width, Color.FromArgb(r, g, b));
                }
                rms.Add(calculatePSNR(bmp,bmpCompare));
                return bmp;
            }
            else
            {
                MessageBox.Show("Byte Size is not valid");
                return bmp;
            }
        }

        private Byte[] ExtractByteFromBitmap2bit(Bitmap bmp)
        {

            int tripbytes = bmp.Width * bmp.Height;

            Random randomer = new Random(getKey(txtKey.Text));

            int choose, maxSize = tripbytes;

            int[] idx = new int[tripbytes];

            for (int ctr = 0; ctr < tripbytes; ctr++)
            {
                idx[ctr] = ctr;
            }


            byte[] sizeByte = new byte[4];
            byte[] extByte = new byte[3];

            int counter = 0, temp, byteIndex = 0, a;

            byte aByte = (byte)0;

            Color pixelColor;

            byte r, g, b;

            for (int i = 0; i < 5; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                } temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 1));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 1));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 1));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
            }

            a = randomer.Next(maxSize);
            choose = idx[a];
            idx[a] = idx[--maxSize];

            pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

            r = pixelColor.R;
            g = pixelColor.G;
            b = pixelColor.B;

            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(r, 0));
            counter++;
            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(g, 0));
            sizeByte[3] = aByte;

            aByte = SetBit(aByte, 0, GetBit(b, 0));
            aByte = SetBit(aByte, 1, GetBit(r, 1));
            aByte = SetBit(aByte, 2, GetBit(g, 1));
            aByte = SetBit(aByte, 3, GetBit(b, 1));

            int maxIdx = BitConverter.ToInt32(sizeByte, 0);

            byte[] extractedByte = new byte[maxIdx];

            int maxBits = 8 * maxIdx;

            maxIdx = (maxBits + 1) / 6;

            byteIndex = 0;
            counter = 4;

            for (int i = 0; i < 3; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 1));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 1));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 1));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
            }

            a = randomer.Next(maxSize);
            choose = idx[a];
            idx[a] = idx[--maxSize];

            pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

            r = pixelColor.R;
            g = pixelColor.G;
            b = pixelColor.B;

            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(r, 0));
            counter++;
            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(g, 0));
            extByte[2] = aByte;
            aByte = SetBit(aByte, 0, GetBit(b, 0));
            aByte = SetBit(aByte, 1, GetBit(r, 1));
            aByte = SetBit(aByte, 2, GetBit(g, 1));
            aByte = SetBit(aByte, 3, GetBit(b, 1));

            String ext = System.Text.Encoding.Default.GetString(extByte);
            extension = ext;

            byteIndex = 0;
            counter = 4;

            for (int i = 0; i < maxIdx; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                if (6 * i + 4 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(r, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (6 * i + 5 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(g, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (6 * i + 6 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(b, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (6 * i + 7 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(r, 1));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (6 * i + 8 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(g, 1));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (6 * i + 9 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(b, 1));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
            }
            return extractedByte;
        }

        private Byte[] ExtractByteFromBitmap(Bitmap bmp)
        {
            int tripbytes = bmp.Width * bmp.Height;

            Random randomer = new Random(getKey(txtKey.Text));

            int choose, maxSize = tripbytes;

            int[] idx = new int[tripbytes];

            for (int ctr = 0; ctr < tripbytes; ctr++)
            {
                idx[ctr] = ctr;
            }


            byte[] sizeByte = new byte[4];
            byte[] extByte = new byte[3];

            int counter = 0, temp, byteIndex = 0, a;

            byte aByte = (byte)0;

            Color pixelColor;

            byte r, g, b;

            for (int i = 0; i < 10; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 0));
                counter++;
                if (temp == 7)
                {
                    sizeByte[byteIndex] = aByte;
                    byteIndex++;
                }
            }

            a = randomer.Next(maxSize);
            choose = idx[a];
            idx[a] = idx[--maxSize];

            pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

            r = pixelColor.R;
            g = pixelColor.G;
            b = pixelColor.B;

            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(r, 0));
            counter++;
            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(g, 0));
            sizeByte[3] = aByte;

            aByte = SetBit(aByte, 0, GetBit(b, 0));

            int maxIdx = BitConverter.ToInt32(sizeByte, 0);

            byte[] extractedByte = new byte[maxIdx];

            int maxBits = 8 * maxIdx;

            maxIdx = (maxBits + 1) / 3;

            byteIndex = 0;
            counter = 1;

            for (int i = 0; i < 7; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(r, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(g, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
                temp = counter % 8;
                aByte = SetBit(aByte, temp, GetBit(b, 0));
                counter++;
                if (temp == 7)
                {
                    extByte[byteIndex] = aByte;
                    byteIndex++;
                }
            }

            a = randomer.Next(maxSize);
            choose = idx[a];
            idx[a] = idx[--maxSize];

            pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

            r = pixelColor.R;
            g = pixelColor.G;
            b = pixelColor.B;

            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(r, 0));
            counter++;
            temp = counter % 8;
            aByte = SetBit(aByte, temp, GetBit(g, 0));
            extByte[2] = aByte;
            temp = 0;
            aByte = SetBit(aByte, temp, GetBit(b, 0));

            String ext = System.Text.Encoding.Default.GetString(extByte);
            extension = ext;

            byteIndex = 0;
            counter = 1;

            for (int i = 0; i < maxIdx; i++)
            {
                a = randomer.Next(maxSize);
                choose = idx[a];
                idx[a] = idx[--maxSize];

                pixelColor = bmp.GetPixel(choose % bmp.Width, choose / bmp.Width);

                r = pixelColor.R;
                g = pixelColor.G;
                b = pixelColor.B;

                if (3 * i + 1 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(r, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (3 * i + 2 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(g, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
                if (3 * i + 3 < maxBits)
                {
                    temp = counter % 8;
                    aByte = SetBit(aByte, temp, GetBit(b, 0));
                    counter++;
                    if (temp == 7)
                    {
                        extractedByte[byteIndex] = aByte;
                        byteIndex++;
                    }
                }
            }
            return extractedByte;
        }

        private Byte[] MergeMessageBytes(List<Byte[]> arr)
        {
            int numBytes = 0;
            foreach (Byte[] b in arr)
            {
                numBytes += b.Length;
            }
            Byte[] resultBytes = new Byte[numBytes];
            int i = 0;
            foreach (Byte[] b in arr)
            {
                foreach (Byte a in b)
                {
                    resultBytes[i] = a;
                    i++;
                }
            }
            return resultBytes;
        }

        private List<Byte[]> SplitMessageBytes(Byte[] arr)
        {
            List<Byte[]> listbyte = new List<Byte[]>();
            int i;
            for (i = 0; i + framesize < arr.Length; i += 0)
            {
                byte[] tempbyte = new byte[framesize];
                for (int k=0; k<framesize; k++) 
                {
                    tempbyte[k] = arr[i];
                    i++;
                }
                listbyte.Add(tempbyte);
            }
            byte[] tempbyte2 = new byte[arr.Length-i];
            for (int k = 0; k < tempbyte2.Length; k++)
            {
                tempbyte2[k] = arr[i];
                i++;
            }
            listbyte.Add(tempbyte2);
            return listbyte;
        }

        private List<Bitmap> GetBitmapStreamFromAvi()
        {
            List<Bitmap> listbmp = new List<Bitmap>();
            AviManager aviManager = new AviManager(txtCover.Text, true);
            VideoStream aviStream = aviManager.GetVideoStream();
            framesize = aviStream.FrameSize;
            aviStream.GetFrameOpen();
            for (int i=0; i<aviStream.CountFrames; i++)
            {
                listbmp.Add(aviStream.GetBitmap(i));
            }
            aviManager.Close();
            return listbmp;
        }

        private void SaveBitmapStreamToAvi(List<Bitmap> listbmp)
        {
            AviManager aviManager = new AviManager(txtCover.Text, true);
            //AudioStream resultAudioStream = aviManager.GetWaveStream();

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Videos (*.avi)|*.avi;";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AviManager managerSave = new AviManager(dlg.FileName, false);
                VideoStream resultVideoStream = managerSave.AddVideoStream(false, aviManager.GetVideoStream().FrameRate, listbmp[0]);
                listbmp.RemoveAt(0);
                while (listbmp.Count != 0)
                {
                    resultVideoStream.AddFrame(listbmp[0]);
                    listbmp.RemoveAt(0);
                }

                //managerSave.AddAudioStream(resultAudioStream, 0);

                managerSave.Close();
            }
            aviManager.Close();
        }

        public byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }

        private void btnDecode_Click2(object sender, EventArgs e)
        {
            try
            {
                List<Bitmap> arraybmp = new List<Bitmap>();
                arraybmp = GetBitmapStreamFromAvi();

                Byte[] resultMessage;
                List<Byte[]> tempByteArr = new List<byte[]>();

                Byte[] tempByte;
                if (radio1Bit.Checked)
                    tempByte = ExtractByteFromBitmap(arraybmp[0]);
                else
                    tempByte = ExtractByteFromBitmap2bit(arraybmp[0]);
                if (radioEncrypt.Checked)
                    if (txtKey.Text == string.Empty)
                        throw new Exception("Key must not be null");
                    else
                        decrypt(ref tempByte, txtKey.Text);
                int numFramesToDecode = BitConverter.ToInt32(tempByte, 0);
                arraybmp.RemoveAt(0);

                for (int i = 0; i < numFramesToDecode; i++)
                {
                    if (radio1Bit.Checked)
                        tempByte = ExtractByteFromBitmap(arraybmp[0]);
                    else
                        tempByte = ExtractByteFromBitmap2bit(arraybmp[0]);
                    if (radioEncrypt.Checked)
                        if (txtKey.Text == string.Empty)
                            throw new Exception("Key must not be null");
                        else
                            decrypt(ref tempByte, txtKey.Text);
                    arraybmp.RemoveAt(0);
                    tempByteArr.Add(tempByte);
                }

                resultMessage = MergeMessageBytes(tempByteArr);

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = extension + " file|*." + extension;
                dialog.Title = "Save extracted file as";
                dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    FileStream abc = File.OpenWrite(dialog.FileName);
                    for (int i = 0; i < resultMessage.Count<byte>(); i++)
                    {
                        abc.WriteByte(resultMessage[i]);
                    }
                    abc.Dispose();
                    slNotification.Text = "Your message has been extracted successfully";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEncode_Click2(object sender, EventArgs e)
        {
            try
            {
                List<Bitmap> arraybmp = new List<Bitmap>();
                arraybmp = GetBitmapStreamFromAvi();

                List<byte[]> arrayBytes = new List<byte[]>();
                arrayBytes = SplitMessageBytes(GetBytesFromFile(txtHidden.Text));
                
                List<Bitmap> arraybmpresult = new List<Bitmap>();
                Bitmap tempbmp;

                if (arrayBytes.Count+1 > arraybmp.Count)
                    throw new Exception("File Message tidak muat untuk cover");

                Byte[] numFrames = BitConverter.GetBytes(arrayBytes.Count);
                if (radioEncrypt.Checked)
                    if (txtKey.Text == string.Empty)
                        throw new Exception("Key must not be null");
                    else
                        encrypt(ref numFrames, txtKey.Text);
                if (radio1Bit.Checked)
                    tempbmp = HideByteIntoBitmap(numFrames, txtKey.Text, arraybmp[0]);
                else
                    tempbmp = HideByteIntoBitmap2bit(numFrames, txtKey.Text, arraybmp[0]);
                arraybmpresult.Add(tempbmp);
                arraybmp.RemoveAt(0);

                while (arrayBytes.Count != 0)
                {
                    Byte[] temp = arrayBytes[0];
                    if (radioEncrypt.Checked)
                        if (txtKey.Text == string.Empty)
                            throw new Exception("Key must not be null");
                        else
                            encrypt(ref temp, txtKey.Text);
                    if (radio1Bit.Checked)
                        tempbmp = HideByteIntoBitmap(temp, txtKey.Text, arraybmp[0]);
                    else
                        tempbmp = HideByteIntoBitmap2bit(temp, txtKey.Text, arraybmp[0]);
                    arraybmpresult.Add(tempbmp);
                    arrayBytes.RemoveAt(0);
                    arraybmp.RemoveAt(0);
                }
                while (arraybmp.Count != 0)
                {
                    arraybmpresult.Add(arraybmp[0]);
                    arraybmp.RemoveAt(0);
                }
                SaveBitmapStreamToAvi(arraybmpresult);
                
                List<double> PSNR = new List<double>();
                foreach (double d in rms)
                {
                    PSNR.Add(20*Math.Log10(256/d));
                }
                double PSNRsum = 0;
                foreach (double d in PSNR)
                {
                    PSNRsum += d;
                }
                double PSNRavg =  PSNRsum/PSNR.Count;
                rtxtPSNR.Text = PSNRavg.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void encrypt(ref byte[] stream, String key)
        {
            int size = stream.Count<byte>();
            byte[] keyBytes = System.Text.Encoding.Default.GetBytes(key);
            int idx = 0;
            int keyLength = keyBytes.Count<byte>();
            for (int i = 0; i < size; i++)
            {
                int value = (int)stream[i];
                value += (int)keyBytes[idx];
                value %= 256;
                idx = (idx + 1) % keyLength;
                stream[i] = (byte)value;
            }
        }

        public void decrypt(ref byte[] stream, String key)
        {
            int size = stream.Count<byte>();
            byte[] keyBytes = System.Text.Encoding.Default.GetBytes(key);
            int idx = 0;
            int keyLength = keyBytes.Count<byte>();
            for (int i = 0; i < size; i++)
            {
                int value = (int)stream[i];
                value -= (int)keyBytes[idx];
                if (value < 0)
                {
                    value += 256;
                }
                idx = (idx + 1) % keyLength;
                stream[i] = (byte)value;
            }
        }

        private void btnLoadOri_Click(object sender, EventArgs e)
        {
            // Set the URL property to the file path obtained from the text box. 
            axWindowsMediaPlayer1.URL = txtCover.Text;

            // Play the media file. 
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void btnLoadRes_Click(object sender, EventArgs e)
        {
            // Set the URL property to the file path obtained from the text box. 
            axWindowsMediaPlayer1.URL = txtCover.Text;

            // Play the media file. 
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCover.Text = "";
            txtHidden.Text = "";
            txtKey.Text = "";
            slNotification.Text = "";
            rtxtPSNR.Text = "";
        }
    }
}
