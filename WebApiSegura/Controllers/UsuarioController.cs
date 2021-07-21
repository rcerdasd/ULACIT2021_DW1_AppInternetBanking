using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Usuario usuario = new Usuario();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Identificacion, Nombre, Username, Password, Email, FechaNacimiento, Estado
                                                             FROM   Usuario
                                                             WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        usuario.Codigo = sqlDataReader.GetInt32(0);
                        usuario.Identificacion = sqlDataReader.GetString(1);
                        usuario.Nombre = sqlDataReader.GetString(2);
                        usuario.Username = sqlDataReader.GetString(3);
                        usuario.Password = sqlDataReader.GetString(4);
                        usuario.Email = sqlDataReader.GetString(5);
                        usuario.FechaNacimiento = sqlDataReader.GetDateTime(6);
                        usuario.Estado = sqlDataReader.GetString(7);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(usuario);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Identificacion, Nombre, Username, Password, Email, FechaNacimiento, Estado
                                                             FROM   Usuario", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.Codigo = sqlDataReader.GetInt32(0);
                        usuario.Identificacion = sqlDataReader.GetString(1);
                        usuario.Nombre = sqlDataReader.GetString(2);
                        usuario.Username = sqlDataReader.GetString(3);
                        usuario.Password = sqlDataReader.GetString(4);
                        usuario.Email = sqlDataReader.GetString(5);
                        usuario.FechaNacimiento = sqlDataReader.GetDateTime(6);
                        usuario.Estado = sqlDataReader.GetString(7);

                        usuarios.Add(usuario);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(usuarios);
        }

        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id < 1)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@" DELETE Usuario WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(id);
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@" UPDATE Usuario 
                                                        SET 
                                                        Identificacion = @Identificacion,
                                                        Nombre = @Nombre, 
                                                        Username = @Username, 
                                                        Password = @Password, 
                                                        Email = @Email, 
                                                        FechaNacimiento = @FechaNacimiento, 
                                                        Estado = @Estado
                                          WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", usuario.Codigo);
                    sqlCommand.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
                    sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    sqlCommand.Parameters.AddWithValue("@Username", usuario.Username);
                    sqlCommand.Parameters.AddWithValue("@Password", usuario.Password);
                    sqlCommand.Parameters.AddWithValue("@Email", usuario.Email);
                    sqlCommand.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                    sqlCommand.Parameters.AddWithValue("@Estado", usuario.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(usuario);
        }
    }
}
