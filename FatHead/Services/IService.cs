using System.Threading.Tasks;

namespace FatHead.Services.Interfaces
{
    public interface IService
    {
        void PerformTasks();

        Task PerformTasksAsync();
    }
}
