using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=blog;";

        [HttpPost]
        public object AddNewBlogger([FromBody] AddNewBloggerDto addNewBloggerDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES ('[value-1]','[value-2]','[value-3]','[value-4]','[value-5]'))";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Name", addNewBloggerDto.Name);
            cmd.Parameters.AddWithValue("@Email", addNewBloggerDto.Email);
            cmd.Parameters.AddWithValue("@Age", addNewBloggerDto.Age);
            cmd.Parameters.AddWithValue("@Password", addNewBloggerDto.Password);
            cmd.Parameters.AddWithValue("@RegistrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres felvétel", result = addNewBloggerDto };
        }
    }
}
