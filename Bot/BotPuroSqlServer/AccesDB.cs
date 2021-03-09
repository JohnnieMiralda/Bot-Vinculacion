using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data.Odbc;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Timers;
using System.Data;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace BotVinculacionUnitec
{
    class AccesDB
    {

        OdbcConnection odbcConnection = null;
        OdbcConnection odbcConnectionBotOnly = null;
        private readonly object l = new object();

        public AccesDB(){
            connection();
            connectionBotOnly();
        }

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

// ya esta
        public void CrearCacheHorasTotales()
        {
            try{
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            string queryString = "SELECT No_Cuenta, sum(Horas_Acum) AS horas FROM [Tabla General] Group By No_Cuenta;";
            var cmd = new OdbcCommand(queryString, odbcConnection);
            var datatable = GetDataTable(cmd);
            foreach (DataRow dr in datatable.Rows)
            {
                var cacheIt = new CacheItem(dr["No_Cuenta"].ToString(), dr["horas"].ToString());
                cache.Add(cacheIt, cacheItemPolicy);
            }
            Logger.Log("Caché creado con éxito!", LogType.Debug);
            }catch(Exception e){
                Logger.Log(e, LogType.Error);
            }
        }

// // ya esta
        public void reCrearCacheHorasTotales(object sender, ElapsedEventArgs e)
        {
            try
            {
                var cacheItemPolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
                };
                string queryString = "SELECT No_Cuenta, sum(Horas_Acum) as horas FROM [Tabla General] Group By No_Cuenta;";
                var cmd = new OdbcCommand(queryString, odbcConnection);
                var datatable = GetDataTable(cmd);
                foreach (DataRow dr in datatable.Rows)
                {
                    var cacheIt = new CacheItem(dr["No_Cuenta"].ToString(), dr["horas"].ToString());
                    cache.Add(cacheIt, cacheItemPolicy);
                }
                Logger.Log("Caché creado con éxito!", LogType.Debug);
            }catch(Exception ex)
            {
                Logger.Log(ex, LogType.Error);
            }
        }

// ya esta
        public void connection()
        {
                odbcConnection = new OdbcConnection(Config.GetBotConnection());
                Logger.Log("Conexion Vinculacion_Base creada con éxito!", LogType.Info);
        }

// ya esta
        public void connectionBotOnly()
        {
            odbcConnectionBotOnly = new OdbcConnection(Config.GetBotConnectionOnly());
            Logger.Log("Conexion Bot_Base creada con éxito!", LogType.Info);
        }

// ya esta
        public void Open()
        {
            try
            {
                if (odbcConnection.State == ConnectionState.Open)
                    return;
                odbcConnection.Open();
                Logger.Log("Conexion Vinculacion_Base abierta con éxito!", LogType.Debug);
            }
            catch(OdbcException e)
            {
                Logger.Log("Error a abrir conexion en Vinculacion_base "+e.Message, LogType.Error);
            }
        }

// ya esta
        public void Close()
        {
            try
            {
                if (odbcConnection.State == ConnectionState.Closed)
                    return;
                odbcConnection.Close();
                Logger.Log("Conexion Vinculacion_Base cerrada con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a cerrar conexion en Vinculacion_base " + e.Message, LogType.Error);
            }
            
        }

// ya esta
        public void OpenBotOnly()
        {
            try
            {
                if (odbcConnectionBotOnly.State == ConnectionState.Open)
                    return;
                odbcConnectionBotOnly.Open();
                Logger.Log("Conexion Bot_Base abierta con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a abrir conexion en Bot_Base " + e.Message, LogType.Error);
                Console.WriteLine(e.Message);
            }
        }

// ya esta
        public void CloseBotOnly()
        {
            try
            {
                if (odbcConnectionBotOnly.State == ConnectionState.Closed)
                    return;

                odbcConnectionBotOnly.Close();
                Logger.Log("Conexion Bot_Base cerrada con éxito!", LogType.Debug);
            }
            catch (OdbcException e)
            {
                Logger.Log("Error a cerrar conexion en Bot_base " + e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
            }

        }

// ya esta
        public bool CuentaExiste(string Cuenta)
        {
            string selectQuery = "SELECT * from [Datos Alumno] where No_Cuenta = ? ;";
            var cmd = new OdbcCommand(selectQuery, odbcConnection);
            try
            {  
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);
                //if (datatable.Rows.Count > 0)
                  //  numeroCuenta = datatable.Rows[0]["NumeroCuenta"].ToString();
                if (datatable.Rows.Count > 0)
                {
                    string retornable = datatable.Rows[0]["P_Nombre"].ToString();
                    Logger.Log("Se encontro cuenta en CuentaExiste " + retornable, LogType.Debug);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error en CunetaExiste "+e.Message, LogType.Error);
                //Console.WriteLine(e.Message);
            }
            return false;
        }

// ya esta
        public bool ExisteDb(string Cuenta)
        {
            string selectQuery = "SELECT CuentaTelegram from [AlumnosBot] where CuentaTelegram = ? ;";
            var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);

                if (datatable.Rows.Count > 0)
                {
                        Logger.Log("Se encontro cuenta en ExisteDb " + datatable.Rows[0]["CuentaTelegram"].ToString(), LogType.Debug);
                        return true;
                }

            }
            catch (OdbcException e)
            {
                Logger.Log("Error en database ExisteDb " + e.Message, LogType.Error);    
            }
            catch (Exception e)
            {
                Logger.Log("Existe en database ExisteDb " + e.Message, LogType.Error);             
            }
            return false;
        }

