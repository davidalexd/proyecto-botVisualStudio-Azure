using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoBot_1274657Perez.Dialogos
{
    public class raizDialogo : ComponentDialog
    {
        public raizDialogo()
        {
            var recopilar = new WaterfallStep[]
            {
                mostrarNombres,
                mostraMenu,
                mostrarDatos

            };
            AddDialog(new WaterfallDialog(nameof(WaterfallStep), recopilar));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            //AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), validarEdad));
            AddDialog(new ChooseSeries());
            AddDialog(new Choosepayment());
        }

        private async Task<DialogTurnResult> mostrarNombres(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Para iniciar la conversacion necesito Tus nombres U_U",
                cancellationToken: cancellationToken);
            await Task.Delay(1000);
            return await stepContext.PromptAsync
                (
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Por favor ingrese sus nombres ") }, cancellationToken
                );
        }

        //private async Task<bool> validarEdad(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        // {
        //     return await Task.FromResult
        //         (
        //         promptContext.Recognized.Succeeded &&
        //         promptContext.Recognized.Value >= 18 &&
        //         promptContext.Recognized.Value <= 80
        //         );
        // }

        private async Task<DialogTurnResult> mostrarDatos(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await Task.Delay(1500);
            var userOption = stepContext.Context.Activity.Text;
            switch (userOption)
            {
                case "biblioteca":
                    await stepContext.BeginDialogAsync(nameof(ChooseSeries), cancellationToken: cancellationToken);
                    break;
                case "pago":
                    await stepContext.BeginDialogAsync(nameof(Choosepayment), cancellationToken: cancellationToken);
                    break;
                case "chau":
                    await stepContext.Context.SendActivityAsync("hasta luego nos vemos nunca");
                    break;
                default:
                   await stepContext.BeginDialogAsync(nameof(mostraMenu), cancellationToken: cancellationToken);
                    break;
            }

            //await stepContext.Context.SendActivityAsync("Gracias por registrar sus datos", cancellationToken: cancellationToken);

            //return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            //return await SeleccionarSerie(stepContext, cancellationToken);
            //return await stepContext.NextAsync(cancellationToken: cancellationToken);
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> setEdad(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            var nombre = stepContext.Context.Activity.Text;
            await stepContext.Context.SendActivityAsync($"Bienvenido {nombre}, que deseas hacer?", cancellationToken: cancellationToken);
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = Rutas()
                    //Prompt = MessageFactory.Text($"Bienvenido {nombre}, que deseas hacer?"),
                    //new PromptOptions { Prompt = Rutas() }
                    //RetryPrompt = MessageFactory.Text($"{nombre}, por favor ingresa edad correcta")
                },
                cancellationToken);
        }

        private Activity Rutas()
        {


            var listAttachments = new List<Attachment>();
            var imgbiblioteca = "https://hips.hearstapps.com/es.h-cdn.co/fotoes/images/series-television/anime-para-principiantes.-con-que-series-estrenarse-en-el-formato/137753952-1-esl-ES/Anime-para-principiantes.-Con-que-series-estrenarse-en-el-formato.jpg";
            var imgPago = "https://st3.depositphotos.com/1129232/12824/v/450/depositphotos_128248364-stock-illustration-vector-digital-mobile-e-wallet.jpg";

                var card = new HeroCard()
                {
                    Title = "Biblioteca de animes",
                    Subtitle = "¡Tus series favoritas ahora en UN SOLO SERVICIO!. ¡Descubre más AQUÍ!",
                    Images = new List<CardImage>() { new CardImage(imgbiblioteca) },
                    Buttons = new List<CardAction>()
                    {
                         new CardAction(){Title = "Entrar a biblioteca", Value ="biblioteca", Type = ActionTypes.ImBack},
                    }
                };

            var carss= new HeroCard()
            {
                Title = "Metodos de pago",
                Subtitle = " plataforma de pagos con aplicaciones que te ayudan a pagar ",
                Images = new List<CardImage>() { new CardImage(imgPago) },
                Buttons = new List<CardAction>()
                    {
                         new CardAction(){Title = "Metodos de pago", Value ="pago", Type = ActionTypes.ImBack},
                    }
            };
            listAttachments.Add(card.ToAttachment());
            listAttachments.Add(carss.ToAttachment());


            var reply = MessageFactory.Attachment(listAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            return reply as Microsoft.Bot.Schema.Activity;
        }

        private async Task<DialogTurnResult> mostraMenu(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre = stepContext.Context.Activity.Text;
           

            await stepContext.Context.SendActivityAsync($"Bienvenido {nombre} Estas se encuentra en el menu de aniSeries, que deseas hacer?", cancellationToken: cancellationToken);
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = Rutas()
                    //Prompt = MessageFactory.Text($"Bienvenido {nombre}, que deseas hacer?"),
                    //new PromptOptions { Prompt = Rutas() }
                    //RetryPrompt = MessageFactory.Text($"{nombre}, por favor ingresa edad correcta")
                },
                cancellationToken);
        }
    }

}
