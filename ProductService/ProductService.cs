using DatabaseApplication;

namespace ProductService
{
    public class ProductService
    {
        public IEnumerable<Product> GetAllProducts()
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Products;
            });
            return res.ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Categories;
            });
            return res.ToList();
        }

        public Product GetProductById(int id)
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Products.Where(u => id == u.Id);
            });
            return res.FirstOrDefault();
        }

        public Category GetCategoryById(int id)
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Categories.Where(u => id == u.Id);
            });
            return res.FirstOrDefault();
        }

        public int AddCategory(Category category)
        {
            int retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                db.Categories.Add(category);
                db.SaveChanges();
                retVal = category.Id;
            });

            return retVal;
        }

        public int AddProduct(Product product)
        {
            int retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var cat = db.Categories.FirstOrDefault(cat => cat.Id == product.ProductCategory.Id);
                product.ProductCategory = cat;
                db.Products.Add(product);
                db.SaveChanges();
                retVal = product.Id;
            });

            return retVal;
        }

        public void RemoveProduct(int productId)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var dbProduct = db.Products.Find(productId);
                if (dbProduct != null)
                {
                    db.Products.Remove(dbProduct);
                    db.SaveChanges();
                }
            });
        }

        public void ClearProducts()
        {
            using var db = new DbApplication();
            db.Products.RemoveRange(db.Products.ToList());
            db.SaveChanges();
        }
        public void ClearCategories()
        {
            using var db = new DbApplication();
            db.Categories.RemoveRange(db.Categories.ToList());
            db.SaveChanges();
        }
    }
}