// ya esta
        public bool estadoDb(string Cuenta)
        {
            string selectQuery = "SELECT Estado from [AlumnosBot] where CuentaTelegram = ? ;";
            var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);
                if (datatable.Rows.Count > 0)
                {
                    int num = Int32.Parse(datatable.Rows[0]["Estado"].ToString());
                    if (num == 2)
                    {
                        Logger.Log("Retorno true en estadoDb ", LogType.Debug);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            Logger.Log("Retorno false en estadoDb ", LogType.Debug);
            return false;
        }

// ya esta
        public string GetCuentaNUMDb(string Cuenta)
        {
            string selectQuery = "SELECT [Datos Alumno].No_Cuenta from [Datos Alumno] WHERE [Datos Alumno].No_Cuenta = ? ; ";
            string selectQuery2 = "SELECT CuentaTelegram, NumeroCuenta from [AlumnosBot] WHERE CuentaTelegram = ? ; ";
            string numeroCuenta = "";
            try
            {
                //cmdBotOnly.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                //OdbcDataReader MyDataReader = cmdBotOnly.ExecuteReader();
                var cmd = new OdbcCommand(selectQuery2, odbcConnectionBotOnly);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);

                if (datatable.Rows.Count > 0)
                {
                        numeroCuenta = datatable.Rows[0]["NumeroCuenta"].ToString();
                }
                
                cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@No_Cuenta", OdbcType.VarChar).Value = numeroCuenta;
                var datatable2 = GetDataTable(cmd);

                if (datatable2.Rows.Count > 0)
                {
                        string retornable = datatable2.Rows[0]["No_Cuenta"].ToString();
                        return retornable;
                }
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
                return "";
            }
            string aus = "";
            return aus;
        }

// ya esta
        public string GetCuentaDb(string Cuenta)
        {
            string selectQuery = "SELECT No_Cuenta, [P_Nombre] as Nombre , [P_ Apellido] as Apellido FROM [Datos Alumno] WHERE No_Cuenta = ?;";
            string selectQueryBotOnly = "SELECT CuentaTelegram, NumeroCuenta FROM [AlumnosBot] WHERE CuentaTelegram = ? ;";
            string numeroCuenta = "";
            Logger.Log($"Obteniendo infomacion de la cuenta: {Cuenta}", LogType.Debug);
            try
            {
                var cmd = new OdbcCommand(selectQueryBotOnly, odbcConnectionBotOnly);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);
                if (datatable.Rows.Count > 0)
                    numeroCuenta = datatable.Rows[0]["NumeroCuenta"].ToString();
                

                cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@No_Cuenta", OdbcType.VarChar).Value = numeroCuenta;

                var datatable2 = GetDataTable(cmd);
                if (datatable2.Rows.Count > 0)
                {
                     string retornable = datatable2.Rows[0]["Nombre"].ToString() +" "+ datatable2.Rows[0]["Apellido"].ToString();
                    Logger.Log($"Nombre del alumno es: {retornable}", LogType.Debug);
                     return retornable;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
                return "";
            }
            return "";
        }

