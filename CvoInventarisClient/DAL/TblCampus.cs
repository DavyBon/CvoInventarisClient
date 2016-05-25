using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.DAL
{
    public class TblCampus : ICrudable<CampusModel>
    {

        #region Connectionstring

        //private string GetConnectionString()
        //{
        //    return ConfigurationManager
        //        .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        //}

        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

        #endregion

        #region GetAll

        public List<CampusModel> GetAll()
        {
            List<CampusModel> list = new List<CampusModel>();
            CampusModel campus;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusReadAll", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        if(dr["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)dr["idPostcode"];
                            postcode.Gemeente = dr["gemeente"].ToString();
                            postcode.Postcode = dr["postcode"].ToString();
                        }

                        campus.Id = (int?)dr["idCampus"];
                        campus.Naam = dr["naam"].ToString();
                        campus.Postcode = postcode;
                        campus.Straat = dr["straat"].ToString();
                        campus.Nummer = dr["nummer"].ToString();
                        list.Add(campus);
                    }
                    return list;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region GetById

        public CampusModel GetById(int id)
        {
            CampusModel campus = new CampusModel();
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusReadOne", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idCampus", id);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        if (dr["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)dr["idPostcode"];
                            postcode.Gemeente = dr["gemeente"].ToString();
                            postcode.Postcode = dr["postcode"].ToString();
                        }

                        campus.Id = (int?)dr["idCampus"];
                        campus.Naam = dr["naam"].ToString();
                        campus.Postcode = postcode;
                        campus.Straat = dr["straat"].ToString();
                        campus.Nummer = dr["nummer"].ToString();
                    }
                    return campus;
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

        #endregion

        #region Create

        public int Create(CampusModel campus)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusInsert", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("naam", campus.Naam);
                    cmd.Parameters.AddWithValue("idPostcode", App_Code.DALutil.checkIntForDBNUll(campus.Postcode.Id));
                    cmd.Parameters.AddWithValue("straat", campus.Straat);
                    cmd.Parameters.AddWithValue("nummer", campus.Nummer);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region Update

        public bool Update(CampusModel campus)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusUpdate", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idCampus", campus.Id);
                    cmd.Parameters.AddWithValue("naam", campus.Naam);
                    cmd.Parameters.AddWithValue("idPostcode", App_Code.DALutil.checkIntForDBNUll(campus.Postcode.Id));
                    cmd.Parameters.AddWithValue("straat", campus.Straat);
                    cmd.Parameters.AddWithValue("nummer", campus.Nummer);
                    cmd.ExecuteReader();
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

        #endregion

        #region Delete

        public bool Delete(int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusDelete", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idCampus", id);
                    cmd.ExecuteReader();
                }
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region Get Top

        public List<CampusModel> GetTop()
        {
            List<CampusModel> list = new List<CampusModel>();
            CampusModel campus;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusReadTop", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@amount", 100);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        if (dr["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)dr["idPostcode"];
                            postcode.Gemeente = dr["gemeente"].ToString();
                            postcode.Postcode = dr["postcode"].ToString();
                        }

                        campus.Id = (int?)dr["idCampus"];
                        campus.Naam = dr["naam"].ToString();
                        campus.Postcode = postcode;
                        campus.Straat = dr["straat"].ToString();
                        campus.Nummer = dr["nummer"].ToString();
                        list.Add(campus);
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

        #endregion

        #region Get Top Amount

        public List<CampusModel> GetTop(int amount)
        {
            List<CampusModel> list = new List<CampusModel>();
            CampusModel campus;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblCampusReadTop", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@amount", amount);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        if (dr["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)dr["idPostcode"];
                            postcode.Gemeente = dr["gemeente"].ToString();
                            postcode.Postcode = dr["postcode"].ToString();
                        }

                        campus.Id = (int?)dr["idCampus"];
                        campus.Naam = dr["naam"].ToString();
                        campus.Postcode = postcode;
                        campus.Straat = dr["straat"].ToString();
                        campus.Nummer = dr["nummer"].ToString();
                        list.Add(campus);
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

        #endregion
    }
}