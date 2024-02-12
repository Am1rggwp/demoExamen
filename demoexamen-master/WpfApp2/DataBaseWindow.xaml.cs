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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для DataBaseWindow.xaml
    /// </summary>
    public partial class DataBaseWindow : Window
    {
        TradeEntities db = new TradeEntities();
        
        
        public DataBaseWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = db.Products.ToList();
            List<Product> list = db.Products.ToList();
            List<String> manufactures = new List<string>();
            manufactures.Add("Все производители");
            foreach (Product product in list)
            {
                if (!manufactures.Contains(product.ProductManufacturer))
                {
                    manufactures.Add(product.ProductManufacturer.ToString());
                }
            }
            manufacturer_cmb_bx.ItemsSource = manufactures; 
            manufacturer_cmb_bx.SelectedIndex = 0;
        }

        private void search_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Product> products = db.Products.Where(w=>w.ProductName.Contains(search_txt.Text)).ToList();
            try
            {
                if (cmb_bx_sort_price.SelectedIndex == 0)
                {
                    products = products.OrderBy(w => w.ProductCost).ToList();
                }
                else if (cmb_bx_sort_price.SelectedIndex == 1)
                {
                    products = products.OrderByDescending(w => w.ProductCost).ToList();
                }
            }
            catch { }
            if (manufacturer_cmb_bx.SelectedValue != "Все производители")
            {
                products = products.Where(w => w.ProductManufacturer == manufacturer_cmb_bx.SelectedValue).ToList();
            }
            dataGrid.ItemsSource = products;
        }

        private void cmb_bx_sort_price_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmb_bx_sort_price.SelectedIndex == 0)
            {
                dataGrid.ItemsSource = db.Products.OrderBy(w => w.ProductCost).ToList();
            }
            else
            {
                dataGrid.ItemsSource = db.Products.OrderByDescending(w => w.ProductCost).ToList();
            }
        }

        private void manufacturer_cmb_bx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Product> products = db.Products.ToList();
            if(manufacturer_cmb_bx.SelectedValue != "Все производители")
            {
                products = products.Where(w => w.ProductManufacturer == manufacturer_cmb_bx.SelectedValue).ToList();
            }
            else
            {
                products = products.ToList();
            }
            try
            {
                if (cmb_bx_sort_price.SelectedIndex == 0)
                {
                    products = products.OrderBy(w => w.ProductCost).ToList();
                }
                else if (cmb_bx_sort_price.SelectedIndex == 1)
                {
                    products = products.OrderByDescending(w => w.ProductCost).ToList();
                }
            }
            catch { }
            dataGrid.ItemsSource = products;
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Product p = (Product)dataGrid.SelectedItem;
                db.Products.Remove(p);
                db.SaveChanges();
                dataGrid.ItemsSource = db.Products.ToList();    
            } catch { }
        }

        private void add_btn_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow apw = new AddProductWindow(true,new Product());
            apw.Show();
            this.Close();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((bool)check_edit.IsChecked)
            {
                AddProductWindow apw = new AddProductWindow(false, (Product)dataGrid.SelectedItem);
                apw.Show();
                this.Close();
            }
            else { }
        }
    }
}
