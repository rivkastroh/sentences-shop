using ListSentence.interfases;
using ListSentence.Modols;
using ListSentence.Services;

namespace ListSentence.Services
{
    public class SentenceService : ISetenceService
    {
        private List<Sentence> List;
        public SentenceService()
        {
            List = new List<Sentence>
                {
                  new Sentence{Id =1, Content="פחות משנה מה תבחרי יותר משנה שתבחרי"},
                  new Sentence{Id= 2, Content ="איך אני אצא מהסיטואציה טובה יותר?!"},
                  new Sentence{Id= 3, Content ="פחות משנה מה תבחרי יותר משנה שתבחרי"},
                  new Sentence{Id= 4, Content ="אני רוצה יותר ולא היתר"},
                  new Sentence{Id= 5, Content ="יש צדיק גוזר וד' מקיים ויש ד' גוזר וצדיק מקיים"},
                  new Sentence{Id= 6, Content ="יש לי מלך!"},
                };
        }
        public void Add(Sentence sN)
        {
            int maxId = List.Max(sc => sc.Id);
            sN.Id = maxId + 1;
            List.Add(sN);
        }

        public void Delete(int id)
        {
            Sentence oldS = List.FirstOrDefault(s => s.Id == id);
            List.Remove(oldS);
        }

        public Sentence Get(int id)
        {
            return List.FirstOrDefault(i => id == i.Id);
        }

        public List<Sentence> GetAll()
        {
            return List;
        }

        public void UpDate(Sentence s)
        {
            Sentence oldS = List.FirstOrDefault(i => s.Id == i.Id);
            if (oldS != null)
            {
                oldS.Content = s.Content;
                oldS.Subject = s.Subject;
            }
        }
    }
    public static class SentenceServiceHelper
    {
        public static void AddSentenceService(this IServiceCollection services)
        {
            services.AddSingleton<ISetenceService, SentenceService>();
        }
    }
}