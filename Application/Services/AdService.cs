using Application.DTOs.Ad;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class AdService : IAdService
    {
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IAdRepo _adRepo;
        private readonly IImageRepo _imageRepo;
        private readonly LocationService _locationService;
        private readonly IUnitOfWork _uow;

        public AdService(IStorageService storageService, IMapper mapper, IAdRepo adRepo, IImageRepo imageRepo, LocationService locationService, IUnitOfWork uow)
        {
            _storageService = storageService;
            _mapper = mapper;
            _adRepo = adRepo;
            _imageRepo = imageRepo;
            _locationService = locationService;
            _uow = uow;
        }

        public async Task CreateAdAsync(CreateAdDTO request, Guid userId, CancellationToken ct = default)
        {
            //check if location setted correctly
            if (!_locationService.CityBelongsToGovernorate(request.GovernorateId, request.CityId))
                throw ApiException.BadRequest("المدينة مدخلة ليس ضمن نطاق محافظة المدخلة");

            //upload images in S3 storage service
            List<Stream> imagesStreams = request.Images
                .Select(img => img.OpenReadStream())
                .ToList();

            List<string> imagesUrls = await _storageService.UploadManyAsync(imagesStreams, ct);
             
            await using var transaction = await _uow.BeginTransactionAsync(ct);
            try
            {
                Ad ad = _mapper.Map<Ad>(request);
                ad.UserId = userId;

                ad.Slug = GenerateSlug(ad.PropertyType.ToArabic(), ad.AdType.ToArabic(), _locationService.GetCityName(ad.GovernorateId, ad.CityId),_locationService.GetGovernorateName(ad.GovernorateId));

                Guid adId = await _adRepo.CreateAdAsync(ad, ct);

                // make images after upload it and make ad 
                List<Image> images = imagesUrls
                .Select(url => new Image { Url = url, AdId = adId })
                .ToList();

                await _imageRepo.AddRangeAsync(images, ct);
                await _uow.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            } 
            catch
            {
                await transaction.RollbackAsync();
                await _storageService.DeleteManyAsync(imagesUrls);
                throw;
            }
            finally
            {
                foreach (var stream in imagesStreams)
                    await stream.DisposeAsync();
            }
        }

        public Task DeleteAdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<AdDTO> GetAdBySlug(string slug, CancellationToken ct = default) 
        {
            Ad? ad = await _adRepo.GetAdBySlugAsync(slug);
            if (ad is null) throw ApiException.NotFound("الاعلان غير موجود");
            AdDTO result = _mapper.Map<AdDTO>(ad);

            result.GovernorateName = _locationService.GetGovernorateName(ad.GovernorateId);
            result.CityName = _locationService.GetCityName(ad.GovernorateId, ad.CityId);

            return result;
        }

        public async Task<AdDTO> GetAdById(Guid id, CancellationToken ct = default)
        {
            Ad? ad = await _adRepo.GetAdByIdAsync(id,ct);
            if (ad is null) throw ApiException.NotFound("الاعلان غير موجود");
            AdDTO result = _mapper.Map<AdDTO>(ad);

            result.GovernorateName = _locationService.GetGovernorateName(ad.GovernorateId);
            result.CityName = _locationService.GetCityName(ad.GovernorateId,ad.CityId);

            return result;
        }

        public async Task<List<AdListItemDTO>> GetAllAds(CancellationToken ct = default)
        {
            List<Ad> ads = await _adRepo.GetAllAsync(ct);
            List<AdListItemDTO> result = _mapper.Map<List<AdListItemDTO>>(ads);

            for (int i = 0; i < ads.Count; i++)
            {
                result[i].GovernorateName = _locationService.GetGovernorateName(ads[i].GovernorateId);
                result[i].CityName = _locationService.GetCityName(ads[i].GovernorateId, ads[i].CityId);
            }

            return result;
        }

        public Task UpdateAdAsync(UpdateAdDTO ad, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        private static string GenerateSlug(string propertyType, string adType, string city, string governorate)
        {
            var slug = string.Join("-", propertyType, adType, city, governorate);

            // remove tashkeel (diacritics)
            slug = Regex.Replace(slug, @"[\u0610-\u061A\u064B-\u065F]", "");

            // replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // keep only arabic letters and hyphens
            slug = Regex.Replace(slug, @"[^\u0600-\u06FF-]", "");

            // remove duplicate hyphens
            slug = Regex.Replace(slug, @"-+", "-").Trim('-');

            // append unique id
            return slug + "-" + Guid.NewGuid().ToString()[..8];
        }
    }
}
