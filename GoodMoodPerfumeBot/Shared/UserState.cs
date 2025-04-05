using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Shared
{
    public class UserState
    {
        public Step Step { get; set; } = Step.None;
        public PaymentDetails PaymentDetails { get; set; } = new PaymentDetails();
    }

    public enum Step
    {
        None,
        WaitingForPhone,
        WaitingForCardNumber,
        Completed
    }
}
