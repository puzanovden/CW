using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CW
{
    public partial class xxxxxfffffff : Form
    {
        public xxxxxfffffff()
        {
            InitializeComponent();
        }

        private void fffffff_Load(object sender, EventArgs e)
        {
            pictureBox1.AllowDrop = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.AllowDrop = true;
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            pictureBox1.DragEnter += pictureBox1_DragEnter;
            pictureBox1.DragDrop += pictureBox1_DragDrop;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //var form = new DirectoryListForm();
            //form.SetData(DataBase.DB.ModelsD);
            //OpenChildForm(form);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var form = new DirectoryListForm();
            //form.SetData(DataBase.DB.PartsD);
            //OpenChildForm(form);
        }



















        private void button1_Click(object sender, EventArgs e)
        {

            SearchImage();
        }

        private void SearchImage()
        {
            string code = codeTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(code))
            {
                string url = $"https://www.google.com/search?tbm=isch&q={Uri.EscapeDataString(code)}";
                webView21.Source = new Uri(url);
            }
        }

        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string data = e.Data.GetData(DataFormats.Text) as string;

            if (!string.IsNullOrEmpty(data))
            {
                string imageUrl = null;

                if (data.StartsWith("data:image"))
                {
                    string base64 = data.Substring(data.IndexOf(",") + 1);
                    byte[] bytes = Convert.FromBase64String(base64);
                    using (var ms = new MemoryStream(bytes))
                        pictureBox1.Image = Image.FromStream(ms);
                    return;
                }
                else if (data.Contains("google.com/imgres") && data.Contains("imgurl="))
                {
                    imageUrl = ExtractImageUrlFromGoogleBlock(data);
                }
                else if (data.StartsWith("http") && (data.EndsWith(".jpg") || data.EndsWith(".png") || data.EndsWith(".jpeg") || data.EndsWith(".gif")))
                {
                    imageUrl = data;
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    try
                    {
                        pictureBox1.Load(imageUrl);
                    }
                    catch
                    {
                        MessageBox.Show("Погана картинка");
                    }
                }
                else
                {
                    MessageBox.Show("Не вдалося визначити посилання на зображення.");
                }
            }
        }

        private string ExtractImageUrlFromGoogleBlock(string url)
        {
            try
            {
                var uri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return Uri.UnescapeDataString(query["imgurl"]);
            }
            catch
            {
                return null;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OpenChildForm(Form childForm)
        {
            //// Очистити попередній вміст панелі
            //panelContainer.Controls.Clear();

            //// Підготувати дочірню форму
            //childForm.TopLevel = false;
            //childForm.FormBorderStyle = FormBorderStyle.None;
            //childForm.Dock = DockStyle.Fill;

            //// Додати до панелі та відобразити
            //panelContainer.Controls.Add(childForm);
            //childForm.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void webView21_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
