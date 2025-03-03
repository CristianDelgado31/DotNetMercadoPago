# MercadoPago Payment Library

<!-- [![License](https://img.shields.io/github/license/tu-usuario/tu-repo)](https://opensource.org/licenses/MIT) -->

Una librería fácil de usar para integrar pagos de MercadoPago en tus aplicaciones. Esta librería proporciona una interfaz sencilla para procesar pagos, generar tokens de tarjetas, manejar cuotas, y más, todo mediante el uso de MercadoPago API.

## Tabla de contenidos
1. [Características](#características)
2. [Requisitos](#requisitos)
3. [Instalación](#instalación)
4. [Uso](#uso)
   - [Configuración](#configuración)
   - [Realizar un pago](#realizar-un-pago)
5. [Contribuir](#contribuir)
6. [Licencia](#licencia)

## Características

- **Procesamiento de pagos**: Permite realizar pagos con tarjetas de crédito, débito, pagofacil, etc.
- **Generación de tokens de tarjeta**: Facilita la creación de tokens seguros para pagos con tarjetas de crédito.
- **Integración con MercadoPago API**: Simplifica el uso de la API de MercadoPago en tus proyectos.
- **Manejo de errores y respuestas de la API**: Proporciona mensajes de error claros y respuestas detalladas para facilitar la depuración.

## Requisitos

- **.NET 8.0**
- **Cuenta de MercadoPago**: Necesitarás configurar una cuenta de MercadoPago y obtener las credenciales de API (clave pública y secreta).

## Instalación

Para instalar esta librería, puedes agregarla a tu proyecto a través de **NuGet**. Ejecuta el siguiente comando:

```bash
dotnet add package DotNetMercadoPago
```

## Uso

 ### Configuración
   - Esta clase tiene que instanciarse una sola vez, usando singleton.
   ```bash
   var builder = WebApplication.CreateBuilder(args);
   string accessToken = builder.Configuration["AccessToken"];
   builder.Services.AddSingleton<MercadoPagoPayment>(sp =>
   {
      return new MercadoPagoPayment(accessToken);
   });
   ```

 ### Realizar un pago
   - **En esta versión solo existen dos tipos de metodos de pago**: Checkout PRO y Checkout Bricks
   - **Checkout PRO**: Crea en el backend un reference id que es la configuración de la compra como el contenido y las urls dependiendo el estado de la compra, que luego esto se tiene que enviar al frontend para comenzar con la compra.
   - Ejemplo:
   ```bash
   var items = new List<PreferenceItemRequest>
   {
      new PreferenceItemRequest
      {
            Title = "Mi producto",
            UnitPrice = 50,
            Quantity = 1,
            PictureUrl = "https://www.images.com/image_1.png"
      },
      new PreferenceItemRequest
      {
            Title = "Mi producto 2",
            UnitPrice = 50,
            Quantity = 1,
      }
   };
        
   var urls = new PreferenceBackUrlsRequest
   {
      Success = "https://www.myexamplepage.com/success",
      Failure = "https://www.myexamplepage.com/failure",
      Pending = "https://www.myexamplepage.com/pending",
   };
   var installments = 2;
   string referenceId = await mercadoPagoPayment.CreatePreferenceRequest(items, urls, installments);
   ```

   - **Checkout Bricks**: Recibe desde el frontend los datos de la compra de tipo JSON, para luego realizar el proceso de pago en el backend devolviendo como respuesta el estado de pago (approved, pending, rejected).
   - Ejemplo:
   ```bash
   string json = """
                   {
                     "description": "Purchase of product XYZ",
                     "cardholderName": "Pepe Lopez",
                     "installments": 1,
                     "payer": {
                       "email": "test@testuser.com",
                       "identification": {
                         "type": "DNI",
                         "number": "12345678"
                       }
                     },
                     "payment_method_id": "debmaster",
                     "token": "adjsjdajdj2j323j2j",
                     "transaction_amount": 100
                   }
                   """;

   # Se puede deserializar manualmente usando la clase PaymentDataRequest que viene con la biblioteca
   PaymentDataRequest paymentData = JsonConvert.DeserializeObject<PaymentDataRequest>(json);
   string status = await mercadoPagoPayment.CreatePaymentRequest(paymentData);

   # O usando el metodo estatico que deserializa y verifica si los datos no son nulos o estan vacios, lanzando una excepcion en caso de error. Es necesario agregar un try catch para manejar el error
   try
   {
      PaymentDataRequest paymentData = MercadoPagoPayment.DeserializePaymentData(json);
      string status = await mercadoPagoPayment.CreatePaymentRequest(paymentData);
   }
   catch (Exception ex)
   {
      # Mensaje de que un dato es nulo o esta vacio
      Console.WriteLine(ex.Message);
   }
```

## Contribuir

¡Gracias por tu interés en contribuir a este proyecto! Todos los contribuyentes son bienvenidos, es mi primera vez haciendo una biblioteca asi que toda ayuda es bienvenida :)