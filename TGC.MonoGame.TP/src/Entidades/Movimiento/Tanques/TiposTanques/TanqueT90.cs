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
        public float vida = 140f;
        public float velocidad(){return 35f;}
        public float anguloRotacionMovimiento(){return 1.0f;}
        public float danio(){return 90f;}
        public string directorioModelo(){return "/T90/T90";}
        public Vector3 angulo(){return new Vector3(4.71f, 4.71f, 0.0f);}
        public float escala(){return 2.0f;} //Posible 0.1f
        public string directorioTextura(){return "/T90/textures_mod/hullB2";}
        public string directorioTexturaNormal(){return "/T90/textures_mod/normal";}
        public string directorioTexturaCinta(){ return "/T90/textures_mod/treadmills"; }
        public string directorioTexturaCintaNormal(){ return "/T90/textures_mod/treadmills_normal"; }
        public float cooldown(){return 1.5f;}
        public float Vida(){return vida;}
        public float VidaMaxima(){return 14000f;}
        public void RecibirDanio(float danio){this.vida -= danio;}
        public bool EstaVivo(){ return vida > 0; }

        //---------------------------Constructor----------------------//
        public TanqueT90(){}

    }
}