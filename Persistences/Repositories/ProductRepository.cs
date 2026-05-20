using Applications.Products;
using Domains;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistences.Repositories
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public List<SpProductFullModel> StoreProcedureExample()
        {
            return Context.Database.SqlQueryRaw<SpProductFullModel>(
                    "EXEC usp_GetProductsByProductName @ProductName, @CategoryId",
                    new SqlParameter("@ProductName", "silgi"),
                    new SqlParameter("@CategoryId", "2"))
                .ToList();
        }

        public void X(string name, params int[] ids)
        {
        }

        public Product StoreProcedureInsertExample(string name, decimal price, string barcode, int categoryId)
        {
            var newId = new SqlParameter("@NewId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            Context.Database.ExecuteSqlRaw(
                $"EXEC usp_InsertProduct @Name, @Price, @Barcode, @CategoryId, @NewId OUTPUT",
                new SqlParameter("@Name", name),
                new SqlParameter("@Price", price),
                new SqlParameter("@Barcode", barcode),
                new SqlParameter("@CategoryId", categoryId), newId
            );

            var newProductId = (int)newId.Value;
            return new Product()
            {
                Id = newProductId,
                Name = name,
                Price = price,
                Barcode = barcode,
                CategoryId = categoryId
            };
        }

        public void SqlClauseExample()
        {
            var productsAsSqlRaw = Context.Products.FromSqlRaw("select * from Products");


            var productId = 1;

            var productsAsFromSqlInterpolated =
                Context.Products.FromSqlInterpolated($"select * from Products where id={productId}").ToList();

            var productsAsFromSql = Context.Products.FromSql($"select * from Products where id={productId}").ToList();
        }


        public void JoinExample()
        {
            // method syntax

            var resultAsMethod = context.Categories
                .Join(context.Products, c => c.Id, p => p.CategoryId, (c, p) => new { c, p })
                .ToList();
            var resultAsMethod2 = context.Categories
                .Join(context.Products, c => c.Id, p => p.CategoryId, (c, p) => new
                {
                    CategoryName = c.Name,
                    ProductName = p.Name,
                    ProductPrice = p.Price
                })
                .ToList();


            foreach (var row in resultAsMethod)
            {
                var category = row.c;

                var product = row.p;
            }

            var resultAsMethod3 = context.Categories
                .Join(context.Products,
                    c => c.Id,
                    p => p.CategoryId,
                    (c, p) => new { c, p })
                .Join(context.ProductDetails,
                    cp => cp.p.Id,
                    pd => pd.ProductId,
                    (cp, pd) => new
                    {
                        CategoryName = cp.c.Name,
                        ProductName = cp.p.Name,
                        ProductPrice = cp.p.Price,
                        Height = pd.Height
                    })
                .ToList();


            //query syntax

            var resultAsQuery = (from c in context.Categories
                join p in context.Products on c.Id equals p.CategoryId
                select new { c, p }).ToList();
            var resultAsQuery2 = (from c in context.Categories
                join p in context.Products on c.Id equals p.CategoryId
                select new
                {
                    CategoryName = c.Name,
                    ProductName = p.Name,
                    ProductPrice = p.Price
                }).ToList();


            var resultAsQuery3 = (from c in context.Categories
                join p in context.Products on c.Id equals p.CategoryId
                join pd in context.ProductDetails on p.Id equals pd.ProductId
                select new
                {
                    CategoryName = c.Name,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    Height = pd.Height
                }).ToList();


            // LEFT JOIN - Query Syntax
            var leftJoinQuery = (from c in context.Categories
                join p in context.Products on c.Id equals p.CategoryId into cp
                from p in cp.DefaultIfEmpty()
                select new
                {
                    CategoryName = c.Name,
                    ProductName = p != null ? p.Name : null,
                    ProductPrice = p != null ? (decimal?)p.Price : null
                }).ToList();

            // RIGHT JOIN - Query Syntax (swap sides + left join)
            var rightJoinQuery = (from p in context.Products
                join c in context.Categories on p.CategoryId equals c.Id into pc
                from c in pc.DefaultIfEmpty()
                select new
                {
                    CategoryName = c != null ? c.Name : null,
                    ProductName = p.Name,
                    ProductPrice = p.Price
                }).ToList();


            // FULL OUTER JOIN - Left side
            var leftPart = from c in context.Categories
                join p in context.Products on c.Id equals p.CategoryId into cp
                from p in cp.DefaultIfEmpty()
                select new
                {
                    CategoryName = (string?)c.Name,
                    ProductName = p != null ? p.Name : null,
                    ProductPrice = p != null ? (decimal?)p.Price : null
                };

            // FULL OUTER JOIN - Right side (only rows with no matching category)
            var rightPart = from p in context.Products
                join c in context.Categories on p.CategoryId equals c.Id into pc
                from c in pc.DefaultIfEmpty()
                where c == null
                select new
                {
                    CategoryName = (string?)null,
                    ProductName = (string?)p.Name,
                    ProductPrice = (decimal?)p.Price
                };

            var fullOuterJoin = leftPart.Union(rightPart).ToList();


            foreach (var row in resultAsQuery)
            {
                var category = row.c;

                var product = row.p;
            }
        }


        public void ClientExpression()
        {
            var resultAsServerExpression = Context.Products.Where(x => x.Name == "kalem1").ToList();

            var result = Context.Products.Where(x => AppendPrefix(x.Name) == "kalem1").ToList();


            var resultAsClientExpression = Context.Products.ToList().Where(x => AppendPrefix(x.Name) == "kalem1");


            var prodcut = context.Products.Select(p => new
            {
                Name = AppendPrefix(p.Name)
            });
        }

        private string AppendPrefix(string name)
        {
            return name + "a";
        }


        //Eager Loading
        public Product? GetByIdWithCategoryAndFeature(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.ProductDetail)
                .FirstOrDefault(x => x.Id == id);
        }

        //Eager Loading
        public List<Product> GetAllWithCategoryAndFeature()
        {
            var categories = Context.Categories.Include(c => c.Products)!.ThenInclude(p => p.ProductDetail).ToList();


            //Eager Loading
            return Context.Products.Include(p => p.Category).Include(p => p.ProductDetail).ToList();
        }


        //Explicit Loading
        public Product? ExplicitLoading(int id, bool isReference)
        {
            var product = Context.Products.First(x => x.Id == id);


            if (!isReference) return product;

            Context.Entry(product).Reference(p => p.Category).Load();

            Context.Entry(product).Reference(p => p.ProductDetail).Load();


            return product;
        }

        //lazy Loading
        public List<Product> LazyLoading()
        {
            var products = context.Products.ToList();
            foreach (Product product in products)
            {
                var category = product.Category;
                var Detail = product.ProductDetail;
            }

            return products;
        }

        public List<Product> GeatAllByPaged(int page, int pageSize)
        {
            // 1,10 => Skip(0).Take(10)
            // 2,10 => Skip(10).Take(10)

            return Context.Products.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> Get(decimal price)
        {
            return Context.Products.Where(x => x.Price == price).ToList();
        }


        public List<ProductFullModel> GetFullModel()
        {
            return Context.ProductFullModels.FromSqlRaw(
                    "select p.id as Id,c.Name as CategoryName,p.Name as ProductName from Categories as c inner join Products as p on c.id = p.CategoryId")
                .ToList();
        }
    }
}
