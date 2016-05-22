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
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        postcode.Id = (int)dr["idPostcode"];
                        postcode.Postcode = dr["postcode"].ToString();
                        postcode.Gemeente = dr["gemeente"].ToString();

                        campus.Id = (int)dr["idCampus"];
                        campus.Naam = dr["naam"].ToString();
                        campus.Postcode = postcode;
                        campus.Straat = dr["straat"].ToString();
                        campus.Nummer = dr["nummer"].ToString();
                        list.Add(campus);
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
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        postcode = new PostcodeModel();

                        postcode.Id = (int)dr["idPostcode"];
                        postcode.Postcode = dr["postcode"].ToString();
                        postcode.Gemeente = dr["gemeente"].ToString();

                        campus.Id = (int)dr["idCampus"];
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
                    cmd.Parameters.AddWithValue("idPostcode", campus.Postcode.Id);
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
                    cmd.Parameters.AddWithValue("idPostcode", campus.Postcode.Id);
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
        public List<CampusModel> Rapportering(string s, string[] keuzeKolommen)
        {
            List<CampusModel> list = new List<CampusModel>();
            CampusModel campus;

            try
            {
                using (SqlCommand cmd = new SqlCommand(s, connection))
                {
                    connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        campus = new CampusModel();
                        if (keuzeKolommen.Contains("idCampus")) { campus.Id = (int)dr["idCampus"]; }
                        if (keuzeKolommen.Contains("naam")) { campus.Naam = dr["naam"].ToString(); }
                        if (keuzeKolommen.Contains("postcode")) { campus.Postcode.Postcode = dr["postcode"].ToString(); }
                        if (keuzeKolommen.Contains("straat")) { campus.Straat = dr["straat"].ToString(); }
                        if (keuzeKolommen.Contains("nummer")) { campus.Nummer = dr["nummer"].ToString(); }
                        list.Add(campus);
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
    }
}