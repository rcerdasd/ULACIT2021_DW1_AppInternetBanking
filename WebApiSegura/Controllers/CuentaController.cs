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
    [RoutePrefix("api/Cuenta")]
    public class CuentaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Cuenta cuenta = new Cuenta();
            try
            {
                using (SqlConnection sqlConnection = new 
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, CodigoMoneda, 
                                                             Descripcion, IBAN, Saldo, Estado
                                                             FROM   Cuenta
                                                             WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo",id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while(sqlDataReader.Read())
                    {
                        cuenta.Codigo = sqlDataReader.GetInt32(0);
                        cuenta.CodigoUsuario = sqlDataReader.GetInt32(1);
                        cuenta.CodigoMoneda = sqlDataReader.GetInt32(2);
                        cuenta.Descripcion = sqlDataReader.GetString(3);
                        cuenta.IBAN = sqlDataReader.GetString(4);
                        cuenta.Saldo = sqlDataReader.GetDecimal(5);
                        cuenta.Estado = sqlDataReader.GetString(6);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(cuenta);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, CodigoMoneda, 
                                                             Descripcion, IBAN, Saldo, Estado
                                                             FROM   Cuenta", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Cuenta cuenta = new Cuenta();
                        cuenta.Codigo = sqlDataReader.GetInt32(0);
                        cuenta.CodigoUsuario = sqlDataReader.GetInt32(1);
                        cuenta.CodigoMoneda = sqlDataReader.GetInt32(2);
                        cuenta.Descripcion = sqlDataReader.GetString(3);
                        cuenta.IBAN = sqlDataReader.GetString(4);
                        cuenta.Saldo = sqlDataReader.GetDecimal(5);
                        cuenta.Estado = sqlDataReader.GetString(6);

                        cuentas.Add(cuenta);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(cuentas);
        }

        [HttpGet]
        [Route("Cliente")]
        public IHttpActionResult GetAllClientAccounts(int codigoUsuario)
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, CodigoMoneda, 
                                                             Descripcion, IBAN, Saldo, Estado
                                                             FROM  Cuenta
                                                             WHERE CodigoUsuario = @CodigoUsuario", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", codigoUsuario);
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Cuenta cuenta = new Cuenta();
                        cuenta.Codigo = sqlDataReader.GetInt32(0);
                        cuenta.CodigoUsuario = sqlDataReader.GetInt32(1);
                        cuenta.CodigoMoneda = sqlDataReader.GetInt32(2);
                        cuenta.Descripcion = sqlDataReader.GetString(3);
                        cuenta.IBAN = sqlDataReader.GetString(4);
                        cuenta.Saldo = sqlDataReader.GetDecimal(5);
                        cuenta.Estado = sqlDataReader.GetString(6);

                        cuentas.Add(cuenta);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(cuentas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Cuenta cuenta)
        {
            if (cuenta == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = 
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = 
                        new SqlCommand(@" INSERT INTO Cuenta (CodigoUsuario, CodigoMoneda, Descripcion, 
                                                                IBAN, Saldo, Estado) 
                                         VALUES (@CodigoUsuario, @CodigoMoneda, @Descripcion, @IBAN, @Saldo, @Estado)",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", cuenta.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@CodigoMoneda", cuenta.CodigoMoneda);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", cuenta.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@IBAN", cuenta.IBAN);
                    sqlCommand.Parameters.AddWithValue("@Saldo", cuenta.Saldo);
                    sqlCommand.Parameters.AddWithValue("@Estado", cuenta.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(cuenta);
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Cuenta cuenta)
        {
            if (cuenta == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@" UPDATE Cuenta 
                                                        SET CodigoUsuario = @CodigoUsuario, 
                                                            CodigoMoneda = @CodigoMoneda,
                                                            Descripcion = @Descripcion, 
                                                            IBAN = @IBAN, 
                                                            Saldo = @Saldo, 
                                                            Estado = @Estado 
                                          WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", cuenta.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", cuenta.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@CodigoMoneda", cuenta.CodigoMoneda);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", cuenta.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@IBAN", cuenta.IBAN);
                    sqlCommand.Parameters.AddWithValue("@Saldo", cuenta.Saldo);
                    sqlCommand.Parameters.AddWithValue("@Estado", cuenta.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(cuenta);
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
                        new SqlCommand(@" DELETE Cuenta WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo",id);

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
