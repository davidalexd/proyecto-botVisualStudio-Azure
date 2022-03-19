// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProyectoBot_1274657Perez
{
    public class EmptyBot<t> : ActivityHandler where t:Dialog

    {
        protected readonly Dialog _dialogo;
        protected readonly BotState _conversacionEstado;
        protected readonly ILogger _logeo;

        public EmptyBot(t dialogo, ConversationState conversacionEstado, ILogger<EmptyBot<t>> logeo)
        {
            this._dialogo = dialogo;
            this._conversacionEstado = conversacionEstado;
            this._logeo = logeo;

        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Bienvenido a AniSeries Lider en entretenimiento juvenil! >_<"), cancellationToken);
                }
            }
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _dialogo.RunAsync(
                turnContext,
                _conversacionEstado.CreateProperty<DialogState>(nameof(DialogState)),
                cancellationToken);
        }
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await _conversacionEstado.SaveChangesAsync(turnContext, false, cancellationToken);
        }
        
    }
}
