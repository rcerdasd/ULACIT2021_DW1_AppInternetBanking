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
    [RoutePrefix("api/Servicio")]
    public class ServicioController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Servicio servicio = new Servicio();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"] .ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Descripcion, Estado
                                                             FROM Servicio
                                                             WHERE Codigo = @Codigo",sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo",id);
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while(sqlDataReader.Read())
                    {
                        servicio.Codigo = sqlDataReader.GetInt32(0);
                        servicio.Descripcion = sqlDataReader.GetString(1);
                        servicio.Estado = sqlDataReader.GetString(2);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(servicio);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Servicio> servicios = new List<Servicio>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Descripcion, Estado
                                                             FROM Servicio", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Servicio servicio = new Servicio();
                        servicio.Codigo = sqlDataReader.GetInt32(0);
                        servicio.Descripcion = sqlDataReader.GetString(1);
                        servicio.Estado = sqlDataReader.GetString(2);
                        servicios.Add(servicio);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(servicios);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Servicio servicio)
        {
            if (servicio == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Servicio (Descripcion, Estado)
                                                             VALUES (@Descripcion, @Estado) ", sqlConnection);
                    
                    sqlCommand.Parameters.AddWithValue("@Descripcion", servicio.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Estado", servicio.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(servicio);
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Servicio servicio)
        {
            if (servicio == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Servicio SET Descripcion = @Descripcion,
                                                                             Estado = @Estado
                                                             WHERE Codigo = @Codigo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", servicio.Codigo);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", servicio.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Estado", servicio.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(servicio);
        }

        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id < 1)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE Servicio WHERE Codigo = @Codigo ", sqlConnection);

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
