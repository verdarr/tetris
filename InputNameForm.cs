using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormsApp1
{
    using System;
    using System.Windows.Forms;

    public class InputNameForm : Form
    {
        public string PlayerName { get; private set; } = string.Empty;
        private TextBox nameTextBox = new TextBox();
        public InputNameForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Введите имя";
            this.ClientSize = new Size(300, 100);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            Label label = new Label
            {
                Text = "Имя игрока:",
                Location = new Point(20, 20),
                Width = 100
            };
            this.Controls.Add(label);

            nameTextBox = new TextBox
            {
                Location = new Point(120, 20),
                Width = 160
            };
            this.Controls.Add(nameTextBox);

            Button okButton = new Button
            {
                Text = "OK",
                Location = new Point(120, 50)
            };
            okButton.Click += OkButton_Click;
            this.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                Text = "Отмена",
                Location = new Point(200, 50)
            };
            cancelButton.Click += CancelButton_Click;
            this.Controls.Add(cancelButton);

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                PlayerName = nameTextBox.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите имя", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeComponent()
        {

        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
