using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Digital_Assesment.Repository
{
	public class Connetion
    {
        private static NpgsqlConnection connection;
        public readonly IConfiguration _configuration;
        public Connetion(IConfiguration configuration)
		{
             _configuration = configuration;
             connection = new NpgsqlConnection(_configuration.GetConnectionString("StudentAppCon"));
        }
        public NpgsqlConnection getConnection()
        { 
            return connection;
        }
	}
}

