using Application.DTOs.Ad;
using Application.DTOs.Ad.Private;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.ThirdPartyService;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.AdEntities;
using Domain.Enums;
using Domain.Services;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Ads;
using Shared.Filters;
using Shared.Pagination;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class AdService : IAdService
    {
        private readonly IImageService _storageService;
        private readonly IMapper _mapper;
        private readonly IAdRepo _adRepo;
        private readonly IImageRepo _imageRepo;
        private readonly ICreditsRepo _creditsRepo;
        private readonly IAdLogRepo _adLogRepo;
        private readonly ICreditsLogRepo _creditsLogRepo;
        private readonly LocationService _locationService;
        private readonly IUnitOfWork _uow;

        public AdService(IImageService storageService, IMapper mapper, IAdRepo adRepo, IImageRepo imageRepo, ICreditsRepo creditsRepo, IAdLogRepo adLogRepo, ICreditsLogRepo creditsLogRepo, LocationService locationService, IUnitOfWork uow)
        {
            _storageService = storageService;
            _mapper = mapper;
            _adRepo = adRepo;
            _imageRepo = imageRepo;
            _creditsRepo = creditsRepo;
            _adLogRepo = adLogRepo;
            _creditsLogRepo = creditsLogRepo;
            _locationService = locationService;
            _uow = uow;
        }

        public async Task CreateAdAsync(CreateAdDTO dto, Guid userId)
        {
            //check if location setted correctly
            if (!_locationService.CityBelongsToGovernorate(dto.GovernorateId, dto.CityId))
                throw ApiException.BadRequest("المدينة مدخلة ليس ضمن نطاق محافظة المدخلة");

            //utrack images uplaod and deletion
            List<Stream> imagesStreams = dto.Images
                .Select(img => img.OpenReadStream())
                .ToList();

            List<string> imagesUrls = new List<string>();


            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                imagesUrls = await _storageService.UploadManyAsync(imagesStreams);
                //verfiy user have required tokens first
                int cost = AdCreditCalculator.CalculatePostCost(dto.Type, dto.PropertyType, dto.Price);
                bool result = await _creditsRepo.DeductAsync(userId, cost);
                if (!result) throw ApiException.BadRequest("لا يوجد كريدت كافية لعمل اعلان");


                Ad ad = _mapper.Map<Ad>(dto);
                ad.UserId = userId;

                ad.Slug = GenerateSlug(ad, _locationService.GetCityName(ad.GovernorateId, ad.CityId), _locationService.GetGovernorateName(ad.GovernorateId));

                //create and log it
                Guid adId = await _adRepo.CreateAdAsync(ad);
                await _adLogRepo.LogAsync(new AdLog
                {
                    AdId = adId,
                    UserId = userId,
                    Action = AdAction.Created,
                });

                await _creditsLogRepo.LogAsync(new CreditsLog
                {
                    UserId = userId,
                    AdId = adId,
                    Credits = -cost,
                    Action = CreditsLogAction.Spend,
                });

                // make images after upload it and make ad 
                List<Image> images = imagesUrls
                .Select(url => new Image { Url = url, AdId = adId })
                .ToList();

                await _imageRepo.AddRangeAsync(images);
                await _uow.SaveChangesAsync();
                await transaction.CommitAsync();
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



        public async Task<AdDTO> GetAdBySlug(string slug, CancellationToken ct = default)
        {
            Ad? ad = await _adRepo.GetAdBySlugAsync(slug);
            if (ad is null) throw ApiException.NotFound("الاعلان غير موجود");
            AdDTO result = _mapper.Map<AdDTO>(ad);

            result.GovernorateName = _locationService.GetGovernorateName(ad.GovernorateId);
            result.CityName = _locationService.GetCityName(ad.GovernorateId, ad.CityId);

            return result;
        }

        public async Task<Ad> GetAdById(Guid id, CancellationToken ct = default)
        {
            Ad? ad = await _adRepo.GetAdByIdAsync(id, ct);
            if (ad is null) throw ApiException.NotFound("الاعلان غير موجود");

            return ad;
        }

        public async Task<PaginationResult<List<AdListItemDTO>>> GetAllAds(AdFilters? filters, Pagination? pagination, CancellationToken ct = default)
        {
            List<Ad> ads = await _adRepo.GetAllAsync(filters, pagination, ct);
            var result = _mapper.Map<List<AdListItemDTO>>(ads);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].GovernorateName = _locationService.
                    GetGovernorateName(ads[i].GovernorateId);
                result[i].CityName = _locationService
                    .GetCityName(ads[i].GovernorateId, ads[i].CityId);
            }

            int totalCount = await _adRepo.Count();

            return new PaginationResult<List<AdListItemDTO>>
            {
                Items = result,
                TotalCount = totalCount,
                Page = pagination?.Page ?? 1,
                PageSize = pagination?.PageSize ?? 12,
            };
        }

        public async Task<PaginationResult<List<AdPrivateListItemDTO>>> GetMyAds(AdFilters? filters, Pagination? pagination, Guid userId, CancellationToken ct = default)
        {
            List<Ad> ads = await _adRepo.GetMineAsync(filters, pagination, userId, ct);
            var result = _mapper.Map<List<AdPrivateListItemDTO>>(ads);

            for (int i = 0; i < ads.Count; i++)
            {
                result[i].GovernorateName = _locationService
                    .GetGovernorateName(ads[i].GovernorateId);
                result[i].CityName = _locationService
                    .GetCityName(ads[i].GovernorateId, ads[i].CityId);
            }

            int totalCount = await _adRepo.Count();

            return new PaginationResult<List<AdPrivateListItemDTO>>
            {
                Items = result,
                TotalCount = totalCount,
                Page = pagination?.Page ?? 1,
                PageSize = pagination?.PageSize ?? 12,
            };
        }

        public async Task UpdateAdAsync(Guid Id, Guid userId, UpdateAdDTO dto)
        {
            var existingAd = await _adRepo.GetByIdToMutateAsync(Id);
            if (existingAd is null) throw ApiException.NotFound("هذا الاعلان غير متاح");

            if (existingAd.UserId != userId) throw ApiException.Forbidden("لا تمتلك صلاحية تعديل على هذا الاعلان");

            //calculate total images user will have (max 5)
            int existingImagesCount = existingAd.Images.Count;
            int newImagesCount = dto.NewImages?.Length ?? 0;
            int deletedImagesCount = dto.DeletedImagesIds?.Count ?? 0;

            List<string> ImagesToDelete = new List<string>();

            if (existingImagesCount + newImagesCount - deletedImagesCount > 5) throw ApiException.BadRequest("العدد الاجمالى لصور يجب ان لا يزيد عن 5 صور");

            if (existingImagesCount + newImagesCount - deletedImagesCount < 1) throw ApiException.BadRequest("يجب ان يحتوى اعلان على صورة واحدة على الاقل");

            int cost = dto.Price.HasValue
      ? AdCreditCalculator.CalculateUpdateCost(existingAd.Price, dto.Price.Value, existingAd.Type, existingAd.CreatedAt)
      : 0;

            if (deletedImagesCount > 0)
            {
                foreach (var imageId in dto.DeletedImagesIds)
                {
                    Image? image = existingAd.Images.FirstOrDefault(img => img.Id == imageId);
                    if (image is not null)
                    {
                        ImagesToDelete.Add(image.Url); existingAd.Images.Remove(image);
                    }
                }
            }

            List<string> uploadedUrls = new List<string>();

            if (newImagesCount > 0)
            {
                List<Stream> imagesStreams = dto.NewImages
                   .Select(img => img.OpenReadStream())
                   .ToList();

                try
                {
                    uploadedUrls = await _storageService.UploadManyAsync(imagesStreams);
                    foreach (var url in uploadedUrls)
                        existingAd.Images.Add(new Image { Url = url });
                }
                catch
                {
                    await _storageService.DeleteManyAsync(uploadedUrls);
                    throw;
                }
                finally
                {
                    foreach (var stream in imagesStreams)
                        await stream.DisposeAsync();
                }
            }

            //update other properties
            _mapper.Map(dto, existingAd);

            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                if (cost > 0)
                {
                    bool deducted = await _creditsRepo.DeductAsync(userId, cost);
                    if (!deducted) throw ApiException.BadRequest("لا يوجد كريدت كافية لتعديل الاعلان");
                }

                await _adLogRepo.LogAsync(new AdLog
                {
                    AdId = Id,
                    UserId = userId,
                    Action = AdAction.Update,
                });

                if (cost > 0)
                    await _creditsLogRepo.LogAsync(new CreditsLog
                    {
                        UserId = userId,
                        AdId = Id,
                        Credits = -cost,
                        Action = CreditsLogAction.Spend,
                    });

                await _uow.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                await _storageService.DeleteManyAsync(uploadedUrls);
                throw;
            }

            if (ImagesToDelete.Count > 0)
                await _storageService.DeleteManyAsync(ImagesToDelete);
        }

        public async Task DeleteAdAsync(Guid id, Guid userId)
        {
            Ad? ad = await _adRepo.GetByIdToMutateAsync(id);
            if (ad is null) throw ApiException.NotFound("الاعلان غير موجود");
            if (ad.UserId != userId) throw ApiException.Forbidden("لا تمتلك صلاحية تعديل على هذا الاعلان");

            List<string> imagesToDelete = ad.Images.Select(img => img.Url).ToList();

            _adRepo.DeleteAd(ad);
            await _adLogRepo.LogAsync(new AdLog
            {
                UserId = userId,
                AdId = id,
                Action = AdAction.Delete,
            });
            await _uow.SaveChangesAsync();
            await _storageService.DeleteManyAsync(imagesToDelete);
        }

        private static string GenerateSlug(Ad ad, string city, string governorate)
        {
            var slug = string.Join("-", ad.PropertyType.ToArabic(), ad.Type.ToArabic(), city, governorate);

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
