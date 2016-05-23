using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.DAL
{
    public class TblVerzekering : ICrudable<VerzekeringModel>
    {
        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

        public int Create(VerzekeringModel t)
        {
            int result = 0;
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblVerzekeringInsert", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Omschrijving", t.Omschrijving);
                SqlParameter id = new SqlParameter("@idVerzekering", SqlDbType.Int);
                id.Direction = ParameterDirection.Output;
                command.Parameters.Add(id);
                try
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        result = (int)id.Value;
                    }
                }
                catch { }
            }

            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblVerzekeringDelete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("idVerzekering", id);
                try
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                }
                catch { }
            }

            return result;
        }

        public List<VerzekeringModel> GetAll()
        {
            List<VerzekeringModel> verzekeringen = new List<VerzekeringModel>();
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblVerzekeringReadAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            VerzekeringModel verzekering = new VerzekeringModel();
                            verzekering.Id = (int?)reader["idVerzekering"];
                            verzekering.Omschrijving = reader["omschrijving"].ToString();
                            verzekeringen.Add(verzekering);
                        }
                    }
                }
                catch { }
            }

            return verzekeringen;
        }

        public VerzekeringModel GetById(int id)
        {
            VerzekeringModel verzekering = new VerzekeringModel();
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblVerzekeringReadOne", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idVerzekering", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        verzekering.Id = (int?)reader["idVerzekering"];
                        verzekering.Omschrijving = reader["omschrijving"].ToString();
                    }
                }
                catch { }
            }

            return verzekering;
        }

        public bool Update(VerzekeringModel t)
        {
            bool result = false;
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblVerzekeringUpdate", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idVerzekering", t.Id);
                command.Parameters.AddWithValue("@Omschrijving", t.Omschrijving);
                try
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                }
                catch { }
            }

            return result;
        }
    }
}