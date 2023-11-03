using dotNES.Renderers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace dotNES.Win32.Renderers
{
    class SoftwareRenderer : Control, IRenderer
    {
        private RenderData _renderData;
        private Bitmap _gameBitmap;
        private GCHandle _rawBitmap;

        public string RendererName => "Software";

        public void InitRendering(RenderData renderData)
        {
            if (renderData == null) return;
            _renderData = renderData;

            BackColor = Color.Gray;
            DoubleBuffered = true;
        }

        public void EndRendering()
        {
            if (_rawBitmap.IsAllocated) _rawBitmap.Free();
        }

        protected override void OnResize(EventArgs e)
        {
            InitRendering(_renderData);
            base.OnResize(e);
        }

        public void Draw()
        {
            _gameBitmap?.Dispose();

            if (_rawBitmap.IsAllocated) _rawBitmap.Free();

            _rawBitmap = GCHandle.Alloc(_renderData.rawBitmap, GCHandleType.Pinned);
            _gameBitmap = new Bitmap(RenderData.GameWidth, RenderData.GameHeight, RenderData.GameWidth * 4, PixelFormat.Format32bppPArgb, _rawBitmap.AddrOfPinnedObject());

            Invalidate();
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_gameBitmap == null || _renderData == null || !_renderData.gameStarted) return;

            Graphics _renderTarget = e.Graphics;
            _renderTarget.CompositingMode = CompositingMode.SourceCopy;
            _renderTarget.InterpolationMode = _renderData._filterMode == RenderData.FilterMode.Linear
                ? InterpolationMode.Bilinear
                : InterpolationMode.NearestNeighbor;

            _renderTarget.DrawImage(_gameBitmap, 0, 0, Size.Width, Size.Height);
        }
    }
}
