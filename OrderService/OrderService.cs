using DatabaseApplication;

namespace OrderService
{
    public class OrderService
    {
        public IEnumerable<Order> GetAllOrders()
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Orders;
            });

            return res.ToList();
        }

        public Order GetOrderById(int orderId)
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Orders.Where(u => orderId == u.Id);
            });
            return res.FirstOrDefault();
        }

        public int AddOrder(Order order)
        {
            int retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var address = db.Addresses.FirstOrDefault(add => add.Id == order.OrderAddress.Id);
                var user = db.Users.FirstOrDefault(us => us.Id == order.User.Id);

                order.OrderAddress = address;
                order.User = user;

                db.Orders.Add(order);
                db.SaveChanges();
                retVal = order.Id;
            });

            return retVal;
        }

        public void RemoveOrder(int id)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var order = db.Orders.Find(id);
                if (order != null)
                {
                    db.Orders.Remove(order);
                    db.SaveChanges();
                }
            });
        }

        public void AddProductsToOrder(int orderId, IEnumerable<Product> products)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var dbOrder = db.Orders.Find(orderId);
                if (dbOrder != null)
                {
                    foreach (var product in products)
                    {
                        var prod = db.Products.FirstOrDefault(p => p.Id == product.Id);
                        dbOrder.Products.Add(prod);
                    }
                    db.SaveChanges();
                }
            });
        }

        public void RemoveProductsFromOrder(int orderId, IEnumerable<Product> products)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var dbOrder = db.Orders.Find(orderId);
                if (dbOrder != null)
                {
                    foreach (var product in products)
                    {
                        if (dbOrder.Products.Contains(product))
                        {
                            dbOrder.Products.Remove(product);
                        }
                    }
                    db.SaveChanges();
                }
            });
        }

        public void ClearOrders()
        {
            using var db = new DbApplication();
            db.Orders.RemoveRange(db.Orders.ToList());
            db.SaveChanges();
        }
    }
}