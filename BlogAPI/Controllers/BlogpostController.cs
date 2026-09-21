using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=blog;";

        [HttpPost("bloggerAdd")]
        public object AddNewBloggerPost([FromBody] AddNewBloggerPostDto addNewBloggerPostDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES (@Title,@Content,@postTime,@updateTime)";
            

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Title", addNewBloggerPostDto.Title);
            cmd.Parameters.AddWithValue("@Content", addNewBloggerPostDto.Content);
            cmd.Parameters.AddWithValue("@postTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@blogId", addNewBloggerPostDto.blogId);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres felvétel", result = addNewBloggerPostDto };
        }

        [HttpGet]
        public List<Blogpost> GetBloggers()
        {
            List<Blogpost> bloggers = new List<Blogpost>();

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = "SELECT * FROM blogger;";

            var cmd = new MySqlCommand(sql, connection);

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var blogger = new Bloggers
                {
                    Id = data.GetInt32("id"),
                    Name = data.GetString("name"),
                    Email = data.GetString("email"),
                    Age = data.GetInt32("age"),
                    Password = data.GetString("password"),
                    RegistradionTime = data.GetDateTime("registrationTime")
                };
                bloggers.Add(blogger);
            }

            connection.Close();

            return bloggers;
        }
    }
}
