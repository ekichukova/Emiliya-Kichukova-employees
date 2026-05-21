using EmployeePairs;

namespace Employees
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Add csv file path here:
            string csvFilePath = @"";

            var service = new EmployeePairsService();
            var result = service.GroupEmployees(csvFilePath);

            foreach (var employeePair in result)
            {
                Console.WriteLine($"{employeePair.Key.Item1}, {employeePair.Key.Item2}:");
                foreach (var project in employeePair.Value.DaysWorkedPerProject)
                {
                    Console.WriteLine($"    Project #{project.Key} - {project.Value} days.");
                }

                Console.WriteLine($"Total days {employeePair.Value.TotalDaysTogether}");
            };
        }
    }
}
