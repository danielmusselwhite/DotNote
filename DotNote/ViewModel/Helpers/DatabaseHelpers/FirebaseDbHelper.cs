using DotNote.Configuration;
using DotNote.Model;
using Newtonsoft.Json;
using SQLite;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DotNote.ViewModel.Helpers.DatabaseHelpers
{
    public class FirebaseDbHelper : IDatabaseHelper
    {
        private static string dbPath = AppSettings.Firebase.DatabaseURL;
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> Insert<T>(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json"); 

            var response = await client.PostAsync($"{dbPath}/{typeof(T).Name.ToLower()}.json", content); // Send to endpoint based on type name (e.g. "notebooks", "notes")

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update<T>(T item) where T : IHasId, new()
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"{dbPath}/{typeof(T).Name.ToLower()}/{item.Id}.json", content); // Send to endpoint based on type name + the Id of the item to update (e.g. "notebooks/{id}", "notes/{id}")

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete<T>(T item) where T : IHasId, new()
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.DeleteAsync($"{dbPath}/{typeof(T).Name.ToLower()}/{item.Id}.json"); // Send to endpoint based on type name + the Id of the item to delete (e.g. "notebooks/{id}", "notes/{id}")

            return response.IsSuccessStatusCode;
        }

        public async Task<List<T>> GetAll<T>() where T : IHasId, new()
        {
            var response = await client.GetAsync($"{dbPath}/{typeof(T).Name.ToLower()}.json"); // Send to endpoint based on type name (e.g. "notebooks", "notes")

            if (!response.IsSuccessStatusCode) return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            
            // deconstruct the response dictionary into a list of objects of type T
            var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(responseJson);

            var list = new List<T>();
            
            if(objects != null)
            {
                foreach (var o in objects)
                {
                    o.Value.Id = o.Key; // assign the Firebase-generated key as the Id of the object
                    list.Add(o.Value); // add the object to the list
                }
            }
            
            return list;
        }
    }
}
