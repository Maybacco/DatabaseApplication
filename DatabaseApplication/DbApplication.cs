using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace DatabaseApplication
{
    public class DbApplication : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public string DbPath { get; set; }

        public DbApplication()
        {
            var path = Environment.CurrentDirectory;
            DbPath = Path.Join(path, "ECommerce.db");

            Database.EnsureCreated();
        }


        public DbApplication(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Debugger.Launch();
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }

    public class Order
    {
        /// <summary>
        /// ID Primary Key AI
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Lista dei prodotti correlati all'ordine e loro quantità
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Utente che ha effettuato l'ordine
        /// </summary>
        [Required]
        public User User { get; set; }

        /// <summary>
        /// Ordine di spedizione
        /// </summary>
        public Address OrderAddress { get; set; }
    }

    public class Product
    {
        /// <summary>
        /// ID Primary Key AI
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome del prodotto
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// Codice seriale
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Ordini in cui è presente il prodotto
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Categoria dove risiede il prodotto
        /// </summary>
        [Required]
        public Category ProductCategory { get; set; }
    }

    public class Category
    {
        /// <summary>
        /// ID Primary Key AI
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string CategoryName { get; set; }

        /// <summary>
        /// Prodotti in una categoria
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class User
    {
        /// <summary>
        /// ID Primary Key AI
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Cognome
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Ordini correlati all'utente
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    public class Address
    {
        /// <summary>
        /// ID Primary Key AI
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Indirizzo
        /// </summary>
        [Required]
        public string StreetName { get; set; }

        /// <summary>
        /// Numero civico
        /// </summary>
        [Required]
        public string CivicNumber { get; set; }

        /// <summary>
        /// Città
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Regione
        /// </summary>
        [Required]
        public string Region { get; set; }

        /// <summary>
        /// Codice postale
        /// </summary>
        [Required]
        public string PostalCode { get; set; }

        /// <summary>
        /// Paese
        /// </summary>
        [Required]
        public string Country { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order> ();
    }
}