﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the DoctorModel class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Project.ClassModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// Represents the model for managing doctor-related operations.
    /// </summary>
    public class DoctorModel
    {
        private const double Type0Rate = 200d;
        private const double Type1Rate = Type0Rate * 1.2d;
        private const double Type2Rate = Type1Rate * 1.5d;

        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Adds a new doctor to the database.
        /// </summary>
        /// <param name="doctor">The doctor to add.</param>
        /// <returns>True if the doctor was added successfully; otherwise, false.</returns>
        public bool AddDoctor(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString)) // Added 'this.'
            {
                string query = "INSERT INTO Doctors (UserID, DepartmentID, Experience, Rating, LicenseNumber) VALUES (@UserID, @DepartmentID, @Experience, @Rating, @LicenseNumber)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", doctor.UserID);
                command.Parameters.AddWithValue("@DepartmentID", doctor.DepartmentID);
                command.Parameters.AddWithValue("@Experience", doctor.Experience);
                command.Parameters.AddWithValue("@Rating", doctor.Rating);
                command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Updates an existing doctor's details in the database.
        /// </summary>
        /// <param name="doctor">The doctor with updated details.</param>
        /// <returns>True if the doctor was updated successfully; otherwise, false.</returns>
        public bool UpdateDoctor(Doctor doctor)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Doctors SET UserID = @UserID, DepartmentID = @DepartmentID, Experience = @Experience, LicenseNumber = @LicenseNumber WHERE DoctorID = @DoctorID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", doctor.UserID);
                    command.Parameters.AddWithValue("@DepartmentID", doctor.DepartmentID);
                    command.Parameters.AddWithValue("@Experience", doctor.Experience);
                    command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
                    command.Parameters.AddWithValue("@DoctorID", doctor.DoctorID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a doctor from the database based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorID">The ID of the doctor to delete.</param>
        /// <returns>True if the doctor was deleted successfully; otherwise, false.</returns>
        public bool DeleteDoctor(int doctorID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString)) // Added 'this.'
            {
                string query = "DELETE FROM Doctors WHERE DoctorID = @DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", doctorID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Checks if a doctor exists in the database based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorID">The ID of the doctor to check.</param>
        /// <returns>True if the doctor exists; otherwise, false.</returns>
        public bool DoesDoctorExist(int doctorID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Doctors WHERE DoctorID = @DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", doctorID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks if a user is already a doctor in the database.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <returns>True if the user is already a doctor; otherwise, false.</returns>
        public bool IsUserAlreadyDoctor(int userID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Doctors WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks if a user exists in the database.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        public bool DoesUserExist(int userID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks if a user has the role of a doctor.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <returns>True if the user is a doctor; otherwise, false.</returns>
        public bool IsUserDoctor(int userID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT Role FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                string role = (string)command.ExecuteScalar();
                return role == "Doctor";
            }
        }

        /// <summary>
        /// Checks if a department exists in the database.
        /// </summary>
        /// <param name="departmentID">The ID of the department to check.</param>
        /// <returns>True if the department exists; otherwise, false.</returns>
        public bool DoesDepartmentExist(int departmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks if a user exists in the doctors table but with a different doctor ID.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <param name="doctorID">The ID of the doctor to exclude from the check.</param>
        /// <returns>True if the user exists in the doctors table with a different doctor ID; otherwise, false.</returns>
        public bool UserExistsInDoctors(int userID, int doctorID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Doctors WHERE UserID = @UserID AND DoctorID != @DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@DoctorID", doctorID);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Retrieves the shifts for the current month for a specific doctor.
        /// </summary>
        /// <param name="doctorID">The ID of the doctor whose shifts are to be retrieved.</param>
        /// <returns>A list of shifts for the current month.</returns>
        public List<Shift> GetShiftsForCurrentMonth(int doctorID)
        {
            List<Shift> shifts = new List<Shift>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = @"
                    SELECT s.ShiftID, s.Date, s.StartTime, s.EndTime
                    FROM GetCurrentMonthShiftsForDoctor(@DoctorID) s";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", doctorID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    shifts.Add(new Shift
                    {
                        ShiftID = reader.GetInt32(0),
                        Date = DateOnly.FromDateTime(reader.GetDateTime(1)),
                        StartTime = reader.GetTimeSpan(2),
                        EndTime = reader.GetTimeSpan(3),
                    });
                }
            }

            return shifts;
        }

        /// <summary>
        /// Computes the salary of a doctor for the current month based on their shifts.
        /// </summary>
        /// <param name="doctorID">The ID of the doctor whose salary is to be computed.</param>
        /// <returns>The total salary of the doctor for the current month.</returns>
        public double ComputeDoctorSalary(int doctorID)
        {
            List<Shift> shifts = this.GetShiftsForCurrentMonth(doctorID);
            double totalSalary = 0;

            foreach (var shift in shifts)
            {
                double shiftRate = 0;

                if (shift.StartTime == new TimeSpan(8, 0, 0) && shift.EndTime == new TimeSpan(20, 0, 0))
                {
                    shiftRate = Type0Rate * 12;
                }
                else if (shift.StartTime == new TimeSpan(20, 0, 0) && shift.EndTime == new TimeSpan(8, 0, 0))
                {
                    shiftRate = Type1Rate * 12;
                }
                else if (shift.StartTime == new TimeSpan(8, 0, 0) && shift.EndTime == new TimeSpan(8, 0, 0).Add(TimeSpan.FromDays(1)))
                {
                    shiftRate = Type2Rate * 24;
                }

                totalSalary += shiftRate;
            }

            return totalSalary;
        }

        /// <summary>
        /// Retrieves all doctors from the database.
        /// </summary>
        /// <returns>A list of all doctors.</returns>
        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT * FROM Doctors";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        Experience = (float)reader.GetDouble(2),
                        Rating = (float)reader.GetDouble(3),
                        DepartmentID = reader.GetInt32(4),
                        LicenseNumber = reader.GetString(5),
                    });
                }
            }

            return doctors;
        }
    }
}