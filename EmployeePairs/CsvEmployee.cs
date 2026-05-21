namespace EmployeePairs
{
    record class CsvEmployee
    {
        public int EmpID { get; init; }

        public int ProjectID { get; init; }

        public DateOnly DateFrom { get; init; }

        public DateOnly DateTo { get; init; }
    }
}