// ya esta
        public bool VerificarDb(string Cuenta, string code)
        {
            string selectQuery = "SELECT TokenGenerado from [AlumnosBot] where CuentaTelegram = ? ;";
            var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);

                if (datatable.Rows.Count > 0)
                {
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            return false;
        }

// ya esta
        private DataTable GetDataTable(OdbcCommand cmd)
        {
            OdbcDataAdapter da1 = new OdbcDataAdapter(cmd);
            var datatable = new DataTable();
            da1.Fill(datatable);
            return datatable;
        }

// ya esta
        private void ExecuteNonQuery(OdbcCommand cmd)
        {
            lock (l)
            {
                try
                {
                    OpenBotOnly();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logger.Log(e, LogType.Error);
                }
                finally
                {
                    CloseBotOnly();
                }
            }
        }

// ya esta
        public bool VerificarUpdateDb(string Cuenta, string code)
        {
            DateTime now = DateTime.Now;
            string updatequery = "UPDATE AlumnosBot set FechaConfirmacion='" + now + "', Confirmado=1, Estado=2, TokenGenerado='" + " " + "' WHERE TokenGenerado='" + code + "' and CuentaTelegram='" + Cuenta + "'";
            try
            {
                var cmd = new OdbcCommand(updatequery, odbcConnectionBotOnly);
                ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log( e, LogType.Error);
                return false;
            }
        }

// ya esta
        public string GetMailDb(string Cuenta)
        {
            string selectQuery = "SELECT NumeroCuenta,CorreoElectronico from [Alumnos] where NumeroCuenta= ? ;";
            var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
            try
                {
                    cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                    var datatable = GetDataTable(cmd);

                if (datatable.Rows.Count > 0)
                {
                    string retornable = datatable.Rows[0]["CorreoElectronico"].ToString();
                    return retornable;
                }
                    
                }
            catch (Exception e)
            {
                Logger.Log("getMail Convert" + e.Message, LogType.Warn);
                return " ";
            }
            string aus = "";
            return aus;
        }

// ya esta
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
                var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);

                // Console.WriteLine(selectResult.GetString(0));
                if (datatable.Rows.Count > 0)
                {
                    DateTime actual = DateTime.Parse(datatable.Rows[0]["CuentaTelegram"].ToString());
                    actual = actual.AddMinutes(5);
                    DateTime nos = DateTime.Now;
                    int compare = DateTime.Compare(actual, nos);
                    if (DateTime.Compare(actual, nos) <= 0)
                    {
                        string tokenNuevo = createToken();
                        EnviarCorreo(mail, tokenNuevo);

                        string updateQuery = "UPDATE AlumnosBot set TokenGenerado='" + tokenNuevo + "',FechaUltimoToken='" + nos + "' WHERE CuentaTelegram= ?";
                        var updateCommand = new OdbcCommand(updateQuery, odbcConnectionBotOnly);
                        updateCommand.Parameters.Add("@Cuenta", OdbcType.Text).Value = Cuenta;
                        // updateCommand.ExecuteNonQuery();
                        ExecuteNonQuery(updateCommand);
                        return true;
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

// ya esta
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
                    var cmd = new OdbcCommand(selectQuery, odbcConnectionBotOnly);
                    var datatable = GetDataTable(cmd);

                    if (datatable.Rows.Count > 0)
                    {
                        ing = true;
                    }
                    
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error generando token" + e.Message, LogType.Warn);
            }
            return random;
        }

// ya esta
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

