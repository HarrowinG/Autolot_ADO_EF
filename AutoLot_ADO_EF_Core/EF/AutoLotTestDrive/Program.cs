using System;
using System.Data.Entity;
using AutoLotDAL.EF;

namespace AutoLotTestDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new MyDataInitializer()); //Deletes, recreates db and calls Seed
            Console.WriteLine("*** Fun with EF Code First ***");
            using (var context = new AutoLotDbContext())
            {
                foreach (var inventory in context.Inventories)
                {
                    Console.WriteLine(inventory);
                }
            }

            Console.ReadLine();
        }
    }
}
