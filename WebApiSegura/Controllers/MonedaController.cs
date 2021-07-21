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
    [RoutePrefix("api/Moneda")]
    public class MonedaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Moneda moneda = new Moneda();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Descripcion, Estado
                                                             FROM   Moneda
                                                             WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        moneda.Codigo = sqlDataReader.GetInt32(0);
                        moneda.Descripcion = sqlDataReader.GetString(1);
                        moneda.Estado = sqlDataReader.GetString(2);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(moneda);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Moneda> monedas = new List<Moneda>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Descripcion, Estado
                                                            FROM   Moneda", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Moneda moneda = new Moneda();
                        moneda.Codigo = sqlDataReader.GetInt32(0);
                        moneda.Descripcion = sqlDataReader.GetString(1);
                        moneda.Estado = sqlDataReader.GetString(2);

                        monedas.Add(moneda);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(monedas);
        }


        [HttpPost]
        public IHttpActionResult Ingresar(Moneda moneda)
        {
            if (moneda == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@" INSERT INTO Moneda (Descripcion, Estado) 
                                         VALUES (@Descripcion, @Estado)",
                                         sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@Descripcion", moneda.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Estado", moneda.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(moneda);
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Moneda moneda)
        {
            if (moneda == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@" UPDATE Moneda 
                                                        SET 
                                                            Descripcion = @Descripcion,
                                                            Estado = @Estado 
                                          WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", moneda.Codigo);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", moneda.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Estado", moneda.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(moneda);
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
                        new SqlCommand(@" DELETE Moneda WHERE Codigo = @Codigo",
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
