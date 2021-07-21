using WebApiSegura.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiSegura.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Error")]
    public class ErrorController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<Error> errores = new List<Error>();
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo,CodigoUsuario,FechaHora,Fuente,Numero,Descripcion,Vista,Accion from Error", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        Error error = new Error { 
                            Codigo = reader.GetInt32(0), 
                            CodigoUsuario = reader.GetInt32(1), 
                            FechaHora = reader.GetDateTime(2), 
                            Fuente = reader.GetString(3), 
                            Numero = reader.GetString(4),
                            Descripcion = reader.GetString(5), 
                            Vista = reader.GetString(6), 
                            Accion = reader.GetString(7) 
                        };
                        errores.Add(error);
                    }
                    sqlConnection.Close();
                }
                return Ok(errores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public IHttpActionResult Ingresar(Error error)
        {
            if (error == null)
            {
                return BadRequest();
            }
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"Insert into 
                                                        Error(CodigoUsuario,FechaHora,Fuente,Numero,Descripcion,Vista,Accion) 
                                                        values(@CodigoUsuario,@FechaHora,@Fuente,@Numero,@Descripcion,@Vista,@Accion)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", error.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@Fuente", error.Fuente);
                    sqlCommand.Parameters.AddWithValue("@Numero", error.Numero);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", error.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Vista", error.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", error.Accion);

                    sqlConnection.Open();
                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return Ok(error);
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }


        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id == 0)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE Error WHERE Codigo=@Codigo", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);
                    sqlConnection.Open();
                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (filasAfectadas > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }


    }
}
