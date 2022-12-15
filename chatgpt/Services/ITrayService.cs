namespace chatgpt.Services;

public interface ITrayService
{
    void Initialize();

    Action ClickHandler { get; set; }

	Action ExistHandler { get; set; }

    bool isDispose { get;}
}
