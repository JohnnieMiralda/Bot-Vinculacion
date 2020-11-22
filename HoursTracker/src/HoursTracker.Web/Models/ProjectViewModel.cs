namespace HoursTracker.Web.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        public string VinculationType { get; set; }

        public int VinculationId { get; set; }
    }
}
