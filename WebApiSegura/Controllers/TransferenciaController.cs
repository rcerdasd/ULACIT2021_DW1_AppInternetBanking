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
    [RoutePrefix("api/Transferencia")]
    public class TransferenciaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Transferencia transferencia = new Transferencia();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CuentaOrigen, CuentaDestino, FechaHora, Descripcion, Monto, Estado FROM Transferencia WHERE Codigo = @Codigo", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        transferencia.Codigo = dataReader.GetInt32(0);
                        transferencia.CuentaOrigen = dataReader.GetInt32(1);
                        transferencia.CuentaDestino = dataReader.GetInt32(2);
                        transferencia.FechaHora = dataReader.GetDateTime(3);
                        transferencia.Descripcion = dataReader.GetString(4);
                        transferencia.Monto = dataReader.GetDecimal(5);
                        transferencia.Estado = dataReader.GetString(6);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return Ok(transferencia);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Transferencia> estadisticas = new List<Transferencia>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CuentaOrigen, CuentaDestino, FechaHora, Descripcion, Monto, Estado FROM Transferencia", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Transferencia transferencia = new Transferencia();

                        transferencia.Codigo = dataReader.GetInt32(0);
                        transferencia.CuentaOrigen = dataReader.GetInt32(1);
                        transferencia.CuentaDestino = dataReader.GetInt32(2);
                        transferencia.FechaHora = dataReader.GetDateTime(3);
                        transferencia.Descripcion = dataReader.GetString(4);
                        transferencia.Monto = dataReader.GetDecimal(5);
                        transferencia.Estado = dataReader.GetString(6);

                        estadisticas.Add(transferencia);
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
        public IHttpActionResult Ingresar(Transferencia transferencia)
        {
            if (transferencia == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Transferencia (CuentaOrigen, CuentaDestino, FechaHora, Descripcion, Monto, Estado) VALUES (@CuentaOrigen, @CuentaDestino, @FechaHora, @Descripcion, @Monto, @Estado)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CuentaOrigen", transferencia.CuentaOrigen);
                    sqlCommand.Parameters.AddWithValue("@CuentaDestino", transferencia.CuentaDestino);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", transferencia.FechaHora);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", transferencia.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Monto", transferencia.Monto);
                    sqlCommand.Parameters.AddWithValue("@Estado", transferencia.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(transferencia);
        }


        [HttpPut]
        public IHttpActionResult Actualizar(Transferencia transferencia)
        {
            if (transferencia == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Transferencia SET CuentaOrigen = @CuentaOrigen, CuentaDestino = @CuentaDestino, FechaHora = @FechaHora, Descripcion = @Descripcion, Monto = @Monto, Estado = @Estado WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", transferencia.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CuentaOrigen", transferencia.CuentaOrigen);
                    sqlCommand.Parameters.AddWithValue("@CuentaDestino", transferencia.CuentaDestino);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", transferencia.FechaHora);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", transferencia.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Monto", transferencia.Monto);
                    sqlCommand.Parameters.AddWithValue("@Estado", transferencia.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(transferencia);
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
                        new SqlCommand(@"DELETE Transferencia WHERE Codigo = @Codigo",
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
