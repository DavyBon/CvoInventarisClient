using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System.Configuration;
using System.Data;


namespace CvoInventarisClient.DAL
{
    public class TblInventaris : ICrudable<InventarisModel>
    {
        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

        public int Create(InventarisModel inventaris)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblInventarisInsert", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@label", inventaris.Label));
                    command.Parameters.Add(new SqlParameter("@idLokaal", App_Code.DALutil.checkIntForDBNUll(inventaris.Lokaal.Id)));
                    command.Parameters.Add(new SqlParameter("@idObject", App_Code.DALutil.checkIntForDBNUll(inventaris.Object.Id)));
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", inventaris.Aankoopjaar));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", inventaris.Afschrijvingsperiode));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", App_Code.DALutil.checkIntForDBNUll(inventaris.Verzekering.Id)));

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
                using (SqlCommand command = new SqlCommand("TblInventarisDelete", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.ExecuteReader();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<InventarisModel> GetAll()
        {
            List<InventarisModel> list = new List<InventarisModel>();
            InventarisModel inventaris;
            LokaalModel lokaal;
            ObjectModel obj;
            LeverancierModel leverancier;
            FactuurModel factuur;
            ObjectTypeModel objType;
            VerzekeringModel verzekering;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand command = new SqlCommand("TblInventarisReadAll", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader mySqlDataReader = command.ExecuteReader();

                    while (mySqlDataReader.Read())
                    {
                        inventaris = new InventarisModel();
                        lokaal = new LokaalModel();
                        obj = new ObjectModel();
                        leverancier = new LeverancierModel();
                        factuur = new FactuurModel();
                        objType = new ObjectTypeModel();
                        verzekering = new VerzekeringModel();
                        postcode = new PostcodeModel();


                        if (mySqlDataReader["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int)mySqlDataReader["idPostcode"];
                            postcode.Gemeente = mySqlDataReader["gemeente"].ToString();
                            postcode.Postcode = mySqlDataReader["postcode"].ToString();
                        }

                        if (mySqlDataReader["idLokaal"] != DBNull.Value)
                        {
                            lokaal.Id = (int?)mySqlDataReader["idLokaal"];
                            lokaal.AantalPlaatsen = (int)mySqlDataReader["aantalPlaatsen"];
                            lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                            lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();
                        }

                        if (mySqlDataReader["idLeverancier"] != DBNull.Value)
                        {
                            leverancier.Id = (int?)mySqlDataReader["idLeverancier"];
                            leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                            leverancier.Bic = mySqlDataReader["bic"].ToString();
                            leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                            leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                            leverancier.Email = mySqlDataReader["email"].ToString();
                            leverancier.Fax = mySqlDataReader["fax"].ToString();
                            leverancier.HuisNummer = mySqlDataReader["huisNummer"].ToString();
                            leverancier.Iban = mySqlDataReader["iban"].ToString();
                            leverancier.Naam = mySqlDataReader["naam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = mySqlDataReader["straat"].ToString();
                            leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                            leverancier.ToegevoegdOp = mySqlDataReader["toegevoegdOp"].ToString();
                            leverancier.Website = mySqlDataReader["website"].ToString();
                        }




                        if (mySqlDataReader["idFactuur"] != DBNull.Value)
                        {
                            factuur.Id = (int?)mySqlDataReader["idFactuur"];
                            factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijvingsperiode"];
                            factuur.Boekjaar = mySqlDataReader["Boekjaar"].ToString();
                            factuur.CvoVolgNummer = mySqlDataReader["CvoVolgNummer"].ToString();
                            factuur.DatumInsert = mySqlDataReader["DatumInsert"].ToString();
                            factuur.DatumModified = mySqlDataReader["DatumModified"].ToString();
                            factuur.FactuurDatum = mySqlDataReader["FactuurDatum"].ToString();
                            factuur.FactuurNummer = mySqlDataReader["FactuurNummer"].ToString();
                            factuur.Garantie = (int)mySqlDataReader["Garantie"];
                            factuur.Leverancier = leverancier;
                            factuur.Omschrijving = mySqlDataReader["Omschrijving"].ToString();
                            factuur.Opmerking = mySqlDataReader["Opmerking"].ToString();
                            factuur.Prijs = mySqlDataReader["Prijs"].ToString();
                            factuur.UserInsert = mySqlDataReader["UserInsert"].ToString();
                            factuur.UserModified = mySqlDataReader["UserModified"].ToString();
                        }


                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();
                        }

                        if (mySqlDataReader["idObject"] != DBNull.Value)
                        {
                            obj.Id = (int?)mySqlDataReader["idObject"];
                            obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                            obj.Factuur = factuur;
                            obj.ObjectType = objType;
                        }

                        if (mySqlDataReader["idVerzekering"] != DBNull.Value)
                        {
                            verzekering.Id = (int?)mySqlDataReader["idVerzekering"];
                            verzekering.Omschrijving = mySqlDataReader["omschrijving"].ToString();
                        }

                        inventaris.Id = (int?)mySqlDataReader["idInventaris"];
                        inventaris.Label = mySqlDataReader["label"].ToString();
                        inventaris.Lokaal = lokaal;
                        inventaris.Object = obj;
                        inventaris.Aankoopjaar = (int)mySqlDataReader["aankoopjaar"];
                        inventaris.Afschrijvingsperiode = (int)mySqlDataReader["afschrijvingsperiode"];
                        inventaris.Historiek = mySqlDataReader["historiek"].ToString();
                        inventaris.IsActief = Convert.ToBoolean(mySqlDataReader["isActief"]);
                        inventaris.IsAanwezig = Convert.ToBoolean(mySqlDataReader["isAanwezig"]);
                        inventaris.Verzekering = verzekering;
                        list.Add(inventaris);
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

        public InventarisModel GetById(int id)
        {
            InventarisModel inventaris = new InventarisModel();
            LokaalModel lokaal;
            ObjectModel obj;
            LeverancierModel leverancier;
            FactuurModel factuur;
            ObjectTypeModel objType;
            VerzekeringModel verzekering;
            PostcodeModel postcode;

            try
            {
                using (SqlCommand command = new SqlCommand("TblInventarisReadOne", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader mySqlDataReader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);

                    while (mySqlDataReader.Read())
                    {
                        inventaris = new InventarisModel();
                        lokaal = new LokaalModel();
                        obj = new ObjectModel();
                        leverancier = new LeverancierModel();
                        factuur = new FactuurModel();
                        objType = new ObjectTypeModel();
                        verzekering = new VerzekeringModel();
                        postcode = new PostcodeModel();


                        if (mySqlDataReader["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int)mySqlDataReader["idPostcode"];
                            postcode.Gemeente = mySqlDataReader["gemeente"].ToString();
                            postcode.Postcode = mySqlDataReader["postcode"].ToString();
                        }


                        if (mySqlDataReader["idLokaal"] != DBNull.Value)
                        {
                            lokaal.Id = (int?)mySqlDataReader["idLokaal"];
                            lokaal.AantalPlaatsen = (int)mySqlDataReader["aantalPlaatsen"];
                            lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                            lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();
                        }

                        if (mySqlDataReader["idLeverancier"] != DBNull.Value)
                        {
                            leverancier.Id = (int?)mySqlDataReader["idLeverancier"];
                            leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                            leverancier.Bic = mySqlDataReader["bic"].ToString();
                            leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                            leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                            leverancier.Email = mySqlDataReader["email"].ToString();
                            leverancier.Fax = mySqlDataReader["fax"].ToString();
                            leverancier.HuisNummer = mySqlDataReader["huisNummer"].ToString();
                            leverancier.Iban = mySqlDataReader["iban"].ToString();
                            leverancier.Naam = mySqlDataReader["naam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = mySqlDataReader["straat"].ToString();
                            leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                            leverancier.ToegevoegdOp = mySqlDataReader["toegevoegdOp"].ToString();
                            leverancier.Website = mySqlDataReader["website"].ToString();
                        }




                        if (mySqlDataReader["idFactuur"] != DBNull.Value)
                        {
                            factuur.Id = (int?)mySqlDataReader["idFactuur"];
                            factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijvingsperiode"];
                            factuur.Boekjaar = mySqlDataReader["Boekjaar"].ToString();
                            factuur.CvoVolgNummer = mySqlDataReader["CvoVolgNummer"].ToString();
                            factuur.DatumInsert = mySqlDataReader["DatumInsert"].ToString();
                            factuur.DatumModified = mySqlDataReader["DatumModified"].ToString();
                            factuur.FactuurDatum = mySqlDataReader["FactuurDatum"].ToString();
                            factuur.FactuurNummer = mySqlDataReader["FactuurNummer"].ToString();
                            factuur.Garantie = (int)mySqlDataReader["Garantie"];
                            factuur.Leverancier = leverancier;
                            factuur.Omschrijving = mySqlDataReader["Omschrijving"].ToString();
                            factuur.Opmerking = mySqlDataReader["Opmerking"].ToString();
                            factuur.Prijs = mySqlDataReader["Prijs"].ToString();
                            factuur.UserInsert = mySqlDataReader["UserInsert"].ToString();
                            factuur.UserModified = mySqlDataReader["UserModified"].ToString();
                        }


                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();
                        }

                        if (mySqlDataReader["idObject"] != DBNull.Value)
                        {
                            obj.Id = (int?)mySqlDataReader["idObject"];
                            obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                            obj.Factuur = factuur;
                            obj.ObjectType = objType;
                        }

                        if (mySqlDataReader["idVerzekering"] != DBNull.Value)
                        {
                            verzekering.Id = (int?)mySqlDataReader["idVerzekering"];
                            verzekering.Omschrijving = mySqlDataReader["omschrijving"].ToString();
                        }

                        inventaris.Id = (int?)mySqlDataReader["idInventaris"];
                        inventaris.Label = mySqlDataReader["label"].ToString();
                        inventaris.Lokaal = lokaal;
                        inventaris.Object = obj;
                        inventaris.Aankoopjaar = (int)mySqlDataReader["aankoopjaar"];
                        inventaris.Afschrijvingsperiode = (int)mySqlDataReader["afschrijvingsperiode"];
                        inventaris.Historiek = mySqlDataReader["historiek"].ToString();
                        inventaris.IsActief = Convert.ToBoolean(mySqlDataReader["isActief"]);
                        inventaris.IsAanwezig = Convert.ToBoolean(mySqlDataReader["isAanwezig"]);
                        inventaris.Verzekering = verzekering;
                    }
                    return inventaris;
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

        public bool Update(InventarisModel inventaris)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblInventarisUpdate", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", inventaris.Id));
                    command.Parameters.Add(new SqlParameter("@idLokaal", App_Code.DALutil.checkIntForDBNUll(inventaris.Lokaal.Id)));
                    command.Parameters.Add(new SqlParameter("@idObject", App_Code.DALutil.checkIntForDBNUll(inventaris.Object.Id)));
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", inventaris.Aankoopjaar));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", inventaris.Afschrijvingsperiode));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", App_Code.DALutil.checkIntForDBNUll(inventaris.Verzekering.Id)));
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

        public List<InventarisModel> Rapportering(string s, string[] keuzeKolommen)
        {
            List<InventarisModel> list = new List<InventarisModel>();

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
                            InventarisModel i = new InventarisModel();
                            if (keuzeKolommen.Contains("Lokaal")) { i.Lokaal = (LokaalModel)dr["Lokaal"]; }
                            if (keuzeKolommen.Contains("Object")) { i.Object = (ObjectModel)dr["Object"]; }
                            if (keuzeKolommen.Contains("Verzekering")) { i.Verzekering = (VerzekeringModel)dr["Verzekering"]; }
                            if (keuzeKolommen.Contains("Aanwezig")) { i.IsAanwezig = Convert.ToBoolean(dr["Aanwezig"]); }
                            if (keuzeKolommen.Contains("Actief")) { i.IsActief = Convert.ToBoolean(dr["Actief"]); }
                            if (keuzeKolommen.Contains("Label")) { i.Label = dr["Label"].ToString(); }
                            if (keuzeKolommen.Contains("Historiek")) { i.Historiek = dr["Historiek"].ToString(); }
                            if (keuzeKolommen.Contains("Aankoopjaar")) { i.Aankoopjaar = (int)dr["Aankoopjaar"]; }
                            if (keuzeKolommen.Contains("Afschrijvingsperiode")) { i.Afschrijvingsperiode = (int)dr["Afschrijvingsperiode"]; }
                            list.Add(i);
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

        private string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        }
    }
}