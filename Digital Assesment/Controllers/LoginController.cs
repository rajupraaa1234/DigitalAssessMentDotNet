using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using Digital_Assesment.Models;
using Digital_Assesment.Models.Registration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Digital_Assesment.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digital_Assesment.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        public readonly IConfiguration _configuration;
        public NpgsqlConnection conn;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            Connetion connection = new Connetion(_configuration);
            conn = connection.getConnection();
        }

        [HttpGet]
        [Route("/GetUser")]
        public string GetAllUser()
        {
            try
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select * from users";
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter(comm);
                DataTable dt = new DataTable();
                nda.Fill(dt);
                comm.Dispose();
                conn.Close();
                List<LoginRequest> list = new List<LoginRequest>();
                LoginResponse response = new LoginResponse();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LoginRequest user = new LoginRequest();
                        user.username = Convert.ToString(dt.Rows[i]["username"]);
                        user.password = Convert.ToString(dt.Rows[i]["password"]);
                        list.Add(user);
                    }
                }
                if (list.Count > 0)
                {
                    return JsonConvert.SerializeObject(list);
                }
                else
                {
                    response.statusCode = 100;
                    response.statusMessage = "Data Not Found";
                    return JsonConvert.SerializeObject(response);

                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpPost]
        [Route("registration")]
        public async Task<string> registration(RegistrationRequest registrationReguest)
        {
            conn.Open();
            string commandText = $"INSERT INTO users (username, password) VALUES (@username, @password)";
            await using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", registrationReguest.username);
                cmd.Parameters.AddWithValue("password", registrationReguest.password);
                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
            return "user created successfully";
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> login(LoginRequest request)
        {
            conn.Open();
            string commandText = $"SELECT * FROM users WHERE username = @username AND password = @password";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", request.username);
                cmd.Parameters.AddWithValue("password", request.password);
                await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        LoginRequest loginUser = new LoginRequest();
                        loginUser.username = reader["username"] as string;
                        loginUser.password = reader["password"] as string;
                        return JsonConvert.SerializeObject(loginUser);
                    }
                Console.WriteLine(reader.VisibleFieldCount);
            }
            LoginResponse response = new LoginResponse();
            response.statusCode = 100;
            response.statusMessage = "Data Not Found";
            return JsonConvert.SerializeObject(response);
            
        }
    }
}

