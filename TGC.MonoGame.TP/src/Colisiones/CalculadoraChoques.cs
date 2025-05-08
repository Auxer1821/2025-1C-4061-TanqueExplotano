using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
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
        //funciones aux
        public static float sqr (float x) {return x * x;}
        
        /*
            Supuestos:
             - 1er objeto (movil) CHOCA al 2do objeto (estatico)
             - choque:punto de choque es respecto al estatico
        */

        // ----------------------------- Coliciones entre Esferas / Esfera - Esfera --------------------------------------------- //

        public static Boolean DetectarColisiones (BVEsfera esferaMovimento, BVEsfera esferaChocada){
            float x = (esferaMovimento._centro.X- esferaChocada._centro.X);
            float y = (esferaMovimento._centro.Y- esferaChocada._centro.Y);
            float z = (esferaMovimento._centro.Z- esferaChocada._centro.Z);
            float radios = esferaMovimento._radio + esferaChocada._radio;
            
            return (x*x + y*y + z*z)<= radios*radios ;
        }
        public static DataChoque ParametrosChoque (BVEsfera esferaMovimento, BVEsfera esferaChocada)
        {
            float x = (esferaMovimento._centro.X- esferaChocada._centro.X);
            float y = (esferaMovimento._centro.Y- esferaChocada._centro.Y);
            float z = (esferaMovimento._centro.Z- esferaChocada._centro.Z);
            
            float distCentros = (float) Math.Sqrt(x*x + y*y + z*z);
            
            float penetracion = esferaMovimento._radio+esferaChocada._radio-distCentros;

            Vector3 normal = new Vector3(x,y,z) / distCentros;

            Vector3 puntoContacto = esferaChocada._centro + normal * esferaChocada._radio;
            
            return new DataChoque(puntoContacto, penetracion, normal);
        }



        // ----------------------------- Coliciones entre Cubos / Cubo - Cubo --------------------------------------------- //
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
        
        // ----------------------------- Coliciones entre Cubo - Esfera / Esfera - Cubo --------------------------------------------- //
        public static bool DetectarColisiones (BVCubo cuboMovimento, BVEsfera esferaChocada){
            
            float distancia2 = 0.0f;

            if (esferaChocada._centro.X < cuboMovimento._minimo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._minimo.X);
            else if (esferaChocada._centro.X > cuboMovimento._maximo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._maximo.X);

            if (esferaChocada._centro.Y < cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);
            else if (esferaChocada._centro.Y > cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);

            if (esferaChocada._centro.Z< cuboMovimento._minimo.Z) distancia2 += sqr(esferaChocada._centro.Z- cuboMovimento._minimo.Z);
            else if (esferaChocada._centro.Z> cuboMovimento._maximo.Z) distancia2 += sqr(esferaChocada._centro.Z- cuboMovimento._maximo.Z);

            return distancia2 <= esferaChocada._radio * esferaChocada._radio;
        }

        public static bool DetectarColisiones (BVEsfera esferaMovimento, BVCubo cuboChocado){
            return CalculadorasChoque.DetectarColisiones(cuboChocado,esferaMovimento);
        }



        public static DataChoque ParametrosChoque (BVCubo cuboMovimento, BVEsfera esferaChocada){
            // 1. Punto más cercano del cubo al centro de la esfera
            Vector3 puntoMasCercano = new Vector3(
            Math.Clamp(esferaChocada._centro.X, cuboMovimento._minimo.X, cuboMovimento._maximo.X),
            Math.Clamp(esferaChocada._centro.Y, cuboMovimento._minimo.Y, cuboMovimento._maximo.Y),
            Math.Clamp(esferaChocada._centro.Z, cuboMovimento._minimo.Z, cuboMovimento._maximo.Z)
            );
            // 2. Vector desde el punto más cercano hacia el centro de la esfera
            //              D =         F            -           I
            Vector3 direccion = puntoMasCercano - esferaChocada._centro;
            float distancia = direccion.Length();
            Vector3 normal = Vector3.Zero;

            // 3. Normal: si la distancia es cero (centro dentro del cubo), elegimos un vector artificial
            if (distancia > 0)
            {
                normal = direccion / distancia; // unitario
            }
            else
            {
                // Centro justo en el borde o dentro completamente: usar normal arbitraria
                normal = Vector3.UnitX; // Podés mejorarlo si querés detectar qué eje está más cerca
                distancia = 0;
            }

            // 4. Punto de choque: en la superficie de la esfera, apuntando hacia el cubo
            Vector3 puntoContacto = esferaChocada._centro + normal * esferaChocada._radio;

            // 5. Penetración: cuánto se metió la esfera
            float penetracion = esferaChocada._radio - distancia;
            return new DataChoque(puntoContacto,penetracion,normal);
        }

        public static DataChoque ParametrosChoque (BVEsfera esferaMovimento, BVCubo cuboChocado){
            //TODO: Optimizar pq hacen lo mismo 
            //TODO: Entrega 3
            // 1. Punto más cercano del cubo al centro de la esfera
            Vector3 puntoContacto = new Vector3(
            Math.Clamp(esferaMovimento._centro.X, cuboChocado._minimo.X, cuboChocado._maximo.X),
            Math.Clamp(esferaMovimento._centro.Y, cuboChocado._minimo.Y, cuboChocado._maximo.Y),
            Math.Clamp(esferaMovimento._centro.Z, cuboChocado._minimo.Z, cuboChocado._maximo.Z)
            );
            // 2. Vector desde el punto más cercano hacia el centro de la esfera
            Vector3 direccion = esferaMovimento._centro - puntoContacto;
            float distancia = direccion.Length();
            Vector3 normal = Vector3.Zero;

            // 3. Normal: si la distancia es cero (centro dentro del cubo), elegimos un vector artificial
            if (distancia > 0)
            {
                normal = direccion / distancia; // unitario
            }
            else
            {
                // Centro justo en el borde o dentro completamente: usar normal arbitraria
                normal = Vector3.UnitX; // Podés mejorarlo si querés detectar qué eje está más cerca
                distancia = 0;
            }

            // 5. Penetración: cuánto se metió la esfera
            float penetracion = esferaMovimento._radio - distancia;
            return new DataChoque(puntoContacto,penetracion,normal);
        }
        
        

        
    }
}