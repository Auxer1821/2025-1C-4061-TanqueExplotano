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

        public float vida = 60f;
        public float velocidad(){return 20f;}
        public float anguloRotacionMovimiento(){return 3f;}
        public float danio(){return 20f;} //100
        public string directorioModelo(){return "/Panzer/Panzer";}
        public Vector3 angulo(){return new Vector3(0.0f, -4.71f, 0.0f);}
        public string directorioTextura(){return "/Panzer/PzVl_Tiger_I";}
        public string directorioTexturaNormal(){return "/Panzer/PzVI_Tiger_I_track_NM";}
        public string directorioTexturaCinta(){ return "/Panzer/PzVI_Tiger_I_track"; }
        public string directorioTexturaCintaNormal(){ return "/Panzer/PzVI_Tiger_I_track_NM"; }
        public float escala(){return 0.015f;}
        public float cooldown(){return 1.0f;} //10
        public float Vida(){return vida;}
        public float VidaMaxima(){return 60f;}
        public void RecibirDanio(float danio){this.vida -= danio;}
        public bool EstaVivo(){ return vida > 0; }


        //---------------------------Constructor----------------------//
        public TanquePanzer() { }

    }
}