using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using Newtonsoft.Json;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake.Payloads;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.ExternalTasks.Intake
{
    [ExternalTaskHandler(topic: "ST.Intake.Articles.Location.Reserve")]
    public class BookStockyardLocationHandler : IExternalTaskHandler<ArticleQuantityPayload, BookLocationResult>
    {
        private readonly IntakeService _service;

        public BookStockyardLocationHandler(IntakeService service)
        {
            _service = service;
        }

        public async Task<BookLocationResult> HandleAsync(ArticleQuantityPayload input, ExternalTask task)
        {
            var location = await _service.BookStockYardLocation(input.NoteId, input.ArticleId, input.Quantity);
            return new BookLocationResult(location);
        }
    }

    public class BookLocationResult
    {
        [JsonProperty]
        public string Barcode { get; set; }

        public BookLocationResult()
        {
        }

        public BookLocationResult(string barcode)
        {
            Barcode = barcode;
        }
    }
}