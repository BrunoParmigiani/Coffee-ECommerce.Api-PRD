using Coffee_Ecommerce.API.ThirdParty.GoogleCloud.MapsPlatform;

namespace Coffee_Ecommerce.API.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<GeocodeResponse?> ValidatePostalCode(string postalCode);
        public string FormatPostalCode(string postalCode);
    }
}
