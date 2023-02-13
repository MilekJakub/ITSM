using ITSM.Data;
using ITSM.Models;

namespace ITSM.Services
{
    public interface IJobPositionService
    {
        bool AddJobPosition(JobPosition jobPosition);
        IEnumerable<JobPosition> GetAll();
    }

    public class JobPositionService : IJobPositionService
    {
        private readonly BoardsContext _dbContext;

        public JobPositionService(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<JobPosition> GetAll()
        {
            return _dbContext.JobPositions;
        }

        public bool AddJobPosition(JobPosition jobPosition)
        {
            try
            {
                _dbContext.JobPositions.Add(jobPosition);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
