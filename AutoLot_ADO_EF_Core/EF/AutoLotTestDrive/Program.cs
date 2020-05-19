using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoLotDAL.Models;
using AutoLotDAL.Repos;

namespace AutoLotTestDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Fun with EF Code First ***");
            using (var repo = new InventoryRepo())
            {
                foreach (var inventory in repo.GetAll())
                {
                    Console.WriteLine(inventory);
                }
            }

            Console.ReadLine();
        }

        private static void AddNewRecord(Inventory car)
        {
            using (var repo = new InventoryRepo())
            {
                repo.Add(car);
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var repo = new InventoryRepo())
            {
                var carToUpdate = repo.GetOne(carId);
                if (carToUpdate == null)
                {
                    return;
                }

                carToUpdate.Color = "Blue";
                repo.Save(carToUpdate);
            }
        }

        private static void RemoveRecordsByCar(Inventory carToDelete)
        {
            using (var repo = new InventoryRepo())
            {
                repo.Delete(carToDelete);
            }
        }

        private static void RemoveRecordsById(int carId, byte[] timeStamp)
        {
            using (var repo = new InventoryRepo())
            {
                repo.Delete(carId, timeStamp);
            }
        }

        private static void TestConcurrency()
        {
            var repo1 = new InventoryRepo();
            var repo2 = new InventoryRepo();
            var car1 = repo1.GetOne(1);
            var car2 = repo1.GetOne(1);
            car1.PetName = "NewName";
            repo1.Save(car1);
            car2.PetName = "OtherName";
            try
            {
                repo2.Save(car2);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var currentValues = entry.CurrentValues;
                var originalValues = entry.OriginalValues;
                var dbValues = entry.GetDatabaseValues();
                Console.WriteLine("*** Concurrency ***");
                Console.WriteLine("Type\tPetName");
                Console.WriteLine($"Current:\t{currentValues[nameof(Inventory.PetName)]}");
                Console.WriteLine($"Original:\t{originalValues[nameof(Inventory.PetName)]}");
                Console.WriteLine($"Db:\t{dbValues[nameof(Inventory.PetName)]}");
            }
        }
    }
}
