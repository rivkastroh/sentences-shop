using ListSentence.Modols;

namespace ListSentence.interfases
{
    public interface IUserService
    {
        List<User> GetAll();
        User Get(int id);
        void Add(User s);
        void Delete(int id);
        void UpDate(User s);
    }
    
}