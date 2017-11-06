using Newtonsoft.Json;
using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

using HackAtHome.Entities;

namespace HackAtHome.SAL
{
    public class Services
    {
        /// <summary>
        /// Realiza la autenticación al servicio Web API.
        /// </summary>
        /// <param name="studentEmail">Correo del usuario</param>
        /// <param name="studentPassword">Password del usuario</param>
        /// <returns>Objeto ResultInfo con los datos del usuario y un token de
        /// autenticación.</returns>
        public async Task<ResultInfo> AutenticateAsync(
        string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;
            // Dirección base de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            // ID del diplomado.
            string EventID = "xamarin30";
            string RequestUri = "api/evidence/Authenticate";
            // El servicio requiere un objeto UserInfo con los datos del usuario y evento.
            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };
            // Utilizamos el objeto System.Net.Http.HttpClient para consumir el servicio REST
            // Debe instalarse el paquete NuGet System.Net.Http

            using (var Client = new HttpClient())
            {
                // Establecemos la dirección base del servicio REST
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                // Limpiamos encabezados de la petición.
                Client.DefaultRequestHeaders.Accept.Clear();
                // Indicamos al servicio que envie los datos en formato JSON.
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Serializamos a formato JSON el objeto a enviar.
                    // Debe instalarse el paquete NuGet Newtonsoft.Json.
                    var JSONUserInfo = JsonConvert.SerializeObject(User);
                    // Hacemos una petición POST al servicio enviando el objeto JSON
                    HttpResponseMessage Response =
                            await Client.PostAsync(RequestUri,
                            new StringContent(JSONUserInfo.ToString(),
                                              Encoding.UTF8, "application/json"));
                    // Leemos el resultado devuelto.
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    // Deserializamos el resultado JSON obtenido
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return Result;
        }

        ///<summary>
        /// Obtiene la lista de evidencias.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Una lista con las evidencias.</returns>
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;
            // Dirección base de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            // URI completo
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";
            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response =
                            await Client.GetAsync(URI);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto.
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return Evidences;
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario</param>
        /// <param name="evidenceID">Identificador de la evidencia.</param>
        /// <returns>Información de la evidencia.</returns>
        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;
            // Dirección base de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            // URI de la evidencia.
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
				Client.DefaultRequestHeaders.Accept.Clear();
				Client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

				try
				{
					// Realizamos una petición GET
					var Response = await Client.GetAsync(URI);
					var ResultWebAPI = await Response.Content.ReadAsStringAsync();
					if (Response.StatusCode == HttpStatusCode.OK)
					{
						// Si el estatus de la respuesta HTTP fue exitosa, leemos
						// el valor devuelto.
						Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
					}
				}
				catch (System.Exception)
				{
                    return null;
                }
                return Result;
            }
        }
    }
}