using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        TradeEntities db;
        public MainWindow()
        {
            InitializeComponent();
            db = new TradeEntities();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = db.Users.Where(w => w.UserLogin == login_txt.Text).First();
                if (user != null && user.UserPassword == password_txt.Text)
                {
                    CaptchaWindow cw = new CaptchaWindow();
                    cw.Show();
                    this.Close();
                }
            } catch {
                MessageBox.Show("Ошибка в логине или пароле");
            }

        }     
    }
}
