using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Repositories;

namespace warehouse.picking.api.Hubs
{
    public interface IIntakeDashboardClient
    {
        Task DeliveryArticles(List<SimplifiedArticle> articles);
    }

    public class SimplifiedArticle
    {
        public int Id { get; }
        public string NoteId { get; }
        public string Name { get; }
        public string Gtin { get; }
        public int Quantity { get; }
        public string Bundle { get; }

        public SimplifiedArticle(Article article)
        {
            Id = article.Id;
            NoteId = article.NoteId;
            Name = article.Name;
            Gtin = article.Gtin;
            Quantity = article.Quantity.Expected;
            Bundle = article.Bundle.Name;
        }
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
            var res = (await _articleRepository.GetByNoteId(noteId)).Select(a => new SimplifiedArticle(a)).ToList();
            await Clients.Client(Context.ConnectionId).DeliveryArticles(res);
        }

    }
}