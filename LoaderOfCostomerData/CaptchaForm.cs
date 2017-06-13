using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoaderOfCostomerData
{
    public partial class CaptchaForm : Form
    {
        public string TextCaptcha { get; set; }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
        public CaptchaForm(byte[] byteImage)
        {
            InitializeComponent();
            button1.Click += new EventHandler(this.button_click);
            pictureCapcha.Image = ByteToImage(byteImage);
        }

        public void  button_click(object sender, EventArgs e)
        {
            TextCaptcha = capTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
