using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

namespace DemoExamen_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<DemoExamen_2.Product> ls = new List<DemoExamen_2.Product>();
        List<Manufacturer> list_manufactures= new List<Manufacturer>();
        static Grid[] grids = new Grid[6];
        static MainWindow mw;
        static Product temp = new Product();
        public MainWindow()
        {
            mw = this;
            demoetwoEntities uue = new demoetwoEntities();
            ls = uue.Products.ToList();
            list_manufactures = uue.Manufacturers.ToList();
            InitializeComponent();
            List<string> manufacturer_names = new List<string>();
            manufacturer_names.Add("Все производители");
            foreach(Manufacturer m in list_manufactures)
            {
                manufacturer_names.Add(m.Name);
            }
            manufacturer_filter.ItemsSource = manufacturer_names;
            grids = new Grid[] { first_box, second_box, third_box, four_box, five_box, six_box };
            FillGrids(ls,grids);
        }
        public static void FillGrids(List<Product> ls, Grid[] grids)
        {
            for (int i = 0; i < grids.Length - 1; i++)
            {
                int TextBlock_Count = 0;
                for (int r = 0; r < VisualTreeHelper.GetChildrenCount(grids[i]); r++)
                {
                    var obj = VisualTreeHelper.GetChild(grids[i], r);
                    switch (obj.GetType().ToString())
                    {
                        
                        case "System.Windows.Controls.TextBlock":
                            TextBlock tb = (TextBlock)obj;
                            if (TextBlock_Count == 0)
                            {
                                try { tb.Text = ls[i].Title.ToString(); } catch (Exception e) { }
                                TextBlock_Count++;
                            }
                            else
                            {
                                try { tb.Text = ls[i].Cost.ToString(); } catch { }
                            }
                            break;
                        case "System.Windows.Controls.Image":
                            
                            try
                            {
                             Image image = (Image)obj;
                             image.MouseDown += Image_MouseDown;
                             image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + '\\' + ls[i].MainImagePath, UriKind.Absolute)); } 
                            catch (Exception e) { }
                            break;

                    }
                }
            } 
        }

        private static void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddWindow aw = new AddWindow(ls[Array.FindIndex(grids, g => g == VisualTreeHelper.GetParent((DependencyObject)sender))]);
            aw.Show();
            mw.Close();
        }

        public static void ClearGrids(Grid[] grids)
        {
            for (int i = 0; i < grids.Length - 1; i++)
            {
                int TextBlock_Count = 0;
                for (int r = 0; r < VisualTreeHelper.GetChildrenCount(grids[i]); r++)
                {
                    var obj = VisualTreeHelper.GetChild(grids[i], r);
                    switch (obj.GetType().ToString())
                    {
                        case "System.Windows.Shapes.Rectangle":
                                Rectangle rc = (Rectangle)obj;
                                rc.Fill = Brushes.White;
                            break;
                        case "System.Windows.Controls.TextBlock":
                            TextBlock tb = (TextBlock)obj;
                            if (TextBlock_Count == 0)
                            {
                                tb.Text = "Наименование товара";
                                TextBlock_Count++;
                            }
                            else
                            {
                                tb.Text = "-";
                            }
                            break;
                        case "System.Windows.Controls.Image":
                            Image image = (Image)obj;
                            image.Source = null;
                            break;

                    }
                }
            }
        }

        private void search_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Product> list = new List<Product>();
            ClearGrids(grids);
            if(manufacturer_filter.SelectedIndex != -1 && manufacturer_filter.SelectedIndex != 0 && sort_box.SelectedIndex != -1)
            {
                if(sort_box.SelectedIndex == 0)
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex
                        && (p.Title.Contains(search_txt.Text.ToString())
                        || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids);
                } else
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex && (p.Title.Contains(search_txt.Text.ToString())
                                        || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
                
            }
            else if (sort_box.SelectedIndex != -1)
            {
                if(sort_box.SelectedIndex == 0)
                {
                   list = ls.Where(p => (p.Title.Contains(search_txt.Text.ToString())
                                                            || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids);
                } else
                {
                    list = ls.Where(p => (p.Title.Contains(search_txt.Text.ToString())
                                        || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
               list = ls.Where(p => (p.Title.Contains(search_txt.Text.ToString())
                    || p.Description.Contains(search_txt.Text.ToString()))).OrderBy(p => p.Cost).ToList();
               FillGrids(list, grids);
            }
            else if (manufacturer_filter.SelectedIndex != -1 && manufacturer_filter.SelectedIndex != 0)
            {
                list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex && (p.Title.Contains(search_txt.Text.ToString())
                                                            || p.Description.Contains(search_txt.Text.ToString()))).ToList();
                FillGrids(list, grids);
            }
            else
            {
                list = ls.Where(p => p.Title.Contains(search_txt.Text.ToString())
                || p.Description.Contains(search_txt.Text.ToString())).ToList();
                FillGrids(list, grids);
            }
            count_items_txt.Content = $"{list.Count.ToString()} из {ls.Count.ToString()}";
        }

        private void manufacturer_filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Product> list = new List<Product>();
            if (manufacturer_filter.SelectedValue == "Все производители")
            {
                list = ls.Where(p => p.Title.Contains(search_txt.Text.ToString())
               || p.Description.Contains(search_txt.Text.ToString())).ToList();
                ClearGrids(grids);
                FillGrids(list, grids);
            } else
            {
                try 
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex && (p.Title.Contains(search_txt.Text.ToString())
                    || p.Description.Contains(search_txt.Text.ToString()))).ToList();
                ClearGrids(grids);
                FillGrids(list, grids);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
            }
            count_items_txt.Content = $"{list.Count.ToString()} из {ls.Count.ToString()}";
        }

        private void sort_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Product> list = new List<Product>();
            if (sort_box.SelectedIndex == 1)
            {
                ClearGrids(grids);
                if (search_txt.Text != "" && manufacturer_filter.SelectedIndex != -1)
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex && (p.Title.Contains(search_txt.Text.ToString())
                        || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
                else if (search_txt.Text != "")
                {
                    list = ls.Where(p => (p.Title.Contains(search_txt.Text.ToString())
                        || p.Description.Contains(search_txt.Text.ToString()))).OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);  
                }
                else if (manufacturer_filter.SelectedIndex != -1 && manufacturer_filter.SelectedIndex != 0)
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex).OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
                else
                {
                    list = ls.OrderBy(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
            }
            else
            {
                ClearGrids(grids);
                if( search_txt.Text != "" && manufacturer_filter.SelectedIndex != -1)
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex && (p.Title.Contains(search_txt.Text.ToString())
                         || p.Description.Contains(search_txt.Text.ToString()))).ToList().OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids); 
                } else if(search_txt.Text != "") 
                {
                    list = ls.Where(p => (p.Title.Contains(search_txt.Text.ToString())
                        || p.Description.Contains(search_txt.Text.ToString()))).OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids);
                } else if(manufacturer_filter.SelectedIndex != -1 && manufacturer_filter.SelectedIndex != 0)
                {
                    list = ls.Where(p => p.ManufacturerID == manufacturer_filter.SelectedIndex).OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids);
                } else
                {
                    list = ls.OrderByDescending(p => p.Cost).ToList();
                    FillGrids(list, grids);
                }
                
            }
            count_items_txt.Content = $"{list.Count.ToString()} из {ls.Count.ToString()}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddWindow aw = new AddWindow();
            aw.Show();
            this.Close();
        }
    }
}
