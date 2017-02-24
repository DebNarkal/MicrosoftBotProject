using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            try
            {
                IMessageActivity message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(new Attachment()
                {
                    ContentUrl = "http://4.bp.blogspot.com/-KugkFbRQ_KM/Vj_J78r1XmI/AAAAAAAAB1w/g1AtmLBefxc/s1600/giphy%2B%252882%2529.gif",
                    ContentType = "image/gif",
                    Name = "Hi"
                });
                message.Text = "*Hi I am John Bot*";
                 await context.PostAsync(message);
                //await context.PostAsync(replyImg);
            }
            catch(Exception e) { }
            await Respond(context);
            context.Wait(MessageReceivedAsync);
        }
        
        public async Task Respond(IDialogContext context)
        {
           // await context.PostAsync("Hi I'm John Bot");
            var userName = string.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if(string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync(String.Format($"Hi {userName}. How can I help you today?"));
            }
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = String.Empty;
            bool getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);
            
           
            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }

            //context.UserData.TryGetValue<string>("Name", out userName);
            #region Old Greetin Dialog used
            //if(string.IsNullOrEmpty(userName))
            //{
            //    await context.PostAsync("What is your name?");
            //    userName = message.Text;
            //    context.UserData.SetValue<bool>("GetName", true);
            //}
            //else
            //{
            //    await context.PostAsync(string.Format($"Hi {userName}. How can I help you today?"));
            //}

            //context.Wait(MessageReceivedAsync);
            #endregion
            await Respond(context);
            context.Done(message);
        }
    }
}