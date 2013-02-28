using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private String getExtension(String text)
        {
            int last = text.LastIndexOf(".");
            return text.Substring(last + 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream toHide = File.OpenRead(textBox2.Text);

                byte[] myArray = new byte[toHide.Length];
                int fileSize = (int) toHide.Length;

                Bitmap bmp = new Bitmap(textBox1.Text);

                int bytes = bmp.Width * bmp.Height;
                byte[] extension = System.Text.Encoding.Default.GetBytes(getExtension(textBox2.Text));
                byte[] size = BitConverter.GetBytes(fileSize);

                for (int i = 0; i < fileSize; i++)
                {
                    myArray[i] = (byte) toHide.ReadByte();
                }

                if (myArray.Length <= (3 * bytes) / 8 - 7)
                {
                    Color pixelColor;
                    byte r, g, b;

                    BitArray bits = new BitArray(myArray);
                    BitArray sizeBits = new BitArray(size);
                    BitArray extBits = new BitArray(extension);

                    int maxIdx = (bits.Count / 3);

                    int[] idx = new int[bytes];

                    int choose, maxSize = bytes, a;

                    for (int ctr = 0; ctr < bytes; ctr++)
                    {
                        idx[ctr] = ctr;
                    }

                    Random randomer = new Random(getKey(textBox3.Text));

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
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "Bitmap (*.bmp)|*.bmp";
                    dialog.Title = "Save embedded file as";
                    dialog.ShowDialog();
                    if (dialog.FileName != "")
                    {
                        bmp.Save(dialog.FileName);
                        bmp.Dispose();
                        label5.Text = "Your message has been embedded successfully";
                    }
                }
                else
                {
                    bmp.Dispose();
                    throw (new Exception("Your message is too large"));
                }
            }
            catch (Exception ex)
            {
                label5.Text = "Something went wrong when proccessing your request : " + ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(textBox1.Text);

                int tripbytes = bmp.Width * bmp.Height;

                Random randomer = new Random(getKey(textBox3.Text));

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

                    if (3 * i + 1 < maxBits) {
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
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = ext + " file|*." + ext;
                dialog.Title = "Save extracted file as";
                dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    FileStream abc = File.OpenWrite(dialog.FileName);
                    for (int i = 0; i < extractedByte.Count<byte>(); i++)
                    {
                        abc.WriteByte(extractedByte[i]);
                    }
                    abc.Dispose();
                    label5.Text = "Your message has been extracted successfully";
                }
            }
            catch (Exception ex)
            {
                label5.Text = "Something went wrong when proccessing your request : " + ex.Message;
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

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Bitmap (*.bmp)|*.bmp";
            dialog.ShowDialog();
            textBox1.Text = dialog.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            dialog.ShowDialog();
            textBox2.Text = dialog.FileName;
        }
    }
}
