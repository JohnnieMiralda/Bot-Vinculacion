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
        OdbcConnection odbcConnectionBotOnly;
        OdbcCommand cmdBotOnly;

        
        public void iniciarTimer()
        {
            CrearCacheHorasTotales();
            //Console.WriteLine("Tiempo corriendo...");
            Timer t = new Timer(TimeSpan.FromDays(1).TotalMilliseconds);
            Logger.Log("Se inicio el timer a las " + t, LogType.Debug);
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
            //Console.WriteLine("Caché creado con éxito!");
            Logger.Log("Caché creado con éxito!", LogType.Debug);


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
                odbcConnection = new OdbcConnection(Config.GetBotConnection());
                Logger.Log("Conexion Vinculacion_Base creada con éxito!", LogType.Info);
        }
        public void connectionBotOnly()
        {
            odbcConnectionBotOnly = new OdbcConnection(Config.GetBotConnectionOnly());
            Logger.Log("Conexion Bot_Base creada con éxito!", LogType.Info);
        }

        public void Open()
        {
            try
            {                
                odbcConnection.Open();
                Logger.Log("Conexion Vinculacion_Base abierta con éxito!", LogType.Debug);
            }
            catch(OdbcException e)
            {
                Logger.Log("Error a abrir conexion en Vinculacion_base "+e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
            }
        }

        public void Close()
        {
            try
            {
                odbcConnection.Close();
                Logger.Log("Conexion Vinculacion_Base cerrada con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a cerrar conexion en Vinculacion_base " + e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
            }
            
        }

        public void OpenBotOnly()
        {
            try
            {
                odbcConnectionBotOnly.Open();
                Logger.Log("Conexion Bot_Base abierta con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a abrir conexion en Bot_Base " + e.Message, LogType.Error);
                Console.WriteLine(e.Message);
            }
        }

        public void CloseBotOnly()
        {
            try
            {
                odbcConnectionBotOnly.Close();
                Logger.Log("Conexion Bot_Base cerrada con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a cerrar conexion en Bot_base " + e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
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
                        Logger.Log("Se encontro cuenta en CuentaExiste " + retornable, LogType.Debug);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error en CunetaExiste "+e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
            }
            Close();
            return false;
        }

        public bool ExisteDb(string Cuenta)
        {
            string selectQuery = "SELECT CuentaTelegram from [AlumnosBot] where CuentaTelegram = ? ;";
            cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            OpenBotOnly();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();

                while (MyDataReader.Read())
                {
                    if (Cuenta == MyDataReader.GetString(0))
                    {
                        Logger.Log("Se encontro cuenta en ExisteDb " + MyDataReader.GetString(0), LogType.Debug);
                        CloseBotOnly();
                        return true;
                    }
                }
                CloseBotOnly();
            }
            catch (OdbcException e)
            {
                Logger.Log("Error en database ExisteDb " + e.Message, LogType.Error);    
            }
            catch (Exception e)
            {
                Logger.Log("Existe en database ExisteDb " + e.Message, LogType.Error);
                //Console.WriteLine("Existe " + e.Message);
            }
          
            CloseBotOnly();
            return false;
        }

        public bool estadoDb(string Cuenta)
        {
            string selectQuery = "SELECT Estado from [AlumnosBot] where CuentaTelegram = ? ;";
            cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            OpenBotOnly();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    int num = MyDataReader.GetInt32(0);
                    if (num == 2)
                    {
                        CloseBotOnly();
                        Logger.Log("Retorno true en estadoDb ", LogType.Debug);
                        return true;
                    }
                   
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error en estadoDb " + e.Message, LogType.Error);
                ////Console.WriteLine("estado " + e.Message);
            }
            CloseBotOnly();
            Logger.Log("Retorno false en estadoDb ", LogType.Debug);
            return false;
        }

        public string GetCuentaNUMDb(string Cuenta)
        {
            string selectQuery = "SELECT [Datos Alumno].No_Cuenta from [Datos Alumno]  ; ";
            string selectQuery2 = "SELECT CuentaTelegram, NumeroCuenta from [AlumnosBot] WHERE CuentaTelegram = ? ; ";
            string numeroCuenta = "";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmdBotOnly = new OdbcCommand(selectQuery2, odbcConnectionBotOnly);
                try
                {
                    OpenBotOnly();
                    try
                    {
                        cmdBotOnly.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                        OdbcDataReader MyDataReader = cmdBotOnly.ExecuteReader();
                        while (MyDataReader.Read())
                        {
                            // Console.WriteLine(selectResult.GetString(0));
                            if (Cuenta == MyDataReader.GetString(0))
                            {
                                numeroCuenta = MyDataReader.GetString(1);
                            }
                        }
                        CloseBotOnly();
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e.Message, LogType.Error);
                        Console.WriteLine("");
                        CloseBotOnly();
                        return "";
                    }
                    Open();
                    try
                    {
                        OdbcDataReader MyDataReader = cmd.ExecuteReader();

                        while (MyDataReader.Read())
                        {
                            // Console.WriteLine(selectResult.GetString(0));
                            if (MyDataReader.GetString(0) == numeroCuenta)
                            {
                                //cuentaUnitec = MyDataReader.GetString(1);
                                string retornable = MyDataReader.GetString(0);
                                Close();
                                return retornable;
                            }
                        }
                        Close();
                    }
                    catch (Exception e)
                    {
                        Close();
                        Logger.Log(e.Message, LogType.Error);
                        return "";
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("esta malo get cuenta", LogType.Error);
                    //Console.WriteLine("esta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Logger.Log("getCuenta" + e.Message, LogType.Error);
                //Console.WriteLine("getCuenta" + e.Message);
            }
            Close();
            string aus = "";
            return aus;
        }

        public string GetCuentaDb(string Cuenta)
        {
            //string selectQuery = "SELECT cuenta_telegram, P_Nombre, [P_ Apellido] FROM [Datos Alumno] inner join [Datos Alumno Bot] on [Datos Alumno].No_Cuenta=[Datos Alumno Bot].No_Cuenta WHERE cuenta_telegram= ?";
            string selectQuery = "SELECT No_Cuenta, P_Nombre, [P_ Apellido] FROM [Datos Alumno];";
            string selectQueryBotOnly = "SELECT CuentaTelegram, NumeroCuenta FROM [AlumnosBot] WHERE CuentaTelegram = ? ;";
            string numeroCuenta = "";
            try
            {
                cmdBotOnly = new OdbcCommand(selectQueryBotOnly, odbcConnectionBotOnly);
                OpenBotOnly();
                try
                {
                    cmdBotOnly.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    OdbcDataReader MyDataReader = cmdBotOnly.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {

                            numeroCuenta = MyDataReader.GetString(1);
                            

                        }
                    }
                    CloseBotOnly();
                }
                catch (Exception e)
                {
                    Logger.Log("esta malo get cuenta", LogType.Error);
                    //Console.WriteLine("casta malo getcuenta");
                }
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                Open();
                try
                {
                    OdbcDataReader MyDataReader = cmd.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        // Console.WriteLine(selectResult.GetString(0));
                        if (numeroCuenta == MyDataReader.GetString(0))
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
                    //Console.WriteLine("casta malo getcuenta");
                }
            }
            catch (Exception e)
            {
                Logger.Log("getCuenta" + e.Message, LogType.Error);
                //Console.WriteLine("getCuenta" + e.Message);
            }
            Close();
            string aus = "";
            return aus;
        }


        public bool VerificarDb(string Cuenta, string code)
        {
            string selectQuery = "SELECT TokenGenerado from [AlumnosBot] where CuentaTelegram = ? ;";
            cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            OpenBotOnly();
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                OdbcDataReader MyDataReader = cmd.ExecuteReader();
                while (MyDataReader.Read())
                {
                    // Console.WriteLine(selectResult.GetString(0));
                   
                    if (code == MyDataReader.GetString(0))
                    {
                        CloseBotOnly();
                        return true;
                    }
                   
                }
            }
            catch (Exception e)
            {
                Logger.Log("verificar" + e.Message, LogType.Error);
                //Console.WriteLine("verificar " + e.Message);
            }
            CloseBotOnly();
            return false;
        }

        public bool VerificarUpdateDb(string Cuenta, string code)
        {
            DateTime now = DateTime.Now;
            //string updatequery = "Update alumnos_bot set Fecha_confirmacion=@now ,confirmado=1 ,estado=2,token_generado=@unique where token_generado=@code and cuenta_telegram=@Cuenta";
            //string updatequery = "UPDATE [Datos Alumno Bot] set Fecha_confirmacion='"+ now + "', confirmado=1, Estado=2, Token_generado='"+" "+"' WHERE Token_generado='"+code+"' and cuenta_telegram='"+Cuenta+"'";
            string updatequery = "UPDATE AlumnosBot set FechaConfirmacion='" + now + "', Confirmado=1, Estado=2, TokenGenerado='" + " " + "' WHERE TokenGenerado='" + code + "' and CuentaTelegram='" + Cuenta + "'";
            try
            {
                cmd = new OdbcCommand(updatequery, odbcConnectionBotOnly);
                OpenBotOnly();
                cmd.ExecuteNonQuery();
                CloseBotOnly();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Error en verificarUpdateDb" + e.Message, LogType.Error);
                //Console.WriteLine("Cgaada Tio" + e.Message);
                return false;
            }


        }

        public string GetMailDb(string Cuenta)
        {

            string selectQuery = "SELECT NumeroCuenta,CorreoElectronico from [Alumnos] where NumeroCuenta= ? ;";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
                OpenBotOnly();
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
                            CloseBotOnly();
                            return retornable;
                        }
                    }

                }
                catch (Exception e)
                {
                    Logger.Log("getMail Convert" + e.Message, LogType.Warn);
                    //Console.WriteLine("getmailconvert " + e.Message);
                    return " ";
                }
            }
            catch (Exception e)
            {
                Logger.Log("get mail" + e.Message, LogType.Warn);
                //Console.WriteLine("get mail" + e.Message);
            }
            CloseBotOnly();
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
            string selectQuery = "SELECT CuentaTelegram,FechaUltimoToken from [AlumnosBot] where CuentaTelegram= ? ;";
            try
            {
                cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
                OpenBotOnly();

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
                        if (DateTime.Compare(actual, nos) <= 0)
                        {
                            //Console.WriteLine(mail);
                            CloseBotOnly();
                            string tokenNuevo = createToken();
                            EnviarCorreo(mail, tokenNuevo);
                            AccesDB db = new AccesDB();
                            db.connectionBotOnly();
                            //string updateQuery = "UPDATE alumnos_bot set token_generado=@token,fecha_ultimo_token=@nos WHERE cuenta_telegram=@Cuenta";
                            string updateQuery = "UPDATE AlumnosBot set TokenGenerado='" + tokenNuevo + "',FechaUltimoToken='" + nos + "' WHERE CuentaTelegram= ?";
                            OdbcCommand updateCommand = new OdbcCommand(updateQuery, db.odbcConnectionBotOnly);
                            db.OpenBotOnly();
                            updateCommand.Parameters.Add("@Cuenta", OdbcType.Text).Value = Cuenta;
                            updateCommand.ExecuteNonQuery();
                            db.CloseBotOnly();
                            return true;
                        }
                    }
                }
                CloseBotOnly();
            }
            catch (Exception e)
            {
                Logger.Log("Error generando token" + e.Message, LogType.Warn);
                //Console.WriteLine("new token" + e.Message);
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
                    string selectQuery = "SELECT TokenGenerado from [AlumnosBot] ;";
                    cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
                    OpenBotOnly();
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
                ////Console.WriteLine(e.Message);
            }
            CloseBotOnly();
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
                //Console.WriteLine("Se envio el correo :)");
                Logger.Log("correo enviado a :" + destinatario, LogType.Debug);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Error al enviar correo a :" + destinatario + e.Message, LogType.Error);
                //Console.WriteLine("Error al enviar el correo :(" + e.Message);
                return false;
            }
        }

        public bool CuentaVerificadaDb(string numeroCuenta)
        {
            //string selectQuery = "SELECT * FROM [Datos Alumno] inner join [Datos Alumno Bot] on [Datos Alumno].No_Cuenta=[Datos Alumno Bot].No_Cuenta WHERE [Datos Alumno].No_Cuenta= ? and [Datos Alumno Bot].Confirmado=1";
            string selectQuery = "SELECT * FROM [Datos Alumno] WHERE No_Cuenta = ? ;";
            string selectQueryBotOnly = "SELECT * FROM [AlumnosBot] WHERE NumeroCuenta = ? AND Confirmado=1 ;";
            string numCuenta="";
            cmd = new OdbcCommand(selectQuery, odbcConnection);
            Open();
            cmd.Parameters.Add("@NumeroCuenta", OdbcType.VarChar).Value = numeroCuenta;
            try
            {
                OdbcDataReader MyDataReader = cmd.ExecuteReader();

                while (MyDataReader.Read())
                {
                    if(numeroCuenta == MyDataReader.GetString(0))
                    {
                        numCuenta = MyDataReader.GetString(0);
                    }
                }
                Close();
            }
            catch (Exception e)
            {
                Logger.Log("Cuenta verificar" + e.Message, LogType.Error);
                //Console.WriteLine("CuentaVerificar" + e.Message);
            }
            cmdBotOnly = new OdbcCommand(selectQueryBotOnly, odbcConnectionBotOnly);
            OpenBotOnly();
            cmdBotOnly.Parameters.Add("@NumeroCuenta", OdbcType.VarChar).Value = numCuenta;
            try
            {
                OdbcDataReader MyDataReader = cmdBotOnly.ExecuteReader();

                if (MyDataReader.HasRows)
                {
                    CloseBotOnly();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Log("Cuenta verificar" + e.Message, LogType.Error);
                //Console.WriteLine("CuentaVerificar" + e.Message);
            }
            CloseBotOnly();
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
            string insertquery = "INSERT into [AlumnosBot] (NumeroCuenta,Deshabilitado,CuentaTelegram,TokenGenerado,Confirmado,FechaConfirmacion,Estado,FechaUltimoToken,ChatId) values ('" + noCuenta + "','" + prueba + "','" + telegramid + "','" + token + "'," + verified + ",'" + confirmacion + "'," + Estado + ",'" + today + "','" + id_alumno + "'); ";
            try
            {
                cmd = new OdbcCommand(insertquery, odbcConnectionBotOnly);
                OpenBotOnly();
                
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = noCuenta;
                cmd.ExecuteNonQuery();
                CloseBotOnly();
            }
            catch (Exception e)
            {
                Logger.Log("Error insertando datos" + e.Message, LogType.Error);
                //Console.WriteLine("insertar" + e.Message);
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
                        Console.WriteLine(MyDataReader.GetString(0));
                        if (Cuenta == MyDataReader.GetString(0))
                        {
                            int retornable = MyDataReader.GetInt32(0);
                            Close();
                            return retornable;                       
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("Get id " + e.Message, LogType.Error);
                    //Console.WriteLine("get id caste " + e.Message);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Erro get id " + e.Message, LogType.Error);
                //Console.WriteLine("get id " + e.Message);
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
                //Console.WriteLine("cast horas tot");
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
                    //Console.WriteLine("cast horas tot");
                    Close();
                    return " ";
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error Horas totales" + e.Message, LogType.Error);
                Close();
                //Console.WriteLine("horas totales" + e.Message);
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
                //Console.WriteLine("detalle" + e.Message);
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
                    Logger.Log("Error al recorrer query en CuentaExisteDb " + e.Message, LogType.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("cuenta existe" + e.Message);
                Logger.Log("Error al hacer el query en CuentaExisteDb " + e.Message, LogType.Error);
            }
            Close();
            return false;
        }
    }
}

