using Application.Shared;

namespace Application.Projects.Models
{
	public class ProjectsModelResult : BaseJsonResult
	{
		public List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
	}
}
