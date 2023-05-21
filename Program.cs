using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = null;

            while (true)
            {
                string input = Console.ReadLine();
                string[] command = input.Split(' ');

                if (command[0] == "create_parking_lot")
                {
                    int totalSlots = int.Parse(command[1]);
                    parkingLot = new ParkingLot(totalSlots);
                    Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                }
                else if (command[0] == "park")
                {
                    string registrationNumber = command[1];
                    string color = command[2];
                    string vehicleType = command[3];

                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    Vehicle vehicle;
                    if (vehicleType == "Mobil")
                        vehicle = new Car(registrationNumber, color);
                    else if (vehicleType == "Motor")
                        vehicle = new Motorcycle(registrationNumber, color);
                    else
                    {
                        Console.WriteLine("Invalid vehicle type");
                        continue;
                    }

                    int allocatedSlot = parkingLot.ParkVehicle(vehicle);
                    if (allocatedSlot == -1)
                    {
                        Console.WriteLine("Sorry, parking lot is full");
                    }
                    else
                    {
                        Console.WriteLine($"Allocated slot number: {allocatedSlot}");
                    }
                }
                else if (command[0] == "leave")
                {
                    int slotNumber = int.Parse(command[1]);

                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    bool success = parkingLot.LeaveSlot(slotNumber);
                    if (success)
                    {
                        Console.WriteLine($"Slot number {slotNumber} is free");
                    }
                    else
                    {
                        Console.WriteLine($"Slot number {slotNumber} doesn't exist or is already vacant");
                    }
                }
                else if (command[0] == "status")
                {
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
                    foreach (var slot in parkingLot.GetOccupiedSlots())
                    {
                        Vehicle vehicle = parkingLot.GetVehicleInSlot(slot);
                        Console.WriteLine($"{slot}\t{vehicle.RegistrationNumber}\t{vehicle.Type}\t{vehicle.Color}");
                    }
                }
                else if (command[0] == "type_of_vehicles")
                {
                    string vehicleType = command[1];

                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    int count = parkingLot.GetNumberOfVehiclesByType(vehicleType);
                    Console.WriteLine(count);
                }
                else if (command[0] == "registration_numbers_for_vehicles_with_odd_plate")
                {
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    var registrationNumbers = parkingLot.GetRegistrationNumbersWithOddPlate();
                    Console.WriteLine(string.Join(", ", registrationNumbers));
                }
                else if (command[0] == "registration_numbers_for_vehicles_with_even_plate")
                {
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    var registrationNumbers = parkingLot.GetRegistrationNumbersWithEvenPlate();
                    Console.WriteLine(string.Join(", ", registrationNumbers));
                }
                else if (command[0] == "registration_numbers_for_vehicles_with_colour")
                {
                    string color = command[1];

                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    var registrationNumbers = parkingLot.GetRegistrationNumbersByColor(color);
                    Console.WriteLine(string.Join(", ", registrationNumbers));
                }
                else if (command[0] == "slot_numbers_for_vehicles_with_colour")
                {
                    string color = command[1];

                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot is not created");
                        continue;
                    }

                    var slotNumbers = parkingLot.GetSlotNumbersByColor(color);
                    Console.WriteLine(string.Join(", ", slotNumbers));
                }
                else if (command[0] == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }
    }

    public class Vehicle
    {
        public string RegistrationNumber { get; }
        public string Color { get; }
        public string Type { get; }

        public Vehicle(string registrationNumber, string color, string type)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            Type = type;
        }
    }

    public class Car : Vehicle
    {
        public Car(string registrationNumber, string color) : base(registrationNumber, color, "Mobil")
        {
        }
    }

    public class Motorcycle : Vehicle
    {
        public Motorcycle(string registrationNumber, string color) : base(registrationNumber, color, "Motor")
        {
        }
    }

    public class ParkingLot
    {
        private Dictionary<int, Vehicle> parkingSlots;
        private int totalSlots;

        public ParkingLot(int totalSlots)
        {
            parkingSlots = new Dictionary<int, Vehicle>();
            this.totalSlots = totalSlots;
        }

        public int ParkVehicle(Vehicle vehicle)
        {
            for (int i = 1; i <= totalSlots; i++)
            {
                if (!parkingSlots.ContainsKey(i))
                {
                    parkingSlots[i] = vehicle;
                    return i;
                }
            }

            return -1; // Parking lot is full
        }

        public bool LeaveSlot(int slotNumber)
        {
            if (parkingSlots.ContainsKey(slotNumber))
            {
                parkingSlots.Remove(slotNumber);
                return true;
            }

            return false; // Slot number doesn't exist or is already vacant
        }

        public IEnumerable<int> GetOccupiedSlots()
        {
            return parkingSlots.Keys.OrderBy(x => x);
        }

        public Vehicle GetVehicleInSlot(int slotNumber)
        {
            if (parkingSlots.ContainsKey(slotNumber))
            {
                return parkingSlots[slotNumber];
            }

            return null;
        }

        public int GetNumberOfVehiclesByType(string vehicleType)
        {
            return parkingSlots.Values.Count(x => x.Type == vehicleType);
        }

        public IEnumerable<string> GetRegistrationNumbersWithOddPlate()
        {
            return parkingSlots.Values
                .Where(x => int.Parse(x.RegistrationNumber.Split('-')[1]) % 2 == 1)
                .Select(x => x.RegistrationNumber);
        }

        public IEnumerable<string> GetRegistrationNumbersWithEvenPlate()
        {
            return parkingSlots.Values
                .Where(x => int.Parse(x.RegistrationNumber.Split('-')[1]) % 2 == 0)
                .Select(x => x.RegistrationNumber);
        }

        public IEnumerable<string> GetRegistrationNumbersByColor(string color)
        {
            return parkingSlots.Values
                .Where(x => x.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.RegistrationNumber);
        }

        public IEnumerable<int> GetSlotNumbersByColor(string color)
        {
            return parkingSlots.Where(x => x.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Key);
        }
    }
}
