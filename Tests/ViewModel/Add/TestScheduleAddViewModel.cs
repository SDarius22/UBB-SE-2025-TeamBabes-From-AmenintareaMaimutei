using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using Project.ViewModels.AddViewModels;
using System.Collections.Generic;
using System.Transactions;
using FluentAssertions;

namespace Tests.ViewModel.Add;

[TestClass]
public class TestScheduleAddViewModel
{
    private ScheduleAddViewModel _scheduleAddViewModel = new ScheduleAddViewModel();

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

            _scheduleAddViewModel.Schedules.Should().NotBeNull();
            _scheduleAddViewModel.Schedules.Count.Should().Be(10);
            _scheduleAddViewModel.DoctorID.Should().Be(0);
            _scheduleAddViewModel.ErrorMessage.Should().Be(string.Empty);
            _scheduleAddViewModel.ShiftID.Should().Be(0);
        }
    }

    [TestMethod]
    public void SaveSchedule_ShouldAddSchedule_WhenValidData()
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

            _scheduleAddViewModel.ShiftID = 1;
            _scheduleAddViewModel.DoctorID= 1;

            _scheduleAddViewModel.SaveScheduleCommand.Execute(null);

            _scheduleAddViewModel.Schedules.Should().NotBeNull();
            _scheduleAddViewModel.Schedules.Count.Should().Be(11);
        }
    }

    [TestMethod]
    public void SaveSchedule_ShouldNotAddSchedule_WhenInvalidData()
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
            _scheduleAddViewModel.ShiftID = 1;
            _scheduleAddViewModel.DoctorID = -1;

            _scheduleAddViewModel.SaveScheduleCommand.Execute(null);

            _scheduleAddViewModel.Schedules.Should().NotBeNull();
            _scheduleAddViewModel.Schedules.Count.Should().Be(10);
        }
    }

    [TestMethod]
    public void OnPropertyChange_ShouldRaisePropertyChangedEvent()
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
            var changedProperties = new List<string>();
            _scheduleAddViewModel.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            _scheduleAddViewModel.DoctorID = 3;
            _scheduleAddViewModel.ErrorMessage = ":3";
            _scheduleAddViewModel.ShiftID = 2;

            changedProperties.Should().Contain(new List<string>
            {
                nameof(_scheduleAddViewModel.DoctorID),
                nameof(_scheduleAddViewModel.ErrorMessage),
                nameof(_scheduleAddViewModel.ShiftID)
            });
        }
    }
}