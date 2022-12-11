namespace WeatherTwentyOne.Services;

public interface ITrayService
{
    void Initialize();

    Action ClickHandler { get; set; }

	Action ExistHandler { get; set; }
}
