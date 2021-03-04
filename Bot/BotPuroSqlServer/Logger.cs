using System;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;


namespace BotVinculacionUnitec {

    class Logger {

        public static ILog log;

        public static void Log(string message,LogType logType) {
            EnsureLogger();

            switch (logType) {
                case LogType.Error:
                    log.Error(message);
                    break;

                case LogType.Warn:
                    log.Warn(message);
                    break;

                case LogType.Fatal:
                    log.Fatal(message);
                    break;
				
				case LogType.Info:
					log.Info(message);
					break;
					
            }
        }

        public static void Log(Exception ex, LogType logType)
        {
            Log(ex.ToString(), logType);
        }
        private static void EnsureLogger() {
            if (log != null) return;
            var assembly = Assembly.GetEntryAssembly();
            var logRepository = LogManager.GetRepository(assembly);
            var configFile = getConfigFile();

            //Configuracion de log4net
            XmlConfigurator.Configure(logRepository, configFile);
            log = LogManager.GetLogger(assembly, assembly.ManifestModule.Name.Replace(".dll", "").Replace(".", " "));
           
        }


        private static FileInfo getConfigFile() {
            FileInfo configFile = null;
            //el archivo esta en la direccion BotPuroSqlServer\bin\Debug\netcoreapp3.1\
            var ConfigFileNames = new[] { "Config/log4net.config", "log4net.config" };
            
            foreach(var configFileNames in ConfigFileNames){
                configFile = new FileInfo(configFileNames);
                if (configFile.Exists) break;
            }
            if (configFile == null || !configFile.Exists) 
                    throw new NullReferenceException("Archivo log4net.config no encontrado");
            return configFile;
        }
  

    }
}
