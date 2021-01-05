namespace WpfHybridApp.Services
{
    public interface ISampleService
    {
        string Prop { get; set; }
        string GetCurrentDate();
        string DiscoverMessages();
    }
}