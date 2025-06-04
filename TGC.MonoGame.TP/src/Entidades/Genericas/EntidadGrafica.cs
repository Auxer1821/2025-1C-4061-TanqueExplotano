using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Modelos;
using TGC.MonoGame.TP.src.Moldes;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class EntidadGrafica : Entidad
    {

        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//

        public virtual void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this.InicializarDataMundo();

            this._boundingVolume = null;
            this._molde = null;
            this._modelo.Initialize(Graphics, Mundo, Content);
            this._escenario = escenario;
            this._posicion = Vector3.Transform(Vector3.Zero, Mundo);
        }

        //----------------------------------------------Metodos--------------------------------------------------//

        public override bool PuedeChocar()
        {
            return false;
        }
        public override bool PuedeSerChocado()
        {
            return false;
        }
        public override bool PuedeDibujar()
        {
            return true;
        }

        public override void Dibujar(GraphicsDevice graphics)
        {
            this._modelo.Dibujar(graphics);
        }

        public virtual void ActualizarMatrizMundo()
        {
            Matrix mundo = Matrix.Identity;
            mundo *= Matrix.CreateScale(this._escala);
            mundo *= Matrix.CreateFromYawPitchRoll(this._angulo.Z, this._angulo.Y, this._angulo.X);
            mundo *= Matrix.CreateTranslation(this._posicion);

            this._modelo.ActualizarMatrizMundo(mundo);

        } 
        
        public override Matrix GetMundo()
        {
            return _modelo.GetMundo();
        }
    }
}