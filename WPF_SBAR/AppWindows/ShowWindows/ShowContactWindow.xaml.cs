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

using WPF_SBAR.Entity;

namespace WPF_SBAR.AppWindows.ShowWindows {
    /// <summary>
    /// Логика взаимодействия для ShowContactWindow.xaml
    /// </summary>
    public partial class ShowContactWindow : Window {

        List<SimpleContact>contactsList = null;

        Suppliers currentSupplier;
        List<ContactType> contactType;

        public ShowContactWindow() {
            InitializeComponent();
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {

                    
                            this.contactType = context.ContactType.ToList();

                    //SerchByContactTypeComboBox.ItemsSource = contactType.Select(s => s.contactType1).ToList();
                    SerchByContactTypeComboBox.Items.Add("Все");
                    contactType.Select(s => s.contactType1).ToList().ForEach(x => {
                        SerchByContactTypeComboBox.Items.Add(x);
                    });
                    SerchByContactTypeComboBox.SelectedIndex = 0;
                      

                    contactsList = (from c in context.Contacts
                                    join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
                                    join s in context.Suppliers on c.supplierID equals s.supplierID

                                    select new SimpleContact() {
                                        ContactID = c.ContactID,
                                        contactType = ct.contactType1,
                                        companyName = s.companyName,
                                        contact = c.contact,
                                        description = c.description ?? "",
                                        //firstName = "",
                                        //lastName = ""
                                    }).ToList();
                    var test = (from c in context.Contacts
                                join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
                                join e in context.Employees on c.employeeID equals e.employeeID

                                select new SimpleContact() {
                                    ContactID = c.ContactID,
                                    contactType = ct.contactType1,
                                    //companyName = "",
                                    contact = c.contact,
                                    description = c.description ?? "",
                                    firstName = e.firstName,
                                    lastName = e.lastName
                                }).ToList();
                    //contactsList.AddRange(tesr);

                    contactsList.AddRange(test);
                    ContactsDataGrid.ItemsSource = contactsList;
                    ContactsDataGrid.SelectedIndex = 0;
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }// using
            //using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
            //    try {
            //        var tesr = from c in context.Contacts
            //                        join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
            //                        join s in context.Suppliers on c.supplierID equals s.supplierID

            //                        select new {
            //                            ContactID = c.ContactID,
            //                            contactType = ct.contactType1,
            //                            companyName = s.companyName,
            //                            contact = c.contact,
            //                            description = c.description,
            //                            firstName = "",
            //                            lastName = ""
            //                        };
            //        var test = from c in context.Contacts
            //                    join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
            //                    join e in context.Employees on c.employeeID equals e.employeeID

            //                    select new {
            //                        ContactID = c.ContactID,
            //                        contactType = ct.contactType1,
            //                        companyName = "",
            //                        contact = c.contact,
            //                        description = c.description,
            //                        firstName = e.firstName,
            //                        lastName = e.lastName
            //                    };

            //        var res = tesr.ToList();
            //        //contactsList.AddRange(tesr);

            //        res.AddRange(test.ToList());
            //        ContactsDataGrid.ItemsSource = res;
            //        ContactsDataGrid.SelectedIndex = 0;
            //    }//try
            //    catch (Exception ex) {

            //        MessageBox.Show(ex.Message);
            //    }//catch

            //}// using
        }//ShowContactWindow

        public ShowContactWindow(Suppliers sup) {
            InitializeComponent();
            this.currentSupplier = sup;
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {
                    this.contactType = context.ContactType.ToList();

                    //SerchByContactTypeComboBox.ItemsSource = contactType.Select(s => s.contactType1).ToList();
                    SerchByContactTypeComboBox.Items.Add("Все");
                    contactType.Select(s => s.contactType1).ToList().ForEach(x => {
                        SerchByContactTypeComboBox.Items.Add(x);
                    });
                    SerchByContactTypeComboBox.SelectedIndex = 0;

                    contactsList = (from c in context.Contacts
                                    join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
                                    join s in context.Suppliers on c.supplierID equals s.supplierID
                                    where s.supplierID == this.currentSupplier.supplierID
                                    select new SimpleContact() {
                                        ContactID = c.ContactID,
                                        contactType = ct.contactType1,
                                        companyName = s.companyName,
                                        contact = c.contact,
                                        description = c.description ?? "",
                                        //firstName = "",
                                        //lastName = ""
                                    }).ToList();
                    //contactsList.AddRange(tesr);

                    
                    ContactsDataGrid.ItemsSource = contactsList;
                    ContactsDataGrid.SelectedIndex = 0;
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }// using
        }

