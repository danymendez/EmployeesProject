using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesProject.Utils.Helpers
{
   public class HelperServiceApi
    {
        public string BASEURL;

        public HelperServiceApi(UriHelpers uriHelpers)
        {
            BASEURL = uriHelpers.BaseUrl;
        }

        public virtual async Task<T> Get<T>(string urlMethod, int? id)
        {
            T entity = default(T);
            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(uri + urlMethod + id);

                if (response.IsSuccessStatusCode)
                    entity = await response.Content.ReadAsAsync<T>();

            }
            catch (Exception ex)
            {
                  throw ex;
            }
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAll<T>(string urlMethod)
        {
            IEnumerable<T> lista = new List<T>();
            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;

                HttpResponseMessage response = await client.GetAsync(uri + urlMethod);

                if (response.IsSuccessStatusCode)
                    lista = await response.Content.ReadAsAsync<IEnumerable<T>>();

            }
            catch (Exception ex)
            {
                  throw ex;
            }
            return lista;
        }

        public virtual async Task<T> Post<T>(string urlMethod, T clase)
        {

            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(uri + urlMethod, clase);

                if (response.IsSuccessStatusCode){
                    clase = await response.Content.ReadAsAsync<T>();
                }

                else {
                     throw new System.InvalidOperationException(response.Content.ReadAsStringAsync().Result);
                        }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return clase;
        }

        public virtual async Task<T> PostAuth<T>(string urlMethod, string Usuario, string Password)
        {
            T clase = default(T);

            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(uri + urlMethod + "?Usuario=" + Usuario + "&Password=" + Password, new StringContent(Usuario));

                if (response.IsSuccessStatusCode)
                    clase = await response.Content.ReadAsAsync<T>();


            }
            catch (Exception ex)
            {
                  throw ex;

            }
            return clase;
        }

        public virtual async Task<bool> Put<T>(string Method, T clase)
        {
            bool isSuccess = false;
            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync(uri + Method, clase);

                isSuccess = response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                  throw ex;
            }
            return isSuccess;
        }

        public virtual async Task<T> Delete<T>(string urlMethod, int? id)
        {
            try
            {
                HttpClient client = new HttpClient();
                string uri = BASEURL;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync(uri + urlMethod + id);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<T>();

            }
            catch (Exception ex)
            {
                  throw ex;
            }
            return default(T);
        }
    }
}
