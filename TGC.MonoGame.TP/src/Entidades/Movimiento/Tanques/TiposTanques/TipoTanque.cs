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
        float danio();
        string directorioModelo();
        float angulo();
        float escala();
        float cooldown();

        float Vida();
        void RecibirDanio(float danio);

        bool EstaVivo();

    }
}