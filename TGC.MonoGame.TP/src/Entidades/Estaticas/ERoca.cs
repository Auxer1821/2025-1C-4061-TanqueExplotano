using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>

    
    //cambio de endidad full primitiva a entidad full
    public class ERoca : Entidades.EntidadFull
    {
        public ERoca(){}
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._modelo = new Rocas.ORoca();
            this._tipo = TipoEntidad.Obstaculo;
            base.Initialize(Graphics, Mundo, Content, escenario);
            this._boundingVolume = new BoundingsVolumes.BVEsfera(1.0f, this._posicion);
        }
    }
}