namespace EmployeePairs
{
    public class ProjectCollaborationData
    {
        /// <summary>
        /// Dictionary with the projects the employees worked together 
        /// and how many days they have worked for on the same project.
        /// </summary>
        public  Dictionary<int,int> ProjectsWorkedTogether { get; set; }

        /// <summary>
        /// Days they have worked together on all projects.
        /// </summary>
        public int TotalDaysTogether { get; set; }
    }
}
