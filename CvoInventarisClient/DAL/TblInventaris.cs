using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;

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
                    command.Parameters.Add(new SqlParameter("@idLokaal", inventaris.Lokaal.IdLokaal));
                    command.Parameters.Add(new SqlParameter("@idObject", inventaris.Object.Id));
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", inventaris.Aankoopjaar));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", inventaris.Afschrijvingsperiode));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", inventaris.Verzekering.IdVerzekering));
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch(Exception e)
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
            catch(Exception e)
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

                        lokaal.IdLokaal = (int)mySqlDataReader["idLokaal"];
                        lokaal.AantalPlaatsen = (int)mySqlDataReader["aantalPlaatsen"];
                        lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                        lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();

                        leverancier.IdLeverancier = (int)mySqlDataReader["idLeverancier"];
                        leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                        leverancier.Bic = mySqlDataReader["bic"].ToString();
                        leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                        leverancier.BusNummer = (int)mySqlDataReader["busNummer"];
                        leverancier.Email = mySqlDataReader["email"].ToString();
                        leverancier.Fax = mySqlDataReader["fax"].ToString();
                        leverancier.HuisNummer = (int)mySqlDataReader["huisNummer"];
                        leverancier.Iban = mySqlDataReader["iban"].ToString();
                        leverancier.Naam = mySqlDataReader["naam"].ToString();
                        leverancier.Postcode = (int)mySqlDataReader["postcode"];
                        leverancier.Straat = mySqlDataReader["straat"].ToString();
                        leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                        leverancier.ToegevoegdOp = (DateTime)mySqlDataReader["toegevoegdOp"];
                        leverancier.Website = mySqlDataReader["website"].ToString();

                        factuur.IdFactuur = (int)mySqlDataReader["idFactuur"];
                        factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijvingsperiode"];
                        factuur.Boekjaar = mySqlDataReader["Boekjaar"].ToString();
                        factuur.CvoVolgNummer = mySqlDataReader["CvoVolgNummer"].ToString();
                        factuur.DatumInsert = (DateTime)mySqlDataReader["DatumInsert"];
                        factuur.DatumModified = (DateTime)mySqlDataReader["DatumModified"];
                        factuur.FactuurDatum = (DateTime)mySqlDataReader["FactuurDatum"];
                        factuur.FactuurNummer = mySqlDataReader["FactuurNummer"].ToString();
                        factuur.Garantie = (int)mySqlDataReader["Garantie"];
                        factuur.Leverancier = leverancier;
                        factuur.Omschrijving = mySqlDataReader["Omschrijving"].ToString();
                        factuur.Opmerking = mySqlDataReader["Opmerking"].ToString();
                        factuur.Prijs = (decimal)mySqlDataReader["Prijs"];
                        factuur.UserInsert = mySqlDataReader["UserInsert"].ToString();
                        factuur.UserModified = mySqlDataReader["UserModified"].ToString();

                        objType.IdObjectType = (int)mySqlDataReader["idObjectType"];
                        objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        obj.Id = (int)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.Factuur = factuur;
                        obj.ObjectType = objType;

                        verzekering.IdVerzekering = (int)mySqlDataReader["idVerzekering"];
                        verzekering.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        inventaris.Id = (int)mySqlDataReader["idInventaris"];
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
            catch
            {
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


                        lokaal.IdLokaal = (int)mySqlDataReader["idLokaal"];
                        lokaal.AantalPlaatsen = (int)mySqlDataReader["aantalPlaatsen"];
                        lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                        lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();

                        leverancier.IdLeverancier = (int)mySqlDataReader["idLeverancier"];
                        leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                        leverancier.Bic = mySqlDataReader["bic"].ToString();
                        leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                        leverancier.BusNummer = (int)mySqlDataReader["busNummer"];
                        leverancier.Email = mySqlDataReader["email"].ToString();
                        leverancier.Fax = mySqlDataReader["fax"].ToString();
                        leverancier.HuisNummer = (int)mySqlDataReader["huisNummer"];
                        leverancier.Iban = mySqlDataReader["iban"].ToString();
                        leverancier.Naam = mySqlDataReader["naam"].ToString();
                        leverancier.Postcode = (int)mySqlDataReader["postcode"];
                        leverancier.Straat = mySqlDataReader["straat"].ToString();
                        leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                        leverancier.ToegevoegdOp = (DateTime)mySqlDataReader["toegevoegdOp"];
                        leverancier.Website = mySqlDataReader["website"].ToString();

                        factuur.IdFactuur = (int)mySqlDataReader["idFactuur"];
                        factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijvingsperiode"];
                        factuur.Boekjaar = mySqlDataReader["Boekjaar"].ToString();
                        factuur.CvoVolgNummer = mySqlDataReader["CvoVolgNummer"].ToString();
                        factuur.DatumInsert = (DateTime)mySqlDataReader["DatumInsert"];
                        factuur.DatumModified = (DateTime)mySqlDataReader["DatumModified"];
                        factuur.FactuurDatum = (DateTime)mySqlDataReader["FactuurDatum"];
                        factuur.FactuurNummer = mySqlDataReader["FactuurNummer"].ToString();
                        factuur.Garantie = (int)mySqlDataReader["Garantie"];
                        factuur.Leverancier = leverancier;
                        factuur.Omschrijving = mySqlDataReader["Omschrijving"].ToString();
                        factuur.Opmerking = mySqlDataReader["Opmerking"].ToString();
                        factuur.Prijs = (decimal)mySqlDataReader["Prijs"];
                        factuur.UserInsert = mySqlDataReader["UserInsert"].ToString();
                        factuur.UserModified = mySqlDataReader["UserModified"].ToString();

                        objType.IdObjectType = (int)mySqlDataReader["idObjectType"];
                        objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        obj.Id = (int)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.Factuur = factuur;
                        obj.ObjectType = objType;

                        verzekering.IdVerzekering = (int)mySqlDataReader["idVerzekering"];
                        verzekering.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        inventaris.Id = (int)mySqlDataReader["idInventaris"];
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
                    command.Parameters.Add(new SqlParameter("@label", inventaris.Label));
                    command.Parameters.Add(new SqlParameter("@idLokaal", inventaris.Lokaal.IdLokaal));
                    command.Parameters.Add(new SqlParameter("@idObject", inventaris.Object.Id));
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", inventaris.Aankoopjaar));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", inventaris.Afschrijvingsperiode));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", inventaris.Verzekering.IdVerzekering));
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