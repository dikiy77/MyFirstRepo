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

using WPF_SBAR.Entity;
namespace WPF_SBAR.AppWindows.OrdersWindows {
    /// <summary>
    /// Логика взаимодействия для InPutOrdersWindow.xaml
    /// </summary>
    public partial class InPutOrdersWindow : Window {

        List<InvoisesTemplate> invoicesList = null;
        Employees currentEmployeer;
        Invoices newInvoice;
        public InPutOrdersWindow(Employees em) {
            InitializeComponent();
            OrderDatePicker.Text = DateTime.Now.ToShortDateString();
            DateTime date = DateTime.Now;
            this.currentEmployeer = em;
            date = DateTime.Parse("10.06.2018");

            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                invoicesList = (from i in context.Invoices
                                join s in context.Suppliers on i.supplierID equals s.supplierID
                                where i.date.Day == date.Day &&
                                      i.date.Month == date.Month &&
                                      i.date.Year == date.Year
                                select new InvoisesTemplate {
                                    CompanyName = s.companyName,
                                    Date = i.date,
                                    EmployeeID = i.employeeID,
                                    InvoiceID = i.invoiceID,
                                    InvoiceNumber = i.invoiceNumber,
                                    Official = i.official,
                                    Overhead = i.overhead,
                                    Paid = i.paid,
                                    PayToDate = i.payToDate,
                                    SupplierID = i.supplierID,
                                    TotalSumm = (from inv in context.Invoices
                                                 join id in context.InvoiceDetails on inv.invoiceID equals id.invoiceID
                                                 where inv.invoiceID == i.invoiceID
                                                 select new {
                                                     quantity = id.quantity,
                                                     price = id.price
                                                 }).Sum(s => (s.price*s.quantity))
                                                 }).ToList();

                InvoicesDataGrid.ItemsSource = invoicesList;
                InvoicesDataGrid.SelectedIndex = 0;
                if (InvoicesDataGrid.SelectedIndex != -1) {
                    InvoicesDataGrid.Focus();
                }
            }//using
        }
        private void ChengeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
           
        }

        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e) {
           
        }

        private void MainDockPanel_KeyDown(object sender, KeyEventArgs e) {
            
            switch (e.Key) {
                case Key.F2: {
                        CreateNewInvoiceWindow cniw = new CreateNewInvoiceWindow(this.currentEmployeer);
                        if(cniw.ShowDialog() == true) {
                            this.newInvoice = cniw.newInvoice;
                            try {
                                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                    context.Invoices.Add(newInvoice);
                                    context.SaveChanges();
                                    this.newInvoice = context.Invoices.OrderByDescending(inv => inv.invoiceID).First();
                                    this.newInvoice.Suppliers = context.Suppliers.FirstOrDefault(s => s.supplierID == this.newInvoice.supplierID);
                                    FillInvoiceFields();
                                    this.AddNewOrderRow.Height = this.MainOrdersShowRow.Height;
                                    this.MainOrdersShowRow.Height = new GridLength(0);
                                    AddNewProductRow.Height = new GridLength(130);
                                }//using

                            }
                            catch (Exception ex) {
                                MessageBox.Show(ex.Message);
                            }//catch
                        }//if
                    }//case Key.F2
                    break;
                
                default:
                    break;
            }

        }

        void FillInvoiceFields() {
            CurrentOrderDatePicker.Text = this.newInvoice.date.ToShortDateString();
            CurrentSupplier.Text = this.newInvoice.Suppliers.companyName;
            CurrentOrderNumberTextBox.Text = this.newInvoice.invoiceID.ToString();
            CurrentOrderNumberAtSupplierTextBox.Text = this.newInvoice.invoiceNumber;
            CurrentPayDayTextBox.Text = ((DateTime)this.newInvoice.payToDate).ToShortDateString();
            InvoiceTypeTextBox.Text = this.newInvoice.official ? "Первая" : "Вторая";
            PaymentStatusTextBox.Text = this.newInvoice.paid ? "Оплачено" : "Неоплачено";
        }//FillInvoiceFields

        private void OrderDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {

            if (invoicesList == null || !DateTime.TryParse(sender.ToString(), out DateTime date)) return;
            
            MessageBox.Show(date.ToString());
            
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                invoicesList = (from i in context.Invoices
                                join s in context.Suppliers on i.supplierID equals s.supplierID
                                where i.date.Day == date.Day &&
                                      i.date.Month == date.Month &&
                                      i.date.Year == date.Year
                                select new InvoisesTemplate {
                                    CompanyName = s.companyName,
                                    Date = i.date,
                                    EmployeeID = i.employeeID,
                                    InvoiceID = i.invoiceID,
                                    InvoiceNumber = i.invoiceNumber,
                                    Official = i.official,
                                    Overhead = i.overhead,
                                    Paid = i.paid,
                                    PayToDate = i.payToDate,
                                    SupplierID = i.supplierID,
                                    TotalSumm = (from inv in context.Invoices
                                                 join id in context.InvoiceDetails on inv.invoiceID equals id.invoiceID
                                                 where inv.invoiceID == i.invoiceID
                                                 select new {
                                                     quantity = id.quantity,
                                                     price = id.price
                                                 }).Sum(s => (s.price * s.quantity))
                                }).ToList();

                InvoicesDataGrid.ItemsSource = invoicesList;
                InvoicesDataGrid.SelectedIndex = 0;
                if(InvoicesDataGrid.SelectedIndex != -1) {
                    InvoicesDataGrid.Focus();
                }
            }//using
        }

        private void AddNewOrderDockPanel_KeyDown(object sender, KeyEventArgs e) {

        }


    }
}
