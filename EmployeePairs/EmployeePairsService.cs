namespace EmployeePairs
{
    public class EmployeePairsService
    {
        public Dictionary<Tuple<int, int>, ProjectCollaborationData>
            GroupEmployees(string csvFilePath)
        {
            var csvReader = new CsvReaderService();
            var csvEmployees = csvReader.ReadFile(csvFilePath);

            var factory = new EmployeeEventFactory();
            var projects = factory.Build(csvEmployees);

            Dictionary<Tuple<int, int>, ProjectCollaborationData> result = new Dictionary<Tuple<int, int>, ProjectCollaborationData>();

            foreach (var project in projects)
            {
                ProcessEmployeesForProject(project.Key, project.Value, result);
            }

            return result;
        }

        private void ProcessEmployeesForProject(
            int projectId,
            List<EmployeeEvent> employeesEvents,
            Dictionary<Tuple<int, int>, ProjectCollaborationData> result)
        {
            Dictionary<int, DateOnly> activeEmployees = new Dictionary<int, DateOnly>();

            foreach (var employeeEvent in employeesEvents)
            {
                if (employeeEvent.Started)
                {
                    activeEmployees.Add(employeeEvent.EmployeeId, employeeEvent.Date);
                }
                else
                {
                    var activeFrom = activeEmployees[employeeEvent.EmployeeId];
                    var activeTo = employeeEvent.Date;

                    activeEmployees.Remove(employeeEvent.EmployeeId);

                    foreach (var coWorker in activeEmployees)
                    {
                        var daysWorkedTogether = CalculateDaysWorkedTogether(activeFrom, activeTo, coWorker.Value);
                        
                        Tuple<int, int> coWorkersPair =
                            new Tuple<int, int> 
                            ( 
                                int.Min(employeeEvent.EmployeeId, coWorker.Key),
                                int.Max(employeeEvent.EmployeeId, coWorker.Key) 
                            );

                        AddCollaboration(result, coWorkersPair, projectId, daysWorkedTogether);
                    }
                }
            }
        }

        private int CalculateDaysWorkedTogether(DateOnly employeeStartDate, DateOnly employeeEndDate, DateOnly coWorkerStartDate)
        {
            var laterStartDate = employeeStartDate > coWorkerStartDate ? employeeStartDate : coWorkerStartDate;
            return employeeEndDate.DayNumber - laterStartDate.DayNumber + 1;
        }

        private void AddCollaboration(
            Dictionary<Tuple<int,int>, ProjectCollaborationData> collaborations ,
            Tuple<int,int> coWorkersPair,
            int projectId,
            int daysWorkedTogether)
        {

            // adds coworkers pair to the result in case it doesn't exists yet
            // and calculates the total days on all projects they have worked for.
            if (collaborations.ContainsKey(coWorkersPair) == false)
            {
                collaborations.Add(coWorkersPair, new ProjectCollaborationData()
                {
                    ProjectsWorkedTogether = new Dictionary<int, int>()
                });
            }
            collaborations[coWorkersPair].TotalDaysTogether += daysWorkedTogether;

            // adds the project to the coworkers in case it's a new project 
            // and calculates the total days they have worked on for the particular project.
            if (collaborations[coWorkersPair].ProjectsWorkedTogether.ContainsKey(projectId) == false)
            {
                collaborations[coWorkersPair].ProjectsWorkedTogether.Add(projectId, 0);
            }
            collaborations[coWorkersPair].ProjectsWorkedTogether[projectId] += daysWorkedTogether;
        }
    }
}
