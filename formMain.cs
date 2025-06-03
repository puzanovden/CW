
namespace CW
{
    public partial class formMain : Form
    {
        //private DataBase db = new DataBase();
        public formMain()
        {
            InitializeComponent();

            GenerateDirectoryButtons();
        }

        private void GenerateDirectoryButtons()
        {
            var left = 10;

            foreach (var meta in cDataBase.DB.GetAll())
            {
                var button = new Button
                {
                    Text = meta.DisplayName,
                    Width = 170,
                    Height = 50,
                    Left = left,
                    Font = new Font("Segoe UI", 14, FontStyle.Regular),
                    Top = 10
                };

                button.Click += (s, e) =>
                {
                    var form = new formDirectoryList();
                    form.SetData(meta.Directory); //  Directory<T>
                    OpenChildForm(form);
                };

                this.Controls.Add(button);
                left += 180;
            }
        }


        private void OpenChildForm(Form childForm)
        {

            panelContainer.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelContainer.Controls.Add(childForm);
            childForm.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            cDataBase.DB.LoadAll("");

        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            cDataBase.DB.SaveAll("");
        }


        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
