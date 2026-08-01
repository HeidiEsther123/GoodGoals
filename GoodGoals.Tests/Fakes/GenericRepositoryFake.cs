using GoodGoals.Repositories;

namespace GoodGoals.Tests.Fakes
{
    // ---------------------------------------------------------------------
    // Fake en memoria de IGenericRepository<T> — mismo Port que usa el
    // proyecto real, con una lista controlada para la prueba. No es una
    // librería de mocks: es una clase real que implementa la interfaz.
    // ---------------------------------------------------------------------
    public class GenericRepositoryFake<T> : IGenericRepository<T> where T : class
    {
        public List<T> Items { get; }

        public GenericRepositoryFake(List<T> items) => Items = items;

        public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult(Items.AsEnumerable());

        public Task<T?> GetByIdAsync(int id)
        {
            // Usamos reflexión mínima solo para leer la propiedad "Id",
            // ya que el repositorio genérico no conoce el tipo concreto.
            var entity = Items.FirstOrDefault(e =>
                (int)(e.GetType().GetProperty("Id")?.GetValue(e) ?? -1) == id);
            return Task.FromResult(entity);
        }

        public Task AddAsync(T entity)
        {
            Items.Add(entity);
            return Task.CompletedTask;
        }

        public void Update(T entity)
        {
            // En este fake, como trabajamos sobre la misma instancia
            // en memoria, no hace falta reemplazar nada — el cambio
            // ya está reflejado en el objeto de la lista.
        }

        public void Delete(T entity) => Items.Remove(entity);

        public Task SaveChangesAsync() => Task.CompletedTask;
    }
}
