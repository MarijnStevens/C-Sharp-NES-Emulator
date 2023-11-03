namespace dotNES.Renderers;

public class RenderData
{
    public const int GameWidth = 256;
    public const int GameHeight = 240;

    public IRenderer? _renderer;

    public enum FilterMode
    {
        NearestNeighbor, Linear
    }    
    public FilterMode _filterMode = FilterMode.Linear;

    public uint[] rawBitmap = new uint[GameWidth * GameHeight];

    public bool ready;
    public bool gameStarted;
}
