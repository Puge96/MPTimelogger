using Data.Entities;

namespace Application.Projects.Models
{
    public class ProjectDTO
    {
        public  ProjectDTO(Data.Entities.Project model)
        {
            this.ProjectId = model.ProjectId;
            this.Name = model.Name;
            this.Status = model.Status;
            this.StartDate = model.StartDate;
            this.EndDate = model.EndDate;
            this.CustomerId = model.CustomerId;
            this.CompanyId = model.CompanyId;
        }

        public int ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ProjectStatus Status { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int CustomerId { get; set; }

        public int CompanyId { get; set; }
    }
}
