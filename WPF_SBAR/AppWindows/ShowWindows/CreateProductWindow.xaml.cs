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
    /// Логика взаимодействия для CreateProductWindow.xaml
    /// </summary>
    public partial class CreateProductWindow : Window {

       
        public Products product = new Products();

        public CreateProductWindow() {
            InitializeComponent();
        }

        public CreateProductWindow(Products product) {
            InitializeComponent();
            this.product.barcode = product.barcode;
            this.product.groupID = product.groupID;
            this.product.productTitle = product.productTitle;
            this.product.productID = product.productID;

            this.OKButton.Content = "Сохранить";
            this.Title = "Изменение продукта";
            this.BarCodeTextBox.Text = product.barcode;
            this.GroupNumberTextBox.Text = product.groupID.ToString();
            this.TitleTextBox.Text = product.productTitle;
            this.GroupNumberTextBox.IsEnabled = false;
            this.BrowseButton.IsEnabled = false;
        }//CreateProductWindow

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            if(this.TitleTextBox.Text.Trim().Length != 0 && this.GroupNumberTextBox.Text.Trim().Length != 0) {
                short groupCode;
                if (!short.TryParse( this.GroupNumberTextBox.Text.Trim(), out  groupCode)) {
                    MessageBox.Show("Проверьте номер группы");
                    return;
                }//if
                this.product.groupID = groupCode;
                this.product.barcode = this.BarCodeTextBox.Text.Trim().Length != 0 ? this.BarCodeTextBox.Text.Trim() : "Undefined";
                this.product.productTitle = this.TitleTextBox.Text.Trim();
                try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                        
                        if (context.Products.ToList().FirstOrDefault(s => (s.productTitle == this.product.productTitle) && s.groupID == this.product.groupID) != null) {
                            MessageBox.Show("Продукт с таким названием в данной группе уже существует!");
                            return;
                        }//if
                        
                    }//using
                    this.DialogResult = true;
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }//catch
                
            }//if
            else {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            
        }//OKButton_Click

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }//CancelButton_Click

        private void BrowseButton_Click(object sender, RoutedEventArgs e) {
            ShowGroupWindow sw = new ShowGroupWindow();
            if (sw.ShowDialog() == true) {
                this.GroupNumberTextBox.Text = sw.number.ToString();
            }//BrowseButton_Click
        }//BrowseButton_Click

        private void GroupNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            //MessageBox.Show("GroupNumberTextBox_TextChanged");
            if (short.TryParse(this.GroupNumberTextBox.Text.Trim(), out short tempCode)) {
                try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                        Groups groups = context.Groups.ToList().FirstOrDefault(s => s.groupID == tempCode);
                      if (groups != null) {
                            this.GroupTitleTextBox.Text = groups.groupTitle;
                        }//if

                    }//using
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }//catch
            }//if
            else {
                this.GroupTitleTextBox.Text = "";
            }
        }//GroupNumberTextBox_TextChanged
    }
}
