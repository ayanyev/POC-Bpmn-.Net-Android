using warehouse.picking.api.Domain;

namespace Warehouse.Picking.Api.Repositories
{
    public interface IDeliveryNoteRepository
    {
        DeliveryNote GetNoteById(string id);
    }
}