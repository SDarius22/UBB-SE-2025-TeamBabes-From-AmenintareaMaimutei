using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Project.ViewModels.DeleteViewModels;
using Microsoft.Data.SqlClient;
using Project.Utils;
using System.Transactions;

namespace Tests.ViewModel.Delete;

[TestClass]
public class TestShiftDeleteViewModel
{
    private ShiftDeleteViewModel _shiftDeleteViewModel = new ShiftDeleteViewModel();

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

            _shiftDeleteViewModel.ShiftID.Should().Be(0);
            _shiftDeleteViewModel.ErrorMessage.Should().Be(String.Empty);
            _shiftDeleteViewModel.MessageColor.Should().Be("Red");
            _shiftDeleteViewModel.Shifts.Count.Should().Be(10);

        }
    }

    [TestMethod]
    public void DeleteShiftCommand_ShouldDeleteShift_WhenValidID()
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
            _shiftDeleteViewModel.ShiftID = 1;
            _shiftDeleteViewModel.DeleteShiftCommand.Execute(null);
            _shiftDeleteViewModel.ErrorMessage.Should().Be("Shift was successfully deleted");
            _shiftDeleteViewModel.MessageColor.Should().Be("Green");
        }
    }

    [TestMethod]
    public void DeleteShiftCommand_ShouldNotDeleteShift_WhenInvalidID()
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
            _shiftDeleteViewModel.ShiftID = -1;
            _shiftDeleteViewModel.DeleteShiftCommand.Execute(null);
            _shiftDeleteViewModel.ErrorMessage.Should().Be("ShiftID doesn't exist in the records");
            _shiftDeleteViewModel.MessageColor.Should().Be("Red");
        }
    }

}