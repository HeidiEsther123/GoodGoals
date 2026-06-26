namespace GoodGoals.Patterns.Observer
{
    // Interfaz Subject: la meta puede registrar y notificar observadores
    public interface IGoalSubject
    {
        void Subscribe(IGoalObserver observer);
        void Unsubscribe(IGoalObserver observer);
        Task NotifyGoalCompletedAsync(GoodGoals.Models.Goal goal);
    }
}
