using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace cv4
{
    internal class Program
    {
        public class Customer
        {
            
            public long Id { get; set; }
            public string Name { get; set; }
            public string? Address { get; set; }
        }
        public class Order
        {
            public long Id { get; set; }
            public long CustomerId { get; set; }
            public Customer Customer { get; set; }
            public string Product { get; set; }
            public double Price { get; set; }
        }

        public class MyDatabase : DbContext
        {
            public DbSet<Customer> Customer { get; set; }
            public DbSet<Order> Order { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source = mydb.db;")
           
            }
        }

        static void Main(string[] args)
        {
            // EntityFramework
            MyDatabase db = new MyDatabase();


            /*
            db.Customer.Add(new Customer
            {
                Name = "Tomas",
                Address = "Pardubice"
            });
            */

            Customer customer = db.Customer
                .Where(x => x.Id == 1)
                .First();

            customer.Address = "Ostrava";

            db.Order.Add(new Order()
            {
                CustomerId = customer.Id,
                Price = 100,
                Product = "auto"
            });

            db.SaveChanges();
            
            foreach(var order in db.Order.Include(x => x.Customer))
            {
                Console.WriteLine(order.Customer.Name + " | " + order.Product);
            }


            //string sql = File.ReadAllText("database-create.sql");

            // SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);

            string connString = "Data Source=mydb.db;";
           
            
           // using (SqliteConnection conn = new SqliteConnection(connString))
           // {
                // DAPPER SIMPLE
                /*
                conn.Insert(new Customer()
                {
                    Name = "Jakub",
                    Address = "Olomouc"
                });

                long count = conn.RecordCount<Customer>();
                Console.WriteLine($"Pocet: {count}");

                Customer c = conn.Get<Customer>(2);
                c.Address = "Ostrava";
                conn.Update(c);

                IEnumerable<Customer> cusotmers = conn.GetList<Customer>();
                */

                // DAPPER
                /*
                conn.Execute(@"INSERT INTO [Customer] ([Name], [Address]) 
                             VALUES (@Name, @Address)",
                             new {
                                 Name = "Jan",
                                 Address = "Praha"
                             });
                */

                /*
                long count = conn.ExecuteScalar<long>("SELECT COUNT(*) FROM [Customer]");
                Console.WriteLine($" Pocet: {count}");
                */

                /*
                IEnumerable<Customer> customers = conn.Query<Customer>("SELECT * FROM [Customer]");
                foreach(Customer customer in customers)
                {
                    Console.WriteLine($" {customer.Id} | {customer.Name} | {customer.Address}");
                }
                */


                // ADO.NET
                /*
                conn.Open();

                using SqliteTransaction tran = conn.BeginTransaction();
                */
                /* create db
                using SqliteCommand cmd = new SqliteCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                */

                 //Vkladani
                 /*
                string name = "Petr";
                string address = null;

                using SqliteCommand cmd = new SqliteCommand();
                cmd.CommandText = @"INSERT INTO [Customer] ([Name], [Address]) VALUES (@Name, @Address)";
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address == null ? DBNull.Value : address);

                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                */

                /* scalar
                using SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM [Customer]", conn);
                long count = (long) cmd.ExecuteScalar();
                Console.WriteLine($"Pocet: {count}");
                */


                /*
                using SqliteCommand cmd2 = new SqliteCommand("SELECT * FROM [Customer]", conn);
                using SqliteDataReader reader = cmd2.ExecuteReader();
                while(reader.Read())
                {
                    long id = reader.GetInt64(reader.GetOrdinal("Id"));
                    string name1 = reader.GetString(reader.GetOrdinal("Name"));
                    string address1 = reader.IsDBNull(2) ? null : reader.GetString(2);

                    Console.WriteLine($"{id} | {name1} | {address1}");
                }
                */


                //tran.Commit();
                // tran.Rollback();


                // close neni potreba, protoze using dispose
                // conn.Close();
                
            //}
            
        }
    }
}