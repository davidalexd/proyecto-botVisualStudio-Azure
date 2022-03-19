using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using ProyectoBot_1274657Perez.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoBot_1274657Perez.Dialogos
{
    public class ChooseSeries : ComponentDialog
    {

        public ChooseSeries()
        {
            var waterfallStep = new WaterfallStep[]
            {
                MostrarAni,
                EscogerAni,
                ConfirmarAni,
                SalirAni,
                Fin,
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> SalirAni(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Context.Activity.Text.ToLower() == "si")
            {
                await stepContext.Context.SendActivityAsync("Pedido confirmado gracias por comprar en Aniseries", cancellationToken: cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync("Pedido no  confirmado", cancellationToken: cancellationToken);
            }
            return await stepContext.PromptAsync(
            nameof(TextPrompt),
                new PromptOptions { Prompt = ButtonBack() },
            cancellationToken
);
        }

        private Microsoft.Bot.Schema.Activity ButtonBack()
        {
            var reply = MessageFactory.Text("¿Deseas salir de la seccion Biblioteca?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Si,deseo salir", Value = "si", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "No, quiero quedarme", Value = "no", Type = ActionTypes.ImBack}

                }
            };

            return reply;
        }

        private async Task<DialogTurnResult> EscogerAni(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = MessageFactory.Text("Selecciona una serie en la lista de arriba >_< 👍👍") },
                 cancellationToken
            );
        }

        private async Task<DialogTurnResult> ConfirmarAni(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
              nameof(TextPrompt),
               new PromptOptions { Prompt = ButtonConfirmation() },
                cancellationToken
            );
        }

        private Microsoft.Bot.Schema.Activity ButtonConfirmation()
        {
            var reply = MessageFactory.Text("Ok Seleccionado 😝😝😝,¿Deseas confirmas este pedido?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Si,deseo confirmar mi pedido", Value = "si", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "No, ahora no", Value = "no", Type = ActionTypes.ImBack}

                }
            };

            return reply;
        }

        private async Task<DialogTurnResult> MostrarAni(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Esta es nuestra biblioteca de animes", cancellationToken: cancellationToken);

            await Task.Delay(1500);
            return await stepContext.PromptAsync(
             nameof(TextPrompt),
             new PromptOptions { Prompt = Opciones() }
            );
        }
        private Microsoft.Bot.Schema.Activity Opciones()
        {
            var ListaAni = DatabaseAni.GetAni();

            var listAttachments = new List<Attachment>();

            foreach (var item in ListaAni)
            {
                var card = new HeroCard()
                {
                    Title = item.nombre,
                    Subtitle = $"Fecha: {item.fecha}",
                    Images = new List<CardImage>() { new CardImage(item.imagen) },
                    Buttons = new List<CardAction>()
                    {
                         new CardAction(){Title = "Escoger", Value =item.nombre, Type = ActionTypes.ImBack},
                         new CardAction(){Title = "Ver información", Value =item.informacion, Type = ActionTypes.OpenUrl}
                    }
                };
                listAttachments.Add(card.ToAttachment());
            }

            var reply = MessageFactory.Attachment(listAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            return reply as Microsoft.Bot.Schema.Activity;
        }
        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userOption = stepContext.Context.Activity.Text.ToLower();

            switch (userOption)
            {
                case "si":

                    stepContext.Context.Activity.Text = null;
                    await stepContext.Context.SendActivityAsync("Hasta luego 😥😢😰😣😖 vuelva pronto Redirigiendo...", cancellationToken: cancellationToken);
                    
                    await stepContext.BeginDialogAsync(nameof(raizDialogo), cancellationToken: cancellationToken);
                    //await stepContext.Context.SendActivityAsync("Felicidades,se finalizo tu pedido con éxito.", cancellationToken: cancellationToken);
                    break;
                case "no":
                    await stepContext.BeginDialogAsync(nameof(ChooseSeries), cancellationToken: cancellationToken);
                    break;
                default:
                    await stepContext.BeginDialogAsync(nameof(raizDialogo), cancellationToken: cancellationToken);
                    break;
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
