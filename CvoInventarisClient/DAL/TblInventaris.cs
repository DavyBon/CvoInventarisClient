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
using CvoInventarisClient.DAL.Helpers;


namespace CvoInventarisClient.DAL
{
    public class TblInventaris : ICrudable<InventarisModel>
    {
        SqlConnection connection = new SqlConnection(DatabaseConnection.GetConnectionString());

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
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", App_Code.DALutil.checkIntForDBNUll(inventaris.Aankoopjaar)));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", App_Code.DALutil.checkIntForDBNUll(inventaris.Afschrijvingsperiode)));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", App_Code.DALutil.checkIntForDBNUll(inventaris.Verzekering.Id)));
                    command.Parameters.Add(new SqlParameter("@idFactuur", App_Code.DALutil.checkIntForDBNUll(inventaris.Factuur.Id)));
                    command.Parameters.Add(new SqlParameter("@waarde", App_Code.DALutil.checkDecimalForDBNUll(inventaris.Waarde)));
                    command.Parameters.Add(new SqlParameter("@costcenter", inventaris.Costcenter));
                    command.Parameters.Add(new SqlParameter("@boekhoudnr", inventaris.Boekhoudnr));
                    command.Parameters.Add(new SqlParameter("@idLeverancier", App_Code.DALutil.checkIntForDBNUll(inventaris.Leverancier.Id)));


                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException e)
            {
                if (e.Number == 2601)
                {
                    return -1;
                }
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
            CampusModel campus;

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
                        campus = new CampusModel();


                        if (mySqlDataReader["idCampus"] != DBNull.Value)
                        {
                            campus.Id = mySqlDataReader["idCampus"] as int?;
                            campus.Naam = mySqlDataReader["campusNaam"].ToString();
                            campus.Straat = mySqlDataReader["campusStraat"].ToString();
                            campus.Nummer = mySqlDataReader["campusNummer"].ToString();
                        }

                        if (mySqlDataReader["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)mySqlDataReader["idPostcode"];
                            postcode.Gemeente = mySqlDataReader["gemeente"].ToString();
                            postcode.Postcode = mySqlDataReader["postcode"].ToString();
                        }

                        if (mySqlDataReader["idLokaal"] != DBNull.Value)
                        {
                            lokaal.Id = mySqlDataReader["idLokaal"] as int?;
                            lokaal.AantalPlaatsen = mySqlDataReader["aantalPlaatsen"] as int?;
                            lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                            lokaal.Campus = campus;
                            lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();
                        }

                        if (mySqlDataReader["idLeverancier"] != DBNull.Value)
                        {
                            leverancier.Id = (int?)mySqlDataReader["idLeverancier"];
                            leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                            leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                            leverancier.Email = mySqlDataReader["email"].ToString();
                            leverancier.Fax = mySqlDataReader["fax"].ToString();
                            leverancier.StraatNummer = mySqlDataReader["straatNummer"].ToString();
                            leverancier.Naam = mySqlDataReader["leverancierNaam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = mySqlDataReader["leverancierStraat"].ToString();
                            leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                            leverancier.ActiefDatum = mySqlDataReader["actiefDatum"].ToString();
                            leverancier.Website = mySqlDataReader["website"].ToString();
                        }

                        if (mySqlDataReader["idFactuur"] != DBNull.Value)
                        {
                            factuur.Id = (int?)mySqlDataReader["idFactuur"];
                            factuur.ScholengroepNummer = mySqlDataReader["scholengroepNummer"].ToString();
                            factuur.Prijs = mySqlDataReader["prijs"] as decimal?;
                            factuur.Garantie = (int?)mySqlDataReader["garantie"];
                            factuur.Omschrijving = mySqlDataReader["factuurOmschrijving"].ToString();
                            factuur.Afschrijfperiode = mySqlDataReader["afschrijvingsperiode"] as int? ?? default(int);
                            factuur.VerwerkingsDatum = mySqlDataReader["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = mySqlDataReader["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = mySqlDataReader["leverancierFactuurNummer"].ToString();
                        }


                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["objectTypeOmschrijving"].ToString();
                        }

                        if (mySqlDataReader["idObject"] != DBNull.Value)
                        {
                            obj.Id = (int?)mySqlDataReader["idObject"];
                            obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                            obj.Omschrijving = mySqlDataReader["objectOmschrijving"].ToString();
                            obj.afmetingen = mySqlDataReader["afmetingen"].ToString();
                            obj.ObjectType = objType;
                        }

                        if (mySqlDataReader["idVerzekering"] != DBNull.Value)
                        {
                            verzekering.Id = (int?)mySqlDataReader["idVerzekering"];
                            verzekering.Omschrijving = mySqlDataReader["verzekeringOmschrijving"].ToString();
                        }

                        inventaris.Id = (int?)mySqlDataReader["idInventaris"];
                        inventaris.Label = mySqlDataReader["label"].ToString();
                        inventaris.Lokaal = lokaal;
                        inventaris.Object = obj;
                        inventaris.Leverancier = leverancier;
                        inventaris.Aankoopjaar = mySqlDataReader["aankoopjaar"] as int?;
                        inventaris.Afschrijvingsperiode = mySqlDataReader["afschrijvingsperiode"] as int?;
                        inventaris.Historiek = mySqlDataReader["historiek"].ToString();
                        inventaris.IsActief = mySqlDataReader["isActief"] as bool? ?? default(bool);
                        inventaris.IsAanwezig = mySqlDataReader["isAanwezig"] as bool? ?? default(bool);
                        inventaris.Verzekering = verzekering;
                        inventaris.Factuur = factuur;
                        inventaris.Waarde = mySqlDataReader["waarde"] as decimal?;
                        inventaris.Costcenter = mySqlDataReader["costcenter"].ToString();
                        inventaris.Boekhoudnr = mySqlDataReader["boekhoudnr"].ToString();

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

        public List<InventarisModel> GetTop(int? amount)
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
            CampusModel campus;

            try
            {
                using (SqlCommand command = new SqlCommand("TblInventarisReadTop", connection))
                {
                    connection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (amount == null)
                    {
                        command.Parameters.AddWithValue("@amount", 100);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@amount", amount);
                    }
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
                        campus = new CampusModel();

                        if (mySqlDataReader["idCampus"] != DBNull.Value)
                        {
                            campus.Id = mySqlDataReader["idCampus"] as int?;
                            campus.Naam = mySqlDataReader["campusNaam"].ToString();
                            campus.Straat = mySqlDataReader["campusStraat"].ToString();
                            campus.Nummer = mySqlDataReader["campusNummer"].ToString();
                        }

                        if (mySqlDataReader["idPostcode"] != DBNull.Value)
                        {
                            postcode.Id = (int?)mySqlDataReader["idPostcode"];
                            postcode.Gemeente = mySqlDataReader["gemeente"].ToString();
                            postcode.Postcode = mySqlDataReader["postcode"].ToString();
                        }

                        if (mySqlDataReader["idLokaal"] != DBNull.Value)
                        {
                            lokaal.Id = mySqlDataReader["idLokaal"] as int?;
                            lokaal.AantalPlaatsen = mySqlDataReader["aantalPlaatsen"] as int?;
                            lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                            lokaal.Campus = campus;
                            lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();
                        }

                        if (mySqlDataReader["idLeverancier"] != DBNull.Value)
                        {
                            leverancier.Id = (int?)mySqlDataReader["idLeverancier"];
                            leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                            leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                            leverancier.Email = mySqlDataReader["email"].ToString();
                            leverancier.Fax = mySqlDataReader["fax"].ToString();
                            leverancier.StraatNummer = mySqlDataReader["straatNummer"].ToString();
                            leverancier.Naam = mySqlDataReader["leverancierNaam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = mySqlDataReader["leverancierStraat"].ToString();
                            leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                            leverancier.ActiefDatum = mySqlDataReader["actiefDatum"].ToString();
                            leverancier.Website = mySqlDataReader["website"].ToString();
                        }

                        if (mySqlDataReader["idFactuur"] != DBNull.Value)
                        {
                            factuur.Id = (int?)mySqlDataReader["idFactuur"];
                            factuur.ScholengroepNummer = mySqlDataReader["scholengroepNummer"].ToString();
                            factuur.Prijs = mySqlDataReader["prijs"] as decimal?;
                            factuur.Garantie = mySqlDataReader["garantie"] as int?;
                            factuur.Omschrijving = mySqlDataReader["factuurOmschrijving"].ToString();
                            factuur.Afschrijfperiode = mySqlDataReader["afschrijvingsperiode"] as int? ?? default(int);
                            factuur.VerwerkingsDatum = mySqlDataReader["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = mySqlDataReader["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = mySqlDataReader["leverancierFactuurNummer"].ToString();
                        }


                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["objectTypeOmschrijving"].ToString();
                        }

                        if (mySqlDataReader["idObject"] != DBNull.Value)
                        {
                            obj.Id = (int?)mySqlDataReader["idObject"];
                            obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                            obj.Omschrijving = mySqlDataReader["objectOmschrijving"].ToString();
                            obj.afmetingen = mySqlDataReader["afmetingen"].ToString();
                            obj.ObjectType = objType;
                        }

                        if (mySqlDataReader["idVerzekering"] != DBNull.Value)
                        {
                            verzekering.Id = (int?)mySqlDataReader["idVerzekering"];
                            verzekering.Omschrijving = mySqlDataReader["verzekeringOmschrijving"].ToString();
                        }

                        inventaris.Id = (int?)mySqlDataReader["idInventaris"];
                        inventaris.Label = mySqlDataReader["label"].ToString();
                        inventaris.Lokaal = lokaal;
                        inventaris.Object = obj;
                        inventaris.Leverancier = leverancier;
                        inventaris.Aankoopjaar = mySqlDataReader["aankoopjaar"] as int?;
                        inventaris.Afschrijvingsperiode = mySqlDataReader["afschrijvingsperiode"] as int?;
                        inventaris.Historiek = mySqlDataReader["historiek"].ToString();
                        inventaris.IsActief = mySqlDataReader["isActief"] as bool? ?? default(bool);
                        inventaris.IsAanwezig = mySqlDataReader["isAanwezig"] as bool? ?? default(bool);
                        inventaris.Verzekering = verzekering;
                        inventaris.Factuur = factuur;
                        inventaris.Waarde = mySqlDataReader["waarde"] as decimal?;
                        inventaris.Costcenter = mySqlDataReader["costcenter"].ToString();
                        inventaris.Boekhoudnr = mySqlDataReader["boekhoudnr"].ToString();

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
                            postcode.Id = (int?)mySqlDataReader["idPostcode"];
                            postcode.Gemeente = mySqlDataReader["gemeente"].ToString();
                            postcode.Postcode = mySqlDataReader["postcode"].ToString();
                        }

                        if (mySqlDataReader["idLokaal"] != DBNull.Value)
                        {
                            lokaal.Id = (int?)mySqlDataReader["idLokaal"];
                            lokaal.AantalPlaatsen = (int?)mySqlDataReader["aantalPlaatsen"];
                            lokaal.IsComputerLokaal = Convert.ToBoolean(mySqlDataReader["isComputerLokaal"]);
                            lokaal.LokaalNaam = mySqlDataReader["lokaalNaam"].ToString();
                        }

                        if (mySqlDataReader["idLeverancier"] != DBNull.Value)
                        {
                            leverancier.Id = (int?)mySqlDataReader["idLeverancier"];
                            leverancier.BtwNummer = mySqlDataReader["btwNummer"].ToString();
                            leverancier.BusNummer = mySqlDataReader["busNummer"].ToString();
                            leverancier.Email = mySqlDataReader["email"].ToString();
                            leverancier.Fax = mySqlDataReader["fax"].ToString();
                            leverancier.StraatNummer = mySqlDataReader["straatNummer"].ToString();
                            leverancier.Naam = mySqlDataReader["leverancierNaam"].ToString();
                            leverancier.Postcode = postcode;
                            leverancier.Straat = mySqlDataReader["leverancierStraat"].ToString();
                            leverancier.Telefoon = mySqlDataReader["telefoon"].ToString();
                            leverancier.ActiefDatum = mySqlDataReader["actiefDatum"].ToString();
                            leverancier.Website = mySqlDataReader["website"].ToString();
                        }

                        if (mySqlDataReader["idFactuur"] != DBNull.Value)
                        {
                            factuur.Id = (int?)mySqlDataReader["idFactuur"];
                            factuur.ScholengroepNummer = mySqlDataReader["scholengroepNummer"].ToString();
                            factuur.Prijs = mySqlDataReader["prijs"] as decimal?;
                            factuur.Garantie = (int?)mySqlDataReader["garantie"];
                            factuur.Omschrijving = mySqlDataReader["factuurOmschrijving"].ToString();
                            factuur.Afschrijfperiode = mySqlDataReader["afschrijvingsperiode"] as int? ?? default(int);
                            factuur.VerwerkingsDatum = mySqlDataReader["verwerkingsDatum"].ToString();
                            factuur.CvoFactuurNummer = mySqlDataReader["cvoFactuurNummer"].ToString();
                            factuur.LeverancierFactuurNummer = mySqlDataReader["leverancierFactuurNummer"].ToString();
                        }


                        if (mySqlDataReader["idObjectType"] != DBNull.Value)
                        {
                            objType.Id = (int?)mySqlDataReader["idObjectType"];
                            objType.Omschrijving = mySqlDataReader["objectTypeOmschrijving"].ToString();
                        }

                        if (mySqlDataReader["idObject"] != DBNull.Value)
                        {
                            obj.Id = (int?)mySqlDataReader["idObject"];
                            obj.Kenmerken = mySqlDataReader["kenmerken"].ToString();
                            obj.Omschrijving = mySqlDataReader["objectOmschrijving"].ToString();
                            obj.afmetingen = mySqlDataReader["afmetingen"].ToString();
                            obj.ObjectType = objType;
                        }

                        if (mySqlDataReader["idVerzekering"] != DBNull.Value)
                        {
                            verzekering.Id = (int?)mySqlDataReader["idVerzekering"];
                            verzekering.Omschrijving = mySqlDataReader["verzekeringOmschrijving"].ToString();
                        }

                        inventaris.Id = (int?)mySqlDataReader["idInventaris"];
                        inventaris.Label = mySqlDataReader["label"].ToString();
                        inventaris.Lokaal = lokaal;
                        inventaris.Object = obj;
                        inventaris.Leverancier = leverancier;
                        inventaris.Aankoopjaar = mySqlDataReader["aankoopjaar"] as int?;
                        inventaris.Afschrijvingsperiode = mySqlDataReader["afschrijvingsperiode"] as int?;
                        inventaris.Historiek = mySqlDataReader["historiek"].ToString();
                        inventaris.IsActief = mySqlDataReader["isActief"] as bool? ?? default(bool);
                        inventaris.IsAanwezig = mySqlDataReader["isAanwezig"] as bool? ?? default(bool);
                        inventaris.Verzekering = verzekering;
                        inventaris.Factuur = factuur;
                        inventaris.Waarde = mySqlDataReader["waarde"] as decimal?;
                        inventaris.Costcenter = mySqlDataReader["costcenter"].ToString();
                        inventaris.Boekhoudnr = mySqlDataReader["boekhoudnr"].ToString();
                    }
                    return inventaris;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);
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
                    command.Parameters.Add(new SqlParameter("@aankoopjaar", App_Code.DALutil.checkIntForDBNUll(inventaris.Aankoopjaar)));
                    command.Parameters.Add(new SqlParameter("@afschrijvingsjaar", App_Code.DALutil.checkIntForDBNUll(inventaris.Afschrijvingsperiode)));
                    command.Parameters.Add(new SqlParameter("@historiek", inventaris.Historiek));
                    command.Parameters.Add(new SqlParameter("@isActief", Convert.ToInt32(inventaris.IsActief)));
                    command.Parameters.Add(new SqlParameter("@isAanwezig", Convert.ToInt32(inventaris.IsAanwezig)));
                    command.Parameters.Add(new SqlParameter("@idVerzekering", App_Code.DALutil.checkIntForDBNUll(inventaris.Verzekering.Id)));
                    command.Parameters.Add(new SqlParameter("@idFactuur", App_Code.DALutil.checkIntForDBNUll(inventaris.Factuur.Id)));
                    command.Parameters.Add(new SqlParameter("@waarde", App_Code.DALutil.checkDecimalForDBNUll(inventaris.Waarde)));
                    command.Parameters.Add(new SqlParameter("@costcenter", inventaris.Costcenter));
                    command.Parameters.Add(new SqlParameter("@boekhoudnr", inventaris.Boekhoudnr));
                    command.Parameters.Add(new SqlParameter("@idLeverancier", App_Code.DALutil.checkIntForDBNUll(inventaris.Leverancier.Id)));
                    command.ExecuteReader();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Write(e);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}