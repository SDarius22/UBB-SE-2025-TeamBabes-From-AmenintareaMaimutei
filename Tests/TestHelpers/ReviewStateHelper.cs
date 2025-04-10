using Microsoft.Data.SqlClient;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestHelpers
{
    class ReviewStateHelper : IDatabaseStateHelper
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();
        private DataTable storedState;

        public void StoreState()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Review";
                SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection);
                storedState = new DataTable();
                adapter.Fill(storedState);

                string deleteQuery = "DELETE FROM Review";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void RestoreState()
        {
            if (storedState == null)
                throw new InvalidOperationException("No stored state available to restore.");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Review";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.ExecuteNonQuery();

                foreach (DataRow row in storedState.Rows)
                {
                    string insertQuery = @"
                        INSERT INTO Review (ReviewID, MedicalRecordID, Text, NrStars)
                        VALUES (@ReviewID, @MedicalRecordID, @Text, @NrStars)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ReviewID", row["ReviewID"]);
                        insertCommand.Parameters.AddWithValue("@MedicalRecordID", row["MedicalRecordID"]);
                        insertCommand.Parameters.AddWithValue("@Text", row["Text"]);
                        insertCommand.Parameters.AddWithValue("@NrStars", row["NrStars"]);

                        insertCommand.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
