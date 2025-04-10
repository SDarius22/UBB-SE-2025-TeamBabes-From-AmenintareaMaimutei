using System;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using Project.ViewModels.AddViewModels;
using System.Collections.Generic;
using System.Transactions;
using FluentAssertions;
using Project.Models;

namespace Tests.ViewModel.Add;

[TestClass]
public class TestShiftAddViewModel
{
    private ShiftAddViewModel _shiftAddViewModel = new ShiftAddViewModel();

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

            _shiftAddViewModel.Shifts.Should().NotBeNull();
            _shiftAddViewModel.Shifts.Count.Should().Be(10);
            _shiftAddViewModel.ErrorMessage.Should().Be(string.Empty);
            _shiftAddViewModel.StartTime.Should().Be(new TimeSpan());
            _shiftAddViewModel.Date.Should().Be(new DateOnly());
            _shiftAddViewModel.EndTime.Should().Be(new TimeSpan());
        }
    }

    [TestMethod]
    public void SaveShift_ShouldAddShift_WhenValidData()
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

            _shiftAddViewModel.StartTime = new TimeSpan(8, 0, 0);
            _shiftAddViewModel.EndTime = new TimeSpan(20, 0, 0);
            _shiftAddViewModel.Date = new DateOnly(2014,12,12);
            
            _shiftAddViewModel.SaveShiftCommand.Execute(null);

            _shiftAddViewModel.Shifts.Should().NotBeNull();
            _shiftAddViewModel.Shifts.Count.Should().Be(11);
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
            _shiftAddViewModel.StartTime = new TimeSpan(8, 0, 0);
            _shiftAddViewModel.EndTime = new TimeSpan(21, 0, 0);
            _shiftAddViewModel.Date = new DateOnly(2014, 12, 12);

            _shiftAddViewModel.SaveShiftCommand.Execute(null);

            _shiftAddViewModel.Shifts.Should().NotBeNull();
            _shiftAddViewModel.Shifts.Count.Should().Be(10);
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
            _shiftAddViewModel.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            _shiftAddViewModel.ErrorMessage = ":3";

            changedProperties.Should().Contain(new List<string>
            {
                nameof(_shiftAddViewModel.ErrorMessage),
            });
        }
    }
}