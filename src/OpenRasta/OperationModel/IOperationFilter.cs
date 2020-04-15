using System.Collections.Generic;
using System.Linq;
using OpenRasta.Diagnostics;
using OpenRasta.OperationModel.Filters;
using OpenRasta.Pipeline;
using OpenRasta.Web;

namespace OpenRasta.OperationModel
{
  public interface IOperationFilter : IOperationProcessor
  {
  }

  public class CompoundOperationFilter : IOperationFilter
  {
    readonly HttpMethodOperationFilter _httpMethodFilter;
    readonly UriNameOperationFilter _uriNameFilter;
    readonly UriParametersFilter _uriParametersFilter;

    public CompoundOperationFilter(IRequest request, IUriResolver uriResolver, ICommunicationContext context,
      IErrorCollector errorCollector)
    {
      _httpMethodFilter = new HttpMethodOperationFilter(request);
      _uriNameFilter = new UriNameOperationFilter(context, uriResolver);
      _uriParametersFilter = new UriParametersFilter(context, errorCollector);
    }

    public IEnumerable<IOperationAsync> Process(IEnumerable<IOperationAsync> operations)
    {
      return _uriParametersFilter.Process(
        _uriNameFilter.Process(
          _httpMethodFilter.Process(operations))).ToList();
    }
  }
}