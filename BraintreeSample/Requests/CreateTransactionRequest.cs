namespace BraintreeSample.Requests
{
    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string? DeviceData { get; set; }
    }
}