        private void SerchByContactTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (contactsList == null) return;

            int index = SerchByContactTypeComboBox.SelectedIndex;
            if (index == 0) {
                ContactsDataGrid.ItemsSource = contactsList;
                ContactsDataGrid.SelectedIndex = 0;
            }//if
            else {
                try {
                    using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {

                        List<SimpleContact> temp;
                        byte id = this.contactType[index - 1].contactTypeID;

                                temp = (from c in context.Contacts
                                            join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
                                            join s in context.Suppliers on c.supplierID equals s.supplierID
                                        where c.ContactTypeID == id
                                        select new SimpleContact() {
                                                ContactID = c.ContactID,
                                                contactType = ct.contactType1,
                                                companyName = s.companyName,
                                                contact = c.contact,
                                                description = c.description ?? "",
                                                //firstName = "",
                                                //lastName = ""
                                            }).ToList();
                            var test = (from c in context.Contacts
                                        join ct in context.ContactType on c.ContactTypeID equals ct.contactTypeID
                                        join em in context.Employees on c.employeeID equals em.employeeID
                                        where c.ContactTypeID == id
                                        select new SimpleContact() {
                                            ContactID = c.ContactID,
                                            contactType = ct.contactType1,
                                            //companyName = "",
                                            contact = c.contact,
                                            description = c.description ?? "",
                                            firstName = em.firstName,
                                            lastName = em.lastName
                                        }).ToList();
                        //contactsList.AddRange(tesr);

                            temp.AddRange(test);
                            ContactsDataGrid.ItemsSource = temp;
                            ContactsDataGrid.SelectedIndex = 0;

                    }// using
                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch
            }

        }

        private void SerchByCompanyNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (contactsList == null) return;
            try {

                string code = (SerchByCompanyNameTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<SimpleContact> temp = (contactsList.Where((s) => (regex.IsMatch(s.companyName)))).ToList();
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = temp;
                }
                else {
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = contactsList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }//catch
        }//SerchByCompanyNameTextBox_TextChanged

        private void SerchByEmployeeLastNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (contactsList == null) return;
            try {

                string code = (SerchByEmployeeLastNameTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<SimpleContact> temp = (contactsList.Where((s) => (regex.IsMatch(s.lastName)))).ToList();
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = temp;
                }
                else {
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = contactsList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }//catch
        }//SerchByEmployeeLastNameTextBox_TextChanged

        private void SerchByContactVelueTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (contactsList == null) return;
            try {

                string code = (SerchByContactVelueTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<SimpleContact> temp = (contactsList.Where((s) => (regex.IsMatch(s.contact)))).ToList();
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = temp;
                }
                else {
                    ContactsDataGrid.ItemsSource = null;
                    ContactsDataGrid.Items.Clear();
                    ContactsDataGrid.ItemsSource = contactsList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }//catch
        }//SerchByContactVelueTextBox_TextChanged

        private void ChengeContactTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            if (ContactsDataGrid.SelectedItem is SimpleContact contact) {
                AddContactWindow acw = new AddContactWindow(contact);
                if (acw.ShowDialog() == true) {
                    Contacts newcont = acw.newContact;
                        if(newcont.contact != contact.contact || newcont.description != contact.description) {
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var ppp = context.Contacts.FirstOrDefault(s => s.ContactID == contact.ContactID);

                                ppp.contact = newcont.contact;
                                ppp.description = newcont.description;
                                
                                context.SaveChanges();
                                MessageBox.Show("Изменения сохранены");
                                
                                contact.contact = newcont.contact; ;
                                contact.description = newcont.description;

                                ContactsDataGrid.Items.Refresh();
                                
                            }
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                    }//if

                }//if
            }//if
        }

        private void Window_Closed(object sender, EventArgs e) {
            
        }
    }
}
