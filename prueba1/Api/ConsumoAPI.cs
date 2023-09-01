using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Nancy.Json;
using Newtonsoft.Json.Linq;

namespace prueba1.Api
{
    internal class ConsumoAPI
    {
        private static readonly string svcURL = "https://www.mindicador.cl";
        private static readonly string pathAPI = "api";
        private static JavaScriptSerializer js = new JavaScriptSerializer();

        public static string ObtenerFecha()
        {
            string fechaResponse = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("api/uf/" + DateTime.Now.ToString("dd-MM-yyyy")).Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(result);
                    fechaResponse = json["serie"][0]["fecha"].ToString();
                }
            }

            return fechaResponse;
        }

        public static string ObtenerValorMoneda(string moneda)
        {
            string response = string.Empty;

            var result = LlamaServicio(moneda);
            JObject json = JObject.Parse(result);
            response = json["serie"][0]["valor"].ToString();

            return response;
        }

        private static string LlamaServicio(string moneda)
        {
            string EmpResponse = string.Empty;
            var qryString = string.Concat(pathAPI, "/", moneda, "/", DateTime.Now.ToString("dd-MM-yyyy"));
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync(qryString).Result;
                if (Res.IsSuccessStatusCode)
                {
                    EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    var objeto = js.DeserializeObject(EmpResponse);
                }
            }
            return EmpResponse;
        }
    }
}
