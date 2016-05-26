using System;
using System.Collections.Generic;
using System.Linq;
using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace CvoInventarisClient.DAL
{
    public class TblLeverancier : ICrudable<LeverancierModel>
    {
        #region Fields

        private string message;

        #endregion

        #region Connectionstring

        private string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        }

        #endregion

        #region Message

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        #endregion

        #region GetAll

        public List<LeverancierModel> GetAll()
        {
            List<LeverancierModel> list = new List<LeverancierModel>();
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierReadAll", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr.Read())
                        {
                            LeverancierModel l = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            l.Id = (int?)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = postcode;
                            l.Telefoon = dr["telefoon"].ToString();
                            l.Fax = dr["fax"].ToString();
                            l.Email = dr["email"].ToString();
                            l.Website = dr["website"].ToString();
                            l.BtwNummer = dr["btwNummer"].ToString();
                            l.Iban = dr["iban"].ToString();
                            l.Bic = dr["bic"].ToString();
                            l.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                            list.Add(l);
                        }
                        return list;
                    }
                }
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region GetTop

        public List<LeverancierModel> GetTop()
        {
            List<LeverancierModel> list = new List<LeverancierModel>();
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierReadAll", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@amount", 100);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr.Read())
                        {
                            LeverancierModel l = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            l.Id = (int?)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = postcode;
                            l.Telefoon = dr["telefoon"].ToString();
                            l.Fax = dr["fax"].ToString();
                            l.Email = dr["email"].ToString();
                            l.Website = dr["website"].ToString();
                            l.BtwNummer = dr["btwNummer"].ToString();
                            l.Iban = dr["iban"].ToString();
                            l.Bic = dr["bic"].ToString();
                            l.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                            list.Add(l);
                        }
                        return list;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public List<LeverancierModel> GetTop(int amount)
        {
            List<LeverancierModel> list = new List<LeverancierModel>();
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierReadAll", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@amount", amount);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr.Read())
                        {
                            LeverancierModel l = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            l.Id = (int?)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = postcode;
                            l.Telefoon = dr["telefoon"].ToString();
                            l.Fax = dr["fax"].ToString();
                            l.Email = dr["email"].ToString();
                            l.Website = dr["website"].ToString();
                            l.BtwNummer = dr["btwNummer"].ToString();
                            l.Iban = dr["iban"].ToString();
                            l.Bic = dr["bic"].ToString();
                            l.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                            list.Add(l);
                        }
                        return list;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion



        #region GetById

        public LeverancierModel GetById(int id)
        {
            LeverancierModel l = null;
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierReadOne", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idLeverancier", id);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr.Read())
                        {
                            l = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            l.Id = (int?)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = postcode;
                            l.Telefoon = dr["telefoon"].ToString();
                            l.Fax = dr["fax"].ToString();
                            l.Email = dr["email"].ToString();
                            l.Website = dr["website"].ToString();
                            l.BtwNummer = dr["btwNummer"].ToString();
                            l.Iban = dr["iban"].ToString();
                            l.Bic = dr["bic"].ToString();
                            l.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                        }
                        return l;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Create

        public int Create(LeverancierModel l)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierInsert", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("naam", l.Naam);
                        cmd.Parameters.AddWithValue("afkorting", l.Afkorting);
                        cmd.Parameters.AddWithValue("straat", l.Straat);
                        cmd.Parameters.AddWithValue("huisNummer", l.HuisNummer);
                        cmd.Parameters.AddWithValue("busNummer", l.BusNummer);
                        cmd.Parameters.AddWithValue("postcode", App_Code.DALutil.checkIntForDBNUll(l.Postcode.Id));
                        cmd.Parameters.AddWithValue("telefoon", l.Telefoon);
                        cmd.Parameters.AddWithValue("fax", l.Fax);
                        cmd.Parameters.AddWithValue("email", l.Email);
                        cmd.Parameters.AddWithValue("website", l.Website);
                        cmd.Parameters.AddWithValue("btwNummer", l.BtwNummer);
                        cmd.Parameters.AddWithValue("iban", l.Iban);
                        cmd.Parameters.AddWithValue("bic", l.Bic);
                        cmd.Parameters.AddWithValue("toegevoegdOp", l.ToegevoegdOp);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }


        #endregion

        #region Update

        public bool Update(LeverancierModel lm)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierUpdate", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idLeverancier", lm.Id);
                        cmd.Parameters.AddWithValue("naam", lm.Naam);
                        cmd.Parameters.AddWithValue("afkorting", lm.Afkorting);
                        cmd.Parameters.AddWithValue("straat", lm.Straat);
                        cmd.Parameters.AddWithValue("huisNummer", lm.HuisNummer);
                        cmd.Parameters.AddWithValue("busNummer", lm.BusNummer);
                        cmd.Parameters.AddWithValue("postcode", lm.Postcode);
                        cmd.Parameters.AddWithValue("telefoon", lm.Telefoon);
                        cmd.Parameters.AddWithValue("fax", lm.Fax);
                        cmd.Parameters.AddWithValue("email", lm.Email);
                        cmd.Parameters.AddWithValue("website", lm.Website);
                        cmd.Parameters.AddWithValue("btwNummer", lm.BtwNummer);
                        cmd.Parameters.AddWithValue("iban", lm.Iban);
                        cmd.Parameters.AddWithValue("bic", lm.Bic);
                        cmd.Parameters.AddWithValue("toegevoegdOp", lm.ToegevoegdOp);
                        cmd.ExecuteReader();
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region Delete

        public bool Delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierDelete", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idLeverancier", id);
                        cmd.ExecuteReader();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        #endregion

        #region Rapportering

        public List<LeverancierModel> Rapportering(string s, string[] keuzeKolommen)
        {
            List<LeverancierModel> list = new List<LeverancierModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(s, con))
                    {
                        con.Open();
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr.Read())
                        {
                            LeverancierModel lm = new LeverancierModel();
                            if (keuzeKolommen.Contains("naam")) { lm.Naam = dr["naam"].ToString(); }
                            if (keuzeKolommen.Contains("afkorting")) { lm.Afkorting = dr["afkorting"].ToString(); }
                            if (keuzeKolommen.Contains("straat")) { lm.Straat = dr["straat"].ToString(); }
                            if (keuzeKolommen.Contains("huisNummer")) { lm.HuisNummer = dr["huisNummer"].ToString(); }
                            if (keuzeKolommen.Contains("busNummer")) { lm.BusNummer = dr["busNummer"].ToString(); }
                            if (keuzeKolommen.Contains("postcode")) { lm.Postcode = (PostcodeModel)dr["postcode"]; }
                            if (keuzeKolommen.Contains("telefoon")) { lm.Telefoon = dr["telefoon"].ToString(); }
                            if (keuzeKolommen.Contains("fax")) { lm.Fax = dr["fax"].ToString(); }
                            if (keuzeKolommen.Contains("email")) { lm.Email = dr["email"].ToString(); }
                            if (keuzeKolommen.Contains("website")) { lm.Website = dr["website"].ToString(); }
                            if (keuzeKolommen.Contains("btwNummer")) { lm.BtwNummer = dr["btwNummer"].ToString(); }
                            if (keuzeKolommen.Contains("iban")) { lm.Iban = dr["iban"].ToString(); }
                            if (keuzeKolommen.Contains("bic")) { lm.Bic = dr["bic"].ToString(); }
                            if (keuzeKolommen.Contains("toegevoegdOp")) { lm.ToegevoegdOp = dr["toegevoegdOp"].ToString(); }
                            list.Add(lm);
                        }
                        return list;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}