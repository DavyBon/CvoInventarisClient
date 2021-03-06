﻿using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CvoInventarisClient.DAL.Helpers;

namespace CvoInventarisClient.DAL
{
    public class TblObject : ICrudable<ObjectModel>
    {
        SqlConnection connection = new SqlConnection(DatabaseConnection.GetConnectionString());

        public int Create(ObjectModel obj)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectInsert", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@idObjectType", App_Code.DALutil.checkIntForDBNUll(obj.ObjectType.Id)));
                    command.Parameters.Add(new SqlParameter("@kenmerken", obj.Kenmerken));
                    command.Parameters.Add(new SqlParameter("@afmetingen", obj.afmetingen));
                    command.Parameters.Add(new SqlParameter("@omschrijving", obj.Omschrijving));
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectDelete", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.ExecuteReader();
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<ObjectModel> GetAll()
        {
            List<ObjectModel> list = new List<ObjectModel>();
            ObjectModel obj;
            ObjectTypeModel objType;

            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectReadAll", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader mySqlDataReader = command.ExecuteReader();

                    while (mySqlDataReader.Read())
                    {
                        obj = new ObjectModel();
                        objType = new ObjectTypeModel();

                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["objectTypeOmschrijving"].ToString();
                        }

                        obj.Id = (int?)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.afmetingen = mySqlDataReader["afmetingen"].ToString();
                        obj.Omschrijving = mySqlDataReader["objectOmschrijving"].ToString();
                        obj.ObjectType = objType;
                        list.Add(obj);
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public ObjectModel GetById(int id)
        {
            ObjectModel obj = new ObjectModel();
            ObjectTypeModel objType;

            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectReadOne", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader mySqlDataReader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);

                    while (mySqlDataReader.Read())
                    {
                        objType = new ObjectTypeModel();

                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();
                        }

                        obj.Id = (int?)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.afmetingen = mySqlDataReader["afmetingen"].ToString();
                        obj.Omschrijving = mySqlDataReader["objectOmschrijving"].ToString();
                        obj.ObjectType = objType;

                    }
                    return obj;
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

        public bool Update(ObjectModel obj)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectUpdate", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", obj.Id));
                    command.Parameters.Add(new SqlParameter("@idObjectType", App_Code.DALutil.checkIntForDBNUll(obj.ObjectType.Id)));
                    command.Parameters.Add(new SqlParameter("@kenmerken", obj.Kenmerken));
                    command.Parameters.Add(new SqlParameter("@afmetingen", obj.afmetingen));
                    command.Parameters.Add(new SqlParameter("@omschrijving", obj.Omschrijving));
                    command.ExecuteReader();
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }


    }
}