// ya esta
        public bool CuentaVerificadaDb(string numeroCuenta)
        {
            string selectQuery = "SELECT * FROM [Datos Alumno] WHERE No_Cuenta = ? ;";
            string selectQueryBotOnly = "SELECT Confirmado FROM [AlumnosBot] WHERE NumeroCuenta = ?;";
            string numCuenta="";
            try
            {   
                var cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@NumeroCuenta", OdbcType.VarChar).Value = numeroCuenta;
                var datatable = GetDataTable(cmd);
              
                if(datatable.Rows.Count > 0)
                    numCuenta = datatable.Rows[0]["No_Cuenta"].ToString();

                var cmdBotOnly = new OdbcCommand(selectQueryBotOnly, odbcConnectionBotOnly);
                cmdBotOnly.Parameters.Add("@NumeroCuenta", OdbcType.VarChar).Value = numCuenta;
                var datatable2 = GetDataTable(cmdBotOnly);
                if(datatable2.Rows.Count > 0)
                {
                    if (datatable2.Rows[0]["Confirmado"].ToString()== "1")
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            return false;
        }

// ya esta
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
            string insertquery = "INSERT into [AlumnosBot] (NumeroCuenta,Deshabilitado,CuentaTelegram,TokenGenerado,Confirmado,FechaConfirmacion,Estado,FechaUltimoToken,ChatId) values ('" + noCuenta + "','" + prueba + "','" + telegramid + "','" + token + "'," + verified + ",'" + confirmacion + "'," + Estado + ",'" + today + "','" + id_alumno + "'); ";
            try
            {
                var cmd = new OdbcCommand(insertquery, odbcConnectionBotOnly);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = noCuenta;
                lock (l)
                {
                    OpenBotOnly();
                    cmd.ExecuteNonQuery();
                    CloseBotOnly();
                }
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
        }

// ya esta
        public int Getid(string Cuenta)
        {
            string selectQuery = "SELECT No_Cuenta from [Datos Alumno] where No_Cuenta= ? ";
            var cmd = new OdbcCommand(selectQuery, odbcConnection);
            try
            {
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);
                if (datatable.Rows.Count > 0)
                {
                    int retornable = Int32.Parse(datatable.Rows[0]["No_Cuenta"].ToString());
                    return retornable;                       
                }
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            string aus = "";
            return -1;
        }

// ya esta
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
                Logger.Log(e, LogType.Error);
            }

            string selectQuery = "SELECT sum(Horas_Acum) as Horas_Totales FROM [Tabla General] where No_Cuenta = ? ";
            try
            {
                // Console.WriteLine("nUMEROcUENTA " + nCuenta);
                int NumeroTotalHoras = 0;
                var cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = nCuenta;
                var datatable = GetDataTable(cmd);
                
                NumeroTotalHoras = Convert.ToInt32(datatable.Rows[0]["Horas_Totales"]);
                
                return NumeroTotalHoras.ToString();
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
                return " ";
            }

        }

// ya esta
        public string HorasDetalle2(string nCuenta)
        {
            string detalles = "";
            string selectQuery = "SELECT id_proyecto,Periodo,Beneficiario,Horas_Acum FROM [Tabla General] where No_Cuenta = ? ";
            try
            {
                var cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = nCuenta;
                var datatable = GetDataTable(cmd);
                
                detalles += "Nombre de Proyecto: " + datatable.Rows[0]["id_proyecto"].ToString() + "\n";
                detalles += "Periodo: " + datatable.Rows[0]["Periodo"].ToString() + "\n";
                detalles += "Beneficiaro: " + datatable.Rows[0]["Beneficiario"].ToString() + "\n";                   //HORAS DE PROYECTO 
                detalles += "Horas Trabajadas:" + datatable.Rows[0]["Horas_Acum"].ToString() + "\n\n";
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            return detalles;
        }

// ya esta
        public bool CuentaExisteDb(string Cuenta)
        {
            string selectQuery = "SELECT No_Cuenta from [Datos Alumno] where No_Cuenta= ? ;";
            try
            {
                var cmd = new OdbcCommand(selectQuery, odbcConnection);
                cmd.Parameters.Add("@Cuenta", OdbcType.VarChar).Value = Cuenta;
                var datatable = GetDataTable(cmd);

                if (datatable.Rows.Count > 0)
                {
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Logger.Log(e, LogType.Error);
            }
            return false;
        }
    }
}

