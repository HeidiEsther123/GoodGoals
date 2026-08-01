using GoodGoals.Patterns.Factory;
using Xunit;

namespace GoodGoals.Tests.Patterns
{
    public class ReminderFactoryTests
    {
        private const string UserId = "user-1";

        [Fact]
        public void Create_ConTipoOneTime_ProgramaParaUnaHoraDespues()
        {
            // Arrange
            var factory = new ReminderFactory();

            // Act
            var reminder = factory.Create("Tomar agua", UserId, ReminderType.OneTime);

            // Assert
            Assert.Equal("Tomar agua", reminder.Title);
            Assert.True((reminder.RemindAt - DateTime.Now).TotalMinutes is > 55 and < 65);
        }

        [Fact]
        public void Create_ConTipoDaily_AgregaPrefijoDiarioYProgramaUnDiaDespues()
        {
            // Arrange
            var factory = new ReminderFactory();

            // Act
            var reminder = factory.Create("Meditar", UserId, ReminderType.Daily);

            // Assert
            Assert.Equal("[Diario] Meditar", reminder.Title);
            Assert.True((reminder.RemindAt - DateTime.Now).TotalHours is > 23 and < 25);
        }

        [Fact]
        public void Create_ConTipoWeekly_AgregaPrefijoSemanalYProgramaUnaSemanaDespues()
        {
            // Arrange
            var factory = new ReminderFactory();

            // Act
            var reminder = factory.Create("Revisar metas", UserId, ReminderType.Weekly);

            // Assert
            Assert.Equal("[Semanal] Revisar metas", reminder.Title);
            Assert.True((reminder.RemindAt - DateTime.Now).TotalDays is > 6.9 and < 7.1);
        }

        [Fact]
        public void Create_SiempreAsignaElUserIdYMarcaComoNoEnviado()
        {
            // Arrange
            var factory = new ReminderFactory();

            // Act
            var reminder = factory.Create("Estirar", UserId, ReminderType.OneTime);

            // Assert
            Assert.Equal(UserId, reminder.UserId);
            Assert.False(reminder.IsSent);
        }
    }
}
