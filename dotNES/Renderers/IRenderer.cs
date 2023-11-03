namespace dotNES.Renderers
{
    public interface IRenderer
    {
        string RendererName { get; }

        void Draw();

        void InitRendering(RenderData renderData);

        void EndRendering();
    }
}
