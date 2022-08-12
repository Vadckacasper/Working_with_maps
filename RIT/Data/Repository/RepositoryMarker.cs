using RIT.Interfece;
using RIT.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIT
{
    public class RepositoryMarker : IRepository<Marker>
    {
        private string SqlConnection;
        public RepositoryMarker(string sqlConnection)
        {
            SqlConnection = sqlConnection;
        }
        public IEnumerable<Marker> GetAll()
        {
            List<Marker> _Markers = new List<Marker>();
            string sqlExpression = "SELECT * FROM Markers";
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) 
                    {
                        _Markers.Add(new Marker
                        {
                            Id = reader.GetInt32(0),
                            Latitude = reader.GetDouble(1),
                            Longitude = reader.GetDouble(2),

                        });
                    }
                }
                reader.Close();
            }
            return _Markers;
        }
        public bool Insert(Marker entity)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                connection.Open();
                string sqlExpression = "INSERT INTO Markers (id, Latitude, Longitude) VALUES (@id, @Lat, @Lon)";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@id", entity.Id);
                command.Parameters.AddWithValue("@Lat", entity.Latitude);
                command.Parameters.AddWithValue("@Lon", entity.Longitude);
                command.ExecuteNonQuery();
            }
            return true;
        }

        public bool Update(Marker entity)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                connection.Open();
                string sqlExpression = "UPDATE Markers SET Latitude = @Lat, Longitude = @Lon WHERE id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@id", entity.Id);
                command.Parameters.AddWithValue("@Lat", entity.Latitude);
                command.Parameters.AddWithValue("@Lon", entity.Longitude);
                command.ExecuteNonQuery();
            }
            return true;
        }
        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                connection.Open();
                string sqlExpression = "DELETE FROM Markers WHERE id=@id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}
