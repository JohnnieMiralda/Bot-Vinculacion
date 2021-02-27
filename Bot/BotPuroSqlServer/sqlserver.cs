using Newtonsoft.Json;
using System;


using System.Data.SqlClient;
using System.IO;

namespace BotVinculacionUnitec
{
    class sqlserver
    {
        

        SqlConnection myConnection = new SqlConnection(Config.GetDataConnection());

        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    myConnection.Open();
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }

        }

        public void CloseConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    myConnection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public bool CuentaVerificada(string numeroCuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from DataBot WHERE NoCuenta=@NumeroCuenta and Verified = 1";
            SqlCommand selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();
            selectCommand.Parameters.AddWithValue("@NumeroCuenta", numeroCuenta);
            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();
            
            if (selectResult.HasRows)
            {
                CloseConnection();
                return true;
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool CuentaVerificadaDb(string numeroCuenta)
        {
            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from alumnos inner join alumnos_bot on alumnos.id=alumnos_bot.id_alumno WHERE alumnos.codigo_alumno=@NumeroCuenta and alumnos_bot.confirmado = 1";
            SqlCommand selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();
            selectCommand.Parameters.AddWithValue("@NumeroCuenta", numeroCuenta);
            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();

                if (selectResult.HasRows)
                {
                    CloseConnection();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CuentaVerificar"+e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool Existe(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from DataBot ";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();


            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();

                while (selectResult.Read())
                {
                    string actual = selectResult.GetString(0);

                    if (Cuenta == actual)
                    {
                        //     Console.WriteLine(selectResult.GetString(0));
                        CloseConnection();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool ExisteDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT cuenta_telegram from alumnos_bot ";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();


            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();

                while (selectResult.Read())
                {
                    string actual = selectResult.GetString(0);

                    if (Cuenta == actual)
                    {
                        //     Console.WriteLine(selectResult.GetString(0));
                        CloseConnection();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Existe "+e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool estado(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from DataBot";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();


            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();
            
            while (selectResult.Read())
            {
                // Console.WriteLine(selectResult.GetString(0));
                if (Cuenta == selectResult.GetString(0))
                {
                    int num = selectResult.GetInt32(6);
                    // Console.WriteLine(num);
                    if (num == 2)
                    {
                        CloseConnection();
                        return true;
                    }
                }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return false;
        }


        public bool estadoDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT cuenta_telegram,estado from alumnos_bot";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();


            SqlDataReader selectResult = null;
            try
            {
                selectResult = selectCommand.ExecuteReader();

                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        int num = selectResult.GetInt32(1);
                        // Console.WriteLine(num);
                        if (num == 2)
                        {
                            CloseConnection();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("estado "+e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool Verificar(string Cuenta, string code)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from DataBot ";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();

            try
            {
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {


                        if (code == selectResult.GetString(3))
                        {
                            CloseConnection();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool VerificarDb(string Cuenta, string code)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT cuenta_telegram,token_generado from alumnos_bot ";
            using var selectCommand = new SqlCommand(selectQuery, myConnection);
            OpenConnection();

            try
            {
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {


                        if (code == selectResult.GetString(1))
                        {
                            CloseConnection();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("verificar "+e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool VerificarUpdate(string Cuenta, string code)
        {
            SqlConnection Connection = new SqlConnection("Server=tcp:hours-tracker-staging.database.windows.net,1433;Initial Catalog=HoursTrackerStaging;Persist Security Info=False;User ID=hours-tracker;Password=H0ur5-7r4ck3r-123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            /*
            SELECT from Database
            */
            string unique = "";
            DateTime now = DateTime.Now;
            string updatequery = "Update DataBot set fecha_confirmacion=@now ,confirmado=1 ,estado=2,token_generado=@unique where token_generado=@code and cuenta_telegram=@Cuenta";

            try {
                using var cmd = new SqlCommand(updatequery, Connection);
                Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@unique", unique));
                cmd.Parameters.Add(new SqlParameter("@code", code));
                cmd.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                cmd.Parameters.Add(new SqlParameter("@now", now));




                cmd.ExecuteNonQuery();

                Connection.Close();
                CloseConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cgaada Tio" + e.Message);
                return false;
            }


        }

        public bool VerificarUpdateDb(string Cuenta, string code)
        {
            SqlConnection Connection = new SqlConnection("Server=tcp:hours-tracker-staging.database.windows.net,1433;Initial Catalog=HoursTrackerStaging;Persist Security Info=False;User ID=hours-tracker;Password=H0ur5-7r4ck3r-123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            /*
            SELECT from Database
            */
            string unique = "";
            DateTime now = DateTime.Now;
            string updatequery = "Update alumnos_bot set Fecha_confirmacion=@now ,confirmado=1 ,estado=2,token_generado=@unique where token_generado=@code and cuenta_telegram=@Cuenta";

            try
            {
                using var cmd = new SqlCommand(updatequery, Connection);
                Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@unique", unique));
                cmd.Parameters.Add(new SqlParameter("@code", code));
                cmd.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                cmd.Parameters.Add(new SqlParameter("@now", now));




                cmd.ExecuteNonQuery();

                Connection.Close();
                CloseConnection();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cgaada Tio" + e.Message);
                return false;
            }


        }


        public string GetCuenta(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from DataBot where telegramid=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));

                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {

                        string retornable = selectResult.GetString(1);
                        CloseConnection();
                        return retornable;

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            string aus = "";
            return aus;
        }

        public string GetCuentaDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT cuenta_telegram,primer_nombre,primer_apellido from alumnos inner join alumnos_bot on alumnos.id=alumnos_bot.id_alumno where cuenta_telegram=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                try
                {
                    
                    selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));

                    SqlDataReader selectResult = selectCommand.ExecuteReader();
                    while (selectResult.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == selectResult.GetString(0))
                        {

                            string retornable = selectResult.GetString(1)+" " + selectResult.GetString(2);
                            CloseConnection();
                            return retornable;

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("casta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getCuenta"+e.Message);
            }
            CloseConnection();
            string aus = "";
            return aus;
        }

        public string GetCuentaNUMDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT cuenta_telegram,codigo_alumno from alumnos inner join alumnos_bot on alumnos.id=alumnos_bot.id_alumno where cuenta_telegram=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                try
                {
                    try
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));

                        SqlDataReader selectResult = selectCommand.ExecuteReader();
                        while (selectResult.Read())
                        {
                            // Console.WriteLine(selectResult.GetString(0));
                            if (Cuenta == selectResult.GetString(0))
                            {

                                string retornable = selectResult.GetString(1);
                                CloseConnection();
                                return retornable;

                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("");
                        return "";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("casta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getCuenta" + e.Message);
            }
            CloseConnection();
            string aus = "";
            return aus;
        }

        public string GetMail(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from Cuentas where Nocuenta=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();

                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        string retornable = selectResult.GetString(1);
                        CloseConnection();
                        return retornable;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            string aus = "";
            return aus;
        }

        public string GetMailDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT codigo_alumno,correo_electronico from alumnos where codigo_alumno=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                try {
                    
                    selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        string retornable = selectResult.GetString(1);
                        CloseConnection();
                        return retornable;

                    }
                }
            
                } catch (Exception e) {
                    Console.WriteLine("getmailconvert "+e.Message);
                    return " ";
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("get mail"+e.Message);
            }
            CloseConnection();
            string aus = "";
            return aus;
        }

        public bool CuentaExiste(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT * from Cuentas where Nocuenta=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();

                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        string retornable = selectResult.GetString(1);
                        CloseConnection();
                        return true;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return false;
        }

        public bool CuentaExisteDb(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT codigo_alumno from alumnos where codigo_alumno=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                try
                {
                    
                    selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                    SqlDataReader selectResult = selectCommand.ExecuteReader();
                    while (selectResult.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == selectResult.GetString(0))
                        {
                            CloseConnection();
                            return true;

                        }
                    }
                }
                catch (Exception e) {
                    return false;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("cuenta existe"+e.Message);
            }
            CloseConnection();
            return false;
        }


        public string createToken()
        {
            Random generator = new Random();
            string random = "";
            bool ing = true;
            try
            {
                while (ing == true)
                {
                    ing = false;
                    random = generator.Next(0, 999999).ToString("D6");
                    string selectQuery = "SELECT token_generado from alumnos_bot ";
                    using var selectCommand = new SqlCommand(selectQuery, myConnection);
                    OpenConnection();


                    SqlDataReader selectResult = selectCommand.ExecuteReader();
                    while (selectResult.Read())
                    {
                        if (selectResult.GetString(0) == random)
                        {
                            ing = true;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return random;
        }

        public bool newToken(string Cuenta) {


            string selectQuery = "SELECT * from DataBot where telegramid=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();

                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        DateTime actual = selectResult.GetDateTime(7);
                        actual = actual.AddMinutes(5);
                        DateTime nos = DateTime.Now;
                        int compare = DateTime.Compare(actual, nos);
                        // Console.WriteLine(nos);
                        if (DateTime.Compare(actual, nos) <= 0)
                        {


                            string mail = selectResult.GetString(2);
                            Console.WriteLine(mail);
                            CloseConnection();
                            string tokenNuevo = createToken();
                            EnviarCorreo(mail, tokenNuevo);

                            sqlserver db = new sqlserver();

                            string updateQuery = "UPDATE DataBot set token=@token,Fecha_UltimoToken=@nos WHERE telegramid=@Cuenta";
                            SqlCommand updateCommand = new SqlCommand(updateQuery, db.myConnection);
                            db.OpenConnection();
                            updateCommand.Parameters.AddWithValue("@Cuenta", Cuenta);
                            updateCommand.Parameters.AddWithValue("@nos", nos);
                            updateCommand.Parameters.AddWithValue("@token", tokenNuevo);

                            var updateResult = updateCommand.ExecuteNonQuery();
                            db.CloseConnection();

                            return true;
                        }



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return false;
        }

        public bool newTokenDb(string Cuenta,string mail)
        {

            //Console.WriteLine(Cuenta);
            string selectQuery = "SELECT cuenta_telegram,fecha_ultimo_token from alumnos_bot where cuenta_telegram=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();

                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        DateTime actual = selectResult.GetDateTime(1);
                        actual = actual.AddMinutes(5);
                        DateTime nos = DateTime.Now;
                        int compare = DateTime.Compare(actual, nos);
                        // Console.WriteLine(nos);
                        if (DateTime.Compare(actual, nos) <= 0)
                        {

                           
                           
                            Console.WriteLine(mail);
                            CloseConnection();
                            string tokenNuevo = createToken();
                            EnviarCorreo(mail, tokenNuevo);

                            sqlserver db = new sqlserver();

                            string updateQuery = "UPDATE alumnos_bot set token_generado=@token,fecha_ultimo_token=@nos WHERE cuenta_telegram=@Cuenta";
                            SqlCommand updateCommand = new SqlCommand(updateQuery, db.myConnection);
                            db.OpenConnection();
                            updateCommand.Parameters.AddWithValue("@Cuenta", Cuenta);
                            updateCommand.Parameters.AddWithValue("@nos", nos);
                            updateCommand.Parameters.AddWithValue("@token", tokenNuevo);

                            var updateResult = updateCommand.ExecuteNonQuery();
                            db.CloseConnection();

                            return true;
                        }



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("new token"+e.Message);
            }


            return false;
        }


        public string ConverMail(string mail)
        {
            var converted = new char[mail.Length];

            int posarroba = 0;

            for (int i = 0; i < mail.Length; i++)
            {

                converted[i] = mail[i];
                if (mail[i] == '@')
                {
                    posarroba = i;
                }

            }

            if (posarroba == 2)
            {

                converted[1] = '*';

            }
            else
            {

                for (int i = 0; i < mail.Length; i++)
                {
                    if (i > 0 && i < posarroba)
                    {
                        converted[i] = '*';
                    }

                }
            }

            mail = new string(converted);



            return mail;
        }


        public bool EnviarCorreo(string destinatario, string codigo)
        {
            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.To.Add(destinatario);
            correo.Subject = "Codigo de verificación Bot Unitec";
            correo.SubjectEncoding = System.Text.Encoding.UTF8;
            //Enviar una copia del correo a otro destinatario
            //correo.Bcc.Add("midencefranco2@gmail.com");
            correo.Body = "Codigo de Verficacion Bot Unitec:\n" + codigo;
            correo.BodyEncoding = System.Text.Encoding.UTF8;
            correo.IsBodyHtml = true;
            correo.From = new System.Net.Mail.MailAddress("midencefranco@gmail.com");

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.UseDefaultCredentials = true;//qwerty456123  vinculacionunitecsps@gmail.com
            cliente.Credentials = new System.Net.NetworkCredential("botvinculacionunitec@gmail.com", "qwerty456123");
            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.Host = "smtp.gmail.com"; // mail.tudominio.com

            try
            {
                cliente.Send(correo);
                Console.WriteLine("Se envio el correo :)");
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine("Error al enviar el correo :(" + e.Message);
                return false;
            }

        }

        public void insertar(string telegramid, string noCuenta, string mail)
        {

            //Console.WriteLine(telegramid);
            // Console.WriteLine(noCuenta);
            //Console.WriteLine(mail);
            string token = createToken();
            int verified = 0;
            string today = "";
            int Estado = 1;
            DateTime exacto = DateTime.Now;
            // Console.WriteLine(token);
            // Console.WriteLine(verified);
            // Console.WriteLine(today);
            //Console.WriteLine(Estado);
            //Console.WriteLine(exacto);
            EnviarCorreo(mail, token);

            string insertquery = "insert into DataBot values(@telegramid,@nocuenta,@mail,@token,@verified,@today,@estado,@exacto)";
            try
            {
                using var cmd = new SqlCommand(insertquery, myConnection);
                OpenConnection();
                cmd.Parameters.Add(new SqlParameter("@telegramid", telegramid));
                cmd.Parameters.Add(new SqlParameter("@nocuenta", noCuenta));
                cmd.Parameters.Add(new SqlParameter("@mail", mail));
                cmd.Parameters.Add(new SqlParameter("@token", token));
                cmd.Parameters.Add(new SqlParameter("@verified", verified));
                cmd.Parameters.Add(new SqlParameter("@today", today));
                cmd.Parameters.Add(new SqlParameter("@estado", Estado));
                cmd.Parameters.Add(new SqlParameter("@exacto", exacto));
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void insertarDb(string telegramid, string noCuenta, string mail)
        {

            //Console.WriteLine(telegramid);
            // Console.WriteLine(noCuenta);
            //Console.WriteLine(mail);
            string token = createToken();
            int verified = 0;
            string today = "";
            int Estado = 1;
            DateTime exacto = DateTime.Now;
            DateTime confirmacion = DateTime.Now;
            // Console.WriteLine(token);
            // Console.WriteLine(verified);
            // Console.WriteLine(today);
            //Console.WriteLine(Estado);
            //Console.WriteLine(exacto);
            EnviarCorreo(mail, token);
            int id_alumno = Getid(noCuenta);
            bool prueba = true;
            string insertquery = "insert into alumnos_bot (deshabilitado,cuenta_telegram,token_generado,confirmado,fecha_confirmacion,estado,fecha_ultimo_token,id_alumno) values(@prueba,@telegramid,@token,@verified,@confirmacion,@estado,@today,@idalumno)";
            try
            {
                using var cmd = new SqlCommand(insertquery, myConnection);
                OpenConnection();
                cmd.Parameters.Add(new SqlParameter("@telegramid", telegramid));
                cmd.Parameters.Add(new SqlParameter("@nocuenta", noCuenta));
                cmd.Parameters.Add(new SqlParameter("@prueba", prueba));

                cmd.Parameters.Add(new SqlParameter("@token", token));
                cmd.Parameters.Add(new SqlParameter("@verified", verified));
                cmd.Parameters.Add(new SqlParameter("@today", today));
                cmd.Parameters.Add(new SqlParameter("@estado", Estado));
                cmd.Parameters.Add(new SqlParameter("@idalumno", id_alumno));
                cmd.Parameters.Add(new SqlParameter("@confirmacion", confirmacion));
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("insertar"+e.Message);
            }

        }

        public int Getid(string Cuenta)
        {

            /*
            SELECT from Database
            */
            string selectQuery = "SELECT codigo_alumno,id from alumnos where codigo_alumno=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();
                try
                {
                    
                    selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                    SqlDataReader selectResult = selectCommand.ExecuteReader();
                    while (selectResult.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == selectResult.GetString(0))
                        {
                            int retornable = selectResult.GetInt32(1);
                            CloseConnection();
                            return retornable;

                        }
                    }
                }
                catch (Exception e) {
                    Console.WriteLine("getidcaste"+e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getid"+e.Message);
            }
            CloseConnection();
            string aus = "";
            return -1;
        }


        public string getTotalHoras(string Cuenta)
        {

            string numero = "";
            string selectQuery = "SELECT * from horas where cuenta=@Cuenta";
            try
            {
                using var selectCommand = new SqlCommand(selectQuery, myConnection);
                OpenConnection();

                selectCommand.Parameters.Add(new SqlParameter("@Cuenta", Cuenta));
                SqlDataReader selectResult = selectCommand.ExecuteReader();
                while (selectResult.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == selectResult.GetString(0))
                    {
                        int retornable = selectResult.GetInt32(1);
                        CloseConnection();
                        return retornable.ToString();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();




            return numero;
        }

        public string HorasDetalle(string nCuenta)
        {
            string detalles = "";
            string lectura;
            string cadena_com = "SELECT * FROM detalleHoras";
            try
            {
                SqlCommand com = new SqlCommand(cadena_com, myConnection);

                OpenConnection();
                SqlDataReader leer = com.ExecuteReader();
                while (leer.Read())
                {

                    //PERIODO DE PROYECTO
                    detalles += "Periodo: " + leer.GetString(0) + "\n";

                    //Extraccion de Campo Nombre Clase TABLA DATOS CLASE

                    detalles += "Nombre Clase: " + leer.GetString(1) + "\n";
                    //Extraccion de Nombre Proyecto TABLA DATOS PROYECTOS


                    detalles += "Nombre Proyecto: " + leer.GetString(2) + "\n";
                    //HORAS DE PROYECTO 
                    detalles += "Horas de Proyecto: " + leer.GetString(3) + "\n\n";

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CloseConnection();
            return detalles;
        }

        public string HorasTotales(string nCuenta)
        {
            string cadena_com = "SELECT horas_trabajadas FROM horas_proyectos inner join alumnos on horas_proyectos.id_alumno=alumnos.id where alumnos.codigo_alumno=@Cuenta";
            try
            {
               // Console.WriteLine("nUMEROcUENTA " + nCuenta);
                int NumeroTotalHoras = 0;
                SqlCommand com = new SqlCommand(cadena_com, myConnection);
                OpenConnection();
                try {
                    
                    com.Parameters.Add(new SqlParameter("@Cuenta", nCuenta));
                    SqlDataReader leer = com.ExecuteReader();

                    while (leer.Read())
                    {

                        NumeroTotalHoras += leer.GetInt32(0);

                    }
                    CloseConnection();

                    return NumeroTotalHoras.ToString();
                }
                catch (Exception e) {
                    Console.WriteLine("cast horas tot");
                    return " ";
                }
        }catch(Exception e){
            Console.WriteLine("horas totales"+e.Message);
                return " ";
                }
        
        }

        public string HorasDetalle2(string nCuenta)
        {
            string detalles = "";
            string lectura;
            string cadena_com = "SELECT codigo_proyecto,nombre_proyecto,horas_trabajadas FROM horas_proyectos inner join alumnos on horas_proyectos.id_alumno=alumnos.id inner join proyectos on horas_proyectos.id_proyecto = proyectos.id where alumnos.codigo_alumno=@Cuenta" +
                "";
            try
            {
                try
                {
                    SqlCommand com = new SqlCommand(cadena_com, myConnection);

                    OpenConnection();
                    
                    com.Parameters.Add(new SqlParameter("@Cuenta", nCuenta));
                    SqlDataReader leer = com.ExecuteReader();
                    while (leer.Read())
                    {

                        //PERIODO DE PROYECTO
                        //detalles += "Periodo: " + leer.GetString(0) + "\n";

                        //Extraccion de Campo Nombre Clase TABLA DATOS CLASE

                        //detalles += "Nombre Clase: " + leer.GetString(1) + "\n";
                        //Extraccion de Nombre Proyecto TABLA DATOS PROYECTOS


                        detalles += "Codigo Proyecto: " + leer.GetString(0) + "\n";
                        detalles += "Nombre de Proyecto: " + leer.GetString(1) + "\n";
                        //HORAS DE PROYECTO 
                        detalles += "Horas Trabajadas:" + leer.GetInt32(2).ToString() + "\n\n";

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("detalle" + e.Message);
                }
                CloseConnection();
                return detalles;
            }
            catch (Exception e) {
                Console.WriteLine("castmalodetalle");
                return detalles;
            }
        }



    }





}
