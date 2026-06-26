using GoodGoals.Models;

namespace GoodGoals.Patterns.Observer
{
    // Gestor de eventos (Subject concreto): mantiene la lista de
    // observadores y los notifica cuando una meta se completa.
    public class GoalEventManager : IGoalSubject
    {
        private readonly List<IGoalObserver> _observers = new();

        public void Subscribe(IGoalObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Unsubscribe(IGoalObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyGoalCompletedAsync(Goal goal)
        {
            foreach (var observer in _observers)
            {
                await observer.OnGoalCompletedAsync(goal);
            }
        }
    }
}
