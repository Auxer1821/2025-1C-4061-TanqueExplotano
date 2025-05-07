using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase de esfera para los bounding Volume.
    /// </summary>
    public static class CalculadorasChoque
    {
        

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        // 1er objeto (movil) CHOCA al 2do objeto (estatico)

        // Coliciones entre esferas
        public static Boolean DetectarColisiones (BVEsfera esferaMovimento, BVEsfera esferaChocada){
            float x = (esferaMovimento._cento.X- esferaChocada._cento.X);
            float y = (esferaMovimento._cento.Y- esferaChocada._cento.Y);
            float z = (esferaMovimento._cento.Z- esferaChocada._cento.Z);
            float radios = esferaMovimento._radio + esferaChocada._radio;
            
            return (x*x + y*y + z*z)<= radios*radios ;
        }
        public static DataChoque ParametrosChoque (BVEsfera esferaMovimento, BVEsfera esferaChocada)
        {
            float x = (esferaMovimento._cento.X- esferaChocada._cento.X);
            float y = (esferaMovimento._cento.Y- esferaChocada._cento.Y);
            float z = (esferaMovimento._cento.Z- esferaChocada._cento.Z);
            
            float distCentros = (float) Math.Sqrt(x*x + y*y + z*z);
            
            float penetracion = esferaMovimento._radio+esferaChocada._radio-distCentros;

            Vector3 normal = new Vector3(x,y,z) / distCentros;

            Vector3 puntoContacto = esferaChocada._cento + normal * esferaChocada._radio;
            
            return new DataChoque(puntoContacto, penetracion, normal);
        }



        // Coliciones entre Cubos
        public static bool DetectarColisiones (BVCubo cubo1, BVCubo cubo2){
            if (cubo1._minimo.X > cubo2._maximo.X || cubo2._minimo.X > cubo1._maximo.X) return false;
            if (cubo1._minimo.Y > cubo2._maximo.Y || cubo2._minimo.Y > cubo1._maximo.Y) return false;
            if (cubo1._minimo.Z > cubo2._maximo.Z || cubo2._minimo.Z > cubo1._maximo.Z) return false;
            return true;

        }
        public static DataChoque ParametrosChoque (BVCubo cuboMovimiento, BVCubo cuboChocado)
        {
            float dif_centros_x = cuboChocado.Centro().X - cuboMovimiento.Centro().X;
            float dif_centros_y = cuboChocado.Centro().Y - cuboMovimiento.Centro().Y;
            float dif_centros_z = cuboChocado.Centro().Z - cuboMovimiento.Centro().Z;

            float pen_x = ((cuboChocado.LadoX() + cuboMovimiento.LadoX()) / 2) - Math.Abs(dif_centros_x);
            float pen_y = ((cuboChocado.LadoY() + cuboMovimiento.LadoY()) / 2) - Math.Abs(dif_centros_y);
            float pen_z = ((cuboChocado.LadoZ() + cuboMovimiento.LadoZ()) / 2) - Math.Abs(dif_centros_z);
            
            float penetracion =  Math.Min(Math.Min(pen_x, pen_y),pen_z);
            Vector3 normal = Vector3.Zero;
            
            if ( penetracion == pen_x)
            normal = Vector3.UnitX * Math.Sign(dif_centros_x);
            else if ( penetracion == pen_y)
            normal = Vector3.UnitZ * Math.Sign(dif_centros_y);
            else if ( penetracion == pen_z)
            normal = Vector3.UnitZ * Math.Sign(dif_centros_z);

            Vector3 puntoContacto = (cuboMovimiento.Centro() + cuboChocado.Centro())/2;

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        public static bool DetectarColisiones (BVCubo vbcubo, BVEsfera vbesfera){

            return true;
        }
        

        
    }
}