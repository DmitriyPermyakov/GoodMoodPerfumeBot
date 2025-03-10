using Bogus;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Seed
{
    public static class FakeProductGenerator
    {
        public static List<Product> GenerateProductList(int count = 10)
        {
            var categories = new[] { "category 1", "category 2", "category 3" };

            return new Faker<Product>("ru")
                .RuleFor(p => p.ProductId, f => f.IndexFaker + 1)
                .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                .RuleFor(p => p.ProductDescription, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.ProductPrice, f => Math.Round(f.Random.Double(1000, 10000), 2))
                .RuleFor(p => p.ProductImageUrls, f => [$"https://plumgoodness.com/cdn/shop/files/MKD_01.jpg?v=1728452056&width=460"])
                .Generate(count);
        }
    }
}
