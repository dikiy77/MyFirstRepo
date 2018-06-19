using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

namespace WPF_SBAR.ApplicationWindows.AuthWindows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizeWindow.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window
    {

        public Employees employees { get; set; }
       
        public AuthorizeWindow()
        {
            InitializeComponent();
            this.Login.Focus();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
                ObjectParameter result = new ObjectParameter("result", -1);
            using (SmallBusinessDBEntities context = new SmallBusinessDBEntities()) {
                int authResult = context.Authorize(Login.Text, Password.Password, result);

                if ((int)result.Value != 0) {
                    MessageBox.Show("Пользователь не найден! (" + result.Value + " )");
                    return;
                }//if
                var user = context.Users.FirstOrDefault(
                    u =>
                        u.login == Login.Text

                );

                if (user == null) {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }//if

                Employees empl = context.Employees.Find(user.ownerID);
                
                if (empl == null) {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }//if

                this.employees = empl;

                
            }//using
                this.DialogResult = true;

           

        }

        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            

            this.DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            //MessageBox.Show(e.Source.ToString());
            switch (e.Key) {
                case Key.Enter: {
                        if (e.Source == Login) {
                           // MessageBox.Show("Login");
                            this.Password.Focus();
                        } else if(e.Source == Password) {
                            //MessageBox.Show("Password");
                            Button_Click(null, null);
                        }
                    }
                    break;
                case Key.Escape: {
                        this.DialogResult = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
