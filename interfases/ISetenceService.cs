using ListSentence.Modols;

namespace ListSentence.interfases
{
    public interface ISetenceService
    {
        List<Sentence> GetAll();
        Sentence Get(int id);
        void Add(Sentence s);
        void Delete(int id);
        void UpDate(Sentence s);
    }
    
}