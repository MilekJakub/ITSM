#pragma warning disable CS8618


namespace ITSM.ViewModels.ProjectViewModels
{
    public class TruncatedProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? WorkItems { get; set; }
    }
}
