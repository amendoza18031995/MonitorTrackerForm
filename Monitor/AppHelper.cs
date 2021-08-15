using MonitorTrackerForm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MonitorTrackerForm
{
    public class AppHelper
    {
        static HttpClient client = new HttpClient();
        public static async Task<bool> ValidateKeyAsync(string key)
        {
            try
            {
                Response rep = new Response();
                writelog("ValidateKeyAsync --- ", string.Empty);
                rep = await CallApiValidate("Validate", key);
                return rep.validkey;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private static async Task<Response> CallApiValidate(string rute, object obj)
        {
            ResponseValidateInstaller rep = new ResponseValidateInstaller();
            try
            {
                writelog("CallApiValidate------------", string.Empty);
                
                client = new HttpClient();
                //local
                client.BaseAddress = new Uri("http://localhost:50221/Api/");

                //prod
                //client.BaseAddress = new Uri("http://25.73.18.157:8085/api/");


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                writelog("Antes del metodo que llama la api", string.Empty);
                response = await client.PostAsJsonAsync(rute, obj);
                writelog("api responde en metodo", string.Empty);
                var cont = response.Content;

                writelog("seriealizando", string.Empty);
                var data = await response.Content.ReadAsStringAsync();
                JObject job = JObject.Parse(data.ToString());
                return JsonConvert.DeserializeObject<Response>(job.ToString());
            }
            catch (Exception ex)
            {
                writelog("Error en llado api: " + ex.Message, string.Empty);
                throw ex;
            }
        }

        public static void writelog(string msg, string module)
        {
            string appdirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string directory = "C:\\Logs";
            string path = "C:\\Logs\\log.txt";


            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("Iniciando");
                sw.WriteLine(appdirectory);

                sw.WriteLine(msg);
                sw.Close();
            }
            else
            {
                List<string> lines = new List<string>();
                lines.Add(msg + " --/ " + module);
                File.AppendAllLines(path, lines);
            }
        }
    }
}
