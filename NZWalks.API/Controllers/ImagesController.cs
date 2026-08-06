using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public ImagesController(IMapper mapper, IImageRepository imageRepository)
        {
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        [ValidateFileUpload]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            // 1. Map DTO to Domain Model using AutoMapper
            var imageDomainModel = mapper.Map<Image>(request);

            // 2. Use Repository to save file and persist DB record
            await imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }
    }
}
