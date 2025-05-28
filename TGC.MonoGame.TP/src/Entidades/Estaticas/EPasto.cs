using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.Pastos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>


    public class EPasto : Entidades.EntidadGraficaPrimitiva
    {

        public EPasto() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._objeto = new Pastos.OPasto();
            this._tipo = TipoEntidad.Otro;//TODO - Actualizar
            base.Initialize(Graphics, Mundo, Content, escenario);
        }

        public void ActualizarTime(float time)
        {
            ((OPasto)(_objeto)).ActualizarTime(time);
        }

    }
}