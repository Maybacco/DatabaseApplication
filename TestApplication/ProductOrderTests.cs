using AddressService;
using DatabaseApplication;

namespace TestApplication
{
    public class ProductOrderTests
    {
        [SetUp]
        public void Setup()
        {
            var service = new ProductService.ProductService();
            service.ClearProducts();
            service.ClearCategories();

            var orderService = new OrderService.OrderService();
            orderService.ClearOrders();

            var addressesService = new AddressService.AddressService();
            addressesService.ClearAddresses();

            var userService = new UserService.UserService();
            userService.ClearUsers();
        }

        [Test]
        public void TestProductCycle()
        {
            var service = new ProductService.ProductService();

            var cat = new Category()
            {
                CategoryName = "Test",
            };
            cat.Id = service.AddCategory(cat);

            Assert.AreEqual(1, service.GetAllCategories().Count());

            var product = new Product
            {
                ProductName = "Banane",
                SerialNumber = "12654",
                ProductCategory = cat,
            };
            service.AddProduct(product);

            Assert.AreEqual(1, service.GetAllProducts().Count());

            service.RemoveProduct(product.Id);

            Assert.AreEqual(0, service.GetAllProducts().Count());
        }

        [Test]
        public void TestOrderCycle()
        {
            var productService = new ProductService.ProductService();
            var orderService = new OrderService.OrderService();
            var cat = new Category()
            {
                CategoryName = "Test",
            };
            cat.Id = productService.AddCategory(cat);

            var product = new Product
            {
                ProductName = "Banane",
                SerialNumber = "12654",
                ProductCategory = cat,
            };
            productService.AddProduct(product);

            var product2 = new Product
            {
                ProductName = "Mele",
                SerialNumber = "1265234",
                ProductCategory = cat,
            };
            productService.AddProduct(product2);

            var userService = new UserService.UserService();
            var user = new User()
            {
                FirstName = "Test",
                LastName = "Baudo",
            };
            userService.AddUser(user);

            var addressService = new AddressService.AddressService();
            Address address = new Address()
            {
                City = "Bari",
                StreetName = "Test",
                CivicNumber = "12",
                Country = "Italy",
                PostalCode = "1210",
                Region = "Puglia"
            };
            addressService.AddAddress(address);

            var ord = orderService.AddOrder(new Order()
            {
                User = user,
                OrderAddress = address
            });

            orderService.AddProductsToOrder(ord, new List<Product>() { product, product2 });

        }
        [Test]
        public void TestAddresses()
        {
            var addressService = new AddressService.AddressService();
            var addVal = addressService.AddAddress(new Address()
            {
                City = "Bari",
                StreetName = "Test",
                CivicNumber = "12",
                Country = "Italy",
                PostalCode = "1210",
                Region = "Puglia"
            });

            Assert.AreEqual(1, addressService.GetAllAddresses().Count());

            addressService.RemoveAddress(addVal);
            Assert.AreEqual(0, addressService.GetAllAddresses().Count());

        }
    }
}