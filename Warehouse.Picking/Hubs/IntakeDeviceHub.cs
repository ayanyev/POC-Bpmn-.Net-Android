using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using warehouse.picking.api.Domain;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Repositories;

namespace warehouse.picking.api.Hubs
{

    public interface IIntakeClient
    {
        Task ProcessStartConfirmed();
        
        Task ArticlesListReceived(List<Article> articles);
        
    }
    
    public class IntakeDeviceHub : Hub<IIntakeClient>
    {
        private const string ProcessModelId = "IntakeProcess";
        
        private const string ProcessStartEvent = "";
        
        private string _processInstanceId = "";
        
        private readonly IProcessClient _processClient;
        
        private readonly IWorkerRepository<Intaker> _workerRepository;
        
        private readonly IArticleRepository _articleRepository;

        public IntakeDeviceHub(IProcessClient processClient)
        {
            _processClient = processClient;
        }

        public async Task StartIntakeProcess(string name)
        {
            var result = 
                await _processClient.CreateProcessInstanceByModelId<>(ProcessModelId, ProcessStartEvent, Context.ConnectionId, null);
            var worker = new Intaker(Context.ConnectionId, name);
            _workerRepository.StartShift(worker);
            
            _processInstanceId = result.ProcessInstanceId;
            await Clients.Caller.ProcessStartConfirmed();
        }

        public async Task ProvideNoteId(string noteId)
        {
            var result = new Dictionary<string, object> {{"noteId", noteId}};
            await _processClient.FinishUserTask(_processInstanceId, "Intake.UT.Input.NoteId", result);
        }
    }
}