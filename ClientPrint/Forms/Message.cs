using System;
using System.Windows.Forms;

namespace ClientPrint.Forms
{
    /// <summary>
    /// Представляет окно сообщения
    /// </summary>
    /// <remarks>Есть таймер закрытия</remarks>>
    public partial class Message : Form
    {
        private int _sec = 10;

#pragma warning disable 1591

        public Message()
#pragma warning restore 1591
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Устанавливает сообщения, которые будут показаны
        /// </summary>
        /// <param name="data"></param>
        public void SetData(params string[] data)
        {
            textBox1.Clear();
            foreach (var item in data)
            {
                textBox1.AppendText(item + Environment.NewLine);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _sec--;
            button1.Text = $@"OK {_sec}";
            if (_sec <= 0)
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}