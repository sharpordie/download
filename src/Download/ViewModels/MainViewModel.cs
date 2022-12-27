namespace Download.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly HttpClient manager = new();

    [ObservableProperty]
    string results = "UNKNOWN";

    [RelayCommand]
    async Task Download()
    {
        Results = "LOADING";
        await Task.Delay(TimeSpan.FromSeconds(5));
        var storage = FileSystem.Current.CacheDirectory;
        var deposit = Directory.CreateDirectory(Path.Combine(storage, Path.GetRandomFileName()[..8])).FullName;
        var address = "https://cdn-icons-png.flaticon.com/512/3081/3081559.png";
        var fetched = Path.Combine(deposit, "picture.png");
        if (File.Exists(fetched)) File.Delete(fetched);
        using var stream = await manager.GetStreamAsync(address);
        using var fileStream = new FileStream(fetched, FileMode.CreateNew);
        await stream.CopyToAsync(fileStream);
        Results = Directory.GetFiles(deposit).FirstOrDefault();
    }
}
