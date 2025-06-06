using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.HUD
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public abstract class IBotonMenu
    {
        protected IBotonMenu _up;
        protected IBotonMenu _down;
        
        private HUD.HImagen _imagen;

        float _tamanioNormalBoton = 0.1f;
        float _tamanioElectoBoton = 0.15f;

        public void CargarBotones(IBotonMenu up, IBotonMenu down)
        {
            this._up = up;
            this._down = down;
        }

        public void CargarImagen(GraphicsDevice device, ContentManager content, string pathTextura , Vector2 pos){

            this._imagen = new HImagen();
            this._imagen.Initialize(pos, content, pathTextura);
            this._imagen.setQuad(_tamanioNormalBoton, device);

        }

        public void Electo(GraphicsDevice device)
        {
            this._imagen.setQuad(_tamanioElectoBoton, device);
        }

        public void NoElecto(GraphicsDevice device)
        {
            this._imagen.setQuad(_tamanioNormalBoton, device);
        }

        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            _imagen.Dibujado(graphicsDevice);
        }


        public IBotonMenu Up()
        {
            return _up;
        }
        public IBotonMenu Down()
        {
            return _down;
        }
        public abstract void Enter();


    }
}