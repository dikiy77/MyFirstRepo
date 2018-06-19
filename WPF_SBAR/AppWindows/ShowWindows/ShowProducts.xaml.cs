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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using WPF_SBAR;
namespace WPF_SBAR.AppWindows.ShowWindows {
    /// <summary>
    /// Логика взаимодействия для ShowProducts.xaml
    /// </summary>
    public partial class ShowProducts : Page {

        public List<Products> productLict = null;

        public ShowProducts() {
            InitializeComponent();
            Page_Loaded(null, null);
        }//ShowProducts


        //Контекстное меню изменение продукта
        private void ChengeProductTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            Products item = (ProductDataGrid.SelectedItem as Products);

            if (item != null) {
                CreateProductWindow agw = new CreateProductWindow(item);
                if (agw.ShowDialog() == true) {
                    Products temp = agw.product;

                    if (item.barcode != temp.barcode || item.groupID != temp.groupID || item.productTitle != temp.productTitle) {
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var ppp = context.Products.FirstOrDefault(s => s.productID == temp.productID);
                                
                                ppp.barcode = temp.barcode;
                                ppp.groupID = temp.groupID;
                                ppp.productTitle = temp.productTitle;
                                context.SaveChanges();
                                MessageBox.Show("Изменения сохранены");
                                ppp = productLict.FirstOrDefault(s => s.productID == temp.productID);
                                ppp.barcode = temp.barcode;
                                ppp.groupID = temp.groupID;
                                ppp.productTitle = temp.productTitle;
                                ProductDataGrid.Items.Refresh();
                            }
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }

                    }//if
                }//if
            }//if
        }

        //Поиск по номеру группы
        private void GroupTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (productLict == null) return;
            try {
                

                if( int.TryParse(GroupTextBox.Text != ""? GroupTextBox.Text : "0", out int code)) {
                    string pattern = $@"\d?{code}\d?";
                    if (code != 0) {
                        Regex regex = new Regex(pattern);
                        List<Products> temp = (productLict.Where((s) => (regex.IsMatch((s.groupID).ToString())))).ToList();
                        ProductDataGrid.ItemsSource = null;
                        ProductDataGrid.Items.Clear();
                        ProductDataGrid.ItemsSource = temp;
                    }//if
                    else {
                        ProductDataGrid.ItemsSource = null;
                        ProductDataGrid.Items.Clear();
                        ProductDataGrid.ItemsSource = productLict;
                    }//else
                }//if

            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }//GroupTextBox_TextChanged


        //Поиск по названию продукта
        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (productLict == null) return;
            try {

                string code = (SerchTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<Products> temp = (productLict.Where((s) => (regex.IsMatch(s.productTitle)))).ToList();
                    ProductDataGrid.ItemsSource = null;
                    ProductDataGrid.Items.Clear();
                    ProductDataGrid.ItemsSource = temp;
                }
                else {
                    ProductDataGrid.ItemsSource = null;
                    ProductDataGrid.Items.Clear();
                    ProductDataGrid.ItemsSource = productLict;
                }//else
                

            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }//SerchTextBox_TextChanged

        //Поиск по группе
        private void Button_Click(object sender, RoutedEventArgs e) {
            ShowGroupWindow sw = new ShowGroupWindow();
            if(sw.ShowDialog() == true) {
                this.GroupTextBox.Text = sw.number.ToString();
            }//if
        }//Button_Click

        // Обработка нажатия кнопок
        private void DockPanel_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                //Добавить новый продукт
                case Key.F2: {
                        CreateProductWindow cpw = new CreateProductWindow();
                        if(cpw.ShowDialog() == true) {
                            try {
                                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                    Products prod = new Products();
                                    prod.barcode = cpw.product.barcode;
                                    prod.groupID = cpw.product.groupID;
                                    prod.productTitle = cpw.product.productTitle;

                                    context.Products.Add(prod);
                                    context.SaveChanges();
                                    MessageBox.Show("Продукт добавлен!");
                                    this.productLict.Add(prod);
                                    ProductDataGrid.Items.Refresh();
                                }//using
                            }
                            catch (Exception ex) {
                                MessageBox.Show(ex.Message);
                            }//catch
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {
                    productLict = context.Products.ToList();
                    ProductDataGrid.ItemsSource = productLict;
                    ProductDataGrid.Focus();
                    ProductDataGrid.SelectedIndex = 0;
                    
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }//using
        }
    }
}
