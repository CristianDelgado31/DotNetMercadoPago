using Newtonsoft.Json;

namespace DotNetMercadoPago.Entities;

public sealed class PaymentDataRequest
{
    public required string Description { get; set; }
    public required string CardholderName { get; set; }
    public required int Installments { get; set; }
    public required Payer Payer { get; set; }
    [JsonProperty("payment_method_id")]
    public required string PaymentMethodId { get; set; }
    public required string Token { get; set; }
    [JsonProperty("transaction_amount")]
    public required int TransactionAmount { get; set; }
}