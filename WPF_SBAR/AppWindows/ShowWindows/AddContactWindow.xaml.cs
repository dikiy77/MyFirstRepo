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
    /// Логика взаимодействия для AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window {

        public Contacts newContact = new Contacts();
        List<ContactType> contactType;

        public AddContactWindow() {
            InitializeComponent();
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {
                    this.contactType = new List<ContactType>();
                    contactType.AddRange(context.ContactType.ToList());

                    ContactTypeComboBox.ItemsSource = contactType.Select(s => s.contactType1).ToList();
                    
                    ContactTypeComboBox.SelectedIndex = 0;
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }// using
        }

        public AddContactWindow(Suppliers item) {
            InitializeComponent();

            this.contactType = new List<ContactType>();

            EmployeerTextBox.IsEnabled = false;
           // BrowseEmployeerButton.IsEnabled = false;
            SupplierTextBox.IsEnabled = false;
            SupplierTextBox.Text = item.supplierID.ToString();
            //BrowseSupplierButton.IsEnabled = false;

            SupplierCommentTextBox.Text = $"{item.companyName} : {item.contactName}";
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {

                    contactType.AddRange( context.ContactType.ToList());
                    
                    ContactTypeComboBox.ItemsSource = contactType.Select(s => s.contactType1).ToList();
                    
                    ContactTypeComboBox.SelectedIndex = 0;
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }// using

        }

        public AddContactWindow(Employees item) {
            InitializeComponent();
        }

        public AddContactWindow(SimpleContact item) {
            InitializeComponent();
            this.contactType = new List<ContactType>();

            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {

                    contactType.AddRange(context.ContactType.ToList());

                    ContactTypeComboBox.ItemsSource = contactType.Select(s => s.contactType1).ToList();

                    ContactTypeComboBox.SelectedItem = item.contactType;
                    ContactTypeComboBox.IsEnabled = false;
                    Contacts contact = context.Contacts.FirstOrDefault(c => c.ContactID == item.ContactID);

                    if (contact == null) this.DialogResult = false;
                    
                    EmployeerTextBox.Text = contact.employeeID == null ? "" : contact.employeeID.ToString();
                    EmployeerTextBox.IsEnabled = false;

                    SupplierTextBox.Text = contact.supplierID == null ? "" : contact.supplierID.ToString();
                    SupplierTextBox.IsEnabled = false;

                    DescriptionTextBox.Text = item.description;

                    ContactTextBox.Text = item.contact;

                    OKButton.Content = "Сохранить";
                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }//catch

            }// using

        }

        private void BrowseSupplierButton_Click(object sender, RoutedEventArgs e) {

        }

        private void BrowseEmployeerButton_Click(object sender, RoutedEventArgs e) {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {

            int employeerID = 0, supplierID = 0;
            if (!int.TryParse(EmployeerTextBox.Text.Trim(), out employeerID) && !int.TryParse(SupplierTextBox.Text.Trim(), out supplierID)) {
                MessageBox.Show("Заполнены не все поля" + employeerID + supplierID);
                return;
            }

            if (this.ContactTextBox.Text.Trim().Length > 0) {
                this.newContact.contact = this.ContactTextBox.Text.Trim();
                this.newContact.description = this.DescriptionTextBox.Text.Trim().Length > 0 ? this.DescriptionTextBox.Text.Trim() : null;
                //this.newContact.supplierID = supplierID != 0 ? supplierID : null;
                
                if(supplierID == 0) {
                    this.newContact.supplierID = null;
                }
                else {
                    this.newContact.supplierID = supplierID;
                }
                if (employeerID == 0) {
                    this.newContact.employeeID = null;
                }
                else {
                    this.newContact.employeeID = employeerID;
                }

                this.newContact.ContactTypeID = this.contactType[ContactTypeComboBox.SelectedIndex].contactTypeID;
                //MessageBox.Show($"{this.group.groupID} {this.group.groupTitle} {this.group.groupDescription}");
                this.DialogResult = true;

            }
            else {
                MessageBox.Show("Заполнены не все поля");
                return;
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        private void EmployeerTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if(int.TryParse(EmployeerTextBox.Text.Trim(), out int employeerID)) {
                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                    try {
                        Employees empl = context.Employees.FirstOrDefault(em => em.employeeID == employeerID);
                        if (empl == null) return;

                        EmployeerCommentTextBox.Text = empl.lastName + " " + empl.firstName;
                    }//try
                    catch (Exception ex) {

                        MessageBox.Show(ex.Message);
                    }//catch
                }//using
            }//if
        }//EmployeerTextBox_TextChanged

        private void SupplierTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (int.TryParse(SupplierTextBox.Text.Trim(), out int supID)) {
                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                    try {
                        Suppliers supl = context.Suppliers.FirstOrDefault(s => s.supplierID == supID);
                        if (supl == null) return;

                        SupplierCommentTextBox.Text = supl.companyName + " " + supl.contactName;
                    }//try
                    catch (Exception ex) {

                        MessageBox.Show(ex.Message);
                    }//catch
                }//using
            }//if
        }
    }//AddContactWindow
}//WPF_SBAR.AppWindows.ShowWindows
