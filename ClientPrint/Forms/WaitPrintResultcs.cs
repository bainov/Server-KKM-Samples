using ClientPrint.PrintServiceRef;
using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

namespace ClientPrint.Forms
{
    public partial class WaitPrintResultcs : Form
    {
        private PrintServiceClient _client;
        private Guid _idDoc;
        private int _secToClose = 5;

        public WaitPrintResultcsModel Model { get; set; }

        public WaitPrintResultcs(string docName, Guid id, PrintServiceClient client)
        {
            InitializeComponent();
            Model = new WaitPrintResultcsModel(docName);
            dataBindingSource.DataSource = Model;
            Text = docName;

            _idDoc = id;
            this._client = client;

            ViewWait();
        }

        private void WaitPrintResultcs_Load(object sender, EventArgs e)
        {
        }

        private DocumentHistory[] GetHistory(Guid docId, PrintServiceClient cl)
        {
            int count = 1;
            while (true)
            {
                try
                {
                    return cl.GetDocumentHistory(docId);
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    textBox1.Text = $"Сервер стал недоступен. Попытка подключения{++count}";
                    return null;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    textBox1.Text = "Разрыв соединения";
                    return null;
                }
                catch (TimeoutException)
                {
                    //ignore
                    textBox1.Text = $"Попытка {++count}";
                }
            }
        }

        private void ViewWait()
        {
            this.pictureBox1.Image = Properties.Resources.Animation;
        }

        private void ViewSuccess()
        {
            this.pictureBox1.Image = Properties.Resources.Information;
        }

        private void ViewError()
        {
            this.pictureBox1.Image = Properties.Resources.Warning;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void WaitPrintResultcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Model.CanExit)
            {
                var res = MessageBox.Show("Закрыть без ожидания результатов", "РБ-Софт: Сервер ККМ", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                    e.Cancel = true;
                else if (res == DialogResult.OK)
                {
                    DialogResult = DialogResult.Abort;
                    Model.SetStatus(-1, "Прерывание ожидания результатов");
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timerСlosing_Tick(object sender, EventArgs e)
        {
            labelAdditional.Text = $"До закрытия осталось {_secToClose} сек...";
            if (_secToClose <= 0)
                Close();
            _secToClose--;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetHistory(_idDoc, _client);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var versionServerKkm = "3.22.0.0";

            var hist = e.Result as DocumentHistory[];

            if (hist == null) return;

            Model.Table.Clear();
            foreach (var item in hist)
            {
                Model.Table.Add(new Row(item));
            }
            bool stop = false;
            bool success = false;
            if (hist.Any(x => x.State == DocumentPrintState.Printed))
            {
                Model.SetStatus(0, "Успешно");
                stop = true;
                success = true;
            }
            else if (hist.Any(x => x.State == DocumentPrintState.PrintingError))
            {
                try
                {
                    var res = _client.GetDocStatus(_idDoc);
                    Model.SetStatus(res.Item1, res.Item2, res.Item3, res.Item3);
                }
                catch (ActionNotSupportedException)
                {
                    var currentSerVersion = _client.GetServerVersion();
                    var msg = $"Описание ошибки недоступно при длительных ожиданиях на версиях Сервера ККМ ниже {versionServerKkm}\r\n" +
                         $"Текущая версия {currentSerVersion}. Для получения детального описания после длительного ожидания обновите Сервер ККМ выше версии {versionServerKkm}";
                    Model.SetStatus(-7, msg);
                }
                stop = true;
            }
            if (stop)
            {
                Model.CanExit = true;
                timer1.Stop();
                if (success)
                {
                    timerСlosing.Start();
                    ViewSuccess();
                }
                else
                    ViewError();
            }
        }
    }

    /// <summary>
    /// Представляем модель данных привязки
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class WaitPrintResultcsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WaitPrintResultcsModel()
        {
        }

        private string description;

        public void SetStatus(int code, string desc, int checkNumber = 0, int sessionNumber = 0)
        {
            DescriptionFull = $"{code} : {desc}";
            Description = desc;
            Code = code;
            CheckNumber = checkNumber;
            SessionNumber = sessionNumber;
        }

        private string descriptionFull;

        public string DescriptionFull
        {
            get { return descriptionFull; }
            set
            {
                NotifyPropertyChanged(nameof(DescriptionFull));
                descriptionFull = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            private set
            {
                description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        private int code;

        public int Code
        {
            get
            {
                return code;
            }

            private set
            {
                code = value;
                NotifyPropertyChanged(nameof(Code));
            }
        }

        public int CheckNumber { get; private set; }

        private string nameTypeDoc;

        public string NameTypeDoc
        {
            get
            {
                return nameTypeDoc;
            }

            set
            {
                nameTypeDoc = value;
                NotifyPropertyChanged(nameof(NameTypeDoc));
            }
        }

        public BindingList<Row> Table { get; set; }
        private bool canExit;

        /// <summary>
        /// Результаты получены и можно закрывать форму
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can exit; otherwise, <c>false</c>.
        /// </value>
        public bool CanExit
        {
            get
            {
                return canExit;
            }

            set
            {
                canExit = value;
                NotifyPropertyChanged(nameof(CanExit));
            }
        }

        public int SessionNumber { get; private set; }

        public WaitPrintResultcsModel(string docName)
        {
            NameTypeDoc = docName;
            Table = new BindingList<Row>();
        }
    }

    public class Row : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public Row()
        {
        }

        public Row(DocumentHistory documentHistory)
        {
            Time = documentHistory.Time.ToLongTimeString();
            string state;
            switch (documentHistory.State)
            {
                case DocumentPrintState.BeginPrinting:
                    state = "Начало печати"; break;
                case DocumentPrintState.Printed:
                    state = "Распечатано"; break;
                case DocumentPrintState.PrintingError:
                    state = "Ошибка печати"; break;
                case DocumentPrintState.PrintingRepeat:
                    state = "Ошибка. Повторная отправка задания печати"; break;
                default:
                    state = documentHistory.State.ToString();
                    break;
            }
            State = state;
        }

        [DisplayName("Время")]
        public string Time { get; set; }

        [DisplayName("Состояние")]
        public string State { get; set; }
    }
}