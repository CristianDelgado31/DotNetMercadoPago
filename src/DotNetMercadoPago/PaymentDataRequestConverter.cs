using DotNetMercadoPago.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetMercadoPago;

public class PaymentDataRequestConverter : JsonConverter<PaymentDataRequest>
{
    public override void WriteJson(JsonWriter writer, PaymentDataRequest? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override PaymentDataRequest? ReadJson(JsonReader reader, Type objectType, PaymentDataRequest? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        
        var description  = jo["description"]?.ToString() ?? throw new ArgumentNullException("Description can not be null");
        if (description.Length == 0)
            throw new ArgumentException("Description can not be empty");
        var cardholderName = jo["cardholderName"]?.ToString() ?? throw new ArgumentNullException("cardholderName can not be null");
        if (cardholderName.Length == 0)
            throw new ArgumentException("CardHolderName can not be empty");
        var installments  = jo["installments"]?.ToString() ?? throw new ArgumentNullException("Installments can not be null");
        int parsedInstallments;
        if (int.TryParse(installments, out parsedInstallments) == false)
            throw new ArgumentException("Installments type is not integer");
        
        var jsonPayer = jo["payer"]?.ToString() ?? throw new ArgumentNullException("Payer can not be null");
        var payerEmail = jo["payer"]?["email"]?.ToString() ?? throw new ArgumentNullException("Email can not be null");
        if (payerEmail.Length == 0)
            throw new ArgumentException("Email can not be empty");
        var identificationType = jo["payer"]?["identification"]?["type"]?.ToString() ?? throw new ArgumentNullException("type can not be null");
        if (identificationType.Length == 0)
            throw new ArgumentException("IdentificationType can not be empty");
        var identificationNumber = jo["payer"]?["identification"]?["number"]?.ToString() ?? throw new ArgumentNullException("Number can not be null");
        if (identificationNumber.Length == 0)
            throw new ArgumentException("IdentificationNumber can not be empty");
        var paymentMethodId = jo["payment_method_id"]?.ToString() ?? throw new ArgumentNullException("PaymentMethodId can not be null");
        if (paymentMethodId.Length == 0)
            throw new ArgumentException("PaymentMethodId can not be empty");
        var token = jo["token"]?.ToString() ?? throw new ArgumentNullException("Token can not be null");
        if (token.Length == 0)
            throw new ArgumentException("Token can not be empty");
        var transactionAmount = jo["transaction_amount"]?.ToString() ?? throw new ArgumentNullException("TransactionAmount can not be null");
        int parsedTransactionAmount;
        if(int.TryParse(transactionAmount, out parsedTransactionAmount) == false)
            throw new ArgumentException("TransactionAmount type is not integer");

        var identification = new Identification
        {
            Type = identificationType,
            Number = identificationNumber
        };
        var payer = new Payer
        {
            Email = payerEmail,
            Identification = identification
        };
        return new PaymentDataRequest
        {
            Description = description,
            CardholderName = cardholderName,
            Installments = parsedInstallments,
            Payer = payer,
            PaymentMethodId = paymentMethodId,
            Token = token,
            TransactionAmount = parsedTransactionAmount
        };
    }
}