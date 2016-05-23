using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.DAL
{
    public class TblObject : ICrudable<ObjectModel>
    {
        SqlConnection connection = new SqlConnection("Data Source=92.222.220.213,1500;Initial Catalog=CvoInventarisdb;Persist Security Info=True;User ID=sa;Password=grati#s1867");

        public int Create(ObjectModel obj)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("TblObjectInsert", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@idObjectType", obj.ObjectType.Id));
                    command.Parameters.Add(new SqlParameter("@kenmerken", obj.Kenmerken));
                    command.Parameters.Add(new SqlParameter("@idFactuur", obj.Factuur.Id));
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
            LeverancierModel leverancier;
            FactuurModel factuur;
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
                        leverancier = new LeverancierModel();
                        factuur = new FactuurModel();
                        objType = new ObjectTypeModel();

                        leverancier.Id = (int)mySqlDataReader["idLeverancier"];
                        leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                        leverancier.Bic = mySqlDataReader["bic"].ToString();
                        leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                        leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                        leverancier.Email = mySqlDataReader["email"].ToString();
                        leverancier.Fax = mySqlDataReader["fax"].ToString();
                        leverancier.HuisNummer = mySqlDataReader["huisNummer"].ToString();
                        leverancier.Iban = mySqlDataReader["iban"].ToString();
                        leverancier.Naam = mySqlDataReader["naam"].ToString();
                        leverancier.Postcode = (int?)mySqlDataReader["idPostcode"];
                        leverancier.Straat = mySqlDataReader["straat"].ToString();
                        leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                        leverancier.ToegevoegdOp = mySqlDataReader["toegevoegdOp"].ToString();
                        leverancier.Website = mySqlDataReader["website"].ToString();

                        factuur.Id = (int)mySqlDataReader["idFactuur"];
                        factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijfperiode"];
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

                        objType.Id = (int)mySqlDataReader["idObjectType"];
                        objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        obj.Id = (int)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.Factuur = factuur;
                        obj.ObjectType = objType;
                        list.Add(obj);
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

        public ObjectModel GetById(int id)
        {
            ObjectModel obj = new ObjectModel();
            LeverancierModel leverancier;
            FactuurModel factuur;
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
                        leverancier = new LeverancierModel();
                        factuur = new FactuurModel();
                        objType = new ObjectTypeModel();

                        leverancier.Id = (int)mySqlDataReader["idLeverancier"];
                        leverancier.Afkorting = mySqlDataReader["afkorting"].ToString();
                        leverancier.Bic = mySqlDataReader["bic"].ToString();
                        leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                        leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                        leverancier.Email = mySqlDataReader["email"].ToString();
                        leverancier.Fax = mySqlDataReader["fax"].ToString();
                        leverancier.HuisNummer = mySqlDataReader["huisNummer"].ToString();
                        leverancier.Iban = mySqlDataReader["iban"].ToString();
                        leverancier.Naam = mySqlDataReader["naam"].ToString();
                        leverancier.Postcode = (int?)mySqlDataReader["idPostcode"];
                        leverancier.Straat = mySqlDataReader["straat"].ToString();
                        leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                        leverancier.ToegevoegdOp = mySqlDataReader["toegevoegdOp"].ToString();
                        leverancier.Website = mySqlDataReader["website"].ToString();

                        factuur.Id = (int)mySqlDataReader["idFactuur"];
                        factuur.Afschrijfperiode = (int)mySqlDataReader["Afschrijfperiode"];
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

                        objType.Id = (int)mySqlDataReader["idObjectType"];
                        objType.Omschrijving = mySqlDataReader["omschrijving"].ToString();

                        obj.Id = (int)mySqlDataReader["idObject"];
                        obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                        obj.Factuur = factuur;
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
                    command.Parameters.Add(new SqlParameter("@idObjectType", obj.ObjectType.Id));
                    command.Parameters.Add(new SqlParameter("@kenmerken", obj.Kenmerken));
                    command.Parameters.Add(new SqlParameter("@idFactuur", obj.Factuur.Id));
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

        public List<ObjectModel> Rapportering(string s, string[] keuzeKolommen)
        {
            List<ObjectModel> list = new List<ObjectModel>();
            ObjectModel objecten = new ObjectModel();

            try
            {
                using (SqlCommand cmd = new SqlCommand(s, connection))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            LokaalModel lokaal = new LokaalModel();
                            if (keuzeKolommen.Contains("TblObject.kenmerken")) { objecten.Kenmerken = dr["kenmerken"].ToString(); }
                            if (keuzeKolommen.Contains("tblFactuur.FactuurNummer")) { objecten.Factuur.FactuurNummer = dr["FactuurNummer"].ToString(); }
                            if (keuzeKolommen.Contains("TblObjectType.omschrijving")) { objecten.ObjectType.Omschrijving = dr["aantalPlaatsen"].ToString(); }

                            list.Add(objecten);
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