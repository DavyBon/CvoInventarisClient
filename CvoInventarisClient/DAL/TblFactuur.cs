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

        //private string GetConnectionString()
        //{
        //    return ConfigurationManager
        //        .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        //}

        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

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
                using (SqlCommand cmd = new SqlCommand("TblFactuurReadAll", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

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
                            leverancier.Afkorting = dr["afkorting"].ToString();
                            leverancier.Bic = dr["bic"].ToString();
                            leverancier.BtwNummer = dr["btwNummer"].ToString();
                            leverancier.BusNummer = dr["busNummer"].ToString();
                            leverancier.Email = dr["email"].ToString();
                            leverancier.Fax = dr["fax"].ToString();
                            leverancier.HuisNummer = dr["huisNummer"].ToString();
                            leverancier.Iban = dr["iban"].ToString();
                            leverancier.Naam = dr["naam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = dr["straat"].ToString();
                            leverancier.Telefoon = dr["telefoon"].ToString();
                            leverancier.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                            leverancier.Website = dr["website"].ToString();
                        }

                        factuur.Id = (int?)dr["idFactuur"];
                        factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                        factuur.Leverancier = leverancier;
                        factuur.Prijs = dr["prijs"].ToString();
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

        public FactuurModel GetById(int id)
        {
            FactuurModel factuur = new FactuurModel();
            LeverancierModel leverancier;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand cmd = new SqlCommand("TblFactuurReadOne", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idFactuur", id);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

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
                            leverancier.Afkorting = dr["afkorting"].ToString();
                            leverancier.Bic = dr["bic"].ToString();
                            leverancier.BtwNummer = dr["btwNummer"].ToString();
                            leverancier.BusNummer = dr["busNummer"].ToString();
                            leverancier.Email = dr["email"].ToString();
                            leverancier.Fax = dr["fax"].ToString();
                            leverancier.HuisNummer = dr["huisNummer"].ToString();
                            leverancier.Iban = dr["iban"].ToString();
                            leverancier.Naam = dr["naam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = dr["straat"].ToString();
                            leverancier.Telefoon = dr["telefoon"].ToString();
                            leverancier.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                            leverancier.Website = dr["website"].ToString();
                        }

                        factuur.Id = (int?)dr["idFactuur"];
                        factuur.ScholengroepNummer = dr["scholengroepNummer"].ToString();
                        factuur.Leverancier = leverancier;
                        factuur.Prijs = dr["prijs"].ToString();
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

        public int Create(FactuurModel factuur)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("TblFactuurInsert", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("scholengroepNummer", App_Code.DALutil.checkStringForDBNull(factuur.ScholengroepNummer));
                    cmd.Parameters.AddWithValue("idLeverancier", App_Code.DALutil.checkIntForDBNUll(factuur.Leverancier.Id));
                    cmd.Parameters.AddWithValue("prijs", App_Code.DALutil.checkStringForDBNull(factuur.Prijs));
                    cmd.Parameters.AddWithValue("garantie", App_Code.DALutil.checkIntForDBNUll(factuur.Garantie));
                    cmd.Parameters.AddWithValue("omschrijving", App_Code.DALutil.checkStringForDBNull(factuur.Omschrijving));
                    cmd.Parameters.AddWithValue("afschrijfperiode", App_Code.DALutil.checkIntForDBNUll(factuur.Afschrijfperiode));
                    cmd.Parameters.AddWithValue("verwerkingsDatum", App_Code.DALutil.checkStringForDBNull(factuur.VerwerkingsDatum));
                    cmd.Parameters.AddWithValue("cvoFactuurNummer", App_Code.DALutil.checkStringForDBNull(factuur.CvoFactuurNummer));
                    cmd.Parameters.AddWithValue("leverancierFactuurNummer", App_Code.DALutil.checkStringForDBNull(factuur.LeverancierFactuurNummer));
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

        public bool Update(FactuurModel factuur)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("TblFactuurUpdate", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idFactuur", factuur.Id);
                    cmd.Parameters.AddWithValue("scholengroepNummer", App_Code.DALutil.checkStringForDBNull(factuur.ScholengroepNummer));
                    cmd.Parameters.AddWithValue("idLeverancier", App_Code.DALutil.checkIntForDBNUll(factuur.Leverancier.Id));
                    cmd.Parameters.AddWithValue("prijs", App_Code.DALutil.checkStringForDBNull(factuur.Prijs));
                    cmd.Parameters.AddWithValue("garantie", App_Code.DALutil.checkIntForDBNUll(factuur.Garantie));
                    cmd.Parameters.AddWithValue("omschrijving", App_Code.DALutil.checkStringForDBNull(factuur.Omschrijving));
                    cmd.Parameters.AddWithValue("afschrijfperiode", App_Code.DALutil.checkIntForDBNUll(factuur.Afschrijfperiode));
                    cmd.Parameters.AddWithValue("verwerkingsDatum", App_Code.DALutil.checkStringForDBNull(factuur.VerwerkingsDatum));
                    cmd.Parameters.AddWithValue("cvoFactuurNummer", App_Code.DALutil.checkStringForDBNull(factuur.CvoFactuurNummer));
                    cmd.Parameters.AddWithValue("leverancierFactuurNummer", App_Code.DALutil.checkStringForDBNull(factuur.LeverancierFactuurNummer));
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
                using (SqlCommand cmd = new SqlCommand("TblFactuurDelete", connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idFactuur", id);
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

    }
}