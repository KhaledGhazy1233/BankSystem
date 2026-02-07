namespace Domainlayer.BankSystem.Requests
{
    public class GetUserActivityRequest
    {
        public int UserId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
}
