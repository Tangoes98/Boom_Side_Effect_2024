public interface IState
{
    void onEnter();
    void onUpdate();
    void onExit();

    string Type();
}
