using Bogus;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Seed
{
    public static class FakeProductGenerator
    {
        public static List<Product> GenerateProductList(int count = 10)
        {
            var categories = new[] { "мужские", "женские" };

            return new Faker<Product>("ru")
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.Price, f => Math.Round(f.Random.Double(1000, 10000), 2))
                .RuleFor(p => p.ImageUrl, f => $"https://plumgoodness.com/cdn/shop/files/MKD_01.jpg?v=1728452056&width=460")
                .Generate(count);
        }
    }
}
