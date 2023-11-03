using dotNES.Renderers;

namespace dotNES
{
    public class RenderData
    {
        public enum FilterMode
        {
            NearestNeighbor, Linear
        }

        public FilterMode _filterMode = FilterMode.Linear;

        public IRenderer _renderer;
        public const int GameWidth = 256;
        public const int GameHeight = 240;
        public uint[] rawBitmap = new uint[GameWidth * GameHeight];
        public bool ready;
        public bool gameStarted;
    }
}
