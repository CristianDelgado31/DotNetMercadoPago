namespace DotNetMercadoPago.Entities;

public sealed class Payer
{
    public required string Email { get; set; }
    public required Identification Identification { get; set; }
}