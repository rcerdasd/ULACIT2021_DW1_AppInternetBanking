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
    public class EstadisticaManager
    {
        string UrlBase = "http://localhost:49220/api/Estadistica/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Estadistica> Ingresar(Estadistica estdistica, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(estdistica), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Estadistica>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Estadistica>> ObtenerEstadisticas(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Estadistica>>(response);
        }
    }
}