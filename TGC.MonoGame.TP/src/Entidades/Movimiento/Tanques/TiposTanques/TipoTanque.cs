using System;
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
        float vida();
        float velocidad();
        float danio();
        string directorioModelo();
        float angulo();
        float escala();
        float cooldown();

    }
}