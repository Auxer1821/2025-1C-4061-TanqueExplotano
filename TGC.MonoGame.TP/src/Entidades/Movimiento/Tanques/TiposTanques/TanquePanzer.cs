using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;


namespace TGC.MonoGame.TP.src.Tanques
{
    /// <summary>
///     Clase para el tipo de tanque "Panzer"
    /// </summary>
    public class TanquePanzer:TipoTanque
    {
        //----------------------------variables------------------------//

        public float vida(){return 60f;}
        public float velocidad(){return 30f;}
        public float danio(){return 100f;}
        public string directorioModelo(){return "/Panzer/Panzer";}
        public string directorioTextura(){return "/T90/textures_mod/hullA";}
        public float angulo(){return 0f;}
        public float escala(){return 0.02f;}
        public float cooldown(){return 10f;}

        //---------------------------Constructor----------------------//
        public TanquePanzer(){}

    }
}