using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class BoundingVolume
    {
        
        // Variables
        protected Modelos.Modelo _modelo;
        protected BoundingVolume _boundingVolume;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 

        public abstract Boolean DetectarColisiones ();
        public abstract Boolean ParametrosChoque ();

        
    }
}