namespace GoodGoals.Repositories
{
    // Repositorio genérico: capa de acceso a datos (EF Core) reutilizable
    // para cualquier entidad. Reduce código repetido entre Goal, TaskItem, Note, Reminder.
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
