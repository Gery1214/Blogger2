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
    public class BlogController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=blog;";
        [HttpGet]
        public List<Bloggers> GetBloggers()
        {
            List<Bloggers> bloggers = new List<Bloggers>();

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

        [HttpPost]
        public object AddNewBlogger([FromBody] AddNewBloggerDto addNewBloggerDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@Name,@Email,@Age,@Password,@RegistrationTime)";

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

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

           return new { message = "Sikeres törlés", result = ""};
        }
        [HttpPut]
        public object UpdateBlogger([FromQuery]int id,UpdateBloggerDto updateBloggerDto)
        {

            var connection = new  MySqlConnection(ConnectionString);

            connection.Open();

            var sql = @"UPDATE `blogger` SET `Name`=@Name,`Email`=@Email,`Age`=@Age,`Password`=@Password WHERE `ID` = @id;";

            var cmd = new MySqlCommand(@sql, connection);

            cmd.Parameters.AddWithValue("@Name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@Email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@Age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@Password", updateBloggerDto.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres frissítés", result = updateBloggerDto };
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT * FROM `blogger` WHERE `ID` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            object? data = null;
            if(datareader.Read() == true)
            {

                var blogger = new Bloggers
                {
                    Id = datareader.GetInt32("id"),
                    Name = datareader.GetString("name"),
                    Email = datareader.GetString("email"),
                    Age = datareader.GetInt32("age"),
                    Password = datareader.GetString("password"),
                    RegistradionTime = datareader.GetDateTime("registrationTime")
                };

                data =  new { message = "Sikeres lekérdezés", result = blogger };
            }
            else 
            {
                data = new { message = "Sikertelen lekérdezés | Nincs ilyen blogger", result = "" };
            }


            connection.Close();

            return data;


        }
    }
}
