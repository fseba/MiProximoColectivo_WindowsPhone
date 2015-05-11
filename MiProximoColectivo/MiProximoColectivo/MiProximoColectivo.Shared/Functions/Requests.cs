using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.ServerReceived;
using Newtonsoft.Json;

namespace MiProximoColectivo.Functions
{
    public static class Requests
    {
        public static async Task<LatestPositions> RequestGetLastestPositions2()
        {
            try
            {
                string searchUrl = "http://www.miproximocolectivo.sanluis.gov.ar/api/EstadoUnidad/GetUltimasPosiciones";

                HttpClient client = new HttpClient();
                var data = await client.GetStringAsync(searchUrl);

                return JsonConvert.DeserializeObject<LatestPositions>(data);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static FramingHttpResponse<LatestPositions> RequestGetLastestPositions()
        {
            FramingHttpResponse<LatestPositions> receivedMessage = null;

            try
            {
                HttpRequestMessage hm = new HttpRequestMessage();
                hm.Method = HttpMethod.Get;
                hm.RequestUri = new Uri("http://www.miproximocolectivo.sanluis.gov.ar/api/EstadoUnidad/GetUltimasPosiciones");
                receivedMessage = MakeARequest<FramingHttpResponse<LatestPositions>>(hm).Result.Result;
            }
            catch (Exception ex)
            {
                receivedMessage = null;
            }
            return receivedMessage;
        }

        public static async Task<FramingHttpResponse<T>> MakeARequest<T>(HttpRequestMessage hm, bool extraHeaders = false)
        {
            FramingHttpResponse<T> result = new FramingHttpResponse<T>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.SendAsync(hm);

                    result.IsSuccess = response.IsSuccessStatusCode;
                    result.ReasonPhrase = response.ReasonPhrase;
                    result.StatusCode = response.StatusCode;

                    string responseString = null;

                    if (response.Content != null)
                        responseString = (response.Content.ReadAsStringAsync()).Result;

                    result.ResponseAsString = responseString;

                    if (response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NoContent)
                        result.Result = JsonConvert.DeserializeObject<T>(responseString);
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                break;
                            case HttpStatusCode.Forbidden:
                                break;
                            case HttpStatusCode.BadRequest:
                                break;
                            case HttpStatusCode.NoContent:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
