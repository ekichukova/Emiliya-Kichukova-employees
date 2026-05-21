namespace Employee.Web.Models
{
    public class EmployeePairViewModel
    {
        public int EmployeeId1 { get; set; }
        public int EmployeeId2 { get; set; }
        public List<ProjectCollaborationViewModel> Projects { get; set; } = new();
        public int TotalDaysWorkedTogether { get; set; }
    }
}
