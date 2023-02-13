using ITSM.Data;
using ITSM.Models;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public interface IStateService
    {
        bool Create(State state);
        State? GetById(int id);
        IEnumerable<State>? GetAll();
        bool Update(State state);
        bool Delete(int id);
    }

    public class StateService : IStateService
    {
        private readonly BoardsContext _dbContext;

        public StateService(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(State state)
        {
            try
            {
                _dbContext.States.Add(state);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<State>? GetAll()
        {
            var states =
                _dbContext.States
                .Include(x => x.WorkItems)
                .ToList();

            return states;
        }

        public State? GetById(int id)
        {
            var state = _dbContext.States
                .Include(x => x.WorkItems)
                .FirstOrDefault(x => x.Id == id);

            return state;
        }

        public bool Update(State state)
        {
            var dbState = _dbContext.States.FirstOrDefault(x => x.Id == state.Id);

            if(dbState == null)
                return false;

            dbState.Name = state.Name;
            dbState.Description = state.Description;
            dbState.WorkItems = state.WorkItems;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public bool Delete(int id)
        {
            var state = _dbContext.States.FirstOrDefault(x => x.Id == id);

            if(state == null)
                return false;

            try
            {
                _dbContext.Remove(state);
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
