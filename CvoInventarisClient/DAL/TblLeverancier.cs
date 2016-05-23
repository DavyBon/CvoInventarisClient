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
                            l.Id = (int)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = (int?)dr["idPostcode"];
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
                            l.Id = (int)dr["idLeverancier"];
                            l.Naam = dr["naam"].ToString();
                            l.Afkorting = dr["afkorting"].ToString();
                            l.Straat = dr["straat"].ToString();
                            l.HuisNummer = dr["huisNummer"].ToString();
                            l.BusNummer = dr["busNummer"].ToString();
                            l.Postcode = (int?)dr["idPostcode"];
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
                        cmd.Parameters.AddWithValue("postcode", l.Postcode);
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

        public bool Update(LeverancierModel l)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblLeverancierUpdate", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idLeverancier", l.Id);
                        cmd.Parameters.AddWithValue("naam", l.Naam);
                        cmd.Parameters.AddWithValue("afkorting", l.Afkorting);
                        cmd.Parameters.AddWithValue("straat", l.Straat);
                        cmd.Parameters.AddWithValue("huisNummer", l.HuisNummer);
                        cmd.Parameters.AddWithValue("busNummer", l.BusNummer);
                        cmd.Parameters.AddWithValue("postcode", l.Postcode);
                        cmd.Parameters.AddWithValue("telefoon", l.Telefoon);
                        cmd.Parameters.AddWithValue("fax", l.Fax);
                        cmd.Parameters.AddWithValue("email", l.Email);
                        cmd.Parameters.AddWithValue("website", l.Website);
                        cmd.Parameters.AddWithValue("btwNummer", l.BtwNummer);
                        cmd.Parameters.AddWithValue("iban", l.Iban);
                        cmd.Parameters.AddWithValue("bic", l.Bic);
                        cmd.Parameters.AddWithValue("toegevoegdOp", l.ToegevoegdOp);
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
                            LeverancierModel l = new LeverancierModel();
                            if (keuzeKolommen.Contains("idLeverancier")) { l.Id = (int)dr["idLeverancier"]; }
                            if (keuzeKolommen.Contains("naam")) { l.Naam = dr["naam"].ToString(); }
                            if (keuzeKolommen.Contains("afkorting")) { l.Afkorting = dr["afkorting"].ToString(); }
                            if (keuzeKolommen.Contains("straat")) { l.Straat = dr["straat"].ToString(); }
                            if (keuzeKolommen.Contains("huisNummer")) { l.HuisNummer = dr["huisNummer"].ToString(); }
                            if (keuzeKolommen.Contains("busNummer")) { l.BusNummer = dr["busNummer"].ToString(); }
                            if (keuzeKolommen.Contains("postcode")) { l.Postcode = (int)dr["postcode"]; }
                            if (keuzeKolommen.Contains("telefoon")) { l.Telefoon = dr["telefoon"].ToString(); }
                            if (keuzeKolommen.Contains("fax")) { l.Fax = dr["fax"].ToString(); }
                            if (keuzeKolommen.Contains("email")) { l.Email = dr["email"].ToString(); }
                            if (keuzeKolommen.Contains("website")) { l.Website = dr["website"].ToString(); }
                            if (keuzeKolommen.Contains("btwNummer")) { l.BtwNummer = dr["btwNummer"].ToString(); }
                            if (keuzeKolommen.Contains("iban")) { l.Iban = dr["iban"].ToString(); }
                            if (keuzeKolommen.Contains("bic")) { l.Bic = dr["bic"].ToString(); }
                            if (keuzeKolommen.Contains("toegevoegdOp")) { l.ToegevoegdOp = dr["toegevoegdOp"].ToString(); }
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
    }
}