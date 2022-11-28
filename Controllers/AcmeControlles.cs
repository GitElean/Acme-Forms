using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;//SHA256
using System.Text;
using System.Web;
using System.Web.Mvc;
//sql server
using System.Data.SqlClient;
using System.Data;

using AcmeForms.Models;

namespace AcmeForms.Controllers
{
    public class AcmeController : Controller
    {
        //database conection
        static string conect = "Data Source=(MSI);Initial Catalog=DB_Acme;Integrated Security=true";//using windows authentication

        //get
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        //post
        [HttpPost]
        public ActionResult Register(User oUser)
        {
            bool signed;
            string message;

            if (oUser.Pass == oUser.ConfirmPassword) {
            
                oUser.Pass = EncryptSha256(oUser.Pass);//encrypting

            }
            else
            {
                ViewData["message"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(conect))
            {

                SqlCommand cmd = new SqlCommand("Register_SignUpUser", cn);
                cmd.Parameters.AddWithValue("email", oUser.Email);
                cmd.Parameters.AddWithValue("pass", oUser.Pass);
                cmd.Parameters.Add("signed", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("message", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                signed = Convert.ToBoolean(cmd.Parameters["signed"].Value);
                message = cmd.Parameters["message"].Value.ToString();


            }

            if (signed)
            {
                return RedirectToAction("Login", "Acme");
            }
            else
            {
                return View();
            }

           
            }

        [HttpPost]
        public ActionResult Login(User oUser)
        {
            oUser.Pass = EncryptSha256(oUser.Pass);

            using (SqlConnection cn = new SqlConnection(conect))
            {

                SqlCommand cmd = new SqlCommand("validateUser", cn);
                cmd.Parameters.AddWithValue("email", oUser.Email);
                cmd.Parameters.AddWithValue("pass", oUser.Pass);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oUser.ID_user = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (oUser.ID_user != 0)
            {

                Session["user"] = oUser;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["message"] = "usuario no encontrado";
                return View();
            }
        }

        //utility
        public static string EncryptSha256(string pass)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(pass));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


    }
}
