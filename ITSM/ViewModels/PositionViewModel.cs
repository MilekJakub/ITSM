#pragma warning disable CS8618

using ITSM.Models;

namespace ITSM.ViewModels
{
    public class PositionViewModel
    {
        public JobPosition Position { get; set; }
        public IEnumerable<JobPosition> JobPositions { get; set; }
    }
}
