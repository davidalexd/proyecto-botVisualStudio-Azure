using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using ProyectoBot_1274657Perez.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoBot_1274657Perez.Dialogos
{
    public class Choosepayment:ComponentDialog
    {

        public Choosepayment()
        {
            var waterfallSteps = new WaterfallStep[]
            {
                MostrarPago,
                SalirPago,
                Fin
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> SalirPago(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
              nameof(TextPrompt),
               new PromptOptions { Prompt = ButtonConfirmation() },
                cancellationToken
            );
        }

        private Activity ButtonConfirmation()
        {
            var reply = MessageFactory.Text("Estas en el submenu Informacion de metodos de pago ¿Deseas salir?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Si,deseo  salir", Value = "si", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "No,deseo quedarme", Value = "no", Type = ActionTypes.ImBack}

                }
            };

            return reply;
        }

        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userOption = stepContext.Context.Activity.Text.ToLower();
            await stepContext.Context.SendActivityAsync("Hasta luego nos vemos pronto");
            switch (userOption)
            {
                case "si":
                    stepContext.Context.Activity.Text = null;
                    await stepContext.BeginDialogAsync(nameof(raizDialogo), cancellationToken: cancellationToken);
                    break;
                case "no":
                    await stepContext.BeginDialogAsync(nameof(Choosepayment), cancellationToken: cancellationToken);
                    break;
                default:
                    await stepContext.BeginDialogAsync(nameof(raizDialogo), cancellationToken: cancellationToken);
                    break;
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> MostrarPago(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Estos son nuestros metodos de pago:", cancellationToken: cancellationToken);

            await Task.Delay(1500);
            return await stepContext.PromptAsync(
             nameof(TextPrompt),
             new PromptOptions { Prompt = Metodos() }
            );
        }

        private Activity Metodos()
        {

            var ListPayment = DatabaseAni.GetPayment();

            var listAttachments = new List<Attachment>();

            foreach (var item in ListPayment)
            {
                var card = new HeroCard()
                {
                    Title = item.nombre,
                    Images = new List<CardImage>() { new CardImage(item.imagen) }
                };
                listAttachments.Add(card.ToAttachment());
            }

            var reply = MessageFactory.Attachment(listAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            return reply as Microsoft.Bot.Schema.Activity;
        }
    }
}