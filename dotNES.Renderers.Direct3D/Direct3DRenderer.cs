﻿using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Windows.Forms;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;
using Resource = SharpDX.Direct3D11.Resource;

namespace dotNES.Renderers.Direct3D
{
    public class Direct3DRenderer : Control, IRenderer
    {
        Device device;
        SwapChain swapChain;
        RenderTarget d2dRenderTarget;
        Bitmap gameBitmap;
        RawRectangleF clientArea;

        private RenderData _renderData;
        private readonly object _drawLock = new object();

        public string RendererName => "Direct3D";

        public void InitRendering(RenderData renderData)
        {
            lock (_drawLock)
            {
                if (renderData == null) return;
                _renderData = renderData; ;

                ResizeRedraw = true;
                var desc = new SwapChainDescription
                {
                    BufferCount = 1,
                    ModeDescription = new ModeDescription(ClientSize.Width, ClientSize.Height, new Rational(60, 1),
                        Format.R8G8B8A8_UNorm),
                    IsWindowed = true,
                    OutputHandle = Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                Device.CreateWithSwapChain(DriverType.Hardware,
                    DeviceCreationFlags.BgraSupport,
                    new[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 },
                    desc,
                    out device,
                    out swapChain);

                var d2dFactory = new SharpDX.Direct2D1.Factory();

                Factory factory = swapChain.GetParent<Factory>();
                factory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAll);

                Texture2D backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);

                Surface surface = backBuffer.QueryInterface<Surface>();

                d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                    new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Ignore));
                gameBitmap = new Bitmap(d2dRenderTarget, new Size2(RenderData.GameWidth, RenderData.GameHeight), bitmapProperties);

                clientArea = new RawRectangleF
                {
                    Left = 0,
                    Top = 0,
                    Bottom = ClientSize.Height,
                    Right = ClientSize.Width
                };

                factory.Dispose();
                surface.Dispose();
                backBuffer.Dispose();
                _renderData.ready = true;
            }
        }

        public void EndRendering()
        {
            DisposeDirect3D();
        }

        private void DisposeDirect3D()
        {
            lock (_drawLock)
            {
                if (_renderData != null && _renderData.ready)
                {
                    _renderData.ready = false;

                    d2dRenderTarget.Dispose();
                    swapChain.Dispose();
                    device.Dispose();
                    gameBitmap.Dispose();
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            try
            {
                DisposeDirect3D();
                InitRendering(_renderData);
                base.OnResize(e);
            }
            catch
            {
                // This is pretty stupid, but Mono will send a resize event to this component
                // even when it's not added to a frame, so this will fail horribly
                // during the renderer self-test procedure, which detects this type of failure...
                // on different thread.
            }
        }

        public void Draw()
        {
            lock (_drawLock)
            {
                if (_renderData == null || d2dRenderTarget == null || !_renderData.ready) return;
                d2dRenderTarget.BeginDraw();
                d2dRenderTarget.Clear(Color.Gray);

                if (_renderData.gameStarted)
                {
                    int stride = RenderData.GameWidth * 4;
                    gameBitmap.CopyFromMemory(_renderData.rawBitmap, stride);

                    d2dRenderTarget.DrawBitmap(gameBitmap, clientArea, 1f,
                        _renderData._filterMode == RenderData.FilterMode.Linear
                            ? BitmapInterpolationMode.Linear
                            : BitmapInterpolationMode.NearestNeighbor);
                }

                d2dRenderTarget.EndDraw();
                swapChain.Present(0, PresentFlags.None);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw();
            base.OnPaint(e);
        }
    }
}