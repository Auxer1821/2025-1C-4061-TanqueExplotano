using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase de esfera para los bounding Volume.
    /// </summary>
    public class BVEsfera : BoundingsVolumes.BoundingVolume
    {
        
        // Variables
        public float _radio { get; set;}
        public Vector3 _centro { get; set;}
        public float _Oradio { get; set;}
        public Vector3 _Ocentro { get; set;}
        private BoundingSphere _esferaTemplate;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        public BVEsfera(float radio, Vector3 centro)
        {
            _radio = radio;
            _centro = centro;
            _Oradio = radio;
            _Ocentro = centro;
            _esferaTemplate.Radius = _radio;
            _esferaTemplate.Center = _centro;
        }

        public BVEsfera(float radio)
        {
            _radio = radio;
            _centro =  Vector3.Zero;
            _Oradio = radio;
            _Ocentro = Vector3.Zero;
        }

        //----------------------------------------------Funciones-de-Detección--------------------------------------------------// 
        public override void Transformar(Vector3 nuevaPosicion, Vector3 rotacionEuler, float escala)
        {
            //SE ESCALA
            this._radio = _Oradio * escala;
            //SE REUBICA
            this._centro = nuevaPosicion;
        }

        public override Vector3 GetCentro(){
            return _centro;
        }

        internal override bool FrustumCulling(BVTrufas boundingFrustum)
        {
            return boundingFrustum.Frustum.Intersects(_esferaTemplate);
        }
        
    }
}