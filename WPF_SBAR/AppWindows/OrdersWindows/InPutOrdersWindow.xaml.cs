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
using WPF_SBAR.AppWindows.ShowWindows;
namespace WPF_SBAR.AppWindows.OrdersWindows {
    /// <summary>
    /// Логика взаимодействия для InPutOrdersWindow.xaml
    /// </summary>
    public partial class InPutOrdersWindow : Window {

        List<InvoisesTemplate> invoicesList = null;
        Employees currentEmployeer;
        Invoices newInvoice;
        List<OrderDeteilsView> detailsList = new List<OrderDeteilsView>();

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
                                    this.InvoiceNewDataGrid.ItemsSource = this.detailsList;
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
            switch (e.Key) {
               
                case Key.Escape: {
                        this.BarcodeTextBox.Text = "";
                        this.GroupTextBox.Text = "";
                        this.GroupTitleLabel.Content = "Выбери группу";
                        this.ProductIDTextBox.Text = "";
                        this.ProductTitleLabel.Content = "Выбери продукт";
                        this.PriceTextBox.Text = "";
                        this.QuantityTextBox.Text = "";
                        this.SummTextBox.Text = "";
                        this.AddNewProductRow.Height = new GridLength(0);
                    }
                    break;
               

            }
        }

        private void BarcodeTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                        GroupTextBox.Focus();
                    } break;
                default: break;
            }//switch


        }

        private void GroupTextBox_KeyDown(object sender, KeyEventArgs e){
            switch (e.Key) {
                case Key.Enter: {
                        if (!short.TryParse(this.GroupTextBox.Text, out short groupID)) {
                            MessageBox.Show("Введите корректную группу!");
                            this.GroupTextBox.Focus();
                            return;
                        }//if

                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var group = context.Groups.FirstOrDefault(g => g.groupID == groupID);
                                this.GroupTitleLabel.Content = (object)group.groupTitle;
                            }//using
                            ProductIDTextBox.Focus();
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                       
                    }
                    break;
                case Key.F1: {
                        ShowGroupWindow sw = new ShowGroupWindow();
                        if (sw.ShowDialog() == true) {
                            this.GroupTextBox.Text = sw.number.ToString();
                        }//if
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var group = context.Groups.FirstOrDefault(g => g.groupID == sw.number);
                                this.GroupTitleLabel.Content = (object)group.groupTitle;
                            }//using
                            ProductIDTextBox.Focus();
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;
                default: break;
            }//switch
            
        }

        private void ProductIDTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {

                        if (!int.TryParse(this.ProductIDTextBox.Text, out int prodID)) {
                            MessageBox.Show("Введите корректный код!");
                            this.ProductIDTextBox.Focus();
                            return;
                        }//if

                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {

                                this.ProductTitleLabel.Content = (object)context.Products.FirstOrDefault(g => g.productID == prodID).productTitle;
                            }//using
                            PriceTextBox.Focus();
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                       
                    }
                    break;
                case Key.F1: {
                        if (!short.TryParse(this.GroupTextBox.Text, out short groupID)) {
                            MessageBox.Show("Введите корректную группу!");
                            this.GroupTextBox.Focus();
                            return;
                        }//if
                        GetProductWindow gpw = new GetProductWindow(groupID);
                        if (gpw.ShowDialog() == true) {
                            this.ProductIDTextBox.Text = gpw.currentProduct.productID.ToString();
                        }//if
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                this.ProductTitleLabel.Content = (object)context.Products.FirstOrDefault(g => g.productID == gpw.currentProduct.productID).productTitle;
                            }//using
                            PriceTextBox.Focus();
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;
                default: break;
            }//switch
            
        }

        private void PriceTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                        QuantityTextBox.Focus();
                    }
                    break;
                default: break;
            }//switch

            
        }

        private void QuantityTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                       if(!decimal.TryParse(this.PriceTextBox.Text, out decimal price)) {
                            this.PriceTextBox.Focus();
                            MessageBox.Show("Введите корректную цену!");
                            return;
                        }//if

                        if (!decimal.TryParse(this.QuantityTextBox.Text, out decimal quantity)) {
                            QuantityTextBox.Focus();
                            MessageBox.Show("Введите корректное количество!");
                            return;
                        }//if

                        this.SummTextBox.Text = $"{(price * quantity):f2}";
                        this.SummTextBox.Focus();
                    }
                    break;
                default: break;
            }//switch

        }

        private void SummTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                        OrderDeteilsView newDetails = new OrderDeteilsView() {
                            InvoiceID = this.newInvoice.invoiceID,
                            ProductID = int.Parse(this.ProductIDTextBox.Text),
                            Price = decimal.Parse(this.PriceTextBox.Text),
                            Quantity = decimal.Parse(this.QuantityTextBox.Text),
                            Barcode = this.BarcodeTextBox.Text,
                            GroupID = short.Parse(this.GroupTextBox.Text),
                            GroupTitle = (string)this.GroupTitleLabel.Content,
                            ProductTitle = (string)this.ProductTitleLabel.Content,
                            SummOfPosition = decimal.Parse(this.SummTextBox.Text)
                        };
                        this.detailsList.Add(newDetails);
                        this.InvoiceNewDataGrid.Items.Refresh();
                        this.SummOfInvoiceTextBox.Text = this.detailsList.Sum(d => d.SummOfPosition).ToString();

                        this.BarcodeTextBox.Text = "";
                        this.GroupTextBox.Text = "";
                        this.GroupTitleLabel.Content = "Выбери группу";
                        this.ProductIDTextBox.Text = "";
                        this.ProductTitleLabel.Content = "Выбери продукт";
                        this.PriceTextBox.Text = "";
                        this.QuantityTextBox.Text = "";
                        this.SummTextBox.Text = "";
                        this.BarcodeTextBox.Focus();
                    }
                    break;
                default: break;
            }//switch
        }
    }
}
