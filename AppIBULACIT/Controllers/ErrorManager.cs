using AppIBULACIT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppIBULACIT.Controllers
{
    public class ErrorManager
    {
        string UrlBase = "http://localhost:49220/api/Error/";


        public async Task<Error> Ingresar(Error error)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Error>> ObtenerErrores(string token)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Error>>(response);
        }
    }
}