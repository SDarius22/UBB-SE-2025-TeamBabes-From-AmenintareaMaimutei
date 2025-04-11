using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Project.Utils;
using Project.ViewModels.DeleteViewModels;
using System.Transactions;

namespace Tests.ViewModel.Delete;

[TestClass]
public class TestScheduleDeleteVIewModel
{
    private ScheduleDeleteViewModel _scheduleDeleteViewModel = new ScheduleDeleteViewModel();

    [TestMethod]
    public void Constructor_ShouldInitializeProperties()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }
            }

            _scheduleDeleteViewModel.ScheduleID.Should().Be(0);
            _scheduleDeleteViewModel.ErrorMessage.Should().Be(string.Empty);
            _scheduleDeleteViewModel.MessageColor.Should().Be("Red");
            _scheduleDeleteViewModel.Schedules.Count.Should().Be(10);

        }
    }

    [TestMethod]
    public void DeletescheduleCommand_ShouldDeleteschedule_WhenValidID()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }
            }
            _scheduleDeleteViewModel.ScheduleID = 1;
            _scheduleDeleteViewModel.DeleteScheduleCommand.Execute(null);
            _scheduleDeleteViewModel.ErrorMessage.Should().Be("Schedule deleted successfully");
            _scheduleDeleteViewModel.MessageColor.Should().Be("Green");
        }
    }

    [TestMethod]
    public void DeletescheduleCommand_ShouldNotDeleteschedule_WhenInvalidID()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }
            }
            _scheduleDeleteViewModel.ScheduleID = -1;
            _scheduleDeleteViewModel.DeleteScheduleCommand.Execute(null);
            _scheduleDeleteViewModel.ErrorMessage.Should().Be("ScheduleID doesn't exist in the records");
            _scheduleDeleteViewModel.MessageColor.Should().Be("Red");
        }
    }

}