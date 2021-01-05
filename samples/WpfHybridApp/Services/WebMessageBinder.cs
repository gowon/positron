using System;
using MediatR;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace WpfHybridApp.Services
{
    public class WebMessageBinder
    {
        private readonly IMediator _mediator;

        public WebMessageBinder(IMediator mediator)
        {
            _mediator = mediator;
        }

        // https://stackoverflow.com/a/3317147/7644876
        public async void MessageBinder_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            var jsonString = args.WebMessageAsJson;
            var message = JsonConvert.DeserializeObject<WebMessage>(jsonString);
            var messageType = Type.GetType(message.Type);
            
            if (messageType != null)
            {
                var request = JsonConvert.DeserializeObject(message.Data, messageType);
                var response = await _mediator.Send(request);

                if (sender is CoreWebView2 webView2 && response != null)
                    webView2.PostWebMessageAsJson(JsonConvert.SerializeObject(response));
            }
        }
    }
}