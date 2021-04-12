using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Repositories;

namespace warehouse.picking.api.Hubs
{
    public interface IIntakeDashboardClient
    {
        Task DeliveryArticles(List<Article> articles);
    }

    public class IntakeDashboardHub : Hub<IIntakeDashboardClient>
    {
        private readonly IArticleRepository _articleRepository;

        public IntakeDashboardHub(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, "Dashboard");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "Dashboard");
            return base.OnDisconnectedAsync(exception);
        }

        public async void GetDeliveryArticles(string noteId)
        {
            var res = await _articleRepository.GetByNoteId(noteId);
            await Clients.Client(Context.ConnectionId).DeliveryArticles(res);
        }

    }
}