using GoodGoals.Models;
using GoodGoals.Services;
using GoodGoals.Tests.Fakes;
using Xunit;

namespace GoodGoals.Tests.Services
{
    public class GoalServiceTests
    {
        private const string UserId = "user-1";
        private const string OtroUserId = "user-2";

        private List<Goal> CrearGoalsDePrueba() => new()
        {
            new Goal { Id = 1, Title = "Aprender xUnit", UserId = UserId, CreatedAt = DateTime.Now.AddDays(-2) },
            new Goal { Id = 2, Title = "Correr 5k", UserId = UserId, CreatedAt = DateTime.Now.AddDays(-1) },
            new Goal { Id = 3, Title = "Meta de otro usuario", UserId = OtroUserId, CreatedAt = DateTime.Now }
        };

        [Fact]
        public async Task GetUserGoalsAsync_SoloRegresaLasMetasDelUsuario()
        {
            // Arrange
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(CrearGoalsDePrueba()),
                new GenericRepositoryFake<TaskItem>(new List<TaskItem>()));

            // Act
            var resultado = await service.GetUserGoalsAsync(UserId);

            // Assert
            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, g => Assert.Equal(UserId, g.UserId));
        }

        [Fact]
        public async Task GetByIdAsync_ConMetaDeOtroUsuario_RegresaNull()
        {
            // Arrange
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(CrearGoalsDePrueba()),
                new GenericRepositoryFake<TaskItem>(new List<TaskItem>()));

            // Act — la meta Id=3 pertenece a otro usuario
            var resultado = await service.GetByIdAsync(3, UserId);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task CreateAsync_AsignaFechaDeCreacionYLaAgregaAlRepositorio()
        {
            // Arrange
            var goals = CrearGoalsDePrueba();
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(goals),
                new GenericRepositoryFake<TaskItem>(new List<TaskItem>()));
            var nueva = new Goal { Id = 4, Title = "Leer un libro", UserId = UserId };

            // Act
            await service.CreateAsync(nueva);

            // Assert
            Assert.Equal(4, goals.Count);
            Assert.True((DateTime.Now - nueva.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task ToggleCompletedAsync_ConMetaExistente_InviertelIsCompleted()
        {
            // Arrange
            var goals = CrearGoalsDePrueba();
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(goals),
                new GenericRepositoryFake<TaskItem>(new List<TaskItem>()));

            // Act
            var resultado = await service.ToggleCompletedAsync(1, UserId);

            // Assert
            Assert.True(resultado);
            Assert.True(goals.First(g => g.Id == 1).IsCompleted);
        }

        [Fact]
        public async Task ToggleCompletedAsync_ConIdInexistente_RegresaFalse()
        {
            // Arrange
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(CrearGoalsDePrueba()),
                new GenericRepositoryFake<TaskItem>(new List<TaskItem>()));

            // Act
            var resultado = await service.ToggleCompletedAsync(999, UserId);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task DeleteAsync_DesvinculaTareasAsociadasAntesDeEliminarLaMeta()
        {
            // Arrange
            var goals = CrearGoalsDePrueba();
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Subtarea 1", UserId = UserId, GoalId = 1 },
                new TaskItem { Id = 2, Title = "Subtarea 2", UserId = UserId, GoalId = 1 }
            };
            var service = new GoalService(
                new GenericRepositoryFake<Goal>(goals),
                new GenericRepositoryFake<TaskItem>(tasks));

            // Act
            var resultado = await service.DeleteAsync(1, UserId);

            // Assert
            Assert.True(resultado);
            Assert.DoesNotContain(goals, g => g.Id == 1);
            Assert.All(tasks, t => Assert.Null(t.GoalId));
        }
    }
}
