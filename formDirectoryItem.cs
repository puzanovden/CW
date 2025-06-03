
using Microsoft.Web.WebView2.WinForms;

namespace CW
{
    public partial class formDirectoryItem : Form
    {
        private cDirectoryItem currentItem;
        private Dictionary<string, Control> fieldInputs = new();
        private List<cFieldDefinition> fieldDefinitions;


        private WebView2 webView;

        public formDirectoryItem()
        {
            InitializeComponent();
        }

        public void SetData(cDirectoryItem directoryItem, List<cFieldDefinition> structure)
        {
            try
            {
                currentItem = directoryItem;
                textBoxCode.Text = currentItem.Code;
                textBoxName.Text = currentItem.Name;
                SetDataWithStructure(directoryItem, structure);
                fieldDefinitions = structure;

                Text = string.IsNullOrWhiteSpace(currentItem.Name) ? "Новий елемент" : $"Елемент: {currentItem.Name}";
            }
            catch (Exception ex)
            {
            }
        }

        private void labelName_Click(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SaveFields();
            DialogResult = DialogResult.OK;
        }


        private void SearchImage()
        {
            string code = textBoxName.Text.Trim();
            if (string.IsNullOrEmpty(code)) return;

            string url = $"https://www.google.com/search?tbm=isch&q={Uri.EscapeDataString(code)}";

            if (webView == null)
            {
                Width += 800;

                webView = new WebView2
                {
                    Location = new Point(textBoxName.Right + 10, textBoxName.Top),
                    Size = new Size(780, ClientSize.Height - 60),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right
                };

                this.Controls.Add(webView);
                webView.BringToFront();

                webView.EnsureCoreWebView2Async().ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        webView.CoreWebView2.Navigate(url);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                webView.CoreWebView2.Navigate(url);
            }
        }
        public void SetDataWithStructure(cDirectoryItem item, List<cFieldDefinition> structure)
        {
            currentItem = item;
            fieldInputs.Clear();

            int top = 50;

            foreach (var field in structure)
            {
                if (field.Name == "Code" || field.Name == "Name") continue;
                var label = new Label
                {
                    Text = field.PublicName + ":",
                    Left = 10,
                    Top = top+5,
                    Width = 155
                };

                Control inputControl;
                if (field.Type == FieldType.Number)
                {
                    var value = item.GetType().GetProperty(field.Name)?.GetValue(item);
                    var nud = new NumericUpDown
                    {
                        Name = "num" + field.Name,
                        Left = 165,
                        Top = top,
                        Width = 150,
                        DecimalPlaces = 2,
                        Minimum = 0,
                        Maximum = 9999999,
                        Increment = 1,
                        Value = Convert.ToDecimal(value),
                        RightToLeft = RightToLeft.Yes
                    };

                    inputControl = nud;
                }
                else if (field.Type == FieldType.Image)
                {
                    var button = new Button
                    {
                        Text = "Шукати",
                        Left = 10,
                        Height = 30,
                        Top = top + 5 + 30,
                        Width = 145
                    };

                    button.Click += (s, e) =>
                    {
                        SearchImage();
                    };

                    Controls.Add(button);
                    var pictureBox = new PictureBox
                    {
                        Name = "pic" + field.Name,
                        Left = 165,
                        Top = top,
                        Width = 505,
                        Height = 505,
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Cursor = Cursors.Hand,
                        AllowDrop = true

                    };

                    pictureBox.DragEnter += (s, e) =>
                    {
                        if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.StringFormat))
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    };

                        pictureBox.DragDrop += (s, e) =>
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
                                pictureBox.Image = Image.FromStream(ms);
                                CheckImage(pictureBox);
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
                                    pictureBox.Load(imageUrl);
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
                    };
                    pictureBox.Click += (s, e) =>
                    {
                        using var ofd = new OpenFileDialog
                        {
                            Filter = "Зображення|*.jpg;*.jpeg;*.png;*.bmp"
                        };
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            pictureBox.Image = Image.FromFile(ofd.FileName);
                        }
                    };

