using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Data;
using CvoInventarisClient.DAL.interfaces;
using CvoInventarisClient.DAL.Helpers;
using CvoInventarisClient.Models;

namespace CvoInventarisClient.DAL
{
    public class TblAccount : ICrudable<AccountModel>
    {
        #region Get Connection String
        private string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["CvoInventarisDBConnection"].ConnectionString;
        }
        #endregion

        #region CRUD TblAccount
        public AccountModel GetById(int id)
        {
            AccountModel acc = new AccountModel();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountReadOne", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    con.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    if (result.HasRows)
                    {
                        result.Read();
                        acc.IdAccount = (int)result["IdAccount"];
                        acc.Email = result["Email"].ToString();
                        acc.Wachtwoord = result["Wachtwoord"].ToString();
                    }
                }
                catch
                {
                }
            }
            return acc;
        }

        public List<AccountModel> GetAll()
        {
            List<AccountModel> accs = new List<AccountModel>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountReadAll", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    con.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            AccountModel acc = new AccountModel();
                            acc.IdAccount = (int)result["IdAccount"];
                            acc.Email = result["Email"].ToString();
                            acc.Wachtwoord = result["Wachtwoord"].ToString();
                            accs.Add(acc);
                        }
                    }
                }
                catch
                {
                }
            }
            return accs;
        }

        public int Create(AccountModel acc)
        {
            int affected = 0;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", acc.Email);
                cmd.Parameters.AddWithValue("@Wachtwoord", PasswordStorage.CreateHash(acc.Wachtwoord));

                try
                {
                    con.Open();
                    affected = cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            return affected;
        }

        public bool Update(AccountModel acc)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdAccount", acc.IdAccount);
                cmd.Parameters.AddWithValue("@Email", acc.Email);
                cmd.Parameters.AddWithValue("@Wachtwoord", PasswordStorage.CreateHash(acc.Wachtwoord));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountDelete", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdAccount", id);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        public AccountModel GetByEmail(string email)
        {
            AccountModel acc = new AccountModel();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("TblAccountReadByEmail", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    con.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    if (result.HasRows)
                    {
                        result.Read();
                        acc.IdAccount = (int)result["IdAccount"];
                        acc.Email = result["Email"].ToString();
                        acc.Wachtwoord = result["Wachtwoord"].ToString();
                    }
                }
                catch
                {
                }
            }
            return acc;
        }

        public bool VerstuurWachtwoordResetEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("spResetWachtwoord", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (Convert.ToBoolean(rdr["ReturnCode"]))
                        {
                            VerstuurEmail(rdr["Email"].ToString(), rdr["UniqueId"].ToString());
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        void VerstuurEmail(string toEmail, string uniqueId)
        {
            string cvoInventarisEmail = "CvoInventaris@gmail.com";

            MailMessage mailMessage = new MailMessage(cvoInventarisEmail, toEmail);

            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Beste,<br/><br/>");
            sbEmailBody.Append("Klik op de onderstaande link om uw wachtwoord te wijzigen.");
            sbEmailBody.Append("<br/>");
            sbEmailBody.Append("http://www.studyplanit.com/cvoi/Account/WijzigWachtwoord?uid=" + uniqueId);
            sbEmailBody.Append("<br/><br/>");
            sbEmailBody.Append("<b>CVO inventaris</b>");

            mailMessage.IsBodyHtml = true;

            mailMessage.Body = sbEmailBody.ToString();
            mailMessage.Subject = "CVO inventaris wachtwoord reset link";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = cvoInventarisEmail,
                Password = "H7zPyCpz"
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }

        public bool IsWachtwoordResetLinkValid(string GUID)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("spIsWachtwoordResetLinkValid", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GUID", GUID);

                try
                {
                    con.Open();
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool WijzigWachtwoord(string GUID, string wachtwoord)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("spWijzigWachtwoord", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GUID", GUID);
                cmd.Parameters.AddWithValue("@Wachtwoord ", PasswordStorage.CreateHash(wachtwoord));

                try
                {
                    con.Open();
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Login(AccountModel a)
        {
            try
            {
                AccountModel accUitDB = GetByEmail(a.Email);

                if (accUitDB.Wachtwoord == "")
                {
                    // het e-mailadres staat niet in de DB, en returnt daardoor geen wachtwoord
                    return false;
                }
                else
                {
                    // het e-mailadres staat in de DB, en returnde een wachtwoord
                    if (PasswordStorage.VerifyPassword(a.Wachtwoord, accUitDB.Wachtwoord))
                    {
                        // ingegeven email en wachtwoord combinatie zijn juist, log de gebruiker in

                        // sla een session op voor deze login
                        //Session s = new Session();
                        //s.IdAccount = accUitDB.IdAccount;
                        //s.Device = Environment.MachineName;
                        //s.Tijdstip = DateTime.Now;
                        //SessionCreate(s);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}