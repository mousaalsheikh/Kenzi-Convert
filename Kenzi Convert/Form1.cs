using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kenzi_Convert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFolder.Text))
            {
                MessageBox.Show("Please select folder");
            }
            else if (string.IsNullOrEmpty(cboFromFile.Text))
            {
                MessageBox.Show("Please select extension");
            }
            else if (string.IsNullOrEmpty(cboToFile.Text))
            {
                MessageBox.Show("Please select extension");
            }
            else if (cboFromFile.Text == cboToFile.Text)
            {
                MessageBox.Show("Please select different extension to convert");
            }
            else
            {
                btnChooseFolder.Enabled = false;
                btnConvert.Enabled = false;
                int count = 0;
                int failed = 0;
                int total = 0;

                foreach (string file in Directory.GetFiles(txtFolder.Text))
                {
                    bool canConvert = false;
                    string ext = Path.GetExtension(file).ToLower();
                    if (cboFromFile.Text == "image/*")
                    {
                        if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif")
                        {
                            total++;
                            canConvert = true;
                        }
                    }
                    else
                    {
                        if (cboFromFile.Text == "jpg")
                        {
                            if (ext == ".jpg" || ext == ".jpeg")
                            {
                                total++;
                                canConvert = true;
                            }
                        }
                        else
                        {
                            if (ext == "." + cboFromFile.Text)
                            {
                                total++;
                                canConvert = true;
                            }
                        }
                    }


                    if (canConvert)
                    {
                        string name = Path.GetFileName(file);
                        string fileId = Path.GetFileNameWithoutExtension(file);
                        string path = Path.GetDirectoryName(file);
                        string fullFilePath = path + @"/" + name;
                        if (cboToFile.Text == "png")
                        {
                            try
                            {
                                using (Bitmap bitmap = new Bitmap(fullFilePath))
                                {
                                    using (Bitmap newImage = new Bitmap(bitmap.Width, bitmap.Height))
                                    {
                                        using (Graphics graphics = Graphics.FromImage(newImage))
                                        {
                                            graphics.Clear(Color.White);
                                            graphics.DrawImage(bitmap, Point.Empty);
                                            newImage.Save(path + @"/" + fileId + ".png", ImageFormat.Png);
                                        }
                                    }
                                }
                                count++;
                            }
                            catch
                            {
                                failed++;
                            }
                        }
                        else if (cboToFile.Text == "jpg")
                        {
                            try
                            {
                                using (Bitmap bitmap = new Bitmap(fullFilePath))
                                {
                                    using (Bitmap newImage = new Bitmap(bitmap.Width, bitmap.Height))
                                    {
                                        using (Graphics graphics = Graphics.FromImage(newImage))
                                        {
                                            graphics.Clear(Color.White);
                                            graphics.DrawImage(bitmap, Point.Empty);
                                            newImage.Save(path + @"/" + fileId + ".jpeg", ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                count++;
                            }
                            catch
                            {
                                failed++;
                            }
                        }
                        else if (cboToFile.Text == "webp")
                        {
                            try
                            {
                                string webPImagePath = path + @"/" + fileId + ".webp";
                                using (var webPFileStream = new FileStream(webPImagePath, FileMode.Create))
                                using (var imageFactory = new ImageFactory(preserveExifData: false))
                                {
                                    using (Bitmap bitmap = new Bitmap(fullFilePath))
                                    {
                                        imageFactory.Load(bitmap)
                                                          .Format(new WebPFormat())
                                                          .Quality(90)
                                                          .Save(webPFileStream);
                                    }
                                }
                                count++;
                            }
                            catch
                            {
                                failed++;
                            }
                        }
                    }
                }
                MessageBox.Show("Total Images: " + total.ToString() + "\n\n" + "Successfull: " + count.ToString() + " - Failed: " + failed.ToString());
                btnChooseFolder.Enabled = true;
                btnConvert.Enabled = true;
            }
        }

        private void cboFromFile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboToFile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
