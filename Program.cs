using static System.Console;
using Microsoft.Data.SqlClient;
using Datalagring_1._0.Models;

namespace Datalagring_1._0
{
    internal class Program
    {
        static string connectionString = "Server=.;Database=ProductManager;Integrated Security=true;Encrypt=False";
        static Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();
        static List<Category> categoryList = new List<Category>();
        static void Main()
        {
            CursorVisible = false;

            bool applicationRunning = true;

            do
            {
                WriteLine("1. Ny produkt");
                WriteLine("2. Sök produkt");
                WriteLine("3. Ny kategori");
                WriteLine("4. Lägg till produkt till kategori");
                WriteLine("5. Lista kategorier");
                WriteLine("6. Avsluta");

                bool invalidSelection = true;

                ConsoleKeyInfo mainMenu;

                mainMenu = KeyOptions(ref invalidSelection);

                Clear();

                CursorVisible = true;

                switch (mainMenu.Key)
                {
                    // Ny produkt
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            RegisterProduct();

                            Clear();
                            break;
                        }

                    // Sök produkt
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            SearchProduct();

                            break;
                        }

                    // Ny kategori
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            do
                            {
                                WriteLine("Ange ny kategori!");
                                WriteLine("\n--------------------------------");
                                Write("Namn på ny kategori: ");

                                var categoryName = ReadLine();

                                WriteLine("\nStämmer detta? (J)a / (N)ej");

                                mainMenu = ReadKey(true);

                                invalidSelection = !(mainMenu.Key == ConsoleKey.J || mainMenu.Key == ConsoleKey.N);

                                if (mainMenu.Key == ConsoleKey.J)
                                {
                                    var Category = new Category(categoryName);
                                    categoryList.Add(Category);

                                    WriteLine("Kategori skapad.");

                                    Thread.Sleep(2000);

                                    invalidSelection = false;
                                }


                            } while (invalidSelection) ;

                            Clear();
                            break;
                        }

                    //Lägg till produkt till kategori
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            bool categoryFound = categoryList != null;

                            {
                                WriteLine("Ange produktens artikelnummer: ");

                                var ItemNumber = ReadLine();

                                Product product = FindProduct(ItemNumber);
                                var productFound = product != null;

                                Clear();
                                if (productFound)
                                {
                                    WriteLine($"{product.ProductName}");


                                    Write("Namn på kategori: ");
                                    var categoryName = ReadLine();

                                    Clear();
                                    Category category = categoryList.FirstOrDefault(x => x.CategoryName == categoryName);

                                    Clear();

                                    try
                                    {
                                        if (category != null)
                                        {
                                            category.AddProduct(product);
                                            WriteLine("Produkten är tillagd till kategorin.");
                                        }
                                        else
                                        {
                                            WriteLine("Kategorin finns inte");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLine("Produkten finns redan i kategorin.");
                                    }
                                    Thread.Sleep(2000);
                                }
                                else
                                {
                                    WriteLine("Produkt existerar inte.");
                                }

                                Thread.Sleep(2000);
                                break;
                            }
                        }

                    //Lista kategorier
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            WriteLine("Namn: ");
                            WriteLine("------------------------------------");

                            foreach (Category category in categoryList)
                            {
                                WriteLine($"{category.CategoryName} ({category.ProductList.Count})");

                                foreach (Product product in category.ProductList)
                                {
                                    WriteLine(product.ProductName, product.ProductPrice);
                                    WriteLine("");
                                    WriteLine("\nTryck på escape för att återgå till huvudmenyn");
                                }
                            }

                            while (ReadKey(true).Key != ConsoleKey.Escape) ;

                            Clear();
                            break;
                        }

                    //Avsluta applikationen
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            Clear();

                            applicationRunning = false;

