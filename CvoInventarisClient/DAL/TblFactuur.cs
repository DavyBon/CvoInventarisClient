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
    public class TblFactuur : ICrudable<FactuurModel>
    {

        #region Connectionstring

        private string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        }

        #endregion

        #region GetAll

        public List<FactuurModel> GetAll()
        {
            List<FactuurModel> list = new List<FactuurModel>();
            FactuurModel factuur;
            LeverancierModel leverancier;
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurReadAll", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            factuur = new FactuurModel();
                            leverancier = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            if (dr["idLeverancier"] != DBNull.Value)
                            {
                                leverancier.Id = (int?)dr["idLeverancier"];
                                leverancier.Naam = dr["naam"].ToString();
                                leverancier.Straat = dr["straat"].ToString();
                                leverancier.StraatNummer = dr["straatNummer"].ToString();
                                leverancier.BusNummer = dr["busNummer"].ToString();
                                leverancier.Postcode = postcode;
                                leverancier.Telefoon = dr["telefoon"].ToString();
                                leverancier.Fax = dr["fax"].ToString();
                                leverancier.Email = dr["email"].ToString();
                                leverancier.Website = dr["website"].ToString();
                                leverancier.BtwNummer = dr["btwNummer"].ToString();
                                leverancier.ActiefDatum = dr["actiefDatum"].ToString();
                            }

                            factuur.Id = (int?)dr["idFactuur"];
                            factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                            factuur.Leverancier = leverancier;
                            factuur.Prijs = dr["prijs"] as decimal?;
                            factuur.Garantie = (int)dr["garantie"];
                            factuur.Omschrijving = dr["omschrijving"].ToString();
                            factuur.Afschrijfperiode = (int)dr["afschrijfperiode"];
                            factuur.VerwerkingsDatum = dr["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = dr["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = dr["leverancierFactuurNummer"].ToString();

                            list.Add(factuur);
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

        public FactuurModel GetById(int id)
        {
            FactuurModel factuur = new FactuurModel();
            LeverancierModel leverancier;
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurReadOne", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idFactuur", id);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            factuur = new FactuurModel();
                            leverancier = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            if (dr["idLeverancier"] != DBNull.Value)
                            {
                                leverancier.Id = (int?)dr["idLeverancier"];
                                leverancier.Naam = dr["naam"].ToString();
                                leverancier.Straat = dr["straat"].ToString();
                                leverancier.StraatNummer = dr["straatNummer"].ToString();
                                leverancier.BusNummer = dr["busNummer"].ToString();
                                leverancier.Postcode = postcode;
                                leverancier.Telefoon = dr["telefoon"].ToString();
                                leverancier.Fax = dr["fax"].ToString();
                                leverancier.Email = dr["email"].ToString();
                                leverancier.Website = dr["website"].ToString();
                                leverancier.BtwNummer = dr["btwNummer"].ToString();
                                leverancier.ActiefDatum = dr["actiefDatum"].ToString();
                            }

                            factuur.Id = (int?)dr["idFactuur"];
                            factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                            factuur.Leverancier = leverancier;
                            factuur.Prijs = dr["prijs"] as decimal?;
                            factuur.Garantie = (int)dr["garantie"];
                            factuur.Omschrijving = dr["omschrijving"].ToString();
                            factuur.Afschrijfperiode = (int)dr["afschrijfperiode"];
                            factuur.VerwerkingsDatum = dr["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = dr["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = dr["leverancierFactuurNummer"].ToString();
                        }
                        return factuur;
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

        public int Create(FactuurModel factuur)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurInsert", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("scholengroepNummer", factuur.ScholengroepNummer);
                        cmd.Parameters.AddWithValue("idLeverancier", App_Code.DALutil.checkIntForDBNUll(factuur.Leverancier.Id));
                        cmd.Parameters.AddWithValue("prijs", App_Code.DALutil.checkDecimalForDBNUll(factuur.Prijs));
                        cmd.Parameters.AddWithValue("garantie", App_Code.DALutil.checkIntForDBNUll(factuur.Garantie));
                        cmd.Parameters.AddWithValue("omschrijving", factuur.Omschrijving);
                        cmd.Parameters.AddWithValue("afschrijfperiode", App_Code.DALutil.checkIntForDBNUll(factuur.Afschrijfperiode));
                        cmd.Parameters.AddWithValue("verwerkingsDatum", factuur.VerwerkingsDatum);
                        cmd.Parameters.AddWithValue("cvoFactuurNummer", factuur.CvoFactuurNummer);
                        cmd.Parameters.AddWithValue("leverancierFactuurNummer", factuur.LeverancierFactuurNummer);
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

        public bool Update(FactuurModel factuur)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurUpdate", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idFactuur", factuur.Id);
                        cmd.Parameters.AddWithValue("scholengroepNummer", factuur.ScholengroepNummer);
                        cmd.Parameters.AddWithValue("idLeverancier", App_Code.DALutil.checkIntForDBNUll(factuur.Leverancier.Id));
                        cmd.Parameters.AddWithValue("prijs", App_Code.DALutil.checkDecimalForDBNUll(factuur.Prijs));
                        cmd.Parameters.AddWithValue("garantie", App_Code.DALutil.checkIntForDBNUll(factuur.Garantie));
                        cmd.Parameters.AddWithValue("omschrijving", factuur.Omschrijving);
                        cmd.Parameters.AddWithValue("afschrijfperiode", App_Code.DALutil.checkIntForDBNUll(factuur.Afschrijfperiode));
                        cmd.Parameters.AddWithValue("verwerkingsDatum", factuur.VerwerkingsDatum);
                        cmd.Parameters.AddWithValue("cvoFactuurNummer", factuur.CvoFactuurNummer);
                        cmd.Parameters.AddWithValue("leverancierFactuurNummer", factuur.LeverancierFactuurNummer);
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
                    using (SqlCommand cmd = new SqlCommand("TblFactuurDelete", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idFactuur", id);
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

        #region Get Top

        public List<FactuurModel> GetTop()
        {
            List<FactuurModel> list = new List<FactuurModel>();
            FactuurModel factuur;
            LeverancierModel leverancier;
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurReadTop", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@amount", 100);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            factuur = new FactuurModel();
                            leverancier = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            if (dr["idLeverancier"] != DBNull.Value)
                            {
                                leverancier.Id = (int?)dr["idLeverancier"];
                                leverancier.Naam = dr["naam"].ToString();
                                leverancier.Straat = dr["straat"].ToString();
                                leverancier.StraatNummer = dr["straatNummer"].ToString();
                                leverancier.BusNummer = dr["busNummer"].ToString();
                                leverancier.Postcode = postcode;
                                leverancier.Telefoon = dr["telefoon"].ToString();
                                leverancier.Fax = dr["fax"].ToString();
                                leverancier.Email = dr["email"].ToString();
                                leverancier.Website = dr["website"].ToString();
                                leverancier.BtwNummer = dr["btwNummer"].ToString();
                                leverancier.ActiefDatum = dr["actiefDatum"].ToString();
                            }

                            factuur.Id = (int?)dr["idFactuur"];
                            factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                            factuur.Leverancier = leverancier;
                            factuur.Prijs = dr["prijs"] as decimal?;
                            factuur.Garantie = (int)dr["garantie"];
                            factuur.Omschrijving = dr["omschrijving"].ToString();
                            factuur.Afschrijfperiode = (int)dr["afschrijfperiode"];
                            factuur.VerwerkingsDatum = dr["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = dr["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = dr["leverancierFactuurNummer"].ToString();
                            list.Add(factuur);
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

        #region Get Top Amount

        public List<FactuurModel> GetTop(int amount)
        {
            List<FactuurModel> list = new List<FactuurModel>();
            FactuurModel factuur;
            LeverancierModel leverancier;
            PostcodeModel postcode;

            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("TblFactuurReadTop", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@amount", amount);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            factuur = new FactuurModel();
                            leverancier = new LeverancierModel();
                            postcode = new PostcodeModel();

                            if (dr["idPostcode"] != DBNull.Value)
                            {
                                postcode.Id = (int?)dr["idPostcode"];
                                postcode.Gemeente = dr["gemeente"].ToString();
                                postcode.Postcode = dr["postcode"].ToString();
                            }

                            if (dr["idLeverancier"] != DBNull.Value)
                            {
                                leverancier.Id = (int?)dr["idLeverancier"];
                                leverancier.Naam = dr["naam"].ToString();
                                leverancier.Straat = dr["straat"].ToString();
                                leverancier.StraatNummer = dr["straatNummer"].ToString();
                                leverancier.BusNummer = dr["busNummer"].ToString();
                                leverancier.Postcode = postcode;
                                leverancier.Telefoon = dr["telefoon"].ToString();
                                leverancier.Fax = dr["fax"].ToString();
                                leverancier.Email = dr["email"].ToString();
                                leverancier.Website = dr["website"].ToString();
                                leverancier.BtwNummer = dr["btwNummer"].ToString();
                                leverancier.ActiefDatum = dr["actiefDatum"].ToString();
                            }

                            factuur.Id = (int?)dr["idFactuur"];
                            factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                            factuur.Leverancier = leverancier;
                            factuur.Prijs = dr["prijs"] as decimal?;
                            factuur.Garantie = (int)dr["garantie"];
                            factuur.Omschrijving = dr["omschrijving"].ToString();
                            factuur.Afschrijfperiode = (int)dr["afschrijfperiode"];
                            factuur.VerwerkingsDatum = dr["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = dr["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = dr["leverancierFactuurNummer"].ToString();
                            list.Add(factuur);
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