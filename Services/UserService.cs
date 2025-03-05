using System.Text;
using System.Text.Json;
using ListSentence.interfases;
using ListSentence.Modols;
using ListSentence.Services;

namespace ListSentence.Services
{
    public class UserService : IUserService
    {
        private List<User> List;
        public UserService()
        {
            List = ReadUsersFromJson("./JSON/users.json");
        }
        public void Add(User uN)
        {
            if (List.FirstOrDefault(u => u.Id == uN.Id) == null)
            {
                if (!List.Any(u => u.Id == uN.Id))
                    List.Add(uN);
            }
            WriteUsersToJson("./JSON/users.json", List);
        }

        public void Delete(int id)
        {
            User oldU = List.FirstOrDefault(u => u.Id == id);
            List.Remove(oldU);
            WriteUsersToJson("./JSON/users.json", List);
        }

        public User Get(int id=-1,string email =null)
        {
            if(id==-1){
                return List.FirstOrDefault(u=> email.Equals(u.Email));
            }
            return List.FirstOrDefault(u => id == u.Id);
        }

        public List<User> GetAll()
        {
            return List;
        }

        public void UpDate(User u)
        {
            User oldU = List.FirstOrDefault(i => u.Id == i.Id);
            if (oldU != null)
            {
                oldU.Name = u.Name;
                oldU.password = u.password;
                oldU.SetenceIds = u.SetenceIds;
                oldU.Email=u.Email;
            }
            WriteUsersToJson("./JSON/users.json", List);
        }

        private static List<User> ReadUsersFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json);
        }
        public static void WriteUsersToJson(string filePath, List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json, Encoding.UTF8); // ודא שאתה משתמש בקידוד UTF-8
        }
    }
    public static class UserServiceHelper
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
        }
    }
}