using CV_Generator.ViewModels;

namespace CV_Generator.Services.Interfaces;

public interface IAiService
{
    Task<OptimizedCv> GenerateCvAsync(CVFormSubmit cv, CancellationToken cancellationToken = default);
}
