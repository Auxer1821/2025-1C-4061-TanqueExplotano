using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Moldes;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>


    //cambio de endidad full primitiva a entidad full
    public class EArbol : Entidades.EntidadFull
    {
        public EArbol() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._modelo = new Arboles.OArbol();
            this._tipo = TipoEntidad.Obstaculo;
            base.Initialize(Graphics, Mundo, Content, escenario);
            this._boundingVolume = new BVCilindroAABB(this._posicion + Vector3.UnitY * 3, 1, 3);

        }
        public void SetMolde(MoldeArbol molde)
        {
            this._molde = molde;
        }


    }
}