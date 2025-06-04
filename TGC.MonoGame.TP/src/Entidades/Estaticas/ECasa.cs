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
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class ECasa : EntidadFull
    {

        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public ECasa() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._modelo = new Casas.OCasa();
            this._tipo = TipoEntidad.Obstaculo;
            //Crear Bounding Volume
            base.Initialize(Graphics, Mundo, Content, escenario);
        }

        public void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario, Vector3 pos)
        {
            this.Initialize(Graphics, Mundo, Content, escenario);
            this._boundingVolume = new BVCuboAABB(ObtenerMinimo(pos), ObtenerMaximo(pos));
        }

        private Vector3 ObtenerMinimo(Vector3 pos)
        {
            Vector3 ret = new Vector3(pos.X - (7.0f / 2.0f), pos.Y, pos.Z - (7.0f / 2.0f));
            return ret;
        }

        private Vector3 ObtenerMaximo(Vector3 pos)
        {
            Vector3 ret = new Vector3(pos.X + (7.0f / 2.0f), pos.Y + 7.0f, pos.Z + (7.0f / 2.0f));
            return ret;
        }
        
        public void SetMolde(MoldeCasa molde)
        {
            this._molde = molde;
        }
        
    }
}