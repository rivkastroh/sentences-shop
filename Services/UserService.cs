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
            //בעיקרון כאן כדאי קריאה מדטה בייס
            List = new List<User>
                {
                 new User{Id=327824348,Name="רבקה",pasword="12345678",SetenceIds=new List<int> { 1, 2, 3 }},
                 new User{Id=1,Name="רחל",pasword="87654321",SetenceIds=new List<int> { 4, 3 }},
                 new User{Id=1,Name="שרה",pasword="אבגדהוזח",SetenceIds=new List<int> { 1, 5, 3 }},
                 new User{Id=1,Name="לאה",pasword="חזוהדגבא",SetenceIds=new List<int> { 1, 2, 6 }},
                };
        }
        public void Add(User uN)
        {
            if (List.FirstOrDefault(u => u.Id == uN.Id) == null)
            {
                List.Add(uN);
            }
        }

        public void Delete(int id)
        {
            User oldU = List.FirstOrDefault(u => u.Id == id);
            List.Remove(oldU);
        }

        public User Get(int id)
        {
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
                oldU.pasword = u.pasword;
                oldU.SetenceIds = u.SetenceIds;
            }
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