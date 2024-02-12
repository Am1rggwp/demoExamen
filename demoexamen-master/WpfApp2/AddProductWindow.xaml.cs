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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        TradeEntities db;
        bool add_product;
        Product product;
        public AddProductWindow(bool add,Product p)
        {
            InitializeComponent();
            db = new TradeEntities();
            add_product = add;
            product = p;
            if (!add)
            {
                add_btn.Content = "Изменить товар";
                add_window.Title = "Изменение товара";
                artikul_txt.Text = product.ProductArticleNumber;
                name_txt.Text = product.ProductName;
                description_txt.Text = product.ProductDescription;
                category_txt.Text = product.ProductCategory;
                photo_txt.Text = "";
                manufacturer_txt.Text = product.ProductManufacturer;
                cost_txt.Text = product.ProductCost.ToString();
                discount_txt.Text = product.ProductDiscountAmount.ToString();
                count_txt.Text = product.ProductQuantityInStock.ToString();
                status_txt.Text = product.ProductStatus.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(add_product) 
            {
                try
                {
                    db.Products.Where(w => w.ProductArticleNumber == artikul_txt.Text).First();
                    MessageBox.Show("Товар с таким артиклем уже существует");
                }
                catch
                {
                    Product p = new Product()
                    {
                        ProductArticleNumber = artikul_txt.Text,
                        ProductName = name_txt.Text,
                        ProductDescription = description_txt.Text,
                        ProductCategory = category_txt.Text,
                        ProductPhoto = new byte[2],
                        ProductManufacturer = manufacturer_txt.Text,
                        ProductCost = int.Parse(cost_txt.Text),
                        ProductDiscountAmount = (byte)(int.Parse(discount_txt.Text)),
                        ProductQuantityInStock = int.Parse(count_txt.Text),
                        ProductStatus = status_txt.Text,
                    };
                    db.Products.Add(p);
                    db.SaveChanges();
                }
            }
            else
            {
                Product p = db.Products.Where(w => w.ProductArticleNumber == artikul_txt.Text).First();
                p.ProductArticleNumber = artikul_txt.Text;
                p.ProductName = name_txt.Text;
                p.ProductDescription = description_txt.Text;
                p.ProductCategory = category_txt.Text;
                p.ProductPhoto = new byte[2];
                p.ProductManufacturer = manufacturer_txt.Text;
                p.ProductCost = decimal.Parse(cost_txt.Text);
                p.ProductDiscountAmount = (byte)(int.Parse(discount_txt.Text));
                p.ProductQuantityInStock = int.Parse(count_txt.Text);
                p.ProductStatus = status_txt.Text;
                db.SaveChanges();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataBaseWindow dw = new DataBaseWindow();
            dw.Show();
            this.Close();
        }
    }
}
