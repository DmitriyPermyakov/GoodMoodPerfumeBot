using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;

namespace GoodMoodPerfumeBot.Services
{
    public class PaymentDetailsService
    {
        private readonly PaymentDetailsRepository paymentDetails;

        public PaymentDetailsService(PaymentDetailsRepository paymentDetailsRepository)
        {
            paymentDetails = paymentDetailsRepository;
        }

        public async Task<PaymentDetails> GetDetailsAsync()
        {
            return await this.paymentDetails.GetAsync();
        }

        public async Task CreateAsync(PaymentDetails details)
        {
            await this.paymentDetails.Create(details);
            await this.paymentDetails.SaveAsync();
        }

        public async Task CreateOrUpdateAsync(PaymentDetails details)
        {
            PaymentDetails detailsFromDb = await this.GetDetailsAsync();
            if(detailsFromDb == null)
            {
                await this.CreateAsync(details);
            } else
            {
                detailsFromDb.Phone = details.Phone;
                detailsFromDb.CardNumber = details.CardNumber;
                this.paymentDetails.UpdateAsync(detailsFromDb);
                await this.paymentDetails.SaveAsync();
            }
        }
    }
}
