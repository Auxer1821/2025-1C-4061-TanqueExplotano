using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;


namespace TGC.MonoGame.TP.src.Tanques
{
    /// <summary>
    ///     Clase para el tipo de tanque "T90"
    /// </summary>
    public class TanqueT90:TipoTanque
    {
        //----------------------------variables------------------------//
        public float vida(){return 140f;}
        public float velocidad(){return 10f;}
        public float danio(){return 90f;}
        public string directorioModelo(){return "/T90/T90";}
        public string directorioTextura(){return "/T90/textures_mod/hullB";}
        public float angulo(){return 4.71f;}
        public float escala(){return 4f;}
        public float cooldown(){return 10f;}

        //---------------------------Constructor----------------------//
        public TanqueT90(){}

    }
}