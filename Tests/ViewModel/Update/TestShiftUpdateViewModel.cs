using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using Project.ViewModels.UpdateViewModels;
using System.Transactions;

namespace Tests.ViewModel.Update;

[TestClass]
public class TestShiftUpdateViewModel
{
    private ShiftUpdateViewModel _shiftUpdateViewModel = new ShiftUpdateViewModel();

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
            _shiftUpdateViewModel = new ShiftUpdateViewModel();
            _shiftUpdateViewModel.Shifts.Should().NotBeNull();
            _shiftUpdateViewModel.Shifts.Count.Should().Be(10);
            _shiftUpdateViewModel.ErrorMessage.Should().Be(string.Empty);
            _shiftUpdateViewModel.SaveChangesCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldUpdateShift_WhenValidData()
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
            _shiftUpdateViewModel.Shifts[0].Date = new DateOnly(2011,11,11);
            _shiftUpdateViewModel.SaveChangesCommand.Execute(null);
            _shiftUpdateViewModel.Shifts[0].Date.Should().Be(new DateOnly(2011, 11, 11));
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

            _shiftUpdateViewModel.Shifts[0].StartTime = new TimeSpan(9, 0, 0);
            _shiftUpdateViewModel.SaveChangesCommand.Execute(null);
            _shiftUpdateViewModel.ErrorMessage.Should().NotBe(string.Empty);
            _shiftUpdateViewModel.ErrorMessage.Should().Contain("Shift 1: Start time");
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
            _shiftUpdateViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_shiftUpdateViewModel.ErrorMessage))
                {
                    eventRaised = true;
                }
            };
            _shiftUpdateViewModel.ErrorMessage = "New Error Message";
            eventRaised.Should().BeTrue();
        }
    }
}