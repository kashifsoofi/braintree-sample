using Braintree;
using BraintreeSample.Requests;
using BraintreeSample.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BraintreeSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
            TransactionStatus.AUTHORIZED,
            TransactionStatus.AUTHORIZING,
            TransactionStatus.SETTLED,
            TransactionStatus.SETTLING,
            TransactionStatus.SETTLEMENT_CONFIRMED,
            TransactionStatus.SETTLEMENT_PENDING,
            TransactionStatus.SUBMITTED_FOR_SETTLEMENT
        };

        private readonly IBraintreeGateway braintreeGateway;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(
            IBraintreeGateway braintreeGateway,
            ILogger<TransactionsController> logger)
        {
            this.braintreeGateway = braintreeGateway;
            this.logger = logger;
        }

        [HttpGet("client-token")]
        public async Task<ClientTokenResponse> GetClientTokenAsync()
        {
            var clientToken = await this.braintreeGateway.ClientToken.GenerateAsync();

            return new ClientTokenResponse
            {
                ClientToken = clientToken,
            };
        }

        [HttpPost]
        public async Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = request.Amount,
                PaymentMethodNonce = request.PaymentMethodNonce,
                DeviceData = request.DeviceData,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                },
            };

            var result = await this.braintreeGateway.Transaction.SaleAsync(transactionRequest);

            return new CreateTransactionResponse
            {
                IsSuccess = result.IsSuccess(),
                TransactionId = result.Transaction?.Id,
                Errors = result.Errors?.All().Select(x => new Error { Code = (int)x.Code, Message = x.Message }),
            };
        }

        [HttpGet("{id}:status")]
        public async Task<string> GetTransactionStatusAsync(string id)
        {
            var transaction = await this.braintreeGateway.Transaction.FindAsync(id);
            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                return "success";
            }

            return "failed";
        }
    }
}