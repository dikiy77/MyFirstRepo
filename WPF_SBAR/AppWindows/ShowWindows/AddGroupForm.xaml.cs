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

namespace WPF_SBAR.AppWindows.ShowWindows
{
    /// <summary>
    /// Логика взаимодействия для AddGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : Window
    {
        public Groups group = new Groups();

        public AddGroupWindow()
        {
            InitializeComponent();

        }
        public AddGroupWindow(Groups item) {
            InitializeComponent();
            this.TitleTextBox.Text = item.groupTitle;
            this.DescriptionTextBox.Text = item.groupDescription;
            this.group.groupID = item.groupID;
            this.group.groupTitle = item.groupTitle;
            this.group.groupDescription = item.groupDescription;
            this.Title = "Изменение группы товара";
            this.OKButton.Content = "Изменить";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            if (this.TitleTextBox.Text.Trim().Length > 0 && this.DescriptionTextBox.Text.Trim().Length > 0) {
                this.group.groupTitle = this.TitleTextBox.Text.Trim();
                this.group.groupDescription = this.DescriptionTextBox.Text.Trim();
                //MessageBox.Show($"{this.group.groupID} {this.group.groupTitle} {this.group.groupDescription}");
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
