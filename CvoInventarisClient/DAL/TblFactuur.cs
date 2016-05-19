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

                        leverancier.IdLeverancier = (int)dr["idLeverancier"];
                        leverancier.Afkorting = dr["afkorting"].ToString();
                        leverancier.Bic = dr["bic"].ToString();
                        leverancier.BtwNummer = dr["btwNummer"].ToString();
                        leverancier.BusNummer = dr["busNummer"].ToString();
                        leverancier.Email = dr["email"].ToString();
                        leverancier.Fax = dr["fax"].ToString();
                        leverancier.HuisNummer = dr["huisNummer"].ToString();
                        leverancier.Iban = dr["iban"].ToString();
                        leverancier.Naam = dr["naam"].ToString();
                        leverancier.Postcode = (int)dr["postcode"];
                        leverancier.Straat = dr["straat"].ToString();
                        leverancier.Telefoon = dr["telefoon"].ToString();
                        leverancier.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                        leverancier.Website = dr["website"].ToString();

                        factuur.IdFactuur = (int)dr["idFactuur"];
                        factuur.Boekjaar = dr["Boekjaar"].ToString();
                        factuur.CvoVolgNummer = dr["CvoVolgNummer"].ToString();
                        factuur.FactuurNummer = dr["FactuurNummer"].ToString();
                        factuur.ScholengroepNummer = dr["ScholengroepNummer"].ToString();
                        factuur.FactuurDatum = dr["FactuurDatum"].ToString();
                        factuur.Leverancier = leverancier;
                        factuur.Prijs = dr["Prijs"].ToString();
                        factuur.Garantie = (int)dr["Garantie"];
                        factuur.Omschrijving = dr["Omschrijving"].ToString();
                        factuur.Opmerking = dr["Opmerking"].ToString();
                        factuur.Afschrijfperiode = (int)dr["Afschrijfperiode"];
                        factuur.DatumInsert = dr["DatumInsert"].ToString();
                        factuur.UserInsert = dr["UserInsert"].ToString();
                        factuur.DatumModified = dr["DatumModified"].ToString();
                        factuur.UserModified = dr["UserModified"].ToString();
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

                        leverancier.IdLeverancier = (int)dr["idLeverancier"];
                        leverancier.Afkorting = dr["afkorting"].ToString();
                        leverancier.Bic = dr["bic"].ToString();
                        leverancier.BtwNummer = dr["btwNummer"].ToString();
                        leverancier.BusNummer = dr["busNummer"].ToString();
                        leverancier.Email = dr["email"].ToString();
                        leverancier.Fax = dr["fax"].ToString();
                        leverancier.HuisNummer = dr["huisNummer"].ToString();
                        leverancier.Iban = dr["iban"].ToString();
                        leverancier.Naam = dr["naam"].ToString();
                        leverancier.Postcode = (int)dr["postcode"];
                        leverancier.Straat = dr["straat"].ToString();
                        leverancier.Telefoon = dr["telefoon"].ToString();
                        leverancier.ToegevoegdOp = dr["toegevoegdOp"].ToString();
                        leverancier.Website = dr["website"].ToString();

                        factuur.IdFactuur = (int)dr["idFactuur"];
                        factuur.Boekjaar = dr["Boekjaar"].ToString();
                        factuur.CvoVolgNummer = dr["CvoVolgNummer"].ToString();
                        factuur.FactuurNummer = dr["FactuurNummer"].ToString();
                        factuur.ScholengroepNummer = dr["ScholengroepNummer"].ToString();
                        factuur.FactuurDatum = dr["FactuurDatum"].ToString();
                        factuur.Leverancier = leverancier;
                        factuur.Prijs = dr["Prijs"].ToString();
                        factuur.Garantie = (int)dr["Garantie"];
                        factuur.Omschrijving = dr["Omschrijving"].ToString();
                        factuur.Opmerking = dr["Opmerking"].ToString();
                        factuur.Afschrijfperiode = (int)dr["Afschrijfperiode"];
                        factuur.DatumInsert = dr["DatumInsert"].ToString();
                        factuur.UserInsert = dr["UserInsert"].ToString();
                        factuur.DatumModified = dr["DatumModified"].ToString();
                        factuur.UserModified = dr["UserModified"].ToString();
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
                    cmd.Parameters.AddWithValue("Boekjaar", factuur.Boekjaar);
                    cmd.Parameters.AddWithValue("CvoVolgNummer", factuur.CvoVolgNummer);
                    cmd.Parameters.AddWithValue("FactuurNummer", factuur.FactuurNummer);
                    cmd.Parameters.AddWithValue("ScholengroepNummer", factuur.ScholengroepNummer);
                    cmd.Parameters.AddWithValue("FactuurDatum", factuur.FactuurDatum);
                    cmd.Parameters.AddWithValue("idLeverancier", factuur.Leverancier.IdLeverancier);
                    cmd.Parameters.AddWithValue("Prijs", factuur.Prijs);
                    cmd.Parameters.AddWithValue("Garantie", factuur.Garantie);
                    cmd.Parameters.AddWithValue("Omschrijving", factuur.Omschrijving);
                    cmd.Parameters.AddWithValue("Opmerking", factuur.Opmerking);
                    cmd.Parameters.AddWithValue("Afschrijfperiode", factuur.Afschrijfperiode);
                    cmd.Parameters.AddWithValue("DatumInsert", factuur.DatumInsert);
                    cmd.Parameters.AddWithValue("UserInsert", factuur.UserInsert);
                    cmd.Parameters.AddWithValue("DatumModified", factuur.DatumModified);
                    cmd.Parameters.AddWithValue("UserModified", factuur.UserModified);
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
                    cmd.Parameters.AddWithValue("idFactuur", factuur.IdFactuur);
                    cmd.Parameters.AddWithValue("Boekjaar", factuur.Boekjaar);
                    cmd.Parameters.AddWithValue("CvoVolgNummer", factuur.CvoVolgNummer);
                    cmd.Parameters.AddWithValue("FactuurNummer", factuur.FactuurNummer);
                    cmd.Parameters.AddWithValue("ScholengroepNummer", factuur.ScholengroepNummer);
                    cmd.Parameters.AddWithValue("FactuurDatum", factuur.FactuurDatum);
                    cmd.Parameters.AddWithValue("idLeverancier", factuur.Leverancier.IdLeverancier);
                    cmd.Parameters.AddWithValue("Prijs", factuur.Prijs);
                    cmd.Parameters.AddWithValue("Garantie", factuur.Garantie);
                    cmd.Parameters.AddWithValue("Omschrijving", factuur.Omschrijving);
                    cmd.Parameters.AddWithValue("Opmerking", factuur.Opmerking);
                    cmd.Parameters.AddWithValue("Afschrijfperiode", factuur.Afschrijfperiode);
                    cmd.Parameters.AddWithValue("DatumInsert", factuur.DatumInsert);
                    cmd.Parameters.AddWithValue("UserInsert", factuur.UserInsert);
                    cmd.Parameters.AddWithValue("DatumModified", factuur.DatumModified);
                    cmd.Parameters.AddWithValue("UserModified", factuur.UserModified);
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
        public List<FactuurModel> Rapportering(string s, string[] keuzeKolommen)
        {
            List<FactuurModel> list = new List<FactuurModel>();
            FactuurModel factuur = new FactuurModel();
            LeverancierModel leverancier;

            try
            {
                using (SqlCommand cmd = new SqlCommand(s, connection))
                {
                    connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            factuur = new FactuurModel();
                            leverancier = new LeverancierModel();

                            if (keuzeKolommen.Contains("TblFactuur.idFactuur")) { factuur.IdFactuur = (int)dr["idFactuur"]; }
                            if (keuzeKolommen.Contains("TblFactuur.Boekjaar")) { factuur.Boekjaar = dr["Boekjaar"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.CvoVolgNummer")) { factuur.CvoVolgNummer = dr["CvoVolgNummer"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.FactuurNummer")) { factuur.FactuurNummer = dr["FactuurNummer"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.ScholengroepNummer")) { factuur.ScholengroepNummer = dr["ScholengroepNummer"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.FactuurDatum")) { factuur.FactuurDatum = dr["FactuurDatum"].ToString(); }
                            if (keuzeKolommen.Contains("TblLeverancier.naam")) { leverancier.Naam = dr["Naam"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.Prijs")) { factuur.Prijs = dr["Prijs"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.Garantie")) { factuur.Garantie = (int)dr["Garantie"]; }
                            if (keuzeKolommen.Contains("TblFactuur.Omschrijving")) { factuur.Omschrijving = dr["Omschrijving"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.Opmerking")) { factuur.Opmerking = dr["Opmerking"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.Afschrijfperiode")) { factuur.Afschrijfperiode = (int)dr["Afschrijfperiode"]; }
                            if (keuzeKolommen.Contains("TblFactuur.DatumInsert")) { factuur.DatumInsert = dr["DatumInsert"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.UserInsert")) { factuur.UserInsert = dr["UserInsert"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.DatumModified")) { factuur.DatumModified = dr["DatumModified"].ToString(); }
                            if (keuzeKolommen.Contains("TblFactuur.UserModified")) { factuur.UserModified = dr["UserModified"].ToString(); }
                            factuur.Leverancier = leverancier;
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
            finally
            {
                connection.Close();
            }
            return list;
        }
    }
}