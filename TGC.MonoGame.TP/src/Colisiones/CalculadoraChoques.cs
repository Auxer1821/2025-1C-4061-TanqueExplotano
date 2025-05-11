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

        // ----------------------------- Coliciones entre / Rayo - Esfera --------------------------------------------- //

        public static Boolean DetectarColisiones (BVRayo rayoMovimento, BVEsfera esferaChocada){
            
            float Cx = rayoMovimento._PuntoPartda.X - esferaChocada._centro.X;
            float Cy = rayoMovimento._PuntoPartda.Y - esferaChocada._centro.Y;
            float Cz = rayoMovimento._PuntoPartda.Z - esferaChocada._centro.Z;

            float Bx = Cx * rayoMovimento._Direccion.X;
            float By = Cy * rayoMovimento._Direccion.Y;
            float Bz = Cz * rayoMovimento._Direccion.Z;

            Cx = sqr(Cx);
            Cy = sqr (Cy);
            Cz = sqr (Cz);
            
            float B = 2*(Bx + By + Bz);
            float C = Cx + Cy + Cz - sqr(esferaChocada._radio);

            if( C <= 0) return true;

            if ( B>= 0) return false;

            return ( sqr(B)- 4*C ) >= 0;
        }

        public static DataChoque ParametrosChoque (BVRayo rayoMovimento, BVEsfera esferaChocada){


            // c 000   p 000  d 100  r 1
            float Cx = rayoMovimento._PuntoPartda.X - esferaChocada._centro.X; //0
            float Cy = rayoMovimento._PuntoPartda.Y - esferaChocada._centro.Y; //0
            float Cz = rayoMovimento._PuntoPartda.Z - esferaChocada._centro.Z; //0

            float Bx = Cx * rayoMovimento._Direccion.X; //0
            float By = Cy * rayoMovimento._Direccion.Y; //0
            float Bz = Cz * rayoMovimento._Direccion.Z; //0

            Cx = sqr(Cx); //0
            Cy = sqr (Cy); //0
            Cz = sqr (Cz); // 0
            
            float B = 2 * (Bx + By + Bz); // 0
            float C = Cx + Cy + Cz - sqr(esferaChocada._radio); // -1

            float determinante = (float) Math.Sqrt(sqr(B) - 4*C ); // 2

            float raiz1 = (-B - determinante) * 0.5f; // -1
            float raiz2 = (-B + determinante) * 0.5f; // 1

            float lamda = 0.0f;

            if (raiz1 > 0 && raiz2 > 0) 
                lamda = Math.Min(raiz1, raiz2);
            else if (raiz1 > 0) 
                lamda = raiz1;
            else 
                lamda = raiz2;


            Vector3 puntoContacto = rayoMovimento._PuntoPartda + lamda * rayoMovimento._Direccion;
            Vector3 normal = (puntoContacto - esferaChocada._centro) / esferaChocada._radio;
            return new DataChoque(puntoContacto, 0, normal);
        }

        // ----------------------------- Coliciones entre CubosAABB / CuboAAABB - CuboAABB --------------------------------------------- //
        public static bool DetectarColisiones (BVCuboAABB cubo1, BVCuboAABB cubo2){
            if (cubo1._minimo.X > cubo2._maximo.X || cubo2._minimo.X > cubo1._maximo.X) return false;
            if (cubo1._minimo.Y > cubo2._maximo.Y || cubo2._minimo.Y > cubo1._maximo.Y) return false;
            if (cubo1._minimo.Z > cubo2._maximo.Z || cubo2._minimo.Z > cubo1._maximo.Z) return false;
            return true;

        }
        public static DataChoque ParametrosChoque (BVCuboAABB cuboMovimiento, BVCuboAABB cuboChocado)
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
        
        // ----------------------------- Coliciones entre CuboAABB - Esfera / Esfera - CuboAABB --------------------------------------------- //
        public static bool DetectarColisiones (BVCuboAABB cuboMovimento, BVEsfera esferaChocada){
            
            float distancia2 = 0.0f;

            if (esferaChocada._centro.X < cuboMovimento._minimo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._minimo.X);
            else if (esferaChocada._centro.X > cuboMovimento._maximo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._maximo.X);

            if (esferaChocada._centro.Y < cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);
            else if (esferaChocada._centro.Y > cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);

            if (esferaChocada._centro.Z< cuboMovimento._minimo.Z) distancia2 += sqr(esferaChocada._centro.Z- cuboMovimento._minimo.Z);
            else if (esferaChocada._centro.Z> cuboMovimento._maximo.Z) distancia2 += sqr(esferaChocada._centro.Z- cuboMovimento._maximo.Z);

            return distancia2 <= esferaChocada._radio * esferaChocada._radio;
        }

        public static bool DetectarColisiones (BVEsfera esferaMovimento, BVCuboAABB cuboChocado){
            return CalculadorasChoque.DetectarColisiones(cuboChocado,esferaMovimento);
        }



        public static DataChoque ParametrosChoque (BVCuboAABB cuboMovimento, BVEsfera esferaChocada){
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

        public static DataChoque ParametrosChoque (BVEsfera esferaMovimento, BVCuboAABB cuboChocado){
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





                public static bool DetectarColisiones(BVCuboOBB a, BVCuboOBB b)
        {
            // ---------------------------------------
            // Extraemos los ejes locales de cada caja
            // Estos ejes representan cómo están rotadas en el espacio
            // ---------------------------------------

            Vector3 aRight = a.Orientacion.Right;       // Eje X de la caja A
            Vector3 aUp = a.Orientacion.Up;             // Eje Y de la caja A
            Vector3 aForward = a.Orientacion.Backward;  // Eje Z de la caja A

            Vector3 bRight = b.Orientacion.Right;       // Eje X de la caja B
            Vector3 bUp = b.Orientacion.Up;             // Eje Y de la caja B
            Vector3 bForward = b.Orientacion.Backward;  // Eje Z de la caja B

            // ---------------------------------------
            // Vector entre los centros de A y B
            // Este vector se proyectará en cada eje
            // ---------------------------------------

            Vector3 centroDiff = b.Centro - a.Centro;

            // ---------------------------------------
            // Test 1: Ejes de A
            // Si hay separación en cualquiera de estos, no hay colisión
            // ---------------------------------------

            if (Separan(a, b, aRight, centroDiff)) return false;
            if (Separan(a, b, aUp, centroDiff)) return false;
            if (Separan(a, b, aForward, centroDiff)) return false;

            // ---------------------------------------
            // Test 2: Ejes de B
            // ---------------------------------------

            if (Separan(a, b, bRight, centroDiff)) return false;
            if (Separan(a, b, bUp, centroDiff)) return false;
            if (Separan(a, b, bForward, centroDiff)) return false;

            // ---------------------------------------
            // Test 3: Productos cruzados entre ejes de A y B (9 combinaciones)
            // Cada uno de estos genera un eje perpendicular a ambos
            // Si hay separación en alguno de estos, no hay colisión
            // ---------------------------------------

            Vector3 axis1 = Vector3.Cross(aRight, bRight);
            if (axis1.LengthSquared() > 1e-6f && Separan(a, b, axis1, centroDiff)) return false;

            Vector3 axis2 = Vector3.Cross(aRight, bUp);
            if (axis2.LengthSquared() > 1e-6f && Separan(a, b, axis2, centroDiff)) return false;

            Vector3 axis3 = Vector3.Cross(aRight, bForward);
            if (axis3.LengthSquared() > 1e-6f && Separan(a, b, axis3, centroDiff)) return false;

            Vector3 axis4 = Vector3.Cross(aUp, bRight);
            if (axis4.LengthSquared() > 1e-6f && Separan(a, b, axis4, centroDiff)) return false;

            Vector3 axis5 = Vector3.Cross(aUp, bUp);
            if (axis5.LengthSquared() > 1e-6f && Separan(a, b, axis5, centroDiff)) return false;

            Vector3 axis6 = Vector3.Cross(aUp, bForward);
            if (axis6.LengthSquared() > 1e-6f && Separan(a, b, axis6, centroDiff)) return false;

            Vector3 axis7 = Vector3.Cross(aForward, bRight);
            if (axis7.LengthSquared() > 1e-6f && Separan(a, b, axis7, centroDiff)) return false;

            Vector3 axis8 = Vector3.Cross(aForward, bUp);
            if (axis8.LengthSquared() > 1e-6f && Separan(a, b, axis8, centroDiff)) return false;

            Vector3 axis9 = Vector3.Cross(aForward, bForward);
            if (axis9.LengthSquared() > 1e-6f && Separan(a, b, axis9, centroDiff)) return false;

            // ---------------------------------------
            // Si no encontramos ningún eje de separación, hay colisión
            // ---------------------------------------

            return true;
        }

        private static bool Separan(BVCuboOBB a, BVCuboOBB b, Vector3 axis, Vector3 centroDiff)
        {
            // Si el eje es muy pequeño, no se considera válido
            if (axis.LengthSquared() < 1e-6f)
                return false;

            // Normalizamos el eje de prueba
            axis.Normalize();

            // Proyectamos cada caja sobre ese eje
            float proyA = ProyectarOBB(a, axis);
            float proyB = ProyectarOBB(b, axis);

            // Proyectamos la distancia entre centros sobre el eje
            float distanciaCentros = MathF.Abs(Vector3.Dot(centroDiff, axis));

            // Si la distancia es mayor que la suma de proyecciones => hay separación
            return distanciaCentros > (proyA + proyB);
        }

        private static float ProyectarOBB(BVCuboOBB obb, Vector3 axis)
        {
            // Obtenemos los ejes locales del OBB
            Vector3 right = obb.Orientacion.Right;
            Vector3 up = obb.Orientacion.Up;
            Vector3 forward = obb.Orientacion.Backward;

            // Cada eje contribuye con su proyección escalada por el tamaño (half-extent)
            float proyRight = MathF.Abs(Vector3.Dot(axis, right)) * obb.Tamaño.X;
            float proyUp = MathF.Abs(Vector3.Dot(axis, up)) * obb.Tamaño.Y;
            float proyForward = MathF.Abs(Vector3.Dot(axis, forward)) * obb.Tamaño.Z;

            // La proyección total es la suma de las 3
            return proyRight + proyUp + proyForward;
        }
        

        

        
    }
}