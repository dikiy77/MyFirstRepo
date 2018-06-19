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
    /// Логика взаимодействия для ShowGroupWindow.xaml
    /// </summary>
    public partial class ShowGroupWindow : Window {

        public short number;
        GroupPage gp = new GroupPage();

        public ShowGroupWindow() {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
            this.Contaner.Content = gp.Content;
        }

        private void OK_Click(object sender, RoutedEventArgs e) {
            this.number = (this.gp.GroupDataGrid.SelectedItem as Groups).groupID;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }
    }
}
