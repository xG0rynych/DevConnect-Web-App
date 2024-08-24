using RelevancheSearchAPI.Data;
using RelevancheSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace RelevancheSearchAPI
{
    public class Facade
    {
        OpenAIAPI _openAI;
        SearchDbContext _context;
        string _openAiKey;
        const string _aiModel = "text-embedding-3-small";
        const int _dimensions = 1536;
        const double _minSimilarity = 0.50;
        public Facade(SearchDbContext context, IConfiguration configuration)
        {
            _context = context;
            _openAiKey = configuration["ExternalAPIs:OpenAI"]!;
            _openAI = new OpenAIAPI(_openAiKey);
        }

        public async Task<float[]> WordsToVector(string str)
        {
            float[] embeddign = await _openAI.Embeddings.GetEmbeddingsAsync(str, _aiModel, _dimensions);
            return embeddign;
        }

        public double CosineSimilarity(float[] A, float[]B)
        {
            double normA = 0;
            double normB = 0;
            double product = 0;
            for (int i = 0; i < _dimensions; i++)
            {
                product += A[i] * B[i];
                normA += Math.Pow(A[i], 2);
                normB += Math.Pow(B[i], 2);
            }
            normA = Math.Sqrt(normA);
            normB = Math.Sqrt(normB);
            return product / (normA * normB);
        }

        //Articles

        public async Task<bool> AddArticleAsync(int id, string title)
        {
            if (await _context.ArticleVectors.FirstOrDefaultAsync(x=>x.ArticleId==id) == null)
            {
                List<ArticleVectors> articles = await _context.ArticleVectors.ToListAsync();

                float[] embedding = await WordsToVector(title);
                string result = JsonSerializer.Serialize(embedding);
                ArticleVectors article = new ArticleVectors() { ArticleId = id, Vector = result };
                await _context.ArticleVectors.AddAsync(article);
                await _context.SaveChangesAsync();

                if(articles.IsNullOrEmpty()==false)
                {
                    foreach (var item in articles)
                    {
                        float[] vec = JsonSerializer.Deserialize<float[]>(item.Vector)!;
                        double cosSim = CosineSimilarity(embedding, vec);
                        await _context.ArticlesCosSims.AddAsync(new ArticlesCosSim()
                        {
                            ArticleId1 = id,
                            ArticleId2 = item.ArticleId,
                            CosSimilarity = cosSim
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<List<int>?> SearchArticlesAsync(string title)
        {
            float[] vecA = await WordsToVector(title);
            var articles = await _context.ArticleVectors.ToListAsync();

            List<int> result = new List<int>();
            foreach (var article in articles)
            {
                float[] vecB = JsonSerializer.Deserialize<float[]>(article.Vector)!;
                double cossim = CosineSimilarity(vecA, vecB);
                if (cossim >= _minSimilarity)
                    result.Add(article.ArticleId);
            }
            return result;
        }

        public async Task<List<int>?> GetSimilarArticles(int articleId)
        {
            var data1 = await _context.ArticlesCosSims
                .Where(x => x.ArticleId1 == articleId && x.CosSimilarity>=_minSimilarity)
                .OrderByDescending(x => x.CosSimilarity)
                .Select(x=>x.ArticleId2)
                .ToListAsync();
            var data2 = await _context.ArticlesCosSims
                .Where(x => x.ArticleId2 == articleId && x.CosSimilarity >= _minSimilarity)
                .OrderByDescending(x => x.CosSimilarity)
                .Select(x => x.ArticleId1)
                .ToListAsync();

            data1.AddRange(data2);
            if(data1!=null)
            {
                data1.Sort();
            }
            return data1;
        }

        public async Task<bool> RemoveArticle(int id)
        {
            ArticleVectors? article = await _context.ArticleVectors.FirstOrDefaultAsync(x => x.ArticleId == id);
            if (article != null)
            {
                _context.ArticleVectors.Remove(article);
                var arr = await _context.ArticlesCosSims
                    .Where(x => x.ArticleId1 == id || x.ArticleId2 == id).ToListAsync();
                if (arr != null)
                    _context.RemoveRange(arr);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //Questions

        public async Task<bool> AddQuestionAsync(int id, string title)
        {
            if (await _context.QuestionVectors.FirstOrDefaultAsync(x => x.QuestionId == id) == null)
            {
                List<QuestionVectors> questions = await _context.QuestionVectors.ToListAsync();

                float[] embedding = await WordsToVector(title);
                string result = JsonSerializer.Serialize(embedding);
                QuestionVectors question = new QuestionVectors() { QuestionId = id, Vector = result };
                await _context.QuestionVectors.AddAsync(question);
                await _context.SaveChangesAsync();



                if (questions.IsNullOrEmpty() == false)
                {
                    foreach (var item in questions)
                    {
                        float[] vec = JsonSerializer.Deserialize<float[]>(item.Vector)!;
                        double cosSim = CosineSimilarity(embedding, vec);
                        await _context.QuestionsCosSims.AddAsync(new QuestionsCosSim()
                        {
                            QuestionId1 = id,
                            QuestionId2 = item.QuestionId,
                            CosSimilarity = cosSim
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<List<int>?> SearchQuestionsAsync(string title)
        {
            float[] vecA = await WordsToVector(title);
            var questions = await _context.QuestionVectors.ToListAsync();

            List<int> result = new List<int>();
            foreach (var question in questions)
            {
                float[] vecB = JsonSerializer.Deserialize<float[]>(question.Vector)!;
                double cossim = CosineSimilarity(vecA, vecB);
                if (cossim >= 0.70)
                    result.Add(question.QuestionId);
            }
            return result;
        }

        public async Task<List<int>?> GetSimilarQuestions(int questionId)
        {
            var data1 = await _context.QuestionsCosSims
                .Where(x => x.QuestionId1 == questionId && x.CosSimilarity >= _minSimilarity)
                .OrderByDescending(x => x.CosSimilarity)
                .Select(x => x.QuestionId2)
                .ToListAsync();
            var data2 = await _context.QuestionsCosSims
                .Where(x => x.QuestionId2 == questionId && x.CosSimilarity >= _minSimilarity)
                .OrderByDescending(x => x.CosSimilarity)
                .Select(x => x.QuestionId1)
                .ToListAsync();

            data1.AddRange(data2);
            if (data1 != null)
            {
                data1.Sort();
            }
            return data1;
        }

        public async Task<bool> RemoveQuestion(int id)
        {
            QuestionVectors? question = await _context.QuestionVectors.FirstOrDefaultAsync(x => x.QuestionId == id);
            if (question != null)
            {
                _context.QuestionVectors.Remove(question);
                var arr = await _context.QuestionsCosSims
                    .Where(x => x.QuestionId1 == id || x.QuestionId2 == id).ToListAsync();
                if (arr != null)
                    _context.RemoveRange(arr);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
