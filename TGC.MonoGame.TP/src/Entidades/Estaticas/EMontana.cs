using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Moldes;
using TGC.MonoGame.TP.src.Objetos;

using TGC.MonoGame.TP.src.BoundingsVolumes;

namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class EMontana : Entidades.EntidadFullPrimitiva
    {
        public EMontana() { }
        
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._objeto = new Montanas.OMontana();
            this._tipo = TipoEntidad.Obstaculo;
            base.Initialize(Graphics, Mundo, Content, escenario);
            this._boundingVolume = new BVCuboAABB(ObtenerMinimo(this._posicion), ObtenerMaximo(this._posicion));
        }

        
        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            switch (entidadEstatica._tipo)
            {
                case TipoEntidad.Bala:
                    //this._escenario.AgregarAEliminar(this);
                    break;
                default:
                    base.Chocar(dataChoque, entidadEstatica);
                    break;
            }
        }
        
        

        public void SetMolde(MoldeMontana molde)
        {
            this._molde = molde;
        }

        //esta entidad tiene que ser dibujada siempre, no importa si esta fuera del frustum
        public override bool ExcluidoDelFrustumCulling()
        {
            return true;
        }

        private Vector3 ObtenerMinimo(Vector3 pos)
        {
            Vector3 ret = new Vector3(pos.X - 100.0f, pos.Y - 10f, pos.Z - 100.0f);
            return ret;
        }

        private Vector3 ObtenerMaximo(Vector3 pos)
        {
            Vector3 ret = new Vector3(pos.X + 100.0f, pos.Y + 200.0f, pos.Z + 100.0f);
            return ret;
        }
    }
}