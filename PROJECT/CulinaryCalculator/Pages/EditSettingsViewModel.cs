using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditSettingsViewModel : ModalPageBaseViewModel
    {
        private Settings m_settings;

        private string m_Token;

        public ICommand SignIn { get; }

        public ICommand SignUp { get; }

        public ICommand UploadToCloud { get; }

        public ICommand RestoreFromCloud { get; }

        private string m_ErrorText;
        public string ErrorText
        {
            get { return m_ErrorText; }
            set { Set(ref m_ErrorText, value); }
        }

        private bool m_HasError;
        public bool HasError
        {
            get { return m_HasError; }
            set { Set(ref m_HasError, value); }
        }

        private bool m_Succeded;
        public bool Succeded
        {
            get { return m_Succeded; }
            set { Set(ref m_Succeded, value); }
        }

        private string m_SucceededText;

        public string SucceededText
        {
            get { return m_SucceededText; }
            set { Set(ref m_SucceededText, value); }
        }


        private bool m_useDisk;
        public bool UseDisk
        {
            get { return m_useDisk; }
            set { Set(ref m_useDisk, value); }
        }

        private bool m_UseCloud;
        public bool UseCloud
        {
            get { return m_UseCloud; }
            set { Set(ref m_UseCloud, value); }
        }

        private string m_ServerUrl;
        public string ServerUrl
        {
            get { return m_ServerUrl; }
            set { Set(ref m_ServerUrl, value); }
        }

        private string m_Login;
        public string Login
        {
            get { return m_Login; }
            set { Set(ref m_Login, value); }
        }

        private string m_password;
        public string Password
        {
            get { return m_password; }
            set { Set(ref m_password, value); }
        }

        public EditSettingsViewModel(INavigation navigation) : base(navigation)
        {
            m_settings = RecipesRepository.GetSettings();
            UseCloud = m_settings.UseCloud;
            UseDisk = !UseCloud;
            ServerUrl = m_settings.ServerUrl;
            Login = m_settings.Login;
            Password = m_settings.Password;
            SignIn = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password), _ => DoSignIn());
            SignUp = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password), _ => DoSignUp());
            UploadToCloud = new PropertyDependentCommand(this, _ => m_Token != null, _ => DoUploadToCloud());
            RestoreFromCloud = new PropertyDependentCommand(this, _ => m_Token != null, _ => DoRestoreFromCloud());
        }

        protected override void DoSave()
        {
            m_settings.Login = Login;
            m_settings.Password = Password;
            m_settings.ServerUrl = ServerUrl;
            m_settings.UseCloud = UseCloud;
            RecipesRepository.UpdateSettings(m_settings);
            base.DoSave();
        }

        private void DoSignIn()
        {
            string token = GetToken();

            if (token == null)
            {
                SetError("Ошибка авторизации");
            }
            else
            {
                m_Token = token;
                SetSucceded("Авторизация выполнена");
            }
        }

        private void DoSignUp()
        {
            string result = TrySignUp();
            if (string.IsNullOrEmpty(result))
            {
                SetSucceded("Регистрация выполнена");
            }
            else
            {
                SetError($"Ошибка регистрации. Cообщение сервера: {result}.");
            }
        }

        private string TrySignUp()
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "login", Login},
                    { "password", Password}
                };

                var content = new FormUrlEncodedContent(values);
                string url = $"{ServerUrl}/user";
                var response = client.PostAsync(url, content).Result;

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                else
                {
                    return "проблема соединения с сервером";
                }
            }
        }

        private string GetToken()
        {
            var client = new HttpClient();
            string url = $"{ServerUrl}/token?login={Login.Trim()}&password={Password}";
            var response = client.GetAsync(url).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return null;
            }
        }

        private void SetError(string description)
        {
            HasError = true;
            Succeded = false;
            SucceededText = string.Empty;
            ErrorText = description;
        }

        private void SetSucceded(string description)
        {
            HasError = false;
            Succeded = true;
            ErrorText = string.Empty;
            SucceededText = description;
        }

        private void DoRestoreFromCloud()
        {
            if (TryRestoreFromCloud())
            {
                SetSucceded("Книга рецептов восстановлена из облака.");
            }
            else
            {
                SetError("Ошибка соедениния с сервером.");
            }
        }

        private bool TryRestoreFromCloud()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_Token);

                var response = client.GetAsync($"{ServerUrl}/file").Result;

                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    if (File.Exists(RecipesRepository.RecipesDBPath))
                    {
                        File.Move(RecipesRepository.RecipesDBPath, RecipesRepository.RecipesDBPath + DateTime.Now.Ticks.ToString());
                    }

                    using (var fs = new FileStream(RecipesRepository.RecipesDBPath, FileMode.CreateNew))
                    {
                        response.Content.CopyToAsync(fs).Wait();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void DoUploadToCloud()
        {
            if (TryUploadToCloud())
            {
                SetSucceded("Книга рецептов сохранена в облаке.");
            }
            else
            {
                SetError("Ошибка соедениния с сервером.");
            }
        }

        private bool TryUploadToCloud()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_Token);

                var content = new MultipartFormDataContent("boundary");
                content.Headers.ContentType.MediaType = "multipart/form-data";
                using (Stream fileStream = File.OpenRead(RecipesRepository.RecipesDBPath))
                {
                    content.Add(new StreamContent(fileStream), "file", "file.db"); ;
                    var response = client.PostAsync($"{ServerUrl}/file", content).Result;
                    if (response?.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
