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
    /// Логика взаимодействия для GetProductWindow.xaml
    /// </summary>
    public partial class GetProductWindow : Window {
        public Products currentProduct;
        
        ShowProducts spWindow;

        public GetProductWindow() {
            InitializeComponent();
        }

        public GetProductWindow(short gp) {
            
            InitializeComponent();
            
            spWindow = new ShowProducts(gp);
            this.Container.Content = spWindow.Content;
        }
        private void CencelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }//CencelButton_Click

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            this.currentProduct = this.spWindow.ProductDataGrid.SelectedItem as Products;
            if (this.currentProduct == null) {
                MessageBox.Show("Выберите продукт!");
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
