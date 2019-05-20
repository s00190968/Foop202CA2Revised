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

namespace DatabaseTestFoop202
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AdventureLiteEntities db = new AdventureLiteEntities();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnEx1_Click_1(object sender, RoutedEventArgs e)
        {
            var q = db.Products.OrderBy(p => p.Name).Select(p => p.Name);

            lbxCustomersQ1.ItemsSource = q.ToList();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            var q = db.Customers.OrderBy(c => c.CompanyName).Select(c => c);
            dgrCustomersQ2.ItemsSource = q.ToList();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            var q = db.Customers.OrderBy(c => c.CompanyName).Select(c => new
            {
                c.CompanyName,
                c.FirstName,
                c.LastName,
                c.Phone,
                c.EmailAddress
            });
            dgrCustomersQ3.ItemsSource = q.ToList().Distinct();
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            var q = db.ProductCategories.OrderBy(p => p.Name).Select(p => p);

            lbxCustomersQ4first.ItemsSource = q.ToList();
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            var q = db.SalesOrderDetails.Where(p =>
            (p.UnitPriceDiscount * p.UnitPrice * p.OrderQty) >= 5).Select(p => new
            {
                p.Product.Name,
                p.UnitPrice,
                p.OrderQty,
                p.LineTotal,
                Discount = p.UnitPriceDiscount,
                DiscountAmount = p.UnitPrice * p.UnitPriceDiscount
            });

            dgrCustomersQ5.ItemsSource = q.ToList();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            var q = db.Products.
                GroupBy(p => p.ProductCategoryID).
                OrderByDescending(p => p.Count()).
                Select(p => new
                {
                    CategoryID = p.Key,
                    NumberInCategory = p.Count()
                });

            dgrCustomersQ6.ItemsSource = q.ToList();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            var q = from o in db.Products
                    group o by o.ProductCategory into g
                    join c in db.Products on g.Key equals c.ProductCategory
                    orderby g.Count() descending
                    select new
                    {
                        CategoryID = c.ProductCategoryID,
                        Category = c.ProductCategory.Name,
                        NumberInCategory = c.ProductCategory.Products.Count()
                    };

            dgrCustomersQ7.ItemsSource = q.ToList().Distinct();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            Product p1 = new Product()
            {
                Name = "Test Product",
                ProductNumber = "Test123",
                StandardCost = 100m,
                ListPrice = 200m,
                SellStartDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            db.Products.Add(p1);
            db.SaveChanges();

            var q = db.Products.OrderByDescending(p => p.ModifiedDate).Select(p => p);
            dgrCustomersQ8.ItemsSource = q.ToList().Distinct();
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            var q = db.ProductCategories.Where(p => p.Products.Count > 0).OrderBy(p => p.Name).Select(p => p);
            lbxCustomersQ9first.ItemsSource = q.ToList();
        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            var q = db.Products.Where(p => p.Name.Contains("bike")).Select(p => p);

            dgrLast.ItemsSource = q.ToList();
        }

        private void lbxCustomersQ4first_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int categoryID = Convert.ToInt32(lbxCustomersQ4first.SelectedValue);

            if(categoryID > 0)
            {
                var q = db.Products.Where(p => p.ProductCategoryID == categoryID).Select(p => p);
                lbxCustomersQ4second.ItemsSource = q.ToList();
            }
        }

        private void lbxCustomersQ9first_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int categoryID = Convert.ToInt32(lbxCustomersQ9first.SelectedValue);

            if (categoryID > 0)
            {
                var q = db.Products.Where(p => p.ProductCategoryID == categoryID).Select(p => p);
                lbxCustomersQ9second.ItemsSource = q.ToList();
            }
        }

    }
    public partial class Product
    {
        public override string ToString()
        {
            return string.Format("{0} - {1:C} - Number of orders: {2}",
               Name, ListPrice, SalesOrderDetails.Select(q => q.OrderQty));
        }
    }
}
