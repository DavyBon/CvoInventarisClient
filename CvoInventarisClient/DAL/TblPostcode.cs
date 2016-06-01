using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CvoInventarisClient.DAL.Helpers;

namespace CvoInventarisClient.DAL
{
    public class TblPostcode : ICrudable<PostcodeModel>
    {
        SqlConnection connection = new SqlConnection(DatabaseConnection.GetConnectionString());

        public int Create(PostcodeModel t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<PostcodeModel> GetAll()
        {
            List<PostcodeModel> list = new List<PostcodeModel>();
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblPostcodeReadAll", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        postcode = new PostcodeModel();
                        postcode.Id = (int?)dr["idPostcode"];
                        postcode.Postcode = dr["postcode"].ToString();
                        postcode.Gemeente = dr["gemeente"].ToString();
                        list.Add(postcode);
                    }
                    return list;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public PostcodeModel GetById(int id)
        {
            PostcodeModel postcode = new PostcodeModel();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblPostcodeReadOne", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idPostcode", id);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        postcode.Id = (int?)dr["idPostcode"];
                        postcode.Postcode = dr["postcode"].ToString();
                        postcode.Gemeente = dr["gemeente"].ToString();
                    }
                    return postcode;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool Update(PostcodeModel t)
        {
            throw new NotImplementedException();
        }
    }
}