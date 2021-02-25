using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotVinculacionUnitec
{
    
    public class Config
	{
		public string telegramToken { get; set; }

		public string ConnectionStringDB { get; set; }

		public string SQLserverConnectionString { get; set; }
	}
}
