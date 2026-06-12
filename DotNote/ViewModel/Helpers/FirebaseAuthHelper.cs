using DotNote.Configuration;
using DotNote.DTOs;
using DotNote.Model;
using Microsoft.CognitiveServices.Speech.Transcription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace DotNote.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        private readonly HttpClient client = new HttpClient();

        public Task<bool> Register(FirebaseAuthDTO user)
        {
            return Authenticate(
                user,
                "accounts:signUp",
                "Registration Failed");
        }

        public Task<bool> Login(FirebaseAuthDTO user)
        {
            return Authenticate(
                user,
                "accounts:signInWithPassword",
                "Login Failed");
        }


        private async Task<bool> Authenticate(
            FirebaseAuthDTO user,
            string endpoint,
            string errorTitle)
        {
            var apiKey = AppSettings.Firebase.ApiKey;

            var body = new
            {
                email = user.Email,
                password = user.Password,
                returnSecureToken = true
            };
            var bodyJson = JsonConvert.SerializeObject(body);
            var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/{endpoint}?key={apiKey}", data);
            string resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) // if the response is not successful, return false
            {
                var error = JsonConvert.DeserializeObject<FirebaseError>(resultJson);
                MessageBox.Show($"{error.error.code} Error: {error.error.message}", errorTitle, MessageBoxButton.OK);
                return false;
            }

            var result = JsonConvert.DeserializeObject<FirebaseResponse>(resultJson);
            App.LoggedInUser = result; // store the user id for use in the app
            return true;
        }

        public async Task<bool> SendPasswordReset(string userEmail)
        {
            var apiKey = AppSettings.Firebase.ApiKey;

            var body = new
            {
                requestType = "PASSWORD_RESET",
                email = userEmail
            };
            var bodyJson = JsonConvert.SerializeObject(body);
            var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={apiKey}", data);

            return response.IsSuccessStatusCode;
        }

        #region Firebase Response Data Models
        public class FirebaseResponse
        {
            public string kind { get; set; }
            public string idToken { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
            public string localId { get; set; }
        }

        public class FirebaseError
        {
            public FirebaseErrorDetails error { get; set; }
        }

        public class FirebaseErrorDetails
        {
            public int code { get; set; }
            public string message { get; set; }
        }
        #endregion
    }
}
