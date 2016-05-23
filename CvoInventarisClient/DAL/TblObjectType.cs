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
    public class TblObjectType : ICrudable<ObjectTypeModel>
    {
        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

        public int Create(ObjectTypeModel t)
        {
            int result = 0;
            using (connection)
            {
                SqlCommand command = new SqlCommand("TbIObjectTypeInsert", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Omschrijving", t.Omschrijving);
                SqlParameter id = new SqlParameter("@idObjectType", SqlDbType.Int);
                id.Direction = ParameterDirection.Output;
                command.Parameters.Add(id);
                try
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
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
                SqlCommand command = new SqlCommand("TbIObjectTypeDelete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idObjectType", id);
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

        public List<ObjectTypeModel> GetAll()
        {
            List<ObjectTypeModel> objectTypes = new List<ObjectTypeModel>();
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblObjectTypeReadAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ObjectTypeModel objectType = new ObjectTypeModel();
                            objectType.Id = (int?)reader["idObjectType"];
                            objectType.Omschrijving = reader["omschrijving"].ToString();
                            objectTypes.Add(objectType);
                        }
                    }
                }
                catch { }
            }

            return objectTypes;
        }

        public ObjectTypeModel GetById(int id)
        {
            ObjectTypeModel objectTypes = new ObjectTypeModel();
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblObjectTypeReadOne", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idObjectType", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        objectTypes.Id = (int?)reader["idObjectType"];
                        objectTypes.Omschrijving = reader["omschrijving"].ToString();
                    }
                }
                catch { }
            }

            return objectTypes;
        }

        public bool Update(ObjectTypeModel t)
        {
            bool result = false;
            using (connection)
            {
                SqlCommand command = new SqlCommand("TblObjectTypeUpdate", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idObjectType", t.Id);
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