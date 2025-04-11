using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using Project.ViewModels.UpdateViewModels;
using System.Transactions;
using System;

namespace Tests.ViewModel.Update;

[TestClass]
public class TestScheduleUpdateViewModel
{
    private ScheduleUpdateViewModel _scheduleUpdateViewModel = new ScheduleUpdateViewModel();

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
            _scheduleUpdateViewModel = new ScheduleUpdateViewModel();
            _scheduleUpdateViewModel.Schedules.Should().NotBeNull();
            _scheduleUpdateViewModel.Schedules.Count.Should().Be(10);
            _scheduleUpdateViewModel.ErrorMessage.Should().Be(string.Empty);
            _scheduleUpdateViewModel.SaveChangesCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldUpdateSchedule_WhenValidData()
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

            _scheduleUpdateViewModel.Schedules[0].DoctorID = 2;
            _scheduleUpdateViewModel.SaveChangesCommand.Execute(null);
            _scheduleUpdateViewModel.Schedules[0].DoctorID.Should().Be(2);
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldSetErrorMessage_WhenInvalidData()
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

            _scheduleUpdateViewModel.Schedules[0].DoctorID = -2;
            _scheduleUpdateViewModel.SaveChangesCommand.Execute(null);
            _scheduleUpdateViewModel.Schedules[0].DoctorID.Should().NotBe(2);
            
            _scheduleUpdateViewModel.ErrorMessage.Should().NotBe(string.Empty);
        }
    }

    [TestMethod]
    public void OnPropertyChanged_ShouldRaisePropertyChangedEvent()
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
            bool eventRaised = false;
            _scheduleUpdateViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_scheduleUpdateViewModel.ErrorMessage))
                {
                    eventRaised = true;
                }
            };
            _scheduleUpdateViewModel.ErrorMessage = "New Error Message";
            eventRaised.Should().BeTrue();
        }
    }
}