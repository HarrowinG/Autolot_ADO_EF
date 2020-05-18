using System;
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
    }
}
