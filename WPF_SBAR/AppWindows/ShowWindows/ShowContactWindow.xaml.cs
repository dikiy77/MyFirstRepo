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

        }

        private void SerchByEmployeeLastNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void SerchByContactVelueTextBox_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void ChengeContactTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        private void Window_Closed(object sender, EventArgs e) {
            
        }
    }
}
