using ImageProcess2;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.Extensions;




namespace CoinsActivity
{
    public partial class Form1 : Form
    {
        public Bitmap loaded;
        public float totalValue = 0;
        public int totalCoins = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loaded = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = loaded;

            }
        }



        

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }




        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }






        private void button3_Click(object sender, EventArgs e)
        {
           if(loaded != null)
            {
              DIPFilter.GrayScale(ref loaded);
              DIPFilter.GaussianBlur(ref loaded);
              DIPFilter.Threshold(ref loaded);
                
              pictureBox1.Image= loaded;

                Algo.Calculate(loaded, ref totalValue, ref totalCoins);

                MessageBox.Show($"Total Value: {totalValue}", $"Coins detected: {totalCoins}");




            } else
            {
                MessageBox.Show($"Load an image first.");

            }




        }


    }
}
