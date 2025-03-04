using System.Text;
using System.Text.Json;
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
            List = ReadSentencesFromJson("./JSON/sentence.json");
        }
        public void Add(Sentence sN)
        {
            int maxId = List.Max(sc => sc.Id);
            sN.Id = maxId + 1;
            List.Add(sN);
            WriteSentencesToJson("./JSON/sentence.json", List);
        }
        public void Delete(int id)
        {
            Sentence oldS = List.FirstOrDefault(s => s.Id == id);
            List.Remove(oldS);
            WriteSentencesToJson("./JSON/sentence.json", List);
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
            WriteSentencesToJson("./JSON/sentence.json", List);
        }

        private void WriteSentencesToJson(string filePath, List<Sentence> sentences)
        {
            var json = JsonSerializer.Serialize(sentences, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json, Encoding.UTF8); // ודא שאתה משתמש בקידוד UTF-8
        }
        private List<Sentence> ReadSentencesFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Sentence>>(json);
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