using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class ECaja : EntidadFullPrimitiva
    {

        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public ECaja() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._objeto = new Cajas.OCaja();
            this._tipo = TipoEntidad.Obstaculo;
            //Crear Bounding Volume
            base.Initialize(Graphics, Mundo,  Content, escenario);
            this._boundingVolume = new BVCuboAABB(ObtenerMinimo(this._posicion), ObtenerMaximo(this._posicion));
        }
        
        
        private Vector3 ObtenerMinimo(Vector3 pos)
        {
            Vector3 ret = new Vector3(pos.X - (1.0f / 2.0f), pos.Y, pos.Z - (1.0f / 2.0f));
            return ret;
        }
        
        private Vector3 ObtenerMaximo(Vector3 pos)
        {
            Vector3 ret = new Vector3( pos.X+(1.0f/2.0f) , pos.Y+1.0f , pos.Z+(1.0f/2.0f) );
            return ret;
        }
        
    }
}