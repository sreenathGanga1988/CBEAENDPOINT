using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApp.Services
{
    public static class HttpTaskService
    {
 
 //public static string baseuri { get; set; } = "http://sreenathganga-001-site6.btempurl.com/";
 //public static string baseuri { get; set; } = "https://localhost:44396/";

        public static string token { get; set; } = "https://localhost:44396/";
         public static string baseuri { get; set; } = AppSettingsProvider.BaseApiUri;
        public static async Task<HttpResponseMessage> PostAsyncwithoutheader(string Apiurl, Object Objdata)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(Objdata);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(Apiurl, data);

            }
            return response;
        }
     



        public static async Task<HttpResponseMessage> GetAsyncData(string Apiurl)
        {

           
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync(Apiurl);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> PutAsyncData(string Apiurl, Object Objdata)
        {
          
            HttpResponseMessage response = new HttpResponseMessage();
         
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(Objdata);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PutAsync(Apiurl, data);

            }
            return response;
        }


        public static async Task<HttpResponseMessage> PostAsyncData(string Apiurl, Object Objdata)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
              
                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(Objdata);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(Apiurl, data);

            }
            return response;
        }


        public static async Task<HttpResponseMessage> DeleteAsyncData(string Apiurl)
        {


            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.DeleteAsync(Apiurl);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsyncData(string Apiurl, Object obj)
        {


            HttpResponseMessage response = new HttpResponseMessage();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var data = JsonConvert.SerializeObject(obj);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                response = await client.PostAsync(Apiurl, content);

            }
            return response;
        }

        public static async Task<HttpResponseMessage> PostMediaAsyncData(string Apiurl, MultipartFormDataContent Objdata)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseuri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
               
                response = await client.PostAsync(Apiurl, Objdata);

            }
            return response;
        }

    }


    
}
