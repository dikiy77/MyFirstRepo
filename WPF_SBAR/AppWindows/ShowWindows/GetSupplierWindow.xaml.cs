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
    /// Логика взаимодействия для GetSupplierWindow.xaml
    /// </summary>
    public partial class GetSupplierWindow : Window {

        ShowSupliersWindow ssw;
        public Suppliers currentSupplier;

        public GetSupplierWindow() {
            InitializeComponent();
            ssw = new ShowSupliersWindow();
            this.Container.Content = ssw.Content;
        }//GetSupplierWindow

        private void CencelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }//CencelButton_Click

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            this.currentSupplier = this.ssw.SuppliersDataGrid.SelectedItem as Suppliers;
            if(this.currentSupplier == null) {
                MessageBox.Show("Выберите поставщика!");
                return;
            }//if
            this.DialogResult = true;
        }//OkButton_Click

        private void DockPanel_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter: {
                        OkButton_Click(null, null);
                    }
                    break;
                case Key.Escape: {
                        CencelButton_Click(null, null);
                    }
                    break;
                default:
                    break;
            }
        }//DockPanel_KeyDown
    }
}
