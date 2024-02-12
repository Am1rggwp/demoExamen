using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class CaptchaWindow : Window
    {
        string cap = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
        public CaptchaWindow()
        {
            InitializeComponent();
            captcha_txt.Text = GenCap(cap.ToCharArray(), 6);
        }

        private void captcha_btn_Click(object sender, RoutedEventArgs e)
        {

            
            if(captcha.Text == captcha_txt.Text)
            {
                DataBaseWindow dbw = new DataBaseWindow();
                dbw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не правильно введена каптча");
                Thread.Sleep(1000);
            }
        }
        public string GenCap(char[] c, int length)
        {
            Random rnd = new Random();
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += c[rnd.Next(c.Length)];
            }
            return result;
        }
    }
}
