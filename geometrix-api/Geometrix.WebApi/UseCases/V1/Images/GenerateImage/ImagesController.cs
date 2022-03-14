using System.ComponentModel.DataAnnotations;
using Geometrix.Application.Services;
using Geometrix.Application.UseCases.GenerateImage;
using Geometrix.Domain;
using Geometrix.WebApi.Modules.Common;
using Geometrix.WebApi.Modules.Common.FeatureFlags;
using Geometrix.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Geometrix.WebApi.UseCases.V1.Images.GenerateImage;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.GenerateImage)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class ImagesController : ControllerBase, IOutputPort
{
    private readonly Notification _notification;
    private IActionResult? _viewModel;

    public ImagesController(Notification notification)
    {
        _notification = notification;
    }

    void IOutputPort.Invalid()
    {
        ValidationProblemDetails problemDetails = new(_notification.ModelState);
        _viewModel = BadRequest(problemDetails);
    }

    void IOutputPort.Ok(ImageDescription imageDescription, byte[] dataArray, string fileName)
    {
        string fileLocation = $"{Request.Scheme}://{Request.Host}/images/{fileName}" ;
        _viewModel = Ok(new GenerateImageResponse(fileLocation, new ImageModel(imageDescription)));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerateImageResponse))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenerateImageResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
    public async Task<IActionResult> GenerateImage(
        [FromServices] IGenerateImageUseCase useCase,
        [FromForm][Required] bool includeEmptyAndFill,
        [FromForm][Required] int seed = 42,
        [FromForm][Required] int mirrorPowerHorizontal = 2,
        [FromForm][Required] int mirrorPowerVertical = 2,
        [FromForm][Required] int cellGroupLength = 4,
        [FromForm][Required] int cellWidthPixel = 64,
        [FromForm][Required] string backgroundColor = "dark",
        [FromForm][Required] string foregroundColor = "indigo")
    {
        useCase.SetOutputPort(this);

        await useCase
            .Execute(
                mirrorPowerHorizontal,mirrorPowerVertical, cellGroupLength, cellWidthPixel,
                includeEmptyAndFill, seed, backgroundColor, foregroundColor)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}