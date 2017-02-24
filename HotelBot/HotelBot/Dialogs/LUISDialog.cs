using HotelBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HotelBot.Dialogs
{
    [LuisModel("cd16f803-b3a4-46cb-9daa-11876cf28b88", "52b188255d0d4f84bd8ec74e7b8a9154")]
    [Serializable]
    public class LUISDialog : LuisDialog<RoomReservation>
    {
        private readonly BuildFormDelegate<RoomReservation> ReserveRoom;

        public LUISDialog(BuildFormDelegate<RoomReservation> reserveRoom)
        {
            this.ReserveRoom = reserveRoom;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            IMessageActivity message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(new Attachment()
            {
                ContentUrl = "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png",
                ContentType = "image/png",
                Name = "Bender_Rodriguez.png"
            });
            message.Text = "sorry!! Still in beta mode";
            await context.PostAsync(message);
            //await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }



        [LuisIntent("RoomReserve")]
        public async Task RoomReservartion(IDialogContext context, LuisResult result)
        {
            var enrollmentForm = new FormDialog<RoomReservation>(new RoomReservation(), this.ReserveRoom, FormOptions.PromptInStart, result.Entities);
            context.Call<RoomReservation>(enrollmentForm, Callback);
           // context.Call<RoomReservation>(enrollmentForm, saveData);
            //context.Wait(MessageReceived);

        }

        private async Task saveData(IDialogContext context, IAwaitable<RoomReservation> result)
        {
            var data = await result;

            context.Wait(this.MessageReceived);

        }

        [LuisIntent("QueryAmenities")]
        private async Task QueryAmenities(IDialogContext context, LuisResult result)
        {
            IMessageActivity message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            Attachment img = new Attachment();

            foreach (var entitiy in result.Entities.Where(Entity => Entity.Type == "Amenity"))
            {
                var value = entitiy.Entity.ToLower();
                string[] amenities = { "wifi", "gym", "pool" };
                if (amenities.Contains(value))
                {
                    message.Text = "Yes we have that";
                    switch (value)
                    {
                        case "wifi":
                            img.ContentType = "image/gif";
                            img.ContentUrl = "https://i0.wp.com/www.tangeroutlet.com/images/ipad-map/WIFI-Icon.gif";
                            img.Name = "Wifi";
                            break;
                        case "gym":
                            img.ContentType = "image/gif";
                            img.ContentUrl = "http://www.gifs.net/Animation11/Sports/Weightlifting/Weight_lifter_4.gif";
                            img.Name = "Gym";
                            break;
                        case "pool":
                            img.ContentType = "image/gif";
                            img.ContentUrl = "https://m.popkey.co/ed363c/J767_f-maxage-0.gif";
                            img.Name = "Pool";
                            break;


                    }
                    message.Attachments.Add(img);
                    // message.AddHeroCard("do you need that?", new List<string>() { "yes","no" });
                    await context.PostAsync(message);

                    // await context.PostAsync("Yes we have that!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    img.ContentType = "image/gif";
                    img.ContentUrl = "https://s-media-cache-ak0.pinimg.com/originals/54/da/8a/54da8a36f2346a21bfa4d77bc8134b5b.gif";
                    img.Name = "nope";
                    message.Text = "I'm sorry we don't have that.";
                    message.Attachments.Add(img);
                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                    return;
                }
            }
            img.ContentType = "image/gif";
            img.ContentUrl = "https://s-media-cache-ak0.pinimg.com/originals/54/da/8a/54da8a36f2346a21bfa4d77bc8134b5b.gif";
            img.Name = "nope";
            message.Text = "I'm sorry we don't have that.";
            message.Attachments.Add(img);
            await context.PostAsync(message);
            context.Wait(MessageReceived);
            return;
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
            //RoomReservation hotelInfo = null;
        }
    }


}