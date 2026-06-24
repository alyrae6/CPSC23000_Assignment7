using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

// =========================
//  MODEL (TABLE STRUCTURE)
// =========================
public class Item
{
    public int Id { get; set; }            // Primary Key
    public string ItemNum { get; set; }
    public string Description { get; set; }
    public int OnHand { get; set; }
    public string Category { get; set; }
    public int Storehouse { get; set; }
    public decimal Price { get; set; }
}

// =========================
//  DATABASE CONTEXT (SQLite)
// =========================
internal class ProductDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // THIS WORKS ON MAC — SQLite database file
        optionsBuilder.UseSqlite("Data Source=Assignment7DB.db");
    }
}

// =========================
//  PROGRAM + MENU
// =========================
class Program
{
    static void Main(string[] args)
    {
        using (ProductDbContext context = new ProductDbContext())
        {
            AddItems(context);

            while (true)
            {
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("S. Show Records");
                Console.WriteLine("A. Add New Record");
                Console.WriteLine("U. Update a Record");
                Console.WriteLine("D. Delete a Record");
                Console.WriteLine("R. Remove All Records");
                Console.WriteLine("Q. Quit");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "S": ShowRecords(context); break;
                    case "A": AddRecord(context); break;
                    case "U": UpdateRecord(context); break;
                    case "D": DeleteRecord(context); break;
                    case "R": RemoveAll(context); break;
                    case "Q": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
    }

    // =========================
    //  LOAD INITIAL DATA
    // =========================
    static void AddItems(ProductDbContext context)
    {
        if (context.Items.Any()) return;

        var items = new[]
        {
            new Item { ItemNum="AH74", Description="Patience", OnHand=9, Category="GME", Storehouse=3, Price=22.99M },
            new Item { ItemNum="BR23", Description="Skittles", OnHand=21, Category="GME", Storehouse=2, Price=29.99M },
            new Item { ItemNum="CD33", Description="Wood Block Set (48 piece)", OnHand=36, Category="TOY", Storehouse=1, Price=89.49M },
            new Item { ItemNum="DL51", Description="Classic Railway Set", OnHand=12, Category="TOY", Storehouse=3, Price=107.95M },
            new Item { ItemNum="DR67", Description="Giant Star Brain Teaser", OnHand=24, Category="PZL", Storehouse=2, Price=31.95M },
            new Item { ItemNum="DW23", Description="Mancala", OnHand=40, Category="GME", Storehouse=3, Price=50.00M },
            new Item { ItemNum="FD11", Description="Rocking Horse", OnHand=8, Category="TOY", Storehouse=3, Price=124.95M },
            new Item { ItemNum="FH24", Description="Puzzle Gift Set", OnHand=65, Category="PZL", Storehouse=1, Price=38.95M },
            new Item { ItemNum="KA12", Description="Cribbage Set", OnHand=56, Category="GME", Storehouse=3, Price=75.00M },
            new Item { ItemNum="KD34", Description="Pentominoes Brain Teaser", OnHand=60, Category="PZL", Storehouse=2, Price=14.95M },
            new Item { ItemNum="KL78", Description="Pick Up Sticks", OnHand=110, Category="GME", Storehouse=1, Price=10.95M },
            new Item { ItemNum="MT03", Description="Zauberkasten Brain Teaser", OnHand=45, Category="PZL", Storehouse=1, Price=45.79M },
            new Item { ItemNum="NL89", Description="Wood Block Set (62 piece)", OnHand=32, Category="TOY", Storehouse=3, Price=119.75M },
            new Item { ItemNum="TR40", Description="Tic Tac Toe", OnHand=75, Category="GME", Storehouse=2, Price=13.99M },
            new Item { ItemNum="TW35", Description="Fire Engine", OnHand=30, Category="TOY", Storehouse=2, Price=118.95M }
        };

        context.Items.AddRange(items);
        context.SaveChanges();
    }

    // =========================
    //  SHOW RECORDS
    // =========================
    static void ShowRecords(ProductDbContext context)
    {
        var items = context.Items.ToList();
        foreach (var i in items)
        {
            Console.WriteLine($"{i.ItemNum} | {i.Description} | {i.OnHand} | {i.Category} | {i.Storehouse} | {i.Price:C}");
        }
    }

    // =========================
    //  ADD RECORD
    // =========================
    static void AddRecord(ProductDbContext context)
    {
        Console.Write("ItemNum: ");
        string itemNum = Console.ReadLine();

        if (context.Items.Any(x => x.ItemNum == itemNum))
        {
            Console.WriteLine("Item already exists.");
            return;
        }

        Console.Write("Description: ");
        string desc = Console.ReadLine();

        Console.Write("OnHand: ");
        int onHand = int.Parse(Console.ReadLine());

        Console.Write("Category: ");
        string cat = Console.ReadLine();

        Console.Write("Storehouse: ");
        int store = int.Parse(Console.ReadLine());

        Console.Write("Price: ");
        decimal price = decimal.Parse(Console.ReadLine());

        Console.Write("Confirm add? (Y/N): ");
        if (Console.ReadLine().ToUpper() != "Y") return;

        Item newItem = new Item
        {
            Id = 0,
            ItemNum = itemNum,
            Description = desc,
            OnHand = onHand,
            Category = cat,
            Storehouse = store,
            Price = price
        };

        context.Items.Add(newItem);
        context.SaveChanges();
    }

    // =========================
    //  UPDATE RECORD
    // =========================
    static void UpdateRecord(ProductDbContext context)
    {
        Console.Write("Enter ItemNum to update: ");
        string itemNum = Console.ReadLine();

        var item = context.Items.FirstOrDefault(x => x.ItemNum == itemNum);
        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nUPDATE MENU");
            Console.WriteLine("D. Description");
            Console.WriteLine("O. OnHand");
            Console.WriteLine("C. Category");
            Console.WriteLine("S. Storehouse");
            Console.WriteLine("P. Price");
            Console.WriteLine("E. Exit");
            Console.Write("Choice: ");

            string choice = Console.ReadLine().ToUpper();

            switch (choice)
            {
                case "D":
                    Console.Write("New Description: ");
                    item.Description = Console.ReadLine();
                    break;

                case "O":
                    Console.Write("New OnHand: ");
                    item.OnHand = int.Parse(Console.ReadLine());
                    break;

                case "C":
                    Console.Write("New Category: ");
                    item.Category = Console.ReadLine();
                    break;

                case "S":
                    Console.Write("New Storehouse: ");
                    item.Storehouse = int.Parse(Console.ReadLine());
                    break;

                case "P":
                    Console.Write("New Price: ");
                    item.Price = decimal.Parse(Console.ReadLine());
                    break;

                case "E":
                    context.SaveChanges();
                    return;
            }
        }
    }

    // =========================
    //  DELETE RECORD
    // =========================
    static void DeleteRecord(ProductDbContext context)
    {
        Console.Write("Enter ItemNum to delete: ");
        string itemNum = Console.ReadLine();

        var item = context.Items.FirstOrDefault(x => x.ItemNum == itemNum);
        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        Console.WriteLine($"{item.ItemNum} | {item.Description} | {item.OnHand} | {item.Category} | {item.Storehouse} | {item.Price:C}");
        Console.Write("Confirm delete? (Y/N): ");

        if (Console.ReadLine().ToUpper() == "Y")
        {
            context.Items.Remove(item);
            context.SaveChanges();
        }
    }

    // =========================
    //  REMOVE ALL RECORDS
    // =========================
    static void RemoveAll(ProductDbContext context)
    {
        ShowRecords(context);

        Console.Write("Confirm remove ALL? (Y/N): ");
        if (Console.ReadLine().ToUpper() != "Y") return;

        context.Items.RemoveRange(context.Items);
        context.SaveChanges();
    }
}
