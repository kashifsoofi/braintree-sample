namespace BraintreeSample.Responses
{
    public class CreateTransactionResponse
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
