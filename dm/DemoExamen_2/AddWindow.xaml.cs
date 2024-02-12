using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace DemoExamen_2
{
    public partial class AddWindow : Window
    {


        demoetwoEntities dbEn = new demoetwoEntities();
        List<string> manufacturer_names = new List<string>();
        Product product = new Product();



        public AddWindow(Product p)
        {
            InitializeComponent();
            product = p;
            id_txt.Text = $"ID: {p.ID}";
            title_box.Text = p.Title;
            description_box.Text = p.Description;
            cost_box.Text = p.Cost.ToString();
            foreach (Manufacturer m in dbEn.Manufacturers.ToList())
            {
                manufacturer_names.Add(m.Name);
            }
            manufacturer_box.ItemsSource = manufacturer_names;
            manufacturer_box.SelectedIndex = (int)p.ManufacturerID;
            main_image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + '\\' + p.MainImagePath, UriKind.Absolute));
        }
        public AddWindow()
        {
            InitializeComponent();
            product.ID = dbEn.Products.Count() + 1;
            id_txt.Text = $"ID:{product.ID}";
            main_image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + '\\' + "Товары автосервиса\\3DA0B713.jpg", UriKind.Absolute));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            product.Title = title_box.Text;
            product.Description = description_box.Text;
            product.Cost = int.Parse(cost_box.Text);
            product.MainImagePath = main_image.Source.ToString().Substring(Environment.CurrentDirectory.Length+9);
            dbEn.Products.AddOrUpdate(product);
            dbEn.SaveChanges();
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void main_image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + '\\' + "Товары автосервиса";
            if (openFileDialog.ShowDialog() == true)
            {
                main_image.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
            }
               
        }
    }
}
