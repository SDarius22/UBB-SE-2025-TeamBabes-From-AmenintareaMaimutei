using System;
using System.Transactions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Tests.Model;

[TestClass]
public class TestShiftModel
{
    private ShiftModel _shiftModel;

    [TestInitialize]
    public void TestInit()
    {
        _shiftModel = new ShiftModel();
    }

    [TestMethod]
    public void AddShift_ShouldReturnTrue_WhenInsertSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shift = new Shift
            {
                ShiftID = 1,
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var result = _shiftModel.AddShift(shift);
            result.Should().BeTrue();
            _shiftModel.DoesShiftExist(1).Should().BeTrue();
        }
    }

    [TestMethod]
    public void UpdateShift_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shift = new Shift
            {
                ShiftID = 1,
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            _shiftModel.AddShift(shift);

            shift.Date = new DateOnly(2025, 04, 13);
            var result = _shiftModel.UpdateShift(shift);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void UpdateShift_ShouldReturnFalse_WhenUpdateFails()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shift = new Shift
            {
                ShiftID = 999, // Non-existent ID
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var result = _shiftModel.UpdateShift(shift);
            result.Should().BeFalse();
        }
    }

    [TestMethod]
    public void GetShifts_ShouldReturnAllShifts_WhenShiftsExist()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shift1 = new Shift
            {
                ShiftID = 1,
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var shift2 = new Shift
            {
                ShiftID = 2,
                Date = new DateOnly(2025, 04, 13),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var shifts = _shiftModel.GetShifts();


            _shiftModel.AddShift(shift1);
            _shiftModel.AddShift(shift2);

            shifts = _shiftModel.GetShifts();
            shifts.Should().HaveCount(2);
        }
    }

    [TestMethod]
    public void GetShifts_ShouldReturnEmptyList_WhenNoShiftsExist()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shifts = _shiftModel.GetShifts();
            shifts.Should().BeEmpty();
        }
    }

    [TestMethod]
    public void DoesShiftExist_ShouldReturnTrue_WhenShiftExists()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var shift = new Shift
            {
                ShiftID = 1,
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            _shiftModel.AddShift(shift);
            var result = _shiftModel.DoesShiftExist(1);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void DoesShiftExist_ShouldReturnFalse_WhenShiftDoesNotExist()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);

            }
            var result = _shiftModel.DoesShiftExist(999); // Non-existent ID
            result.Should().BeFalse();
        }
    }

    [TestMethod]
    public void DeleteShift_ShouldReturnTrue_WhenDeleteSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);

            }
            var shift = new Shift
            {
                ShiftID = 1,
                Date = new DateOnly(2025, 04, 12),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            _shiftModel.AddShift(shift);
            var result = _shiftModel.DeleteShift(1);
            result.Should().BeTrue();
            _shiftModel.DoesShiftExist(1).Should().BeFalse();
        }
    }

    [TestMethod]
    public void DeleteShift_ShouldReturnFalse_WhenDeleteFails()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                ResetDatabase(connection);
            }

            var result = _shiftModel.DeleteShift(999); // Non-existent ID
            result.Should().BeFalse();
        }
    }

    private void ResetDatabase(SqlConnection connection)
    {
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
    }
}
