using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Threading;
using System.Windows.Threading;

using WPF_SBAR.AppWindows.ShowWindows;
using WPF_SBAR.AppWindows.OrdersWindows;
using WPF_SBAR.ApplicationWindows.AuthWindows;
namespace WPF_SBAR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        enum CurrentSection {
            Scan, FilesList, Export, Import
        }//CurrentSection
        CurrentSection section = CurrentSection.Scan;
        

        double MenuLabelWidth;

        DoubleAnimation angleDA;

        List<int> indexes = new List<int>();

        Employees currentEmployeer;

        public MainWindow() {
            InitializeComponent();
            Authorize();
            MenuLabelWidth = MenuTextBlock.Width;

            angleDA = new DoubleAnimation {
                Duration = TimeSpan.FromSeconds(0.3),
                DecelerationRatio = 0.8
            };

        }//MainWindow

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e) {

            this.Cursor = Cursors.Hand;


        }//TextBlock_MouseEnter

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e) {
            this.Cursor = Cursors.Arrow;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

            GridLengthAnimation da = new GridLengthAnimation {
                Duration = TimeSpan.FromSeconds(0.3),
                DecelerationRatio = 0.3,
                AccelerationRatio = 0.7
            };

            DoubleAnimation daMenu = new DoubleAnimation {
                Duration = TimeSpan.FromSeconds(0.3),
                DecelerationRatio = 0.3,
                AccelerationRatio = 0.7
            };
            var expanders = this.SideBarWrapper.Children.OfType<Expander>();

            if (SideBarColumn.Width.Value == 200) {

                daMenu.From = MenuLabelWidth;
                daMenu.To = 0;

                da.From = new GridLength(200);
                da.To = new GridLength(30);
                int i = 0;

                indexes.Clear();

                foreach (var expander in expanders) {

                    if (expander.IsExpanded) {
                        expander.IsExpanded = false;
                        indexes.Add(i);
                    }//if

                    i++;
                }//foreach

            }//if
            else {


                da.From = new GridLength(25);
                da.To = new GridLength(200);

                daMenu.From = 0;
                daMenu.To = MenuLabelWidth;

                int i = 0;

                foreach (Expander ex in expanders) {

                    if (indexes.IndexOf(i) != -1) {
                        ex.IsExpanded = true;
                    }//if

                    i++;

                }//foreach

                indexes.Clear();

            }//else

            MenuTextBlock.BeginAnimation(WidthProperty, daMenu);

            da.Completed += (x, y) => {

                Window_SizeChanged(null, null);

            };


            SideBarColumn.BeginAnimation(ColumnDefinition.WidthProperty, da);



        }

        private void Expander_Expanded(object sender, RoutedEventArgs e) {
            Expander expander = (Expander)sender;

            var toogleButton = expander.Template.FindName("ExName", expander) as ToggleButton;
            var logo = toogleButton.Template.FindName("ExpanderLogoAngle", toogleButton) as RotateTransform;
            angleDA.From = 90;
            angleDA.To = -90;
            logo.BeginAnimation(RotateTransform.AngleProperty, angleDA);

            if (SideBarColumn.Width.Value != 200) {
                TextBlock_MouseDown(null, null);
            }//if



        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e) {
            Expander expander = (Expander)sender;

            var toogleButton = expander.Template.FindName("ExName", expander) as ToggleButton;
            var logo = toogleButton.Template.FindName("ExpanderLogoAngle", toogleButton) as RotateTransform;
            //logo.Angle = 90;

            angleDA.From = -90;
            angleDA.To = 90;
            logo.BeginAnimation(RotateTransform.AngleProperty, angleDA);


        }

        private void Expander_Loaded(object sender, RoutedEventArgs e) {



        }


        object dummy = new object();

       

        void Authorize() {

            AuthorizeWindow authorize = new AuthorizeWindow();

            if (authorize.ShowDialog() == true) {

                this.currentEmployeer = authorize.employees;

                MessageBox.Show(
                        $"Добро пожаловать, {currentEmployeer.firstName} !"
                );
            }
            else {
                MWindow_Closed(null, null);
            }
            

        }//Authorize

        private void Window_Loaded(object sender, RoutedEventArgs e) {

           

        }//Window_Loaded

        void Expand(object sender, RoutedEventArgs args) {

           
        }//Expand

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {



        }//Window_SizeChanged

        private void ScanEnableButton_MouseDown(object sender, MouseButtonEventArgs e) {

            section = CurrentSection.Scan;
            Window_SizeChanged(null, null);

        }

        private void CurrentListEnabled_MouseDown(object sender, MouseButtonEventArgs e) {
            section = CurrentSection.FilesList;
            Window_SizeChanged(null, null);
        }

        private void ExportXmlEnabled_MouseDown(object sender, MouseButtonEventArgs e) {
            section = CurrentSection.Export;
            Window_SizeChanged(null, null);
        }

        private void ImportXmlEnabled_MouseDown(object sender, MouseButtonEventArgs e) {
            section = CurrentSection.Import;
            Window_SizeChanged(null, null);
        }

        private void GroupTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            GroupPage gp = new GroupPage();
            ContentContainer.Content = gp.Content;
            this.Dispatcher.Invoke(() => {
                gp.GroupDataGrid.Focus();
            });
            
        }

        private void ProductsTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            ShowProducts showProducts = new ShowProducts();
            ContentContainer.Content = showProducts.Content;
            ContentContainer.Focus();
        }

        private void SuppliersTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            ShowSupliersWindow ssw = new ShowSupliersWindow();
            ContentContainer.Content = ssw.Content;
        }

        private void MWindow_Closed(object sender, EventArgs e) {
            
                Application.Current.Shutdown(0);
        }

        private void ContactsTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
           
            ShowContactWindow scw = new ShowContactWindow();
            ContentContainer.Content = scw.Content;
        }

        private void InPutTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            InPutOrdersWindow ipow = new InPutOrdersWindow(this.currentEmployeer);
            ContentContainer.Content = ipow.Content;
           ipow.AddNewOrderRow.Height = new GridLength(0);
            ipow.AddNewProductRow.Height = new GridLength(0);
            //ipow.MainOrdersShowRow.Height = new GridLength(0);
        }

        private void OutPutTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        private void UndistributedTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        private void DepartmentsTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        private void EmployeesTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }
    }
}
