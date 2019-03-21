using ClientPrint.PrintServiceRef;
using System;
using System.Windows.Forms;

namespace ClientPrint.Forms
{
    /// <summary>
    /// Форма дополнительной проверки данных для чека коррекции
    /// </summary>
    public partial class DialogCheckCorrection : Form
    {
#pragma warning disable 1591
        public CheckCorrection Check { get; private set; }

        public DialogCheckCorrection(CheckCorrection checkCorr)
#pragma warning restore 1591
        {
            InitializeComponent();
            Check = checkCorr;
            ViewData();
        }

        private void ViewData()
        {
            comboBoxTypeCorrection.SelectedIndex = (int)Check.СorrectionType;

            switch (Check.CheckType)
            {
                case CheckTypes.ЧекКоррекцииПрихода:
                    comboBoxTypeOperation.SelectedIndex = 0;
                    break;

                case CheckTypes.ЧекКоррекцииРасхода:
                    comboBoxTypeOperation.SelectedIndex = 1;
                    break;
            }

            comboBoxSNO.SelectedItem = (int)Check.TaxType;
            if (Check.CorrectionBaseDate > dateTimePickerBaseDate.MinDate)
                dateTimePickerBaseDate.Value = Check.CorrectionBaseDate;
            else
                dateTimePickerBaseDate.Value = DateTime.Now;
            textBoxCashier.Text = Check.Cashier;
            maskedTextCashierVatin.Text = Check.CashierVATIN;

            maskedTextBoxBaseNumber.Text = Check.CorrectionBaseNumber;
            textBoxBaseName.Text = Check.CorrectionBaseName;

            if (Check.TaxType == TaxVariants.None)
                comboBoxSNO.SelectedIndex = -1;
            else
                comboBoxSNO.SelectedIndex = (int)Check.TaxType;
            var format = "0.00";
            maskedTextBoxSumTaxNone.Text = Check.TaxSumNone.ToString(format);
            maskedTextBoxSumTax0.Text = Check.TaxSum0.ToString(format);
            maskedTextBoxSumTax10.Text = Check.TaxSum10.ToString(format);
            maskedTextBoxSumTax20.Text = Check.TaxSum18.ToString(format);
            maskedTextBoxSumTax110.Text = Check.TaxSum110.ToString(format);
            maskedTextBoxSumTax120.Text = Check.TaxSum118.ToString(format);
            var defaultValue = 0.ToString(format);
            maskedTextBoxCashPayment.Text = Check.CashPayment == null ? defaultValue : Check.CashPayment.Summ.ToString(format);
            maskedTextBoxElectronicPayment.Text = Check.ElectronicPayment == null ? defaultValue : Check.ElectronicPayment.Summ.ToString(format);
            maskedTextBoxCreditPayment.Text = Check.CreditPayment == null ? defaultValue : Check.CreditPayment.Summ.ToString(format);
            maskedTextBoxAdvancePayment.Text = Check.AdvancePayment == null ? defaultValue : Check.AdvancePayment.Summ.ToString(format);
            maskedTextBoxCashProvisionPayment.Text = Check.CashProvisionPayment == null ? defaultValue : Check.CashProvisionPayment.Summ.ToString(format);
        }

        private bool SetData()
        {
            try
            {
                Check.СorrectionType = (CorrectionTypes)comboBoxTypeCorrection.SelectedIndex;
                switch (comboBoxTypeOperation.SelectedIndex)
                {
                    case 0:
                        Check.CheckType = CheckTypes.ЧекКоррекцииПрихода;
                        break;

                    case 1:
                        Check.CheckType = CheckTypes.ЧекКоррекцииРасхода;
                        break;
                }

                if (comboBoxSNO.SelectedIndex == -1)
                    Check.TaxType = TaxVariants.None;
                else
                    Check.TaxType = (TaxVariants)comboBoxSNO.SelectedIndex;

                Check.CorrectionBaseDate = dateTimePickerBaseDate.Value;
                Check.Cashier = textBoxCashier.Text;
                Check.CashierVATIN = maskedTextCashierVatin.Text;

                Check.CorrectionBaseNumber = maskedTextBoxBaseNumber.Text;
                Check.CorrectionBaseName = textBoxBaseName.Text;

                Check.TaxSumNone = decimal.Parse(maskedTextBoxSumTaxNone.Text);
                Check.TaxSum0 = decimal.Parse(maskedTextBoxSumTax0.Text);
                Check.TaxSum10 = decimal.Parse(maskedTextBoxSumTax10.Text);
                Check.TaxSum18 = decimal.Parse(maskedTextBoxSumTax20.Text);
                Check.TaxSum110 = decimal.Parse(maskedTextBoxSumTax110.Text);
                Check.TaxSum118 = decimal.Parse(maskedTextBoxSumTax120.Text);

                Check.CashPayment = new Payment()
                {
                    Summ = decimal.Parse(maskedTextBoxCashPayment.Text),
                    TypeClose = PayTypes.Наличные
                };
                Check.ElectronicPayment = new Payment()
                {
                    Summ = decimal.Parse(maskedTextBoxElectronicPayment.Text),
                    TypeClose = PayTypes.Электронными
                };

                Check.CreditPayment = new Payment()
                {
                    Summ = decimal.Parse(maskedTextBoxCreditPayment.Text),

                    TypeClose = PayTypes.Кредит
                };
                Check.AdvancePayment = new Payment()
                {
                    Summ = decimal.Parse(maskedTextBoxAdvancePayment.Text),

                    TypeClose = PayTypes.Предоплата
                };
                Check.CashProvisionPayment = new Payment()
                {
                    Summ = decimal.Parse(maskedTextBoxCashProvisionPayment.Text),
                    TypeClose = PayTypes.Представление
                };

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка {ex.Message}");
                return false;
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (SetData() && IsValidData())
                DialogResult = DialogResult.OK;
        }

        private bool IsValidData()
        {
            decimal summPayments = Check.CashPayment.Summ +
                Check.ElectronicPayment.Summ +
                Check.CreditPayment.Summ +
                Check.AdvancePayment.Summ +
                Check.CashProvisionPayment.Summ;

            if (summPayments == 0)
            {
                MessageBox.Show(@"Оплата не заполнена", @"Сервер КММ");
                return false;
            }

            if (string.IsNullOrEmpty(Check.CorrectionBaseNumber))
            {
                MessageBox.Show(@"Номер документа основания коррекции не может быть пустым", @"Сервер КММ");
                return false;
            }

            if (string.IsNullOrEmpty(Check.CorrectionBaseName))
            {
                MessageBox.Show(@"Описание коррекции не может быть пустым", @"Сервер КММ");
                return false;
            }

            // 1С Рохница не передает эти данные

            //if (Math.Round(Sum, 2) != Math.Round(summPayments, 2))
            //{
            //    return false;
            //}

            var sumAllTax =
                Check.TaxSumNone +
                Check.TaxSum0 +
                Check.TaxSum10 +
                Check.TaxSum18 +
                Check.TaxSum110 +
                Check.TaxSum118;
            if ((sumAllTax) == 0)
            {
                MessageBox.Show(@"Не переданы налоги", @"Сервер КММ");
                return false;
            }

            return true;
        }
    }
}