using OpenGL;
using System;
using System.Drawing;

namespace dotNES.Renderers
{
    class OpenGLRenderer : GlControl, IRenderer
    {
        private RenderData _renderData;
        private uint _textureId;
        private readonly Object _drawLock = new Object();

        public string RendererName => "OpenGL";

        public OpenGLRenderer()
        {
            ContextCreated += RenderControl_ContextCreated;
            Render += RenderControl_Render;
        }

        private void RenderControl_ContextCreated(object sender, GlControlEventArgs e)
        {
            lock (_drawLock)
            {
                _textureId = Gl.GenTexture();
                Gl.BindTexture(TextureTarget.Texture2d, _textureId);
                Gl.TextureParameterEXT(_textureId, TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, Gl.NEAREST);
                Gl.TextureParameterEXT(_textureId, TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, Gl.NEAREST);
            }
        }

        private void RenderControl_Render(object sender, GlControlEventArgs e)
        {
            lock (_drawLock)
            {
                Gl.Enable(EnableCap.Texture2d);
                Gl.ClearColor(Color.Gray.R / 255.0f, Color.Gray.G / 255.0f, Color.Gray.B / 255.0f, 0);
                Gl.Clear(ClearBufferMask.ColorBufferBit);

                if (_renderData.gameStarted)
                {
                    Gl.MatrixMode(MatrixMode.Projection);
                    Gl.LoadIdentity();
                    Gl.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
                    Gl.Ortho(0d, 1d, 0d, 1d, -1d, 1d);
                    Gl.MatrixMode(MatrixMode.Modelview);
                    Gl.LoadIdentity();

                    Gl.BindTexture(TextureTarget.Texture2d, _textureId);

                    using (MemoryLock locked = new MemoryLock(_renderData.rawBitmap))
                    {
                        Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, RenderData.GameWidth, RenderData.GameHeight, 0,
                            PixelFormat.Bgra, PixelType.UnsignedByte, locked.Address);
                    }

                    Gl.TextureParameterEXT(_textureId, TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
                        _renderData._filterMode == RenderData.FilterMode.Linear ? Gl.LINEAR : Gl.NEAREST);

                    Gl.Begin(PrimitiveType.Quads);
                    Gl.TexCoord2(0, 1);
                    Gl.Vertex2(0, 0);
                    Gl.TexCoord2(0, 0);
                    Gl.Vertex2(0, 1);
                    Gl.TexCoord2(1, 0);
                    Gl.Vertex2(1, 1);
                    Gl.TexCoord2(1, 1);
                    Gl.Vertex2(1, 0);
                    Gl.End();
                }
            }
        }

        public void InitRendering(RenderData renderData)
        {
            if (renderData == null) return;
            _renderData = renderData;

            ResizeRedraw = true;
            _renderData.ready = true;
        }

        public void EndRendering()
        {

        }

        protected override void OnResize(EventArgs e)
        {
            InitRendering(_renderData);
            base.OnResize(e);
        }

        public void Draw()
        {
            if (_renderData == null || !_renderData.ready) return;
            Invalidate();
            Update();
        }
    }
}