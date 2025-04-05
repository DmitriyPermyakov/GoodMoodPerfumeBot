using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repository
{
    public class PaymentDetailsRepository
    {
        private readonly AppDatabaseContext context;
        public PaymentDetailsRepository(AppDatabaseContext context)
        {
            this.context = context;
        }

        public async Task<PaymentDetails> GetAsync()
        {
            var details = await this.context.PaymentDetails.ToListAsync();
            return details.FirstOrDefault();
        }

        public async Task Create(PaymentDetails details)
        {
            await this.context.PaymentDetails.AddAsync(details);
        }

        public void UpdateAsync(PaymentDetails details)
        {
            this.context.PaymentDetails.Update(details);
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
