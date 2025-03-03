using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Newtonsoft.Json;

namespace DotNetMercadoPago;

using PaymentDataRequest = Entities.PaymentDataRequest;

public sealed class MercadoPagoPayment
{
    public MercadoPagoPayment(string accessToken)
    {
        MercadoPagoConfig.AccessToken = accessToken;
    }

    /// <summary>
    /// You must use this method with the Checkout PRO payment.
    /// </summary>
    /// <param name="items">PreferenceItemRequest List</param>
    /// <param name="backUrl">PreferenceBackUrlsRequest</param>
    /// <param name="installments">int</param>
    /// <returns>Returns a preference id that you must send in your frontend.</returns>
    public async Task<string> CreatePreferenceRequest(List<PreferenceItemRequest> items, PreferenceBackUrlsRequest backUrl, int installments)
    {
        var request = new PreferenceRequest
        {
            Items = items,
            BackUrls = backUrl,
            AutoReturn = "approved",
            PaymentMethods = new PreferencePaymentMethodsRequest
            {
                Installments = installments
            }
        };

        var client = new PreferenceClient();
        Preference preference = await client.CreateAsync(request);
        return preference.Id;
    }

    
    /// <summary>
    /// You must use this method with the Checkout Bricks payment
    /// </summary>
    /// <param name="data">PaymentDataRequest</param>
    /// <returns>Returns payment status (approved, pending, rejected)</returns>
    public async Task<string> CreatePaymentRequest(PaymentDataRequest data)
    {
        var request = new PaymentCreateRequest
        {
            TransactionAmount = data.TransactionAmount,
            Token = data.Token,
            Description = data.Description,
            Installments = data.Installments,
            PaymentMethodId = data.PaymentMethodId,
            Payer = new PaymentPayerRequest
            {
                Email = data.Payer.Email,
                Identification = new IdentificationRequest
                {
                    Type = data.Payer.Identification.Type,
                    Number = data.Payer.Identification.Number
                },
                FirstName = data.CardholderName
            }
        };
        
        var client = new PaymentClient();
        try
        {
            Payment payment = await client.CreateAsync(request);
            return payment.Status;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static PaymentDataRequest DeserializePaymentData(string json)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new PaymentDataRequestConverter());
            var data = JsonConvert.DeserializeObject<PaymentDataRequest>(json, settings);
            if (data == null)
                throw new Exception("Error: data is null");
            
            return data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }
    
}
