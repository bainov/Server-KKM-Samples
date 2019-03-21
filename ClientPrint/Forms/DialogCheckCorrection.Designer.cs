using ClientPrint;
namespace ClientPrint.Forms
{
    partial class DialogCheckCorrection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCheckCorrection));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Send = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.maskedTextBoxSumTaxNone = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSumTax0 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSumTax10 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSumTax20 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSumTax110 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSumTax120 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxCashPayment = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxElectronicPayment = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxAdvancePayment = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxCreditPayment = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxCashProvisionPayment = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxBaseName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCashier = new System.Windows.Forms.TextBox();
            this.maskedTextCashierVatin = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxBaseNumber = new System.Windows.Forms.MaskedTextBox();
            this.comboBoxTypeCorrection = new System.Windows.Forms.ComboBox();
            this.comboBoxTypeOperation = new System.Windows.Forms.ComboBox();
            this.dateTimePickerBaseDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxSNO = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Send)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Send, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBaseName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(772, 675);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Send
            // 
            this.Send.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Send.Image = global::ClientPrint.Properties.Resources._001_note;
            this.Send.Location = new System.Drawing.Point(5, 601);
            this.Send.Margin = new System.Windows.Forms.Padding(5);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(762, 69);
            this.Send.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Send.TabIndex = 21;
            this.Send.TabStop = false;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTaxNone, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTax0, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTax10, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTax20, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTax110, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxSumTax120, 4, 6);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxCashPayment, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxElectronicPayment, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxAdvancePayment, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxCreditPayment, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxCashProvisionPayment, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label8, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.label9, 3, 5);
            this.tableLayoutPanel2.Controls.Add(this.label10, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.label11, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.label12, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.label13, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label20, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label21, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 323);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(766, 269);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // maskedTextBoxSumTaxNone
            // 
            this.maskedTextBoxSumTaxNone.Location = new System.Drawing.Point(669, 53);
            this.maskedTextBoxSumTaxNone.Name = "maskedTextBoxSumTaxNone";
            this.maskedTextBoxSumTaxNone.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTaxNone.TabIndex = 0;
            // 
            // maskedTextBoxSumTax0
            // 
            this.maskedTextBoxSumTax0.Location = new System.Drawing.Point(669, 89);
            this.maskedTextBoxSumTax0.Name = "maskedTextBoxSumTax0";
            this.maskedTextBoxSumTax0.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTax0.TabIndex = 0;
            // 
            // maskedTextBoxSumTax10
            // 
            this.maskedTextBoxSumTax10.Location = new System.Drawing.Point(669, 125);
            this.maskedTextBoxSumTax10.Name = "maskedTextBoxSumTax10";
            this.maskedTextBoxSumTax10.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTax10.TabIndex = 0;
            // 
            // maskedTextBoxSumTax20
            // 
            this.maskedTextBoxSumTax20.Location = new System.Drawing.Point(669, 161);
            this.maskedTextBoxSumTax20.Name = "maskedTextBoxSumTax20";
            this.maskedTextBoxSumTax20.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTax20.TabIndex = 0;
            // 
            // maskedTextBoxSumTax110
            // 
            this.maskedTextBoxSumTax110.Location = new System.Drawing.Point(669, 197);
            this.maskedTextBoxSumTax110.Name = "maskedTextBoxSumTax110";
            this.maskedTextBoxSumTax110.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTax110.TabIndex = 0;
            // 
            // maskedTextBoxSumTax120
            // 
            this.maskedTextBoxSumTax120.Location = new System.Drawing.Point(669, 233);
            this.maskedTextBoxSumTax120.Name = "maskedTextBoxSumTax120";
            this.maskedTextBoxSumTax120.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxSumTax120.TabIndex = 0;
            // 
            // maskedTextBoxCashPayment
            // 
            this.maskedTextBoxCashPayment.Location = new System.Drawing.Point(253, 53);
            this.maskedTextBoxCashPayment.Name = "maskedTextBoxCashPayment";
            this.maskedTextBoxCashPayment.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxCashPayment.TabIndex = 0;
            // 
            // maskedTextBoxElectronicPayment
            // 
            this.maskedTextBoxElectronicPayment.Location = new System.Drawing.Point(253, 89);
            this.maskedTextBoxElectronicPayment.Name = "maskedTextBoxElectronicPayment";
            this.maskedTextBoxElectronicPayment.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxElectronicPayment.TabIndex = 0;
            // 
            // maskedTextBoxAdvancePayment
            // 
            this.maskedTextBoxAdvancePayment.Location = new System.Drawing.Point(253, 125);
            this.maskedTextBoxAdvancePayment.Name = "maskedTextBoxAdvancePayment";
            this.maskedTextBoxAdvancePayment.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxAdvancePayment.TabIndex = 0;
            // 
            // maskedTextBoxCreditPayment
            // 
            this.maskedTextBoxCreditPayment.Location = new System.Drawing.Point(253, 161);
            this.maskedTextBoxCreditPayment.Name = "maskedTextBoxCreditPayment";
            this.maskedTextBoxCreditPayment.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxCreditPayment.TabIndex = 0;
            // 
            // maskedTextBoxCashProvisionPayment
            // 
            this.maskedTextBoxCashProvisionPayment.Location = new System.Drawing.Point(253, 197);
            this.maskedTextBoxCashProvisionPayment.Name = "maskedTextBoxCashProvisionPayment";
            this.maskedTextBoxCashProvisionPayment.Size = new System.Drawing.Size(94, 29);
            this.maskedTextBoxCashProvisionPayment.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(429, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 24);
            this.label8.TabIndex = 2;
            this.label8.Text = "Сумма НДС 20/120";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(429, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(173, 24);
            this.label9.TabIndex = 3;
            this.label9.Text = "Сумма НДС 10/110";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(429, 158);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 24);
            this.label10.TabIndex = 4;
            this.label10.Text = "Сумма НДС 20";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(429, 122);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(138, 24);
            this.label11.TabIndex = 5;
            this.label11.Text = "Сумма НДС 10";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(429, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(128, 24);
            this.label12.TabIndex = 6;
            this.label12.Text = "Сумма НДС 0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(429, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(150, 24);
            this.label13.TabIndex = 7;
            this.label13.Text = "Сумма без НДС";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(3, 194);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(214, 20);
            this.label15.TabIndex = 9;
            this.label15.Text = "Встречное представление";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 158);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(192, 24);
            this.label16.TabIndex = 10;
            this.label16.Text = "Постоплата(кредит)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 122);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(119, 24);
            this.label17.TabIndex = 11;
            this.label17.Text = "Предоплата";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 86);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(221, 24);
            this.label18.TabIndex = 12;
            this.label18.Text = "Электронные средства";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 50);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(162, 24);
            this.label19.TabIndex = 13;
            this.label19.Text = "Наличная оплата";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(76, 24);
            this.label20.TabIndex = 14;
            this.label20.Text = "Оплата";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(429, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(149, 24);
            this.label21.TabIndex = 15;
            this.label21.Text = "Суммы налогов";
            // 
            // textBoxBaseName
            // 
            this.textBoxBaseName.Location = new System.Drawing.Point(3, 223);
            this.textBoxBaseName.Multiline = true;
            this.textBoxBaseName.Name = "textBoxBaseName";
            this.textBoxBaseName.Size = new System.Drawing.Size(766, 91);
            this.textBoxBaseName.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.textBoxCashier, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextCashierVatin, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.maskedTextBoxBaseNumber, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxTypeCorrection, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxTypeOperation, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.dateTimePickerBaseDate, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(766, 138);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // textBoxCashier
            // 
            this.textBoxCashier.Location = new System.Drawing.Point(153, 3);
            this.textBoxCashier.Name = "textBoxCashier";
            this.textBoxCashier.Size = new System.Drawing.Size(181, 29);
            this.textBoxCashier.TabIndex = 0;
            // 
            // maskedTextCashierVatin
            // 
            this.maskedTextCashierVatin.Location = new System.Drawing.Point(536, 3);
            this.maskedTextCashierVatin.Name = "maskedTextCashierVatin";
            this.maskedTextCashierVatin.Size = new System.Drawing.Size(181, 29);
            this.maskedTextCashierVatin.TabIndex = 0;
            // 
            // maskedTextBoxBaseNumber
            // 
            this.maskedTextBoxBaseNumber.Location = new System.Drawing.Point(536, 95);
            this.maskedTextBoxBaseNumber.Name = "maskedTextBoxBaseNumber";
            this.maskedTextBoxBaseNumber.Size = new System.Drawing.Size(181, 29);
            this.maskedTextBoxBaseNumber.TabIndex = 0;
            // 
            // comboBoxTypeCorrection
            // 
            this.comboBoxTypeCorrection.FormattingEnabled = true;
            this.comboBoxTypeCorrection.Items.AddRange(new object[] {
            "Самостоятельно",
            "По предписанию"});
            this.comboBoxTypeCorrection.Location = new System.Drawing.Point(153, 95);
            this.comboBoxTypeCorrection.Name = "comboBoxTypeCorrection";
            this.comboBoxTypeCorrection.Size = new System.Drawing.Size(181, 32);
            this.comboBoxTypeCorrection.TabIndex = 1;
            // 
            // comboBoxTypeOperation
            // 
            this.comboBoxTypeOperation.FormattingEnabled = true;
            this.comboBoxTypeOperation.Items.AddRange(new object[] {
            "Приход",
            "Расход"});
            this.comboBoxTypeOperation.Location = new System.Drawing.Point(153, 49);
            this.comboBoxTypeOperation.Name = "comboBoxTypeOperation";
            this.comboBoxTypeOperation.Size = new System.Drawing.Size(181, 32);
            this.comboBoxTypeOperation.TabIndex = 1;
            // 
            // dateTimePickerBaseDate
            // 
            this.dateTimePickerBaseDate.Location = new System.Drawing.Point(536, 49);
            this.dateTimePickerBaseDate.Name = "dateTimePickerBaseDate";
            this.dateTimePickerBaseDate.Size = new System.Drawing.Size(181, 29);
            this.dateTimePickerBaseDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Кассир";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Операция";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Тип коррекции";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(386, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 46);
            this.label4.TabIndex = 6;
            this.label4.Text = "Номер док. осн-я";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(386, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 46);
            this.label5.TabIndex = 7;
            this.label5.Text = "Дата осн-я кор.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(386, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 24);
            this.label6.TabIndex = 8;
            this.label6.Text = "ИНН кассира";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label14, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxSNO, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 173);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(766, 44);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 44);
            this.label7.TabIndex = 4;
            this.label7.Text = "Основание коррекции";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(401, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(146, 40);
            this.label14.TabIndex = 5;
            this.label14.Text = "Система налогообложения";
            // 
            // comboBoxSNO
            // 
            this.comboBoxSNO.FormattingEnabled = true;
            this.comboBoxSNO.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.comboBoxSNO.Items.AddRange(new object[] {
            "Общая",
            "Упрощенная Доход",
            "Упрощенная Доход минус Расход",
            "Единый налог на вмененный доход",
            "Единый сельскохозяйственный налог",
            "Патентная система налогообложения"});
            this.comboBoxSNO.Location = new System.Drawing.Point(571, 3);
            this.comboBoxSNO.Name = "comboBoxSNO";
            this.comboBoxSNO.Size = new System.Drawing.Size(192, 32);
            this.comboBoxSNO.TabIndex = 6;
            // 
            // DialogCheckCorrection
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(793, 691);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "DialogCheckCorrection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Чек коррекции \"Сервер ККМ\". Дополнительная проверка";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Send)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTaxNone;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTax0;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTax10;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTax20;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTax110;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSumTax120;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxCashPayment;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxElectronicPayment;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxAdvancePayment;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxCreditPayment;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxCashProvisionPayment;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox textBoxCashier;
        private System.Windows.Forms.MaskedTextBox maskedTextCashierVatin;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxBaseNumber;
        private System.Windows.Forms.ComboBox comboBoxTypeCorrection;
        private System.Windows.Forms.ComboBox comboBoxTypeOperation;
        private System.Windows.Forms.DateTimePicker dateTimePickerBaseDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxBaseName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxSNO;
        private System.Windows.Forms.PictureBox Send;
    }
}
