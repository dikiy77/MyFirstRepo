using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WPF_SBAR.AppWindows.ShowWindows;
namespace WPF_SBAR.AppWindows.OrdersWindows
{
    /// <summary>
    /// Логика взаимодействия для CreateNewInvoiceWindow.xaml
    /// </summary>
    public partial class CreateNewInvoiceWindow : Window
    {
        public Invoices newInvoice = new Invoices();
        bool loaded = false;
        Employees currentEmployeer;
        public CreateNewInvoiceWindow(Employees empl)
        {
            InitializeComponent();
            OrderDatePicker.Text = DateTime.Now.ToShortDateString();
            PayDatePicker.Text = DateTime.Now.ToShortDateString();
            IsPayFlagCheckBox.IsChecked = true;
            this.currentEmployeer = empl;
        }

        private void MainDockPanel_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                        OkButton_Click(null, null);
                    }
                    break;
                case Key.Escape: {
                        CancelButton_Click(null, null);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {

            if (!int.TryParse(SupplierIDTextBox.Text, out int supId)) {
                MessageBox.Show("Укажите код поставщика");
                return;
            }//if
            newInvoice.date = DateTime.Parse(OrderDatePicker.Text);
            newInvoice.supplierID = supId;
            newInvoice.employeeID = currentEmployeer.employeeID;
            newInvoice.invoiceNumber = NumberOfOrderSupplierNameTextBox.Text.Trim().Length > 0 ? NumberOfOrderSupplierNameTextBox.Text.Trim() : "Б/Н";
            newInvoice.official = (bool)OficialFlagCheckBox.IsChecked;
            newInvoice.paid = (bool)IsPayFlagCheckBox.IsChecked;
            newInvoice.payToDate = DateTime.Parse(PayDatePicker.Text);

            this.DialogResult = true;
        }//OkButton_Click

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }//CancelButton_Click


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            loaded = true;
        }//Window_Loaded

        private void SupplierIDTextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            if (!loaded) return;
            if (SupplierIDTextBox.Text == "F2-поиск") {
                SupplierIDTextBox.Text = "";
            }
        }//SupplierIDTextBox_SelectionChanged

        private void SupplierIDTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (SupplierIDTextBox.Text == "F2-поиск") return;
            if (int.TryParse(SupplierIDTextBox.Text, out int supId)) {
                try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                        Suppliers mySuppliers = context.Suppliers.ToList().FirstOrDefault(s => (s.supplierID == supId));
                        if (mySuppliers != null) {
                            SupplierNameLabel.Content = mySuppliers.companyName;
                        }//if

                    }//using
                    
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }//catch
            }//if
        }//SupplierIDTextBox_TextChanged

        private void SupplierIDTextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.F2) {
                GetSupplierWindow gsw = new GetSupplierWindow();
                if(gsw.ShowDialog() == true) {
                    newInvoice.supplierID = gsw.currentSupplier.supplierID;
                    SupplierIDTextBox.Text = gsw.currentSupplier.supplierID.ToString();

                    
                }//if
            }//if
        }//SupplierIDTextBox_KeyDown
    }
}