                            break;
                        }
                }

            } while (applicationRunning);
        }

        //Metoder
        private static void SearchProduct()
        {
            Write("Ange produkt: ");

            var itemNumber = ReadLine().ToLower();

            Product product = FindProduct(itemNumber);

            var productFound = product != null;

            if (productFound)
            {
                Clear();

                PrintProduct(product);

                WriteLine("\nTryck på Escape för att återgå till menyn");
                while (ReadKey(true).Key != ConsoleKey.Escape) ;

                Clear();
            }
            else
            {
                WriteLine("Produkten finns inte.");
                Thread.Sleep(2000);
            }
        }

        private static Product FindProduct(string itemNumber)
        {
            var sql = @" 
                    
                SELECT 
                       ProductName,
                       ItemNumber,
                       ProductDescription,
                       ProductPrice
                 FROM Product
                 WHERE ItemNumber = @ItemNumber";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ItemNumber", itemNumber);

            connection.Open();

            var reader = command.ExecuteReader();

            Product product = null;

            if (reader.Read()) ;
            {
                product = new Product(
                (string)reader["ProductName"],
                (string)reader["ItemNumber"],
                (string)reader["ProductDescription"],
                (decimal)reader["ProductPrice"]);
            }

            connection.Close();

            return product;
        }

        static void PrintProduct(Product product)
        {
            WriteLine(product.ItemNumber);
            WriteLine(product.ProductName);
            WriteLine(product.ProductDescription);
            WriteLine(product.ProductPrice);
        }

        private static void RegisterProduct()
        {
            Write("Ange produktens artikelnummer: ");
            string itemNumber = ReadLine();

            Write("Ange produktens namn: ");
            string productName = ReadLine();

            Write("Ange produktens beskrivning: ");
            string productDescription = ReadLine();

            Write("Ange produktens pris: ");
            var productPrice = decimal.Parse(ReadLine());

            Clear();

            CursorVisible = false;

            Product product;

            product = new Product(itemNumber, productName, productDescription, productPrice);

            WriteLine($"Produktens artikelnummer: {product.ItemNumber}");
            WriteLine($"Produktens namn: {product.ProductName}");
            WriteLine($"Produktens beskrivning: {product.ProductDescription}");
            WriteLine($"Produktens pris: {product.ProductPrice} Kr");

            WriteLine(" ");
            WriteLine("Är detta korrekt? (J)a (N)ej");

            ConsoleKeyInfo yesNo = ReadKey(true);

            if (yesNo.Key == ConsoleKey.J)
            {
                WriteLine("\nProdukten är registrerad.");
                AddProduct(product);

                Thread.Sleep(2000);
            }
            else
            {
                WriteLine("\nArtikelnumret är redan registrerat.");
            }

            if (yesNo.Key == ConsoleKey.N)

            WriteLine("\nDu har valt att inte spara produkten, återgår till huvudmenyn.");

            Thread.Sleep(2000);
        }

        private static void AddProduct(Product product)
        {
            var sql = @"
                INSERT INTO Product (
                    ProductName, 
                    ItemNumber, 
                    ProductDescription, 
                    ProductPrice
                     ) VALUES (
                    @ProductName, 
                    @ItemNumber, 
                    @ProductDescription, 
                    @ProductPrice)
            ";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@ItemNumber", product.ItemNumber);
            command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
            command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();


        }

        private static ConsoleKeyInfo KeyOptions(ref bool invalidSelection)
        {
            ConsoleKeyInfo mainMenu;
            do
            {
                mainMenu = ReadKey(true);

                if (
                   mainMenu.Key == ConsoleKey.D1 ||
                   mainMenu.Key == ConsoleKey.NumPad1 ||
                   mainMenu.Key == ConsoleKey.D2 ||
                   mainMenu.Key == ConsoleKey.NumPad2 ||
                   mainMenu.Key == ConsoleKey.D3 ||
                   mainMenu.Key == ConsoleKey.NumPad3 ||
                   mainMenu.Key == ConsoleKey.D4 ||
                   mainMenu.Key == ConsoleKey.NumPad4 ||
                   mainMenu.Key == ConsoleKey.D5 ||
                   mainMenu.Key == ConsoleKey.NumPad5 ||
                   mainMenu.Key == ConsoleKey.D6 ||
                   mainMenu.Key == ConsoleKey.NumPad6)
                {
                    invalidSelection = false;
                }

            } while (invalidSelection);
            return mainMenu;
        }
    }
}