using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digital_Assesment.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        public readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/GetTest")]
        public string GetAllTest()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("StudentAppCon"));
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select * from test";
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter(comm);
                DataTable dt = new DataTable();
                nda.Fill(dt);
                comm.Dispose();
                conn.Close();
                List<UserTest> list = new List<UserTest>();
                Response response = new Response();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UserTest test = new UserTest();
                        test.Id = Convert.ToInt32(dt.Rows[i]["id"]);
                        list.Add(test);
                    }
                }
                if (list.Count > 0)
                {
                    return JsonConvert.SerializeObject(list);
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "Data Not Found";
                    return JsonConvert.SerializeObject(response);

                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpPost]
        [Route("/AddTest")]
        public void AddTest(UserTest userTest)
        {
            try
            {
                Console.WriteLine(userTest);
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("StudentAppCon"));
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into test(id) values (12)";
                Console.WriteLine(comm.CommandText);
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter(comm);
                DataTable dt = new DataTable();
                nda.Fill(dt);
                comm.Dispose();
                conn.Close();

            }
            catch (Exception e)
            {
            }

        }


    }
}

///api/v1/digital-assessment/employee -> post
/// api / v1 / digital - assessment / employee
/// api / v1 / digital - assessment / employee /{employeeId}

//Employee

//    {
//        id : 1
//    },
//    { }
