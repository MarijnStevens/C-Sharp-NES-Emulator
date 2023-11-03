//set interface for controller
namespace dotNES.Controllers
{
    interface IController
    {
        void Strobe(bool on);

        int ReadState();

        void PressKey(int keyCode);

        void ReleaseKey(int keyCode);
    }
}
