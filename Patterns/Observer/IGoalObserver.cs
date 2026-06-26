using GoodGoals.Models;

namespace GoodGoals.Patterns.Observer
{
    // Interfaz Observer: cualquier clase que quiera "escuchar"
    // cambios en las metas debe implementar esta interfaz
    public interface IGoalObserver
    {
        Task OnGoalCompletedAsync(Goal goal);
    }
}
