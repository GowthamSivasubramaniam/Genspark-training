using cardioManagement.Services;
using cardioManagement.Models;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        AppointmentService appointmentService = new AppointmentService();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n----- Appointment Management Menu -----");
            Console.WriteLine("1. Add Appointment");
            Console.WriteLine("2. Search Appointments");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddAppointment(appointmentService);
                    break;

                case "2":
                    SearchAppointments(appointmentService);
                    break;

                case "3":
                    running = false;
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddAppointment(AppointmentService service)
    {
        Console.Write("Enter patient name: ");
        string? name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Invalid input. Please enter a valid name: ");
            name = Console.ReadLine();
        }

        Console.Write("Enter patient age: ");
        int age;
        while (!int.TryParse(Console.ReadLine(), out age))
        {
            Console.Write("Invalid input. Please enter a valid age: ");
        }

        Console.Write("Enter appointment reason: ");
        string? reason = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(reason))
        {
            Console.Write("Invalid input. Please enter a valid reason: ");
            reason = Console.ReadLine();
        }

        var appointment = new Appointment
        {
            PatientName = name ?? "",
            PatientAge = age,
            AppointmentDate = DateTime.Now,
            Reason = reason ?? ""
        };

        int id = service.Add(appointment);
        Console.WriteLine($"Appointment added with ID: {id}");
    }

    static void SearchAppointments(AppointmentService service)
    {
        var search = new SearchModel();

        Console.Write("Enter ID (optional): ");
        string? idInput = Console.ReadLine();
        if (int.TryParse(idInput, out int id))
            search.Id = id;

        Console.Write("Enter patient name (optional): ");
        string? name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            search.PatientName = name;

        Console.Write("Enter patient age (optional): ");
        string? ageInput = Console.ReadLine();
        if (int.TryParse(ageInput, out int age))
            search.PatientAge = age;

        Console.Write("Enter appointment date (yyyy-mm-dd) (optional): ");
        string? dateInput = Console.ReadLine();
        if (DateTime.TryParse(dateInput, out DateTime date))
            search.AppointmentDate = date;

        Console.Write("Enter reason (optional): ");
        string? reason = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(reason))
            search.Reason = reason;

        var results = service.SearchAppointments(search);
        Console.WriteLine($"\nFound {results.Count} result(s):");
        foreach (var a in results)
        {
            Console.WriteLine($"ID: {a.Id}, Name: {a.PatientName}, Age: {a.PatientAge}, Date: {a.AppointmentDate}, Reason: {a.Reason}");
        }
    }
}
