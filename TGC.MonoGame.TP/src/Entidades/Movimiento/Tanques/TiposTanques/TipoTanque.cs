using System;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;


namespace TGC.MonoGame.TP.src.Tanques
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public interface TipoTanque
    {

        //----------------------------metodos------------------------//
        float velocidad();
        float anguloRotacionMovimiento();
        float danio();
        string directorioModelo();
        Vector3 angulo(); //Es el vector que debe ser perpendicular a la normal
        public string directorioTextura();
        public string directorioTexturaNormal();
        public string directorioTexturaCinta();
        public string directorioTexturaCintaNormal();
        float escala();
        float cooldown();

        float Vida();
        float VidaMaxima();
        void RecibirDanio(float danio);

        bool EstaVivo();
        float RepararDeformaciones();

    }
}