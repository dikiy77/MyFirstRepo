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

namespace WPF_SBAR.AppWindows.ShowWindows {
    /// <summary>
    /// Логика взаимодействия для AddSuppliersWindow.xaml
    /// </summary>
    public partial class AddSuppliersWindow : Window {

        public Suppliers sup = new Suppliers();

        public AddSuppliersWindow() {
            InitializeComponent();
        }

        public AddSuppliersWindow(Suppliers item) {
            InitializeComponent();
            this.Title = "Изменение поставщика";
            this.OKButton.Content = "Изменить";
            this.TitleTextBox.Text = item.companyName;
            this.DescriptionTextBox.Text = item.contactName;
            this.sup.supplierID = item.supplierID;
            this.sup.companyName = item.companyName;
            this.sup.contactName = item.contactName;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            if (this.TitleTextBox.Text.Trim().Length > 0 && this.DescriptionTextBox.Text.Trim().Length > 0) {
                this.sup.companyName = this.TitleTextBox.Text.Trim();
                this.sup.contactName = this.DescriptionTextBox.Text.Trim();
                
                this.DialogResult = true;

            }
            else {
                MessageBox.Show("Заполнены не все поля");
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
