using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data.Odbc;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Timers;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace BotVinculacionUnitec
{
   
    
    class AccesDB
    {

        

            
        
        

        OdbcConnection odbcConnection;
        OdbcCommand cmd;
  
        public void iniciarTimer()
        {
            CrearCacheHorasTotales();
            //Console.WriteLine("Tiempo corriendo...");
            Timer t = new Timer(TimeSpan.FromDays(1).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(reCrearCacheHorasTotales);
            t.Start();
        }
        System.Runtime.Caching.MemoryCache cache = new System.Runtime.Caching.MemoryCache("HorasTotalesCache");
        public void CrearCacheHorasTotales()
        {

            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            OdbcConnection con = new OdbcConnection(@Config.GetBotConnection());
            string queryString = "SELECT No_Cuenta, sum(Horas_Acum) FROM [Tabla General] Group By No_Cuenta;";
            OdbcCommand command = new OdbcCommand(queryString, con);
            con.Open();
            OdbcDataReader reader = command.ExecuteReader();
            //Console.WriteLine(reader.FieldCount);
            while (reader.Read())
            {
                var cacheIt = new CacheItem(reader.GetString(0), reader.GetString(1));
                cache.Add(cacheIt, cacheItemPolicy);
            }
            reader.Close();
            var chek = cache.Get("21841180");
            //Console.WriteLine(chek);
            /*foreach (var item in cache)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }*/
            Console.WriteLine("Caché creado con éxito!");
        }
        
        public void reCrearCacheHorasTotales(object sender, ElapsedEventArgs e)
        {
            
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            OdbcConnection con = new OdbcConnection(Config.GetBotConnection());
            string queryString = "SELECT No_Cuenta, sum(Horas_Acum) FROM [Tabla General] Group By No_Cuenta;";
            OdbcCommand command = new OdbcCommand(queryString, con);
            con.Open();
            OdbcDataReader reader = command.ExecuteReader();
            //Console.WriteLine(reader.FieldCount);
            while (reader.Read())
            {
                var cacheIt = new CacheItem(reader.GetString(0), reader.GetString(1));
                cache.Add(cacheIt, cacheItemPolicy);
            }
            reader.Close();
            /*foreach (var item in cache)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }*/
        }
        public void connection()
        {
            //Colocar el path donde tengan la base de datos 
            //En un futuro se cambiara de manera dinamica, no se como pero simon
            //string path = @"C:\Users\Johel\Downloads\";
            //var file = Directory.GetFiles(@path, "BASE DATOS MODIFICADA 29 Ene.accdb").FirstOrDefault();
            //if (File.Exists(file))
            //{
                //string connetionString = null;
                //connetionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=" + file;
                odbcConnection = new OdbcConnection(Config.GetBotConnection());
            //}
            //else
            //{ 
            //    Console.WriteLine("file not found");
           // }
        }

        public void Open()
        {
            try
            {
                
                odbcConnection.Open();

            }catch(OdbcException e)
            {
                Logger.Log(e.Message, LogType.Error);
                Console.WriteLine(e.Message);
            }
        }

        public void Close()
        {
         
            try
            {
                odbcConnection.Close();
            }
            catch (OdbcException e)
            {
                Logger.Log(e.Message, LogType.Error);
                Console.WriteLine(e.Message);
            }
            
        }

        public bool CuentaExiste(string Cuenta)
        {
            string selectQuery = "SELECT * from [Datos Alumno] where No_Cuenta = ? ;";
            try
            {  
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                Open();
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    if (Cuenta == MyDataReader.GetString(0))
                    {
                        string retornable = MyDataReader.GetString(1);
                        Close();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, LogType.Error);
                Console.WriteLine(e.Message);
            }
            Close();
            return false;
        }

        public bool ExisteDb(string Cuenta)
        {
            string selectQuery = "SELECT cuenta_telegram from [Datos Alumno Bot] where cuenta_telegram = ? ;";
            cmd = new OdbcCommand(selectQuery, odbcConnection);
            Open();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();

                while (MyDataReader.Read())
                {
                    if (Cuenta == MyDataReader.GetString(0))
                    {
                        Close();
                        return true;
                    }
                }
            }
            catch (OdbcException e)
            {
                Logger.Log("database " + e.Message, LogType.Error);    
            }
            catch (Exception e)
            {
                Logger.Log("Existe " + e.Message, LogType.Warn);
                Console.WriteLine("Existe " + e.Message);
            }
          
            Close();
            return false;
        }

        public bool estadoDb(string Cuenta)
        {

            string selectQuery = "SELECT estado from [Datos Alumno Bot] where cuenta_telegram = ? ;";
            cmd = new OdbcCommand(selectQuery, odbcConnection);
            Open();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    int num = MyDataReader.GetInt32(0);
                    // Console.WriteLine(num);
                    if (num == 2)
                    {
                        Close();
                        return true;
                    }
                   
                }
            }
            catch (Exception e)
            {
                Logger.Log("estado " + e.Message, LogType.Warn);
                Console.WriteLine("estado " + e.Message);
            }
            Close();
            return false;
        }

        public string GetCuentaNUMDb(string Cuenta)
        {

            string selectQuery = "SELECT cuenta_telegram,[Datos Alumno].No_Cuenta from [Datos Alumno] inner join [Datos Alumno Bot] on [Datos Alumno].No_Cuenta=[Datos Alumno Bot].No_Cuenta where cuenta_telegram= ? ;";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    try
                    {
                        cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                        OdbcDataReader MyDataReader = cmd.ExecuteReader();
                        while (MyDataReader.Read())
                        {
                            // Console.WriteLine(selectResult.GetString(0));
                            if (Cuenta == MyDataReader.GetString(0))
                            {

                                string retornable = MyDataReader.GetString(1);
                                Close();
                                return retornable;

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e.Message, LogType.Error);
                        Console.WriteLine("");
                        return "";
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("esta malo get cuenta", LogType.Error);
                    Console.WriteLine("esta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Logger.Log("getCuenta" + e.Message, LogType.Error);
                Console.WriteLine("getCuenta" + e.Message);
            }
            Close();
            string aus = "";
            return aus;
        }

        public string GetCuentaDb(string Cuenta)
        {
            string selectQuery = "SELECT cuenta_telegram, P_Nombre, [P_ Apellido] FROM [Datos Alumno] inner join [Datos Alumno Bot] on [Datos Alumno].No_Cuenta=[Datos Alumno Bot].No_Cuenta WHERE cuenta_telegram= ?";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {

                            string retornable = MyDataReader.GetString(1) + " " + MyDataReader.GetString(2);
                            Close();
                            return retornable;

                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("esta malo get cuenta", LogType.Error);
                    Console.WriteLine("casta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Logger.Log("getCuenta" + e.Message, LogType.Error);
                Console.WriteLine("getCuenta" + e.Message);
            }
            Close();
            string aus = "";
            return aus;
        }


        public bool VerificarDb(string Cuenta, string code)
        {
            string selectQuery = "SELECT token_generado from [Datos Alumno Bot] where cuenta_telegram = ? ";
            cmd = new OdbcCommand(selectQuery, odbcConnection);
            Open();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                   
                    if (code == MyDataReader.GetString(0))
                    {
                        Close();
                        return true;
                    }
                   
                }
            }
            catch (Exception e)
            {
                Logger.Log("verificar" + e.Message, LogType.Error);
                Console.WriteLine("verificar " + e.Message);
            }
            Close();
            return false;
        }

        public bool VerificarUpdateDb(string Cuenta, string code)
        {
            DateTime now = DateTime.Now;
            //string updatequery = "Update alumnos_bot set Fecha_confirmacion=@now ,confirmado=1 ,estado=2,token_generado=@unique where token_generado=@code and cuenta_telegram=@Cuenta";
            string updatequery = "UPDATE [Datos Alumno Bot] set Fecha_confirmacion='"+ now + "', confirmado=1, Estado=2, Token_generado='"+" "+"' WHERE Token_generado='"+code+"' and cuenta_telegram='"+Cuenta+"'";
            try
            {
                cmd = new OdbcCommand(updatequery, odbcConnection);
                Open();
                cmd.ExecuteNonQuery();
                Close();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("XD" + e.Message, LogType.Error);
                Console.WriteLine("Cgaada Tio" + e.Message);
                return false;
            }


        }

        public string GetMailDb(string Cuenta)
        {

            string selectQuery = "SELECT No_Cuenta,correo_electronico from [Datos Alumno Bot] where No_Cuenta= ? ";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
         
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {
                            string retornable = MyDataReader.GetString(1);
                            Close();
                            return retornable;

                        }
                    }

                }
                catch (Exception e)
                {
                    Logger.Log("getMail Convert" + e.Message, LogType.Warn);
                    Console.WriteLine("getmailconvert " + e.Message);
                    return " ";
                }

            }
            catch (Exception e)
            {
                Logger.Log("get mail" + e.Message, LogType.Warn);
                Console.WriteLine("get mail" + e.Message);
            }
            Close();
            string aus = "";
            return aus;
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


        public bool newTokenDb(string Cuenta, string mail)
        {
            string selectQuery = "SELECT cuenta_telegram,fecha_ultimo_token from [Datos Alumno Bot] where cuenta_telegram= ? ;";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();

                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();

                while (MyDataReader.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                    if (Cuenta == MyDataReader.GetString(0))
                    {
                        DateTime actual = MyDataReader.GetDateTime(1);
                        actual = actual.AddMinutes(5);
                        DateTime nos = DateTime.Now;
                        int compare = DateTime.Compare(actual, nos);
                        // Console.WriteLine(nos);
                        if (DateTime.Compare(actual, nos) <= 0)
                        {



                            Console.WriteLine(mail);
                            Close();
                            string tokenNuevo = createToken();
                            EnviarCorreo(mail, tokenNuevo);

                            AccesDB db = new AccesDB();
                            db.connection();
                            //string updateQuery = "UPDATE alumnos_bot set token_generado=@token,fecha_ultimo_token=@nos WHERE cuenta_telegram=@Cuenta";
                            string updateQuery = "UPDATE [Datos Alumno Bot] set token_generado='"+tokenNuevo+"',fecha_ultimo_token='"+nos+"' WHERE cuenta_telegram= ?";
                            OdbcCommand updateCommand = new OdbcCommand(updateQuery, db.odbcConnection);
                            db.Open();

                            updateCommand.Parameters.Add("@Cuenta", OdbcType.Text).Value = Cuenta;
                            updateCommand.ExecuteNonQuery();
                            db.Close();

                            return true;
                        }



                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error generando token" + e.Message, LogType.Warn);
                Console.WriteLine("new token" + e.Message);
            }


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
                    string selectQuery = "SELECT token_generado from [Datos Alumno Bot] ";
                    cmd = new OdbcCommand(selectQuery, odbcConnection);
                    Open();

                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        if (MyDataReader.GetString(0) == random)
                        {
                            ing = true;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Logger.Log("Error generando token" + e.Message, LogType.Warn);
                Console.WriteLine(e.Message);
            }
            Close();
            return random;
        }

        public bool EnviarCorreo(string destinatario, string codigo)
        {
            var fromAddress = new MailAddress("botvinculacionunitec@gmail.com", "Vinculacion Unitec");
            var toAddress = new MailAddress(destinatario, "");
            const string fromPassword = "qwerty456123";
            string subject = "Codigo de verificación Bot Unitec";
            string body = "<h2>Codigo de Verficacion Bot Unitec:<h2>\n <h1>" + codigo + "<h1>";

            var cliente = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)

            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            try
            {
                cliente.Send(message);
                Console.WriteLine("Se envio el correo :)");
                Logger.Log("correo enviado a :" + destinatario, LogType.Info);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Error al enviar correo a :" + destinatario + e.Message, LogType.Error);
                Console.WriteLine("Error al enviar el correo :(" + e.Message);
                return false;
            }

        }

        public bool CuentaVerificadaDb(string numeroCuenta)
        { 
            string selectQuery = "SELECT * FROM [Datos Alumno] inner join [Datos Alumno Bot] on [Datos Alumno].No_Cuenta=[Datos Alumno Bot].No_Cuenta WHERE [Datos Alumno].No_Cuenta= ? and [Datos Alumno Bot].Confirmado=1";

            cmd = new OdbcCommand(selectQuery, odbcConnection);
            Open();
            cmd.Parameters.Add("@NumeroCuenta", OdbcType.VarChar).Value = numeroCuenta;
            
            try
            {
                OdbcDataReader MyDataReader = cmd.ExecuteReader();

                if (MyDataReader.HasRows)
                {
                    Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Log("Cuenta verificar" + e.Message, LogType.Error);
                Console.WriteLine("CuentaVerificar" + e.Message);
            }
            Close();
            return false;
        }

        public void insertarDb(string telegramid, string noCuenta, string mail)
        {
            string token = createToken();
            int verified = 0;
            int Estado = 1;
            DateTime today = DateTime.Now;
            DateTime confirmacion = DateTime.Now;
          
            EnviarCorreo(mail, token);
            int id_alumno = Getid(noCuenta);
            bool prueba = true;
            //string insertquery = "INSERT into [Datos Alumno Bot] (deshabilitado,cuenta_telegram,token_generado,confirmado,fecha_confirmacion,estado,fecha_ultimo_token,id) values ('"+ prueba+ "','"+ telegramid+"','"+ token + "',"+ verified + ",'"+confirmacion+"',"+ Estado + ",'"+today+"','"+ id_alumno +"');";
            string updatequery = "UPDATE [Datos Alumno Bot] set deshabilitado='" + prueba + "', cuenta_telegram= '"+ telegramid + "', Token_generado='" + token + "', confirmado = "+verified+", fecha_confirmacion = '"+ confirmacion + "', estado = "+ Estado + ", fecha_ultimo_token = '"+today+"'  WHERE No_cuenta= ? ;";
            try
            {
                cmd = new OdbcCommand(updatequery, odbcConnection);
                Open();
                
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = noCuenta;
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception e)
            {
                Logger.Log("Error insertando datos" + e.Message, LogType.Error);
                Console.WriteLine("insertar" + e.Message);
            }

        }

        public int Getid(string Cuenta)
        {
            string selectQuery = "SELECT No_Cuenta from [Datos Alumno] where No_Cuenta= ? ";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {
                            int retornable = MyDataReader.GetInt32(1);
                            Close();
                            return retornable;

                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("Get id " + e.Message, LogType.Error);
                    Console.WriteLine("getidcaste" + e.Message);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Erro get id" + e.Message, LogType.Error);
                Console.WriteLine("getid" + e.Message);
            }
            Close();
            string aus = "";
            return -1;
        }

        public string HorasTotales(string nCuenta)
        {
            try
            {
                var checkCache = cache.Contains(nCuenta);
                if (checkCache)
                {
                    var value = cache.Get(nCuenta);
                    double val = double.Parse(value.ToString());
                    int v = Convert.ToInt32(val);
                    return v.ToString();
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error Horas totales" + e.Message, LogType.Error);
                Console.WriteLine("cast horas tot");
                Close();
                //return " ";
            }
            string selectQuery = "SELECT sum(Horas_Acum) as Horas_Totales FROM [Tabla General] where No_Cuenta = ? ";
            try
            {
                // Console.WriteLine("nUMEROcUENTA " + nCuenta);
                int NumeroTotalHoras = 0;
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = nCuenta;
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        NumeroTotalHoras = MyDataReader.GetInt32(0);
                    }
                    Close();
                    return NumeroTotalHoras.ToString();
                }
                catch (Exception e)
                {
                    Logger.Log("Error Horas totales" + e.Message, LogType.Error);
                    Console.WriteLine("cast horas tot");
                    Close();
                    return " ";
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error Horas totales" + e.Message, LogType.Error);
                Close();
                Console.WriteLine("horas totales" + e.Message);
                return " ";
            }

        }

        public string HorasDetalle2(string nCuenta)
        {
            string detalles = "";
            string selectQuery = "SELECT id_proyecto,Periodo,Beneficiario,Horas_Acum FROM [Tabla General] where No_Cuenta = ? ";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();

                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = nCuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    detalles += "Nombre de Proyecto: " + MyDataReader.GetString(0) + "\n";
                    detalles += "Periodo: " + MyDataReader.GetString(1) + "\n";
                    detalles += "Beneficiaro: " + MyDataReader.GetString(2) + "\n";
                    //HORAS DE PROYECTO 
                    detalles += "Horas Trabajadas:" + MyDataReader.GetInt32(3).ToString() + "\n\n";

                }
            }
            catch (Exception e)
            {
                Logger.Log("Error detalles" + e.Message, LogType.Error);
                Console.WriteLine("detalle" + e.Message);
            }
            Close();
            return detalles;

        }

        public bool CuentaExisteDb(string Cuenta)
        {

            string selectQuery = "SELECT No_Cuenta from [Datos Alumno] where No_Cuenta= ? ;";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {

                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {
                            Close();
                            return true;

                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("cuenta existe" + e.Message);
            }
            Close();
            return false;
        }




    }
}

