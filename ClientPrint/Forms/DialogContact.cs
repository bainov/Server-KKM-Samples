using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClientPrint.Forms
{
    /// <summary>
    /// Представляет диалоговое окно для ввода телефона или e-mail при закрытии чека
    /// </summary>
    public partial class DialogContact : Form
    {
        /// <summary>
        /// Задает и возвращает контакт клинента
        /// </summary>
        /// <remarks>Телефон или e-mail</remarks>
        public string ClientData
        {
            get { return _clientData; }
            set
            {
                _clientData = value;
                if (IsValidMail(_clientData))
                {
                    _email = _clientData;
                    _emailSelected = true;
                }
                else if (IsValidTelefon(_clientData))
                {
                    _telefon = _clientData;
                    _telefonSelected = true;
                }
            }
        }

        private bool TelefonSelected
        {
            get
            {
                return _telefonSelected;
            }

            set
            {
                _email = textBoxContact.Text;
                textBoxContact.Clear();
                _emailSelected = false;

                textBoxContact.Text = _telefon;
                textBoxContact.Mask = @"+7(000) 000-00-00";
                _telefonSelected = value;
                ShowKeybord();
                FocusTextBox();
            }
        }

        private bool EmailSelected
        {
            get
            {
                return _emailSelected;
            }

            set
            {
                _telefon = textBoxContact.Text;
                textBoxContact.Mask = "";
                textBoxContact.Clear();
                _telefonSelected = false;

                textBoxContact.Text = _email;
                _emailSelected = value;
                ShowKeybord();
                FocusTextBox();
            }
        }

        private string _clientData;
        private string _telefon;
        private string _email;
        private bool _timerFlag;
        private int _timerCount = 1;
        private bool _keybordVisible;
        private bool _emailSelected;
        private bool _telefonSelected;
        private readonly InputLanguage _language;

#pragma warning disable 1591

        public DialogContact(bool virtualKeybord = false)
#pragma warning restore 1591
        {
            InitializeComponent();
            //TODO обеспечить чтение параметров
            _keybordVisible = virtualKeybord;

            try
            {
                _language = InputLanguage.CurrentInputLanguage;
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
            }
            catch
            {
                // ignored
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            textBoxContact.SelectedText = (sender as Button)?.Text;
            FocusTextBox();
        }

        private void FocusTextBox(int selectionStart = 0)
        {
            try
            {
                if (textBoxContact.CanFocus)
                {
                    textBoxContact.Focus();
                    if (selectionStart != 0)
                        textBoxContact.SelectionStart = selectionStart;
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Отображает клавиатуру для ввода в зависимости от параметров
        /// </summary>
        private void ShowKeybord()
        {
            if (_keybordVisible)
            {
                Size = new Size(1020, 522);
            }
            else
            {
                Size = new Size(1020, 225);
            }
        }

        private bool IsValidTelefon(string text)
        {
            try
            {
                string str = ReplacePhone(text);
                string patternPhone = "([0-9]{10,11})$";
                Match isMatchPhone = Regex.Match(str, patternPhone, RegexOptions.IgnoreCase);
                return isMatchPhone.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Чистит номер телефона от маски
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Номер телефона только из цифр</returns>
        private string ReplacePhone(string text)
        {
            try
            {
                return Regex.Replace(text, @"[^\d]", "", RegexOptions.Compiled);
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool IsValidMail(string email)
        {
            try
            {
                // Проверку на валидность осуществляет конструктор
                var unused = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Check()
        {
            if (EmailSelected)
                if (IsValidMail(textBoxContact.Text))
                {
                    ClientData = textBoxContact.Text;
                    DialogResult = DialogResult.OK;
                    return;
                }
                else
                {
                    timer1.Enabled = true;
                    return;
                }

            if (TelefonSelected)
                if (IsValidTelefon(textBoxContact.Text))
                {
                    ClientData = ReplacePhone(textBoxContact.Text);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    timer1.Enabled = true;
                }
        }

        private void DeleteSymbol()
        {
            int selectionStart = textBoxContact.SelectionStart - 1;
            try
            {
                textBoxContact.Text = textBoxContact.Text.Remove(textBoxContact.SelectionStart - 1, 1);
            }
            catch
            {
                // ignored
            }

            FocusTextBox(selectionStart);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxContact.Clear();

            if (_telefonSelected)
                TelefonSelected = true;
            else if (_emailSelected)
                EmailSelected = true;
            else
                TelefonSelected = true;

            ShowKeybord();
        }

        private void textBoxContact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Check();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 175;
            if (_timerFlag == false)
            {
                textBoxContact.BackColor = Color.Orange;
                _timerFlag = true;
            }
            else
            {
                textBoxContact.BackColor = Color.LightYellow;

                _timerFlag = false;
            }
            _timerCount++;
            if (_timerCount > 8)
            {
                timer1.Enabled = false;
                textBoxContact.BackColor = Color.White;
                _timerCount = 1;
            }
        }

        private void pictureBoxSms_Click(object sender, EventArgs e)
        {
            TelefonSelected = true;
        }

        private void pictureBoxEmail_Click(object sender, EventArgs e)
        {
            EmailSelected = true;
        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DialogContact_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Вернуть язык как было при открытии
            try
            {
                InputLanguage.CurrentInputLanguage = _language;
            }
            catch
            {
                // ignored
            }
        }

        private void pictureBoxDel_Click(object sender, EventArgs e)
        {
            DeleteSymbol();
        }

        private void pictureBoxKeyBord_Click(object sender, EventArgs e)
        {
            _keybordVisible = !_keybordVisible;
            ShowKeybord();
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            Check();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBoxContact.SelectedText = (sender as Button)?.Text;
            FocusTextBox();
        }
    }
}