                   // top += 505;

                    var base64 = item.GetType().GetProperty(field.Name)?.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(base64))
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(base64);
                            using var ms = new MemoryStream(bytes);
                            pictureBox.Image = Image.FromStream(ms);
                        }
                        catch { /* не робитть base64 */ }
                    }

                    
                    inputControl = pictureBox;
                }

                else if (field.Type == FieldType.Reference && field.ReferenceName != null)
                {
                    var textBox = new TextBox
                    {
                        Name = "txt" + field.Name,
                        Left = 165,
                        Top = top,
                        Width = 445,
                        ReadOnly = true,
                        BackColor = Color.White,
                        Text = item.GetType().GetProperty(field.Name)?.GetValue(item)?.ToString() ?? ""
                    };

                    var button = new Button
                    {
                        Text = "...",
                        Left = textBox.Left + textBox.Width,
                        Top = top,
                        Width = 30,
                        Height = textBox.Height
                    };

                    var buttonClear = new Button
                    {
                        Text = "X",
                        Left = button.Left + button.Width,
                        Top = top,
                        Width = 30,
                        Height = textBox.Height
                    };

                    button.Click += (s, e) =>
                    {
                        var dir = cDataBase.DB.GetDirectoryByName(field.ReferenceName);
                        if (dir is IDirectory addable)
                        {
                            var form = new formDirectoryList();
                            form.ChoseMode = true;
                            form.SetData(dir); 
                            if (form.ShowDialog(this) == DialogResult.OK)
                            {
                                var selectedItem = form.GetSelectedItem(); 
                                if (selectedItem != null)
                                {
                                    textBox.Text = selectedItem.Name;
                                    textBox.Tag = selectedItem; 
                                }
                            }
                        }
                    };

                    buttonClear.Click += (s, e) =>
                    {
                        textBox.Text = string.Empty;
                        textBox.Tag = null;
                    };

                    Controls.Add(button);
                    Controls.Add(buttonClear);
                    inputControl = textBox;
                }
                else
                {
                    var textBox = new TextBox
                    {
                        Name = "txt" + field.Name,
                        Left = 165,
                        Top = top,
                        Width = 505,
                        Text = item.GetType().GetProperty(field.Name)?.GetValue(item)?.ToString() ?? ""
                    };

                    inputControl = textBox;
                }

                Controls.Add(label);
                Controls.Add(inputControl);
                fieldInputs[field.Name] = inputControl;



                top = inputControl.Top + inputControl.Height + 10;
                Height = top+85;
                //buttonOk.Top = top + 160;
                
                
            }
        }

        private void CheckImage(PictureBox pb)
        {
            try
            {
                using var ms = new MemoryStream();
                pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var base64 = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                pb.Image = null;
                MessageBox.Show("Картинка не зконвертується, не підійде:)");
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

        private void SaveFields()
        {
            currentItem.Name = textBoxName.Text;

            foreach (var field in fieldDefinitions)
            {
                if (!fieldInputs.TryGetValue(field.Name, out Control control))
                    continue;

                object? value = null;

                switch (field.Type)
                {
                    case FieldType.String:
                        value = (control as TextBox)?.Text;
                        break;

                    case FieldType.Number:
                        if (control is NumericUpDown nud)
                            value = (float)nud.Value;
                        else if (control is TextBox tb && float.TryParse(tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float f))
                            value = f;
                        break;
                    
                    case FieldType.Image:
                        if (control is PictureBox pic && pic.Image != null)
                        {
                            using var ms = new MemoryStream();
                            pic.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            var base64 = Convert.ToBase64String(ms.ToArray());
                            value = base64;
                        }
                        break;

                    case FieldType.Reference:
                        value = control.Tag as cDirectoryItem;
                        break;
                }

                var prop = currentItem.GetType().GetProperty(field.Name);
                if (prop != null && value != null)
                    prop.SetValue(currentItem, value);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void labelCode_Click(object sender, EventArgs e)
        {

        }
    }
}
