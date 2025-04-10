using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using System.Transactions;
using FluentAssertions;
using Project.ClassModels;
using Project.Models;

namespace Tests.Model;

[TestClass]
public class TestScheduleModel
{
    private ScheduleModel _scheduleModel;

    [TestInitialize]
    public void TestInitialize()
    {
        
        _scheduleModel = new ScheduleModel();
    }

    [TestMethod]
    public void AddSchedule_ShouldReturnTrue_WhenInsertSucceeds()
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
            // Arrange
            var schedule = new Schedule
            {
                ScheduleID = 1,
                DoctorID = 1,
                ShiftID = 1
            };

            // Act
            var result = _scheduleModel.AddSchedule(schedule);

            // Assert
            result.Should().Be(true);

            _scheduleModel.DoesScheduleExist(schedule.ScheduleID).Should().Be(true);
        }
    }

    [TestMethod]
    public void UpdateSchedule_ShouldReturnTrue_WhenUpdateSucceeds()
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
            // Arrange
            var schedule = new Schedule
            {
                ScheduleID = 1,
                DoctorID = 1,
                ShiftID = 1
            };

            // Act
            var result = _scheduleModel.AddSchedule(schedule);

            // Assert
            result.Should().Be(true);

            _scheduleModel.DoesScheduleExist(schedule.ScheduleID).Should().Be(true);

            schedule.DoctorID = 2;

            result = _scheduleModel.UpdateSchedule(schedule);

            result.Should().Be(true);

            _scheduleModel.GetSchedules().FirstOrDefault(s => s.ScheduleID == 1).DoctorID.Should().Be(2);
        }
    }

    [TestMethod]
    public void UpdateSchedule_ShouldReturnFalse_WhenUpdateFails()
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

            // Arrange
            var schedule = new Schedule
            {
                ScheduleID = 1,
                DoctorID = 1,
                ShiftID = 1
            };

            // Act
            var result = _scheduleModel.AddSchedule(schedule);

            // Assert
            result.Should().Be(true);

            _scheduleModel.DoesScheduleExist(schedule.ScheduleID).Should().Be(true);

            schedule.DoctorID = -2;

            result = _scheduleModel.UpdateSchedule(schedule);

            result.Should().Be(false);

            _scheduleModel.GetSchedules().FirstOrDefault(s => s.ScheduleID == 1).DoctorID.Should().Be(1);
        }
        
    }

    [TestMethod]
    public void DeleteSchedule_ShouldReturnTrue_WhenDeleteSucceeds()
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
            var result = _scheduleModel.DeleteSchedule(2);
            result.Should().BeTrue();
            result = _scheduleModel.DeleteSchedule(2);
            result.Should().BeFalse();
        }
    }

    [TestMethod]
    public void DoesScheduleExist_ShouldReturnTrue_WhenScheduleExists()
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

            var result = _scheduleModel.DoesScheduleExist(2);
            result.Should().BeTrue();
            _scheduleModel.DeleteSchedule(2);

            result = _scheduleModel.DoesScheduleExist(2);
            result.Should().BeFalse();
        }
    }

    [TestMethod]
    public void GetSchedules_ShouldReturnEquipmentList_WhenDataExists()
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
            var result = _scheduleModel.GetSchedules();
            result.Should().NotBeNull();
            result.Count.Should().Be(10);
            result[0].ScheduleID.Should().Be(1);
        }
    }

    [TestMethod]
    public void DoesDoctorExist_ShouldReturnTrue_WhenDoctorExists()
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

            var result = _scheduleModel.DoesDoctorExist(1);
            result.Should().BeTrue();

            result = _scheduleModel.DoesDoctorExist(999);
            result.Should().BeFalse();
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

            var result = _scheduleModel.DoesShiftExist(1);
            result.Should().BeTrue();

            result = _scheduleModel.DoesShiftExist(999);
            result.Should().BeFalse();
        }
        
    }

    [TestMethod]
    public void DoesShiftExist_ShouldReturnCorrectResult_BasedOnShiftExistence()
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

            // Act & Assert
            var result = _scheduleModel.DoesShiftExist(1);
            result.Should().BeTrue();

            result = _scheduleModel.DoesShiftExist(999);
            result.Should().BeFalse();
        }
        
    }


}