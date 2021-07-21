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
    [RoutePrefix("api/Sesion")]
    public class SesionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Sesion sesion = new Sesion();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaInicio, FechaExpiracion, Estado, CodigoSesion FROM Sesion WHERE Codigo = @Codigo", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        sesion.Codigo = dataReader.GetInt32(0);
                        sesion.CodigoUsuario = dataReader.GetInt32(1);
                        sesion.FechaInicio = dataReader.GetDateTime(2);
                        sesion.FechaExpiracion = dataReader.GetDateTime(3);
                        sesion.Estado = dataReader.GetString(4);
                        sesion.CodigoSesion = dataReader.GetString(5);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return Ok(sesion);
        }


        [HttpPut]
        public IHttpActionResult Actualizar(Sesion sesion)
        {
            if (sesion == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Sesion SET CodigoUsuario = @CodigoUsuario,
                                                                            FechaInicio = @FechaInicio,
                                                                            FechaExpiracion = @FechaExpiracion,
                                                                            Estado = @Estado, 
                                                                            CodigoSesion = @CodigoSesion
                                                             WHERE CodigoSesion = @CodigoSesion", sqlConnection);

                    //sqlCommand.Parameters.AddWithValue("@Codigo", sesion.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", sesion.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaInicio", sesion.FechaInicio);
                    sqlCommand.Parameters.AddWithValue("@FechaExpiracion", sesion.FechaExpiracion);
                    sqlCommand.Parameters.AddWithValue("@Estado", sesion.Estado);
                    sqlCommand.Parameters.AddWithValue("@CodigoSesion", sesion.CodigoSesion);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(sesion);
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Sesion> estadisticas = new List<Sesion>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaInicio, FechaExpiracion, Estado, CodigoSesion FROM Sesion", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Sesion sesion = new Sesion();

                        sesion.Codigo = dataReader.GetInt32(0);
                        sesion.CodigoUsuario = dataReader.GetInt32(1);
                        sesion.FechaInicio = dataReader.GetDateTime(2);
                        sesion.FechaExpiracion = dataReader.GetDateTime(3);
                        sesion.Estado = dataReader.GetString(4);
                        sesion.CodigoSesion = dataReader.GetString(5);

                        estadisticas.Add(sesion);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return Ok(estadisticas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Sesion sesion)
        {
            if (sesion == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Sesion (CodigoUsuario, FechaInicio, FechaExpiracion, Estado, CodigoSesion) VALUES (@CodigoUsuario, @FechaInicio, @FechaExpiracion, @Estado, @CodigoSesion)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", sesion.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaInicio", sesion.FechaInicio);
                    sqlCommand.Parameters.AddWithValue("@FechaExpiracion", sesion.FechaExpiracion);
                    sqlCommand.Parameters.AddWithValue("@Estado", sesion.Estado);
                    sqlCommand.Parameters.AddWithValue("@CodigoSesion", sesion.CodigoSesion);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(sesion);
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
                        new SqlCommand(@"DELETE Sesion WHERE Codigo = @Codigo",
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
    }
}
