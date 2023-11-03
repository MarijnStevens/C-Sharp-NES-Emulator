namespace dotNES.Controllers;

public interface IController
{
    void Strobe(bool on);

    int ReadState();

    void PressKey(int keyCode);

    void ReleaseKey(int keyCode);
}
