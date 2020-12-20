using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Media;

namespace ImageLike
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class Window1 : Window
    {
        int currentNumber = 0;
        int currentIndex = 0;
        String reString = "";
        private class ColorfulPixel
        {
            byte red;
            byte green;
            byte blue;
            byte alpha;

            public ColorfulPixel(byte red, byte green, byte blue, byte alpha)
            {
                this.red = red;
                this.green = green;
                this.blue = blue;
                this.alpha = alpha;
            }
            public ColorfulPixel(byte red, byte green, byte blue)
            {
                this.red = red;
                this.green = green;
                this.blue = blue;
                this.alpha = 0;
            }

            public byte getRed() { return red; }
            public byte getGreen() { return green; }
            public byte getBlue() { return blue; }
            public byte getAlpha() { return alpha; }
        }


        public Window1()
        {
            InitializeComponent();
            Uri suri = setFileDirectory(currentNumber, currentIndex);
            presentImage(suri);
        }

        private void LoadFilesClick(object sender, RoutedEventArgs e)
        {
            String ImageName = DirBox.Text;
            String[] SplitName = ImageName.Split(',');
            currentNumber = SplitName[0][0] - '0';
            currentIndex = strToNum(SplitName[1]);
            Uri suri = setFileDirectory(currentNumber, currentIndex);
            presentImage(suri);
        }

        private int strToNum(String str)
        {
            int[] num = new int[str.Length];
            int i, j;
            int res = 0;
            for (i = 0; i < str.Length; i++)
            {
                num[i] = str[i] - '0';
            }
            for (i = str.Length - 1; i > 0; i--)
            {
                for (j = 0; j < i; j++)
                {
                    num[j] *= 10;
                }
            }
            for (i = 0; i < num.Length; i++)
            {
                res += num[i];
            }
            return res;
        }

        public Uri setFileDirectory(int number, int index)
        {
            String uriString = "file:///C:/Users/AFOC/Desktop/TrainData/";
            uriString += number.ToString() + "/" + number.ToString() + "-" + index.ToString() + ".jpg";
            Uri suri = new Uri(uriString);
            FileNameBox.Text = uriString;
            return suri;
        }

        public void presentImage(Uri suri)
        {
            BitmapImage trainImage = new BitmapImage(suri);
            imageBox.Source = trainImage as ImageSource;
            BinBox.Clear();
            ImageToBin(trainImage);
        }

        public void ImageToBin(BitmapImage trainImage)
        {
            int width = trainImage.PixelWidth;
            int height = trainImage.PixelHeight;
            int stride = width * 4;
            int size = height * stride;
            byte[] pixels = new byte [size];
            trainImage.CopyPixels(pixels, stride, 0);
            int flagCnt = 0;
            int i=0;

            try
            {
                for (i = 0; i < pixels.Length; i += 4)
                {
                    ColorfulPixel thisPixel = new ColorfulPixel(pixels[i], pixels[i + 1], pixels[i + 2], pixels[i + 3]);
                    if (thisPixel.getRed() + thisPixel.getGreen() + thisPixel.getBlue() <= 693)
                    {
                        BinBox.Text += "1 ";
                    }
                    else
                    {
                        BinBox.Text += "0 ";
                    }

                    if (++flagCnt % 28 == 0)
                    {
                        BinBox.Text += " \n";
                    }
                }
            }
            catch (Exception e)
            {
                BinBox.Text += e.ToString() + ' ' + i.ToString();
            }
        }

        public void SaveBinText()
        {
            int i, j;
            Uri suri;
            Random r = new Random();
            int cuN = r.Next(10);
            int cuI = r.Next(1280);
            int[,] check = new int[10, 1280];
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 1280; j++)
                {
                    check[i, j] = 1;
                }
            }
            for (i = 0; i <= 9; i++)
            {
                for (j = 0; j < 1280; j++)
                {
                    while (check[cuN, cuI] != 1)
                    {
                        cuN = r.Next(10);
                        cuI = r.Next(1280);
                    }
                    suri = setFileDirectory(cuN, cuI);
                    BitmapImage trainImage = new BitmapImage(suri);
                    ImageToBinText(trainImage);
                    reString += " " + cuN.ToString() +'-'+ cuI.ToString()+ '\n';
                    check[cuN, cuI] = 0;
                }
            }
        }

        public void ImageToBinText(BitmapImage trainImage)
        {
            int width = trainImage.PixelWidth;
            int height = trainImage.PixelHeight;
            int stride = width * 4;
            int size = height * stride;
            byte[] pixels = new byte[size];
            trainImage.CopyPixels(pixels, stride, 0);
            int i = 0;
            String bin = "";
            try
            {
                for (i = 0; i < pixels.Length; i += 4)
                {
                    ColorfulPixel thisPixel = new ColorfulPixel(pixels[i], pixels[i + 1], pixels[i + 2], pixels[i + 3]);
                    if (thisPixel.getRed() + thisPixel.getGreen() + thisPixel.getBlue() <= 693)
                    {
                        bin += "1 ";
                    }
                    else
                    {
                        bin += "0 ";
                    }
                }
                reString += bin;
            }
            catch (Exception e)
            {
                BinBox.Text += e.ToString() + ' ' + i.ToString();
            }
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentIndex++ == 1279){
                if(currentNumber!=9)currentNumber++;
                currentIndex = 0;
            }
            Uri suri = setFileDirectory(currentNumber, currentIndex);
            presentImage(suri);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex-- == 0)
            {
                if(currentNumber!=0)currentNumber--;
                currentIndex = 1279;
            }
            Uri suri = setFileDirectory(currentNumber, currentIndex);
            presentImage(suri);
        }

        private void ToBinButton_Click(object sender, RoutedEventArgs e)
        {
            BinBox.Clear();
            SaveBinText();
            BinBox.Text += reString;
        }

    }
}
