using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace BotVinculacionUnitec
{
	class Config
	{
		static ConnectionStrings connectionManager;

		public Config()
		{
			connectionManager = JsonConvert.DeserializeObject<ConnectionStrings>(File.ReadAllText(@"appsettings.json"));

			Console.WriteLine(connectionManager.TelegramToken);
			Console.WriteLine(connectionManager.ConnectionStringBot);
			Console.WriteLine(connectionManager.ConnectionStringData);

		}

		public static string GetTelegramToken()
		{
			return connectionManager.TelegramToken;
		}

		public static string GetBotConnection()
		{
			return connectionManager.ConnectionStringBot;
		}

		public static string GetBotConnectionOnly()
		{
			return connectionManager.ConnectionStringBotOnly;
		}

		public static string GetDataConnection()
		{
			return connectionManager.ConnectionStringData;
		}

	}

	struct ConnectionStrings
	{
		public string TelegramToken { get; set; }

		public string ConnectionStringBot { get; set; }

		public string ConnectionStringBotOnly { get; set; }

		public string ConnectionStringData { get; set; }
	}

}
