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
using System.Text.RegularExpressions;

using WPF_SBAR;
namespace WPF_SBAR.AppWindows.ShowWindows {
    /// <summary>
    /// Логика взаимодействия для ShowSupliersWindow.xaml
    /// </summary>
    public partial class ShowSupliersWindow : Window {

        List<Suppliers> suppList = null;

        public ShowSupliersWindow() {
            InitializeComponent();
            try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                        
                            suppList = context.Suppliers.ToList();
                            SuppliersDataGrid.ItemsSource = suppList;
                            SuppliersDataGrid.SelectedIndex = 0;
                       

                    }//using

            }//try
            catch (Exception ex) {

                    MessageBox.Show(ex.Message);
            }//catch


        }//ShowSupliersWindow

        private void AddContactTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            Suppliers supplier = SuppliersDataGrid.SelectedItem as Suppliers;
            if (supplier == null) return;
            AddContactWindow scw = new AddContactWindow(supplier);
            if (scw.ShowDialog() == true) {
                try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {

                        Contacts newContacts = new Contacts();
                        newContacts.contact = scw.newContact.contact;
                        newContacts.ContactTypeID = scw.newContact.ContactTypeID;
                        newContacts.description = scw.newContact.description;
                        newContacts.employeeID = scw.newContact.employeeID;
                        newContacts.supplierID = scw.newContact.supplierID;
                        context.Contacts.Add(newContacts);
                        context.SaveChanges();
                        MessageBox.Show("Контакт добавлен");
                        
                    }
                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void ChengeContactTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        // Вызов окна демонстрации контактов
        private void ShowContactsTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

            Suppliers supplier = SuppliersDataGrid.SelectedItem as Suppliers;
            if (supplier == null) return;
            ShowContactWindow scw = new ShowContactWindow(supplier);
            scw.Show();
        }//ShowContactsTextBlock_MouseDown

        // Поиск по контактному лицу
        private void SerchByContactNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (suppList == null) return;
            try {

                string code = (SerchByContactNameTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<Suppliers> temp = (suppList.Where((s) => (regex.IsMatch(s.contactName)))).ToList();
                    SuppliersDataGrid.ItemsSource = null;
                    SuppliersDataGrid.Items.Clear();
                    SuppliersDataGrid.ItemsSource = temp;
                }
                else {
                    SuppliersDataGrid.ItemsSource = null;
                    SuppliersDataGrid.Items.Clear();
                    SuppliersDataGrid.ItemsSource = suppList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }//SerchByContactNameTextBox_TextChanged

        // Поиск по названию компании
        private void SerchByCompanyNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (suppList == null) return;
            try {

                string code = (SerchByCompanyNameTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<Suppliers> temp = (suppList.Where((s) => (regex.IsMatch(s.companyName)))).ToList();
                    SuppliersDataGrid.ItemsSource = null;
                    SuppliersDataGrid.Items.Clear();
                    SuppliersDataGrid.ItemsSource = temp;
                }
                else {
                    SuppliersDataGrid.ItemsSource = null;
                    SuppliersDataGrid.Items.Clear();
                    SuppliersDataGrid.ItemsSource = suppList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }//SerchByCompanyNameTextBox_TextChanged

        private void ChengeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            Suppliers item = (SuppliersDataGrid.SelectedItem as Suppliers);

            if (item != null) {
                AddSuppliersWindow asw = new AddSuppliersWindow(item);
                if (asw.ShowDialog() == true) {
                    Suppliers temp = asw.sup;

                    if (item.companyName != temp.companyName || item.contactName != temp.contactName) {
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var ppp = context.Suppliers.FirstOrDefault(s => s.supplierID == temp.supplierID);
                                //context.Groups.FirstOrDefault(s => s.groupID == temp.groupID).groupDescription = temp.groupDescription;
                                ppp.companyName = temp.companyName;
                                ppp.contactName = temp.contactName;

                                context.SaveChanges();
                                MessageBox.Show("Изменения сохранены");
                                ppp = suppList.FirstOrDefault(s => s.supplierID == temp.supplierID);
                                ppp.companyName = temp.companyName;
                                ppp.contactName = temp.contactName;
                                SuppliersDataGrid.Items.Refresh();
                            }
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }

                    }//if
                }//if
            }//if
        }//ChengeTextBlock_MouseDown

        private void MainSupDockPanel_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.F2: {
                        AddSuppliersWindow adsw = new AddSuppliersWindow();
                        if (adsw.ShowDialog() == true) {
                            Suppliers temp = adsw.sup;
                            try {
                                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {

                                    context.Suppliers.Add(temp);
                                    context.SaveChanges();
                                    MessageBox.Show("Изменения сохранены");
                                    suppList.Add(temp);
                                    SuppliersDataGrid.Items.Refresh();
                                }//using
                            }//try
                            catch (Exception ex) {

                                MessageBox.Show(ex.Message);
                            }//catch

                        }//if
                    }//case Key.F2
                    break;

                default:
                    break;
            }//switch
        }//MainSupDockPanel_KeyDown

    }//ShowSupliersWindow
}//namespace
