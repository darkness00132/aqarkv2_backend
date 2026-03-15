using Infrastructure.Data;

namespace Application.Services
{
    public class LocationService
    {
        public string GetGovernorateName(int id) =>
            EgyptLocations.GetGovernorate(id)?.NameAr ?? "";

        public string GetCityName(int governorateId, int cityId) =>
            EgyptLocations.GetCity(governorateId, cityId)?.NameAr ?? "";

        public bool CityBelongsToGovernorate(int governorateId, int cityId) =>
            EgyptLocations.CityBelongsToGovernorate(governorateId, cityId);


    }
}
