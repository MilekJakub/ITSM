using ITSM.Data;
using ITSM.Models;

namespace ITSM.Services
{
    public interface IWorkItemService
    {
        
    }

    public class WorkItemService : IWorkItemService
    {
        private readonly BoardsContext _dbContext;

        public WorkItemService(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        
    }
}
