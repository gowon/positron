(async (window, undefined) => {

    window.chrome.webview.addEventListener('message', function(e) {
         console.info("messagereceived: " + e.data, e);
    });

    window.sample = {

        discoverMessages: function () {
            var x =  window.chrome.webview.hostObjects.sync.sampleService.DiscoverMessages();
            return JSON.parse(x);
        },

        postMessage: function (type, data) {

            var message = { Type: type, Data: JSON.stringify(data) }
            window.chrome.webview.postMessage(message);
        },

        //is3DModeActive: function () {
        //    return window.chrome.webview.hostObjects.sync.DisplaySettingsInternal.is3DModeActive();
        //}
    };
})(window);