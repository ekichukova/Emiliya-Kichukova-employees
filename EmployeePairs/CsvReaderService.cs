namespace EmployeePairs
{
    internal class CsvReaderService
    {
        private static readonly string[] SupportedFormats =
        [
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "dd-MM-yyyy",
            "dd.MM.yyyy"
        ];

        internal List<CsvEmployee> ReadFile(string filePath)
        {
            var result = new HashSet<CsvEmployee>();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(',');

                var employeeProject = new CsvEmployee
                {
                    EmpID = int.Parse(parts[0].Trim()),
                    ProjectID = int.Parse(parts[1].Trim()),
                    DateFrom = ParseDate(parts[2].Trim()),
                    DateTo = ParseDateTo(parts[3].Trim())
                };

                ValidateEntry(employeeProject);
                result.Add(employeeProject);
            }

            return result.ToList();
        }

        private DateOnly ParseDate(string value)
        {
            DateOnly parsedDate;

            if (DateOnly.TryParseExact(value, SupportedFormats, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unsupported data format.");
            }
        }

        private DateOnly ParseDateTo(string value)
        {
            if (value.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }

            return ParseDate(value);
        }

        private void ValidateEntry(CsvEmployee employee)
        {
            if (employee.DateFrom > employee.DateTo)
            {
                throw new InvalidDataException($"Employee {employee.EmpID} has wrong dates.");
            }
        }
    }
}
