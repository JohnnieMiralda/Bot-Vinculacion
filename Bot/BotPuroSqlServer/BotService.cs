using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotVinculacionUnitec
{
    class BotService
    {

        Config con;
        public static string connectionString;
        
        //bot de enriquecs 
        //1242656066:AAF3AqRwRp3VHVJ0ULpY53HLQKrZjkt5bH8
        //1341384254:AAHMn7Q-48X4eUYOftbeUixZrXMJMDPyjZY
        //1099955313:AAE4MUcmOzK09op7z8K-K5VNANtumC2n9WQ
        

        public BotService()
        {
            con = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"c:\appsettings.json"));

            connectionString = con.telegramToken;
            //Método que se ejecuta cuando se recibe un mensaje
            Bot.OnMessage += Bot_OnMessage; ;

            //Método que se ejecuta cuando se recibe un callbackQuery
            Bot.OnCallbackQuery += Bot_OnCallbackQuery; ;

            //Método que se ejecuta cuando se recibe un error
            Bot.OnReceiveError += Bot_OnReceiveError; ;
        }

        private static readonly TelegramBotClient Bot = new TelegramBotClient(connectionString);

        public void Stop()
        {
            Bot.StopReceiving();
        }
        AccesDB access = new AccesDB();
        public void Start()
        {
			var me = Bot.GetMeAsync().Result;
			Console.WriteLine($"Bot Iniciado:  Usuario ID: {me.Id}, Username: {me.FirstName}");
			Logger.Log($"Bot Iniciado:  Usuario ID: {me.Id}, Username: {me.FirstName}", LogType.Info);
			
            Bot.StartReceiving();
            //access = new AccesDB();
            access.iniciarTimer();
        }



        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
          
            var message = e.Message;
            Logger.Log(message.Chat.Username+ ":" + message.Text, LogType.Info);
            //AccesDB access = new AccesDB();
            access.connection();
            Console.WriteLine($"Mensaje de @{message.Chat.Username}:" + message.Text);

            if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text) return; ;

            bool exists = access.ExisteDb(message.Chat.Username);
            bool estate = access.estadoDb(message.Chat.Username);
            //Prueba oara dia actual
            // sqlite.FechaA();
            if (message.Chat.Username == null)
            {
                Bot.SendTextMessageAsync(message.Chat.Id, "Su cuenta de Telegram no esta configurada correctamente porfavor\n Ve a Ajustes>Elegir Nombre de Usuario");
            }
            else
            {
                if (exists && estate)
                {
                    string numCuenta = access.GetCuentaNUMDb(message.Chat.Username);

                    string nombre = access.GetCuentaDb(message.Chat.Username);
                    //Declaracion Botones
                    var BotonesHYD = new InlineKeyboardMarkup(new[]
                  {
                                        new []
                                      {
                                 InlineKeyboardButton.WithCallbackData(
                                  text:"Horas Totales",
                                  callbackData: "Horas "+ numCuenta),//Aqui mando el numero de Cuenta para devolver al usuario indicado
                                    InlineKeyboardButton.WithCallbackData(
                                       text:"Detalle de Horas",
                                      callbackData: "Detalles "+numCuenta)
                                          }});
                    //INformacion botones

                    // MOstrar Botones  
                    Bot.SendTextMessageAsync(
                      message.Chat.Id,
                      "Estimado estudiante " + nombre + ": Bienvenido al Asistente de Vinculación UNITEC-SPS \n Elija una Opcion",
                      replyMarkup: BotonesHYD);
                }
                else if (exists && estate == false)
                {



                    if (access.VerificarDb(message.Chat.Username, message.Text.Split(" ").Last().ToString()))
                    {

                        bool verifcadoCorrectamen = access.VerificarUpdateDb(message.Chat.Username, message.Text.Split(" ").Last().ToString());
                        if (verifcadoCorrectamen)
                        {
                            Bot.SendTextMessageAsync(message.Chat.Id, "¡Tu cuenta se verifico exitosamente!");
                            //Declaracion Botones
                            string numCuenta = access.GetCuentaNUMDb(message.Chat.Username);
                            string nombre = access.GetCuentaDb(message.Chat.Username);
                            var BotonesHYD = new InlineKeyboardMarkup(new[]
                              {
                                        new []
                                      {
                                 InlineKeyboardButton.WithCallbackData(
                                  text:"Horas Totales",
                                  callbackData: "Horas "+ numCuenta),//Aqui mando el numero de Cuenta para devolver al usuario indicado
                                    InlineKeyboardButton.WithCallbackData(
                                       text:"Detalle de Horas",
                                      callbackData: "Detalles "+numCuenta)
                                          }});
                            //INformacion botones

                            // MOstrar Botones  
                            Bot.SendTextMessageAsync(
                              message.Chat.Id,
                              "Estimado estudiante " + nombre + ": Bienvenido al Asistente de Vinculación UNITEC-SPS \n Elija una Opcion",
                              replyMarkup: BotonesHYD);
                        }


                    }
                    else if (message.Text.Split(" ").Last().ToString().ToLower() == "reenviar")
                    {

                        // Obtengo el correo de cuenta actual que quiere que le reenvie codigo
                        string cuenta = access.GetCuentaNUMDb(message.Chat.Username);
                        string Mail = access.GetMailDb(cuenta);



                        // oculto ciertas partes del corroe para que no sea visible en su totalidad
                        string changedMail = access.ConverMail(Mail);
                        //envio respeusta de donde envie su codigo de confirmacion
                        if (access.newTokenDb(message.Chat.Username, Mail))
                        {
                            Console.WriteLine("Reenvio exitoso al correo:" + changedMail);
                            Bot.SendTextMessageAsync(message.Chat.Id, "Se reenvio un nuevo codigo a de confimacion al correo:" + changedMail);
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(message.Chat.Id, "Debes esperar almenos 5 minutos desde tu ultima solicitud ");
                        }
                    }
                    else
                    {

                        Bot.SendTextMessageAsync(message.Chat.Id, "Token Ingresado Incorrecto\nEscribe reenviar para solicitar nuevo token");

                    }
                }
                else
                {
                    switch (message.Text.Split(" ").First().ToLower())
                    {
                        case "/start":


                            Bot.SendTextMessageAsync(message.Chat.Id,
                                     "Estimado estudiante: Bienvenido al Asistente de Vinculación UNITEC-SPS\n\nIngrese su número de cuenta para sus consultas");
                            
                            break;
                        default:

                            switch (!access.CuentaExisteDb(message.Text.Split(" ").First().ToString()))
                            {

                                case false:
                                    // revisa que el numero de cuenta venga solo sin ningun otra palabra
                                    if (access.CuentaExisteDb(message.Text.Split(" ").Last().ToString()))
                                    {
                                        // verificar si la cuenta esta verificada
                                        switch (access.CuentaVerificadaDb(message.Text))
                                        {
                                            //Caso cuenta verificada pero con otro user de telegram
                                            case true:

                                                Bot.SendTextMessageAsync(message.Chat.Id, "El numero de Cuenta que ingresaste ya esta vinculado a otra cuenta de telegram \nPara consultas enviar correo a:\nvinculacionsps@unitec.edu ó andrea.orellana@unitec.edu.hn");

                                                break;//case cuenta verificada con otro user de telegram

                                            //Caso Cuenta no verificada
                                            case false:

                                                // Obtengo el correo de cuenta ingresada
                                                string Mail = access.GetMailDb(message.Text);
                                                // oculto ciertas partes del corroe para que no sea visible en su totalidad
                                                string changedMail = access.ConverMail(Mail);
                                                //envio respeusta de donde envie su codigo de confirmacion




                                                //Proceso de enviar correo y generar token
                                                if (Mail == null || Mail == "" || Mail == " " || Mail == "NULL")
                                                {
                                                    Bot.SendTextMessageAsync(message.Chat.Id, "Tu Numero de cuenta no tiene un correo vinculado \nPara consultas enviar correo a:\nvinculacionsps@unitec.edu ó andrea.orellana@unitec.edu.hn");

                                                }
                                                else
                                                {
                                                    Bot.SendTextMessageAsync(message.Chat.Id, "Numero de Cuenta no verificado \nPara verificar Tu Numero de Cuenta ingresas token enviado a tu correo: " + changedMail);

                                                    access.insertarDb(message.Chat.Username, message.Text, Mail);
                                                    Console.WriteLine("Se ha reenviado un codigo de Verificacion al correo:" + Mail);

                                                }

                                                break;


                                        }
                                    }
                                    break;

                                default:


                                    Bot.SendTextMessageAsync(message.Chat.Id, "Numero de Cuenta Incorrecto Vuelve a ingresarlo Para consultas enviar correo a:\nvinculacionsps@unitec.edu ó andrea.orellana@unitec.edu.hn");


                                    break;
                            }


                            break;
                    }
                }

            }
        }
        

        private static void Bot_OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {

            Console.WriteLine(e.ApiRequestException.Message);
        }

        private void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        { 
            //AccesDB access = new AccesDB();
            access.connection();
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            switch (callbackQuery.Data.Split(" ").First().ToLower())
            {

                case "horas":

                    string totalHoras = access.HorasTotales(callbackQuery.Data.Split(" ").Last());

                    ///vuelve a mostrar el boton

                    var BotonesHYD = new InlineKeyboardMarkup(new[]
                          {
                                        new []
                                      {
                                 InlineKeyboardButton.WithCallbackData(
                                  text:"Horas Totales",
                                  callbackData: "Horas "+ callbackQuery.Data.Split(" ").Last()),//Aqui mando el numero de Cuenta para devolver al usuario indicado
                                    InlineKeyboardButton.WithCallbackData(
                                       text:"Detalle de Horas",
                                      callbackData: "Detalles "+callbackQuery.Data.Split(" ").Last())
                                          }});
                    //vuelve a mostrar boton

                    Bot.SendTextMessageAsync(
                     callbackQuery.Message.Chat.Id,
                     "Tienes un total de " + totalHoras + " horas a la fecha.\nPara consultas enviar correo a:\nvinculacionsps@unitec.edu ó andrea.orellana@unitec.edu.hn\n\nOpciones ",
                     replyMarkup: BotonesHYD);

                    break;

                case "detalles":
                    string DetalleHoras = access.HorasDetalle2(callbackQuery.Data.Split(" ").Last());
                    ///vuelve a mostrar el boton

                    var BotonesHY = new InlineKeyboardMarkup(new[]
                          {
                                         new []
                                       {
                                  InlineKeyboardButton.WithCallbackData(
                                   text:"Horas Totales",
                                   callbackData: "Horas "+ callbackQuery.Data.Split(" ").Last()),//Aqui mando el numero de Cuenta para devolver al usuario indicado
                                     InlineKeyboardButton.WithCallbackData(
                                        text:"Detalle de Horas",
                                       callbackData: "Detalles "+callbackQuery.Data.Split(" ").Last())
                                           }});
                    //vuelve a mostrar boton

                    Bot.SendTextMessageAsync(
                   callbackQuery.Message.Chat.Id,
                    "Tu Informacion es la siguiente:\n" + DetalleHoras + "\nPara consultas enviar correo a:\nvinculacionsps@unitec.edu ó andrea.orellana@unitec.edu.hn\n\nOpciones",
                    replyMarkup: BotonesHY);
                    break;


                default:
                    break;
            }

        }

        //Revision de Si existe la cuenta
    }


}