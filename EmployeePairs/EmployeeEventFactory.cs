namespace EmployeePairs
{
    internal class EmployeeEventFactory
    {
        internal Dictionary<int, List<EmployeeEvent>> Build(List<CsvEmployee> csvEmployees)
        {
            Dictionary<int, List<EmployeeEvent>> result = new Dictionary<int, List<EmployeeEvent>>();

            var eventsByProject = csvEmployees
                .GroupBy(e => e.ProjectID)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .SelectMany(emp => new[]
                        {
                            new EmployeeEvent
                            {
                                EmployeeId = emp.EmpID,
                                Date = emp.DateFrom,
                                Started = true
                            },
                            new EmployeeEvent
                            {
                                EmployeeId = emp.EmpID,
                                Date = emp.DateTo,
                                Started = false
                            }
                        })
                        .OrderBy(e => e.Date)
                        .ThenByDescending(e => e.Started)
                        .ToList());

            return eventsByProject;
        }
    }
}
