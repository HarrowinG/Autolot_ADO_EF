using System;
using System.Collections.Generic;
using System.Linq;
using AutoLotEfConsoleApp.EF;

namespace AutoLotEfConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Fun with EF ***");
            //            int carId = AddNewRecord();
            //            Console.WriteLine(carId);
            //GetShortCars();
            PrintAllInventory();
            Console.ReadLine();
        }

        private static int AddNewRecord()
        {
            using (var context = new AutoLotDbContext())
            {
                try
                {
                    var car = new Car{ Make = "Yugo", Color = "Brown", CarNickName = "Brownie" };
                    context.Cars.Add(car);
                    context.SaveChanges();
                    return car.CarId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    return 0;
                }
            }
        }

        private static void AddNewRecords(IEnumerable<Car> carsToAdd)
        {
            using (var context = new AutoLotDbContext())
            {
                context.Cars.AddRange(carsToAdd);
                context.SaveChanges();
            }
        }

        private static void PrintAllInventory()
        {
            using (var context = new AutoLotDbContext())
            {
                foreach (var c in context.Cars.Where(x => x.Make == "BMW"))
                {
                    Console.WriteLine(c);
                }
            }
        }

        private static void GetShortCars()
        {
            using (var context = new AutoLotDbContext())
            {
                //entities from Database calls remains untracked
                foreach (var c in context.Database.SqlQuery(typeof(ShortCar), "Select CarId, Make from dbo.Inventory"))
                {
                    Console.WriteLine(c);
                }
            }
        }

        private static void Find(int id)
        {
            using (var context = new AutoLotDbContext())
            {
                //Find search in DbChangeTracker first and then in Db
                Console.WriteLine(context.Cars.Find(id));
            }
        }
    }
}
