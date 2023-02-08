using ITSM.Models;
using ITSM.ViewModels.ProjectViewModels;
using ITSM.ViewModels.StateViewModels;

namespace ITSM.Services
{
    public interface IStateService
    {
        void Create(State projectDetailsVM);
        State? GetById(int id);
        IEnumerable<TruncatedStateViewModel> GetTruncatedStates();
        bool Update(State projectCreateVM);
        bool Delete(int id);
    }

    public class StateService
    {
    }
}
