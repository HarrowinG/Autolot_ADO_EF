﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoLotDal.BulkImport;
using AutoLotDal.DataOperations;
using AutoLotDal.Models;

namespace AutoLotClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //DalTest();
            //MoveCustomer();
            //DoBulkCopy();
        }

        public static void DalTest()
        {
            var dal = new InventoryDal();
            var list = dal.GetAllInventory();
            Console.WriteLine("***** All Cars ******");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.CarId}\t{item.Make}\t{item.Color}\t{item.PetName}");
            }

            Console.WriteLine();
            var car = dal.GetCar(list.OrderBy(x => x.Color).Select(x => x.CarId).First());
            Console.WriteLine("***** First Car by Color ********");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");

            try
            {
                dal.DeleteCar(5);
                Console.WriteLine("Car deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occured: {ex.Message}");
            }

            dal.InsertAuto(new Car { Color = "Blue", Make = "Pilot", PetName = "TownMonster" });
            list = dal.GetAllInventory();
            var newCar = list.First(x => x.PetName == "TownMonster");
            Console.WriteLine("***** New Car ********");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{newCar.CarId}\t{newCar.Make}\t{newCar.Color}\t{newCar.PetName}");

            dal.DeleteCar(newCar.CarId);
            var petName = dal.LookUpPetName(car.CarId);
            Console.WriteLine($"Car pet name: {petName}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static void MoveCustomer()
        {
            Console.WriteLine("****** Simple Transaction Example *********");
            bool throwEx = true;
            Console.Write("Do you want to throw Exception (y or n): ?");
            var userAnswer = Console.ReadLine();
            if (userAnswer?.ToLower() == "n")
            {
                throwEx = false;
            }

            var dal = new InventoryDal();
            dal.ProcessCreditRisk(throwEx, 1);
            Console.WriteLine("Check CreditRisk table for result");
            Console.ReadLine();
        }

        public static void DoBulkCopy()
        {
            Console.WriteLine("**** Do Bulk Copy *****");
            var cars = new List<Car>
            {
                new Car{ Color = "Blue", Make = "Honda", PetName = "MyCar1" },
                new Car{ Color = "Red", Make = "Volvo", PetName = "MyCar2" },
                new Car{ Color = "White", Make = "VW", PetName = "MyCar3" },
                new Car{ Color = "Yellow", Make = "Toyota", PetName = "MyCar4" }
            };
            ProcessBulkImport.ExecuteBulkCopy(cars, "Inventory");
            var dal = new InventoryDal();
            var list = dal.GetAllInventory();
            Console.WriteLine("**** All Cars ****");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.CarId}\t{item.Make}\t{item.Color}\t{item.PetName}");
            }
            Console.WriteLine();
        }
    }
}