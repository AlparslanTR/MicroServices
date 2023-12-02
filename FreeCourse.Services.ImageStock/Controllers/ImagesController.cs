using FreeCourse.Services.ImageStock.Dtos;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

namespace FreeCourse.Services.ImageStock.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            if (image is not null && image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream,cancellationToken);
                var returnPath = $"{Request.Scheme}://{Request.Host}/images/{image.FileName}";
                ImagesDto imagesDto = new() { Url = returnPath };

                return CreateActionResultInstance(ResponseDto<ImagesDto>.CreateSuccess(imagesDto, "Resim Eklendi",HttpStatusCode.OK));
            }
            return CreateActionResultInstance(ResponseDto<ImagesDto>.CreateFail("Resim Eklenemedi",HttpStatusCode.BadRequest));
        }

        [HttpDelete]
        public IActionResult DeleteImage(string pictureUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images",pictureUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseDto<ImagesDto>.CreateFail("Resim Bulunamadı",HttpStatusCode.NotFound));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(ResponseDto<ImagesDto>.CreateSuccess(null, "Resim Silindi", HttpStatusCode.NoContent));
        }
    }
}
