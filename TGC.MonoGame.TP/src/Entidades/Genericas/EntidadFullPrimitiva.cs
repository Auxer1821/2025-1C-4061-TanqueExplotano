using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class EntidadFullPrimitiva : EntidadFull
    {

        // Variables
        protected Objeto _objeto;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this.InicializarDataMundo();

            this._modelo = null;
            this._objeto.Initialize(Graphics, Mundo, Content);
            this._escenario = escenario;
            this._posicion = Vector3.Transform(Vector3.Zero, Mundo);
            this._boundingVolume = new BoundingsVolumes.BVEsfera(3.0f, this._posicion);
            this._molde = null;

        }

        public override void Dibujar(GraphicsDevice graphics)
        {
            this._objeto.Dibujar(graphics);
        }


        public override void ActualizarMatrizMundo()
        {
            Matrix mundo = Matrix.Identity;
            mundo *= Matrix.CreateScale(this._escala);
            mundo *= Matrix.CreateFromYawPitchRoll(this._angulo.Z, this._angulo.Y, this._angulo.X);
            mundo *= Matrix.CreateTranslation(this._posicion);

            this._objeto.ActualizarMatrizMundo(mundo);

        }
        public override void EfectCamera(Matrix vista, Matrix proyeccion)
        {
            _objeto.EfectCamera(vista, proyeccion);
        }
        
        public override Matrix GetMundo()
        {
            return _objeto.GetMundo();
        }
        
    }
}