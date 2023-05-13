 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestMAUI
{
    public class MainViewModel
    {
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        string baseUrl = "https://6447565450c253374422a3c0.mockapi.io";
        private List<User> Users;

        public MainViewModel()
        {
            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }

        public ICommand GetAllUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data =
                        await JsonSerializer.DeserializeAsync<List<User>>(responseStream, _serializerOptions);
                        Users = data;
                    }
                }

            });

        public ICommand GetSingleUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users/25";
                var response = await client.GetStringAsync(url);
            });

        public ICommand AddUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users";
                var user =
                new User
                {
                    createdAt = DateTime.Now,
                    name = "Steven",
                    avatar = "https://stevenlizarzaburupezua.com/wp-content/uploads/2021/12/Foto-de-Perfil.png"
                };
                string json = JsonSerializer.Serialize<User>(user, _serializerOptions);

                StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

            });

        public ICommand UpdateUserCommand =>
            new Command(async () =>
            {
                var user = Users.FirstOrDefault(x => x.id == "1");

                var url = $"{baseUrl}/users/1";

                user.name = "Miguel";

                string json = JsonSerializer.Serialize<User>(user, _serializerOptions);

                StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, content);

            });

        public ICommand DeleteUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users/10";
                var response = await client.DeleteAsync(url);
            });

    }
}
