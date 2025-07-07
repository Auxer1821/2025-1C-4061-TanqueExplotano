using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.Moldes;
using TGC.MonoGame.TP.src.Cameras;



namespace TGC.MonoGame.TP.src.Graficos.Utils
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ShadowMapping
    {
        // para el postprocesado
        public const int _shadowmapSize = 2048 * 3;
        private const float _lightCameraFarPlaneDistance = 2300f;
        private const float _lightCameraNearPlaneDistance = 500f;
        private RenderTarget2D _shadowMapRenderTarget;
        private TargetCamera _targetLightCamera;
        private Vector3 _posSOL;


        private IndexBuffer _indexBuffer;
        private VertexBuffer _vertexBuffer;

        public ShadowMapping()
        {
            this._posSOL = new Vector3();
        }
        public void Initialize(Vector3 posSOL, GraphicsDevice graphics)
        {

            this._posSOL = posSOL;
            _targetLightCamera = new TargetCamera(1f, _posSOL, Vector3.Zero);
            _targetLightCamera.BuildProjection(1f, _lightCameraNearPlaneDistance, _lightCameraFarPlaneDistance, MathHelper.PiOver2 - 0.70f);
            _shadowMapRenderTarget = new RenderTarget2D(graphics, _shadowmapSize, _shadowmapSize, false,
                SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            
            this.ConfigurarQuadRenderTarget(graphics);
        }


        public void ActualizarShadowMap(GraphicsDevice graphics, List<Entidades.Entidad> listaEntidades, Terrenos.Terreno terreno)
        {
            this.ActualizarDepthMap(graphics);
            this.DibujarShadowMap(graphics, listaEntidades, terreno);
            graphics.SetRenderTarget(null);
            //graphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
        }
        private void ActualizarDepthMap(GraphicsDevice graphics)
        {
            graphics.DepthStencilState = DepthStencilState.Default; // Si falla ver de sacarlo y si cambia algo
            graphics.SetRenderTarget(_shadowMapRenderTarget);
            graphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0); // Si falla o algo raro comentar
        }


        protected virtual Effect ConfigEfectos2(GraphicsDevice graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/BasicShader");
        }

        public void DibujarShadowMap(GraphicsDevice graphics, List<Entidades.Entidad> listaEntidades, Terrenos.Terreno terreno)
        {
            this.DibujarShadowMap(graphics, terreno);
            this.DibujarShadowMap(graphics, listaEntidades);
        }
        public void DibujarShadowMap(GraphicsDevice graphics, List<Entidades.Entidad> listaEntidades)
        {
            foreach (var entidad in listaEntidades)
            {
                
                if(entidad._tipo == Entidades.TipoEntidad.Obstaculo)
                {
                    Matrix mvp = entidad.GetMundo() * _targetLightCamera.Vista * _targetLightCamera.Proyeccion;
                    entidad.GetMolde().DibujarShadowMap(mvp, graphics);
                }
                else // tanque
                {
                    entidad.DibujarShadowMap(graphics, _targetLightCamera.Vista, _targetLightCamera.Proyeccion);
                }
            }
        }

        public void DibujarShadowMap(GraphicsDevice graphics, Terrenos.Terreno terreno)
        {
            terreno.DibujarShadowMap(graphics, _targetLightCamera.Vista, _targetLightCamera.Proyeccion);
        }

        private void ConfigurarQuadRenderTarget(GraphicsDevice graphics)
        {
            var vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(-1f, -1f, 0f);
            vertices[0].TextureCoordinate = new Vector2(0f, 1f);
            vertices[1].Position = new Vector3(-1f, 1f, 0f);
            vertices[1].TextureCoordinate = new Vector2(0f, 0f);
            vertices[2].Position = new Vector3(1f, -1f, 0f);
            vertices[2].TextureCoordinate = new Vector2(1f, 1f);
            vertices[3].Position = new Vector3(1f, 1f, 0f);
            vertices[3].TextureCoordinate = new Vector2(1f, 0f);

            _vertexBuffer = new VertexBuffer(graphics, VertexPositionTexture.VertexDeclaration, 4,
                BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

               var indices = new ushort[6];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            _indexBuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }


        public RenderTarget2D GetShadowMap(){
            return this._shadowMapRenderTarget;
        }

        internal int GetShadowMapSize()
        {
            return _shadowmapSize;
        }

        internal Matrix GetLightViewProjection()
        {
            return _targetLightCamera.Vista * _targetLightCamera.Proyeccion;
        }
    }
}