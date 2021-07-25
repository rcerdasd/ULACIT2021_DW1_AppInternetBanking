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
    [RoutePrefix("api/CuentasCliente")]
    public class CuentasClienteController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAll(int id)
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, CodigoMoneda, 
                                                             Descripcion, IBAN, Saldo, Estado
                                                             FROM   Cuenta WHERE CodigoUsuario = @CodigoUsuario", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", id);
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
    }
}
