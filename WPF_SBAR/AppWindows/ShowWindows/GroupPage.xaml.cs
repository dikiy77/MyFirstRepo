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
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page {

        List<Groups> groupList = null;

        public GroupPage() {
            InitializeComponent();
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                try {
                    groupList = context.Groups.ToList();
                    GroupDataGrid.ItemsSource = groupList;
                    GroupDataGrid.SelectedIndex = 0;
                    GroupDataGrid.Focus();
                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void ChengeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            Groups item = (GroupDataGrid.SelectedItem as Groups);
            
            if (item != null) {
                AddGroupWindow agw = new AddGroupWindow(item);
                if (agw.ShowDialog() == true) {
                    Groups temp = agw.group;
                    
                    if (item.groupTitle != temp.groupTitle ||item.groupDescription != temp.groupDescription) {
                        try {
                            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                                var ppp = context.Groups.FirstOrDefault(s => s.groupID == temp.groupID);
                                //context.Groups.FirstOrDefault(s => s.groupID == temp.groupID).groupDescription = temp.groupDescription;
                                ppp.groupTitle = temp.groupTitle;
                                ppp.groupDescription = temp.groupDescription;

                                context.SaveChanges();
                                MessageBox.Show("Изменения сохранены");
                                ppp = groupList.FirstOrDefault(s=> s.groupID == temp.groupID);
                                ppp.groupTitle = temp.groupTitle;
                                ppp.groupDescription = temp.groupDescription;
                                GroupDataGrid.Items.Refresh();
                            }
                        }
                        catch (Exception ex) {

                            MessageBox.Show(ex.Message);
                        }
                        
                    }//if
                }//if
            }//if
        }

        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (groupList == null) return;
            try {

                string code = (SerchTextBox.Text).Trim();
                if (code.Length > 0) {
                    string pattern = $@"\S?{code}\S?";

                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    List<Groups> temp = (groupList.Where((s) => (regex.IsMatch(s.groupTitle) ))).ToList();
                    GroupDataGrid.ItemsSource = null;
                    GroupDataGrid.Items.Clear();
                    GroupDataGrid.ItemsSource = temp;
                }
                else {
                    GroupDataGrid.ItemsSource = null;
                    GroupDataGrid.Items.Clear();
                    GroupDataGrid.ItemsSource = groupList;
                }//else


            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }

        private void MainDockPanel_KeyDown(object sender, KeyEventArgs e) {
           
            switch (e.Key) {
                case Key.F2: {
                        AddGroupWindow ad = new AddGroupWindow();
                        if (ad.ShowDialog() == true) {
                            Groups temp = ad.group;
                            try {
                                using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {

                                    context.Groups.Add(temp);
                                    context.SaveChanges();
                                    MessageBox.Show("Изменения сохранены");
                                    groupList.Add(temp);
                                    GroupDataGrid.Items.Refresh();
                                }//using
                            }//try
                            catch (Exception ex) {

                                MessageBox.Show(ex.Message);
                            }//catch

                        }//if
                    }//case Key.F2
                    break;
                case Key.Escape: {
                        this.Content = null;
                    }//case Key.Escape
                    break;
                default:
                    break;
            }

        }
    }
}
