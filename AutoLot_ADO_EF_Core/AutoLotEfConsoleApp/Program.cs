using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            //PrintAllInventory();
            //LazyLoading();
            //EagerLoading();
            //RemoveRecordUsingEntityState(2);
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

        private static void ChainingLinqQueries()
        {
            using (var context = new AutoLotDbContext())
            {
                var allData = context.Cars;
                var colorsMakes = from item in allData select new { item.Color, item.Make };

                foreach (var item in colorsMakes)
                {
                    Console.WriteLine(item);
                }
            }
        }

        private static void LazyLoading()
        {
            using (var context = new AutoLotDbContext())
            {
                context.Configuration.LazyLoadingEnabled = true; //by default
                foreach (var car in context.Cars)
                {
                    //will hit database many times
                    foreach (var order in car.Orders)
                    {
                        Console.WriteLine(order.OrderId);
                    }
                }
            }
        }

        private static void EagerLoading()
        {
            using (var context = new AutoLotDbContext())
            {
                context.Configuration.LazyLoadingEnabled = false; //doesn't matter, we loaded all we need
                foreach (var car in context.Cars.Include("Orders"))
                {
                    foreach (var order in car.Orders)
                    {
                        Console.WriteLine(order.OrderId);
                    }
                }
            }
        }

        private static void ExplicitLoading()
        {
            using (var context = new AutoLotDbContext())
            {
                context.Configuration.LazyLoadingEnabled = false; //to guarantee nothing more loaded
                foreach (var car in context.Cars)
                {
                    context.Entry(car).Collection(x => x.Orders).Load();
                    foreach (var order in car.Orders)
                    {
                        Console.WriteLine(order.OrderId);
                    }
                }

                foreach (var order in context.Orders)
                {
                    context.Entry(order).Reference(x => x.Car).Load();
                }
            }
        }

        private static void RemoveRecord(int carId)
        {
            using (var context = new AutoLotDbContext())
            {
                var carToDelete = context.Cars.Find(carId); //first load entity and then delete it
                if (carToDelete != null)
                {
                    context.Cars.Remove(carToDelete);
                }

                Console.WriteLine(context.Entry(carToDelete).State); //Deleted
                context.SaveChanges();
            }
        }

        private static void RemoveMultipleRecords(IEnumerable<Car> carsToRemove)
        {
            using (var context = new AutoLotDbContext())
            {
                context.Cars.RemoveRange(carsToRemove);
                context.SaveChanges();
            }
        }

        private static void RemoveRecordUsingEntityState(int carId)
        {
            using (var context = new AutoLotDbContext())
            {
                var carToDelete = new Car{ CarId = carId };
                //var car = context.Cars.Find(carId);  //to fail
                context.Entry(carToDelete).State = EntityState.Deleted; //will fail if item with the same key already tracked
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex) //will fail if no entity with such Id
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var context = new AutoLotDbContext())
            {
                var carToUpdate = context.Cars.Find(carId);
                if (carToUpdate != null)
                {
                    Console.WriteLine(context.Entry(carToUpdate).State);
                    carToUpdate.Color = "Blue";
                    Console.WriteLine(context.Entry(carToUpdate).State);
                    context.SaveChanges();
                }
            }
        }
    }
}
