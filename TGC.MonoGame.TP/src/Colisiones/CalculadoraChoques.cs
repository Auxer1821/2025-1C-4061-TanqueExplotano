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
        public static float sqr(float x) { return x * x; }

        /*
            Supuestos:
             - 1er objeto (movil) CHOCA al 2do objeto (estatico)
             - choque:punto de choque es respecto al estatico
        */
        public static bool DetectarColisiones(BoundingVolume Movimiento, BoundingVolume Chocada)
        {
            // Usa despacho dinámico para elegir la sobrecarga correcta en tiempo de ejecución
            return DetectarColisiones((dynamic)Movimiento, (dynamic)Chocada);
        }
        public static DataChoque ParametrosChoque(BoundingVolume Movimiento, BoundingVolume Chocada)
        {
            return ParametrosChoque((dynamic)Movimiento, (dynamic)Chocada);
        }

        // ----------------------------- Coliciones entre Esferas / Esfera - Esfera --------------------------------------------- //

        public static Boolean DetectarColisiones(BVEsfera esferaMovimento, BVEsfera esferaChocada)
        {
            float x = (esferaMovimento._centro.X - esferaChocada._centro.X);
            float y = (esferaMovimento._centro.Y - esferaChocada._centro.Y);
            float z = (esferaMovimento._centro.Z - esferaChocada._centro.Z);
            float radios = esferaMovimento._radio + esferaChocada._radio;

            return (x * x + y * y + z * z) <= radios * radios;
        }
        public static DataChoque ParametrosChoque(BVEsfera esferaMovimento, BVEsfera esferaChocada)
        {
            float x = (esferaMovimento._centro.X - esferaChocada._centro.X);
            float y = (esferaMovimento._centro.Y - esferaChocada._centro.Y);
            float z = (esferaMovimento._centro.Z - esferaChocada._centro.Z);

            float distCentros = (float)Math.Sqrt(x * x + y * y + z * z);

            float penetracion = esferaMovimento._radio + esferaChocada._radio - distCentros;

            Vector3 normal = new Vector3(x, y, z) / distCentros;

            Vector3 puntoContacto = esferaChocada._centro + normal * esferaChocada._radio;

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        // ----------------------------- Coliciones entre / Rayo - Esfera --------------------------------------------- //
        public static Boolean DetectarColisiones(BVEsfera esferaChocada, BVRayo rayoMovimento) { return DetectarColisiones(rayoMovimento, esferaChocada); }
        public static Boolean DetectarColisiones(BVRayo rayoMovimento, BVEsfera esferaChocada)
        {

            float Cx = rayoMovimento._PuntoPartda.X - esferaChocada._centro.X;
            float Cy = rayoMovimento._PuntoPartda.Y - esferaChocada._centro.Y;
            float Cz = rayoMovimento._PuntoPartda.Z - esferaChocada._centro.Z;

            float Bx = Cx * rayoMovimento._Direccion.X;
            float By = Cy * rayoMovimento._Direccion.Y;
            float Bz = Cz * rayoMovimento._Direccion.Z;

            Cx = sqr(Cx);
            Cy = sqr(Cy);
            Cz = sqr(Cz);

            float B = 2 * (Bx + By + Bz);
            float C = Cx + Cy + Cz - sqr(esferaChocada._radio);

            if (C <= 0) return true;

            if (B >= 0) return false;

            return (sqr(B) - 4 * C) >= 0;
        }

        public static DataChoque ParametrosChoque(BVEsfera esferaChocada, BVRayo rayoMovimento) { return ParametrosChoque(rayoMovimento, esferaChocada); }
        public static DataChoque ParametrosChoque(BVRayo rayoMovimento, BVEsfera esferaChocada)
        {


            // c 000   p 000  d 100  r 1
            float Cx = rayoMovimento._PuntoPartda.X - esferaChocada._centro.X; //0
            float Cy = rayoMovimento._PuntoPartda.Y - esferaChocada._centro.Y; //0
            float Cz = rayoMovimento._PuntoPartda.Z - esferaChocada._centro.Z; //0

            float Bx = Cx * rayoMovimento._Direccion.X; //0
            float By = Cy * rayoMovimento._Direccion.Y; //0
            float Bz = Cz * rayoMovimento._Direccion.Z; //0

            Cx = sqr(Cx); //0
            Cy = sqr(Cy); //0
            Cz = sqr(Cz); // 0

            float B = 2 * (Bx + By + Bz); // 0
            float C = Cx + Cy + Cz - sqr(esferaChocada._radio); // -1

            float determinante = (float)Math.Sqrt(sqr(B) - 4 * C); // 2

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
        public static bool DetectarColisiones(BVCuboAABB cuboMovimiento, BVCuboAABB cuboChocado)
        {
            if (cuboMovimiento._minimo.X > cuboChocado._maximo.X || cuboChocado._minimo.X > cuboMovimiento._maximo.X) return false;
            if (cuboMovimiento._minimo.Y > cuboChocado._maximo.Y || cuboChocado._minimo.Y > cuboMovimiento._maximo.Y) return false;
            if (cuboMovimiento._minimo.Z > cuboChocado._maximo.Z || cuboChocado._minimo.Z > cuboMovimiento._maximo.Z) return false;
            return true;

        }
        public static DataChoque ParametrosChoque(BVCuboAABB cuboMovimiento, BVCuboAABB cuboChocado)
        {
            float dif_centros_x = cuboChocado.Centro().X - cuboMovimiento.Centro().X;
            float dif_centros_y = cuboChocado.Centro().Y - cuboMovimiento.Centro().Y;
            float dif_centros_z = cuboChocado.Centro().Z - cuboMovimiento.Centro().Z;

            float pen_x = ((cuboChocado.LadoX() + cuboMovimiento.LadoX()) / 2) - Math.Abs(dif_centros_x);
            float pen_y = ((cuboChocado.LadoY() + cuboMovimiento.LadoY()) / 2) - Math.Abs(dif_centros_y);
            float pen_z = ((cuboChocado.LadoZ() + cuboMovimiento.LadoZ()) / 2) - Math.Abs(dif_centros_z);

            float penetracion = Math.Min(Math.Min(pen_x, pen_y), pen_z);
            Vector3 normal = Vector3.Zero;

            if (penetracion == pen_x)
                normal = Vector3.UnitX * Math.Sign(dif_centros_x);
            else if (penetracion == pen_y)
                normal = Vector3.UnitZ * Math.Sign(dif_centros_y);
            else if (penetracion == pen_z)
                normal = Vector3.UnitZ * Math.Sign(dif_centros_z);

            Vector3 puntoContacto = (cuboMovimiento.Centro() + cuboChocado.Centro()) / 2;

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        // ----------------------------- Coliciones entre CuboAABB - Esfera / Esfera - CuboAABB --------------------------------------------- //
        public static bool DetectarColisiones(BVCuboAABB cuboMovimento, BVEsfera esferaChocada)
        {

            float distancia2 = 0.0f;

            if (esferaChocada._centro.X < cuboMovimento._minimo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._minimo.X);
            else if (esferaChocada._centro.X > cuboMovimento._maximo.X) distancia2 += sqr(esferaChocada._centro.X - cuboMovimento._maximo.X);

            if (esferaChocada._centro.Y < cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);
            else if (esferaChocada._centro.Y > cuboMovimento._minimo.Y) distancia2 += sqr(esferaChocada._centro.Y - cuboMovimento._minimo.Y);

            if (esferaChocada._centro.Z < cuboMovimento._minimo.Z) distancia2 += sqr(esferaChocada._centro.Z - cuboMovimento._minimo.Z);
            else if (esferaChocada._centro.Z > cuboMovimento._maximo.Z) distancia2 += sqr(esferaChocada._centro.Z - cuboMovimento._maximo.Z);

            return distancia2 <= esferaChocada._radio * esferaChocada._radio;
        }

        public static bool DetectarColisiones(BVEsfera esferaMovimento, BVCuboAABB cuboChocado)
        {
            return CalculadorasChoque.DetectarColisiones(cuboChocado, esferaMovimento);
        }



        public static DataChoque ParametrosChoque(BVCuboAABB cuboMovimento, BVEsfera esferaChocada)
        {
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
            return new DataChoque(puntoContacto, penetracion, normal);
        }

        public static DataChoque ParametrosChoque(BVEsfera esferaMovimento, BVCuboAABB cuboChocado)
        {
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
            return new DataChoque(puntoContacto, penetracion, normal);
        }




        //---------------cuboobb----------------------------//
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

        /*
                public static DataChoque ParametrosChoque(BVCuboOBB cubo1, BVCuboOBB cubo2)
                {
                    // Primero verificamos si hay colisión
                    if (!DetectarColisiones(cubo1, cubo2))
                        return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

                    // Implementación del algoritmo GJK/EPA para OBB-OBB
                    // Para simplificar, usaremos un enfoque basado en ejes de separación (SAT)

                    Vector3 centroDiff = cubo2.Centro - cubo1.Centro;

                    // Ejes de prueba: los 3 de cada OBB (6) + los 9 productos cruzados (15 en total)
                    Vector3[] ejes = {
                        cubo1.Orientacion.Right, cubo1.Orientacion.Up, cubo1.Orientacion.Backward,
                        cubo2.Orientacion.Right, cubo2.Orientacion.Up, cubo2.Orientacion.Backward,
                        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Right),
                        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Up),
                        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Backward),
                        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Right),
                        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Up),
                        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Backward),
                        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Right),
                        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Up),
                        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Backward)
                    };

                    float minPenetracion = float.MaxValue;
                    Vector3 ejeColision = Vector3.Zero;

                    foreach (Vector3 eje in ejes)
                    {
                        if (eje.LengthSquared() < 0.001f) continue;

                        Vector3 ejeNormalizado = Vector3.Normalize(eje);

                        // Proyectamos ambos cubos en el eje
                        float proy1 = ProyectarOBB(cubo1, ejeNormalizado);
                        float proy2 = ProyectarOBB(cubo2, ejeNormalizado);

                        // Proyectamos la distancia entre centros
                        float distancia = Math.Abs(Vector3.Dot(centroDiff, ejeNormalizado));

                        // Calculamos la penetración en este eje
                        float penetracion = (proy1 + proy2) - distancia;

                        if (penetracion <= 0)
                            return new DataChoque(Vector3.Zero, 0, Vector3.Zero); // No debería ocurrir si hay colisión

                        if (penetracion < minPenetracion)
                        {
                            minPenetracion = penetracion;
                            ejeColision = ejeNormalizado * Math.Sign(Vector3.Dot(centroDiff, ejeNormalizado));
                        }
                    }

                    // Punto de contacto aproximado (podría mejorarse con Closest Points)
                    Vector3 puntoContacto = (cubo1.Centro + cubo2.Centro) * 0.5f;

                    // Mejorar el punto de contacto encontrando la cara de colisión
                    // Esto es una aproximación simple, una implementación más precisa requeriría GJK/EPA
                    Vector3 direccionCentros = cubo2.Centro - cubo1.Centro;
                    if (Vector3.Dot(direccionCentros, ejeColision) < 0)
                    {
                        ejeColision = -ejeColision;
                    }

                    return new DataChoque(puntoContacto, minPenetracion, ejeColision);
                }
        */
public static DataChoque ParametrosChoque(BVCuboOBB cubo1, BVCuboOBB cubo2)
{
    Vector3 centroDiff = cubo2.Centro - cubo1.Centro;
    // Ejes de prueba (15)
        Vector3[] ejes = {
        cubo1.Orientacion.Right, cubo1.Orientacion.Up, cubo1.Orientacion.Backward,
        cubo2.Orientacion.Right, cubo2.Orientacion.Up, cubo2.Orientacion.Backward,
        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Right),
        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Up),
        Vector3.Cross(cubo1.Orientacion.Right, cubo2.Orientacion.Backward),
        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Right),
        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Up),
        Vector3.Cross(cubo1.Orientacion.Up, cubo2.Orientacion.Backward),
        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Right),
        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Up),
        Vector3.Cross(cubo1.Orientacion.Backward, cubo2.Orientacion.Backward)
        };

    float minPenetracion = float.MaxValue;
    Vector3 ejeColision = Vector3.Zero;

    foreach (Vector3 eje in ejes)
    {
        if (eje.LengthSquared() < 0.001f)
            continue;

        Vector3 ejeNormalizado = Vector3.Normalize(eje);

        float proy1 = ProyectarOBB(cubo1, ejeNormalizado);
        float proy2 = ProyectarOBB(cubo2, ejeNormalizado);
        float distancia = Math.Abs(Vector3.Dot(centroDiff, ejeNormalizado));

        float penetracion = (proy1 + proy2) - distancia;

        // Si hay un eje de separación, no hay colisión real
        if (penetracion <= 0)
            return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

        if (penetracion < minPenetracion)
        {
            minPenetracion = penetracion;
            // Aseguramos que el eje apunte de cubo1 hacia cubo2
            ejeColision = ejeNormalizado * Math.Sign(Vector3.Dot(centroDiff, ejeNormalizado));
        }
    }

    // Si la penetración mínima es negativa o cero, no hay colisión
    if (minPenetracion <= 0)
        return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

    // Punto de contacto estimado: proyección del centro de cubo1 hacia cubo2 en la dirección del eje de colisión
    float distanciaProyectada = Vector3.Dot(centroDiff, ejeColision);
    Vector3 puntoContacto = cubo1.Centro + ejeColision * (distanciaProyectada * 0.5f);

    return new DataChoque(puntoContacto, minPenetracion, ejeColision);
}



        // ----------------------------- Colisiones Esfera - OBB --------------------------------------------- //
        public static bool DetectarColisiones(BVEsfera esferaMovimento, BVCuboOBB cuboChocado)
        {
            // Transformamos el centro de la esfera al espacio local del OBB
            Vector3 centroLocal = Vector3.Transform(esferaMovimento._centro - cuboChocado.Centro, Matrix.Invert(cuboChocado.Orientacion));

            // Encontramos el punto más cercano en el OBB al centro de la esfera
            Vector3 puntoMasCercano = new Vector3(
                MathHelper.Clamp(centroLocal.X, -cuboChocado.Tamaño.X, cuboChocado.Tamaño.X),
                MathHelper.Clamp(centroLocal.Y, -cuboChocado.Tamaño.Y, cuboChocado.Tamaño.Y),
                MathHelper.Clamp(centroLocal.Z, -cuboChocado.Tamaño.Z, cuboChocado.Tamaño.Z)
            );

            // Calculamos la distancia entre el centro y el punto más cercano
            float distancia = (centroLocal - puntoMasCercano).Length();

            return distancia <= esferaMovimento._radio;
        }

        public static DataChoque ParametrosChoque(BVEsfera esferaMovimento, BVCuboOBB cuboChocado)
        {
            // Transformamos el centro de la esfera al espacio local del OBB
            Vector3 centroLocal = Vector3.Transform(esferaMovimento._centro - cuboChocado.Centro, Matrix.Invert(cuboChocado.Orientacion));

            // Encontramos el punto más cercano en el OBB al centro de la esfera
            Vector3 puntoMasCercanoLocal = new Vector3(
                MathHelper.Clamp(centroLocal.X, -cuboChocado.Tamaño.X, cuboChocado.Tamaño.X),
                MathHelper.Clamp(centroLocal.Y, -cuboChocado.Tamaño.Y, cuboChocado.Tamaño.Y),
                MathHelper.Clamp(centroLocal.Z, -cuboChocado.Tamaño.Z, cuboChocado.Tamaño.Z)
            );

            // Calculamos la distancia y la normal en espacio local
            Vector3 direccionLocal = centroLocal - puntoMasCercanoLocal;
            float distancia = direccionLocal.Length();
            Vector3 normalLocal = distancia > 0 ? direccionLocal / distancia : Vector3.UnitX;

            // Convertimos la normal y el punto de contacto al espacio mundial
            Vector3 normal = Vector3.TransformNormal(normalLocal, cuboChocado.Orientacion);
            normal.Normalize();

            Vector3 puntoContactoLocal = puntoMasCercanoLocal;
            Vector3 puntoContacto = Vector3.Transform(puntoContactoLocal, cuboChocado.Orientacion) + cuboChocado.Centro;

            float penetracion = esferaMovimento._radio - distancia;

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        public static bool DetectarColisiones(BVCuboOBB cuboMovimiento, BVEsfera esferaChocada)
        {
            // Simplemente llama a la función existente con los argumentos invertidos
            return DetectarColisiones(esferaChocada, cuboMovimiento);
        }

        public static DataChoque ParametrosChoque(BVCuboOBB cuboMovimiento, BVEsfera esferaChocada)
        {
            // Llama a la función existente
            DataChoque choqueOriginal = ParametrosChoque(esferaChocada, cuboMovimiento);

            // Invierte la normal para que sea desde el punto de vista del cubo
            // El punto de contacto y la penetración siguen siendo los mismos
            return new DataChoque(choqueOriginal._puntoContacto, choqueOriginal._penetracion, -choqueOriginal._normal);
        }

        // ----------------------------- Colisiones Esfera - Cilindro AABB --------------------------------------------- //
        public static bool DetectarColisiones(BVEsfera esferaMovimento, BVCilindroAABB cilindroChocado)
        {
            // Proyectamos la esfera en el eje Y del cilindro (AABB)
            float distanciaY = Math.Abs(esferaMovimento._centro.Y - cilindroChocado._centro.Y);
            float sumaAlturas = esferaMovimento._radio + cilindroChocado._alto / 2;

            if (distanciaY > sumaAlturas)
                return false;

            // Proyectamos en el plano XZ
            Vector2 centroEsferaXZ = new Vector2(esferaMovimento._centro.X, esferaMovimento._centro.Z);
            Vector2 centroCilindroXZ = new Vector2(cilindroChocado._centro.X, cilindroChocado._centro.Z);
            float distanciaXZ = Vector2.Distance(centroEsferaXZ, centroCilindroXZ);

            return distanciaXZ <= (esferaMovimento._radio + cilindroChocado._radio);
        }

        public static DataChoque ParametrosChoque(BVEsfera esferaMovimento, BVCilindroAABB cilindroChocado)
        {
            // Primero chequeamos colisión en Y (altura)
            float distanciaY = esferaMovimento._centro.Y - cilindroChocado._centro.Y;
            float penetracionY = (cilindroChocado._alto / 2 + esferaMovimento._radio) - Math.Abs(distanciaY);

            // Chequeamos colisión en XZ (radio)
            Vector2 centroEsferaXZ = new Vector2(esferaMovimento._centro.X, esferaMovimento._centro.Z);
            Vector2 centroCilindroXZ = new Vector2(cilindroChocado._centro.X, cilindroChocado._centro.Z);
            Vector2 direccionXZ = centroEsferaXZ - centroCilindroXZ;
            float distanciaXZ = direccionXZ.Length();
            float penetracionXZ = (cilindroChocado._radio + esferaMovimento._radio) - distanciaXZ;

            // Determinamos el eje principal de colisión
            Vector3 normal;
            float penetracion;
            Vector3 puntoContacto;

            if (penetracionXZ < penetracionY)
            {
                // Colisión lateral (XZ)
                if (distanciaXZ > 0)
                    direccionXZ /= distanciaXZ;
                else
                    direccionXZ = Vector2.UnitX;

                normal = new Vector3(direccionXZ.X, 0, direccionXZ.Y);
                penetracion = penetracionXZ;
                puntoContacto = new Vector3(
                    cilindroChocado._centro.X + direccionXZ.X * cilindroChocado._radio,
                    esferaMovimento._centro.Y,
                    cilindroChocado._centro.Z + direccionXZ.Y * cilindroChocado._radio);
            }
            else
            {
                // Colisión en Y (arriba/abajo)
                normal = new Vector3(0, Math.Sign(distanciaY), 0);
                penetracion = penetracionY;
                puntoContacto = new Vector3(
                    esferaMovimento._centro.X,
                    cilindroChocado._centro.Y + (cilindroChocado._alto / 2) * Math.Sign(distanciaY),
                    esferaMovimento._centro.Z);
            }

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        // ----------------------------- Colisiones Esfera - Cilindro OBB --------------------------------------------- //
        public static bool DetectarColisiones(BVEsfera esferaMovimento, BVCilindroOBB cilindroChocado)
        {
            // Vector del centro del cilindro al centro de la esfera
            Vector3 centroACentro = esferaMovimento._centro - cilindroChocado._centro;

            // Proyectamos en el eje del cilindro
            float proyeccionEje = Vector3.Dot(centroACentro, cilindroChocado._direccion);
            float distanciaEje = Math.Abs(proyeccionEje);
            float sumaAlturas = esferaMovimento._radio + cilindroChocado._alto / 2;

            if (distanciaEje > sumaAlturas)
                return false;

            // Proyectamos en el plano perpendicular al eje del cilindro
            Vector3 proyeccionEjeVector = cilindroChocado._direccion * proyeccionEje;
            Vector3 centroEsferaPlano = centroACentro - proyeccionEjeVector;
            float distanciaPlano = centroEsferaPlano.Length();

            return distanciaPlano <= (esferaMovimento._radio + cilindroChocado._radio);
        }

        public static DataChoque ParametrosChoque(BVEsfera esferaMovimento, BVCilindroOBB cilindroChocado)
        {
            Vector3 centroACentro = esferaMovimento._centro - cilindroChocado._centro;

            // Proyección en el eje del cilindro
            float proyeccionEje = Vector3.Dot(centroACentro, cilindroChocado._direccion);
            float distanciaEje = Math.Abs(proyeccionEje);
            float penetracionEje = (cilindroChocado._alto / 2 + esferaMovimento._radio) - distanciaEje;

            // Proyección en el plano perpendicular
            Vector3 proyeccionEjeVector = cilindroChocado._direccion * proyeccionEje;
            Vector3 centroEsferaPlano = centroACentro - proyeccionEjeVector;
            float distanciaPlano = centroEsferaPlano.Length();
            float penetracionPlano = (cilindroChocado._radio + esferaMovimento._radio) - distanciaPlano;

            Vector3 normal;
            float penetracion;
            Vector3 puntoContacto;

            if (penetracionPlano < penetracionEje)
            {
                // Colisión lateral
                if (distanciaPlano > 0)
                    centroEsferaPlano /= distanciaPlano;
                else
                    centroEsferaPlano = Vector3.Cross(cilindroChocado._direccion, Vector3.UnitX);

                normal = centroEsferaPlano;
                penetracion = penetracionPlano;
                puntoContacto = cilindroChocado._centro + proyeccionEjeVector + centroEsferaPlano * cilindroChocado._radio;
            }
            else
            {
                // Colisión en los extremos
                normal = cilindroChocado._direccion * Math.Sign(proyeccionEje);
                penetracion = penetracionEje;
                puntoContacto = cilindroChocado._centro + cilindroChocado._direccion * (cilindroChocado._alto / 2 * Math.Sign(proyeccionEje));
            }

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        // ----------------------------- Colisiones CuboAABB - OBB --------------------------------------------- //
        public static bool DetectarColisiones(BVCuboAABB cuboMovimiento, BVCuboOBB cuboChocado)
        {
            // Convertimos el AABB a un OBB con orientación identidad
            Vector3 centroAABB = (cuboMovimiento._minimo + cuboMovimiento._maximo) * 0.5f;
            Vector3 tamañoAABB = (cuboMovimiento._maximo - cuboMovimiento._minimo) * 0.5f;
            BVCuboOBB obbAABB = new BVCuboOBB(centroAABB, tamañoAABB, Matrix.Identity);

            return DetectarColisiones(obbAABB, cuboChocado);
        }

        public static DataChoque ParametrosChoque(BVCuboAABB cuboMovimiento, BVCuboOBB cuboChocado)
        {
            // Implementación similar a OBB-OBB pero considerando que uno no está rotado
            // Esta es una versión simplificada

            Vector3 centroAABB = (cuboMovimiento._minimo + cuboMovimiento._maximo) * 0.5f;
            Vector3 tamañoAABB = (cuboMovimiento._maximo - cuboMovimiento._minimo) * 0.5f;

            Vector3 centroDiff = cuboChocado.Centro - centroAABB;

            // Ejes de prueba: los 3 del AABB y los 3 del OBB
            Vector3[] ejes = {
                Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
                cuboChocado.Orientacion.Right, cuboChocado.Orientacion.Up, cuboChocado.Orientacion.Backward
            };

            float minPenetracion = float.MaxValue;
            Vector3 ejeColision = Vector3.Zero;

            foreach (Vector3 eje in ejes)
            {
                if (eje.LengthSquared() < 0.001f) continue;

                Vector3 ejeNormalizado = Vector3.Normalize(eje);

                // Proyectamos ambos cubos en el eje
                float proyAABB = ProyectarAABB(cuboMovimiento, ejeNormalizado);
                float proyOBB = ProyectarOBB(cuboChocado, ejeNormalizado);

                // Proyectamos la distancia entre centros
                float distancia = Math.Abs(Vector3.Dot(centroDiff, ejeNormalizado));

                // Calculamos la penetración en este eje
                float penetracion = (proyAABB + proyOBB) - distancia;

                if (penetracion <= 0) return new DataChoque(Vector3.Zero, 0, Vector3.Zero); // No hay colisión

                if (penetracion < minPenetracion)
                {
                    minPenetracion = penetracion;
                    ejeColision = ejeNormalizado * Math.Sign(Vector3.Dot(centroDiff, ejeNormalizado));
                }
            }

            // Punto de contacto aproximado (podría mejorarse)
            Vector3 puntoContacto = (centroAABB + cuboChocado.Centro) * 0.5f;

            return new DataChoque(puntoContacto, minPenetracion, ejeColision);
        }

        private static float ProyectarAABB(BVCuboAABB aabb, Vector3 eje)
        {
            Vector3 tamaño = (aabb._maximo - aabb._minimo) * 0.5f;
            return Math.Abs(eje.X) * tamaño.X + Math.Abs(eje.Y) * tamaño.Y + Math.Abs(eje.Z) * tamaño.Z;
        }

        // ----------------------------- Colisiones CuboAABB - Cilindro AABB --------------------------------------------- //
        public static bool DetectarColisiones(BVCuboAABB cuboMovimiento, BVCilindroAABB cilindroChocado)
        {
            // Convertimos el cilindro a un AABB temporal para una primera comprobación rápida
            BVCuboAABB aabbCilindro = new BVCuboAABB(
                new Vector3(cilindroChocado._centro.X, cilindroChocado._centro.Y, cilindroChocado._centro.Z),
                new Vector3(
                    cilindroChocado._centro.X + cilindroChocado._radio,
                    cilindroChocado._centro.Y + cilindroChocado._alto / 2,
                    cilindroChocado._centro.Z + cilindroChocado._radio
                )
            );

            if (!DetectarColisiones(cuboMovimiento, aabbCilindro))
                return false;

            // Comprobación más precisa
            Vector3 centroCubo = (cuboMovimiento._minimo + cuboMovimiento._maximo) * 0.5f;
            Vector3 puntoMasCercano = new Vector3(
                MathHelper.Clamp(centroCubo.X, cilindroChocado._centro.X - cilindroChocado._radio, cilindroChocado._centro.X + cilindroChocado._radio),
                MathHelper.Clamp(centroCubo.Y, cilindroChocado._centro.Y - cilindroChocado._alto / 2, cilindroChocado._centro.Y + cilindroChocado._alto / 2),
                MathHelper.Clamp(centroCubo.Z, cilindroChocado._centro.Z - cilindroChocado._radio, cilindroChocado._centro.Z + cilindroChocado._radio)
            );

            return Vector3.DistanceSquared(centroCubo, puntoMasCercano) <= 0;
        }

        public static DataChoque ParametrosChoque(BVCuboAABB cuboMovimiento, BVCilindroAABB cilindroChocado)
        {
            // Implementación similar a la de esfera-cilindro pero adaptada para AABB
            Vector3 centroCubo = (cuboMovimiento._minimo + cuboMovimiento._maximo) * 0.5f;
            Vector3 tamañoCubo = (cuboMovimiento._maximo - cuboMovimiento._minimo) * 0.5f;

            // Comprobación en Y (altura)
            float distanciaY = centroCubo.Y - cilindroChocado._centro.Y;
            float penetracionY = (cilindroChocado._alto / 2 + tamañoCubo.Y) - Math.Abs(distanciaY);

            // Comprobación en XZ (radio)
            Vector2 centroCuboXZ = new Vector2(centroCubo.X, centroCubo.Z);
            Vector2 centroCilindroXZ = new Vector2(cilindroChocado._centro.X, cilindroChocado._centro.Z);
            Vector2 direccionXZ = centroCuboXZ - centroCilindroXZ;
            float distanciaXZ = direccionXZ.Length();
            float penetracionXZ = (cilindroChocado._radio + Math.Max(tamañoCubo.X, tamañoCubo.Z)) - distanciaXZ;

            Vector3 normal;
            float penetracion;
            Vector3 puntoContacto;

            if (penetracionXZ < penetracionY)
            {
                // Colisión lateral
                if (distanciaXZ > 0)
                    direccionXZ /= distanciaXZ;
                else
                    direccionXZ = Vector2.UnitX;

                normal = new Vector3(direccionXZ.X, 0, direccionXZ.Y);
                penetracion = penetracionXZ;
                puntoContacto = new Vector3(
                    cilindroChocado._centro.X + direccionXZ.X * cilindroChocado._radio,
                    centroCubo.Y,
                    cilindroChocado._centro.Z + direccionXZ.Y * cilindroChocado._radio);
            }
            else
            {
                // Colisión en Y
                normal = new Vector3(0, Math.Sign(distanciaY), 0);
                penetracion = penetracionY;
                puntoContacto = new Vector3(
                    centroCubo.X,
                    cilindroChocado._centro.Y + (cilindroChocado._alto / 2) * Math.Sign(distanciaY),
                    centroCubo.Z);
            }

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        // ----------------------------- Colisiones Rayo - CuboAABB --------------------------------------------- //
        public static bool DetectarColisiones(BVCuboAABB cuboChocado, BVRayo rayoMovimiento) { return DetectarColisiones(rayoMovimiento, cuboChocado); }
        public static bool DetectarColisiones(BVRayo rayoMovimiento, BVCuboAABB cuboChocado)
        {
            Vector3 dirFrac = new Vector3(
                1.0f / rayoMovimiento._Direccion.X,
                1.0f / rayoMovimiento._Direccion.Y,
                1.0f / rayoMovimiento._Direccion.Z
            );

            float t1 = (cuboChocado._minimo.X - rayoMovimiento._PuntoPartda.X) * dirFrac.X;
            float t2 = (cuboChocado._maximo.X - rayoMovimiento._PuntoPartda.X) * dirFrac.X;
            float t3 = (cuboChocado._minimo.Y - rayoMovimiento._PuntoPartda.Y) * dirFrac.Y;
            float t4 = (cuboChocado._maximo.Y - rayoMovimiento._PuntoPartda.Y) * dirFrac.Y;
            float t5 = (cuboChocado._minimo.Z - rayoMovimiento._PuntoPartda.Z) * dirFrac.Z;
            float t6 = (cuboChocado._maximo.Z - rayoMovimiento._PuntoPartda.Z) * dirFrac.Z;

            float tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            float tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            // Si tmax < 0, el rayo está intersectando pero en la dirección opuesta
            if (tmax < 0) return false;

            // Si tmin > tmax, no hay intersección
            if (tmin > tmax) return false;

            return true;
        }

        public static DataChoque ParametrosChoque(BVCuboAABB cuboChocado, BVRayo rayoMovimiento) { return ParametrosChoque(rayoMovimiento, cuboChocado); }
        public static DataChoque ParametrosChoque(BVRayo rayoMovimiento, BVCuboAABB cuboChocado)
        {
            Vector3 dirFrac = new Vector3(
                1.0f / rayoMovimiento._Direccion.X,
                1.0f / rayoMovimiento._Direccion.Y,
                1.0f / rayoMovimiento._Direccion.Z
            );

            float t1 = (cuboChocado._minimo.X - rayoMovimiento._PuntoPartda.X) * dirFrac.X;
            float t2 = (cuboChocado._maximo.X - rayoMovimiento._PuntoPartda.X) * dirFrac.X;
            float t3 = (cuboChocado._minimo.Y - rayoMovimiento._PuntoPartda.Y) * dirFrac.Y;
            float t4 = (cuboChocado._maximo.Y - rayoMovimiento._PuntoPartda.Y) * dirFrac.Y;
            float t5 = (cuboChocado._minimo.Z - rayoMovimiento._PuntoPartda.Z) * dirFrac.Z;
            float t6 = (cuboChocado._maximo.Z - rayoMovimiento._PuntoPartda.Z) * dirFrac.Z;

            float tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            float tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            if (tmax < 0 || tmin > tmax)
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

            // Calculamos el punto de contacto y la normal
            Vector3 puntoContacto = rayoMovimiento._PuntoPartda + rayoMovimiento._Direccion * tmin;
            Vector3 normal = Vector3.Zero;

            // Determinamos la normal según la cara con la que chocó
            if (tmin == t1) normal = -Vector3.UnitX;
            else if (tmin == t2) normal = Vector3.UnitX;
            else if (tmin == t3) normal = -Vector3.UnitY;
            else if (tmin == t4) normal = Vector3.UnitY;
            else if (tmin == t5) normal = -Vector3.UnitZ;
            else if (tmin == t6) normal = Vector3.UnitZ;

            return new DataChoque(puntoContacto, 0, normal);
        }

        // ----------------------------- Colisiones Rayo - CuboOBB --------------------------------------------- //
        public static bool DetectarColisiones(BVCuboOBB cuboChocado, BVRayo rayoMovimiento) { return DetectarColisiones(rayoMovimiento, cuboChocado); }
        public static bool DetectarColisiones(BVRayo rayoMovimiento, BVCuboOBB cuboChocado)
        {
            // Transformamos el rayo al espacio local del OBB
            Matrix invOrientacion = Matrix.Invert(cuboChocado.Orientacion);
            Vector3 origenLocal = Vector3.Transform(rayoMovimiento._PuntoPartda - cuboChocado.Centro, invOrientacion);
            Vector3 direccionLocal = Vector3.TransformNormal(rayoMovimiento._Direccion, invOrientacion);

            // Ahora tratamos como un AABB
            BVCuboAABB aabbLocal = new BVCuboAABB(
                -cuboChocado.Tamaño,
                cuboChocado.Tamaño
            );

            BVRayo rayoLocal = new BVRayo(direccionLocal, origenLocal);
            return DetectarColisiones(rayoLocal, aabbLocal);
        }

        public static DataChoque ParametrosChoque(BVCuboOBB cuboChocado, BVRayo rayoMovimiento) { return ParametrosChoque(rayoMovimiento, cuboChocado); }
        public static DataChoque ParametrosChoque(BVRayo rayoMovimiento, BVCuboOBB cuboChocado)
        {
            Matrix invOrientacion = Matrix.Invert(cuboChocado.Orientacion);
            Vector3 origenLocal = Vector3.Transform(rayoMovimiento._PuntoPartda - cuboChocado.Centro, invOrientacion);
            Vector3 direccionLocal = Vector3.TransformNormal(rayoMovimiento._Direccion, invOrientacion);

            BVCuboAABB aabbLocal = new BVCuboAABB(
                -cuboChocado.Tamaño,
                cuboChocado.Tamaño
            );

            BVRayo rayoLocal = new BVRayo(direccionLocal, origenLocal);
            DataChoque choqueLocal = ParametrosChoque(rayoLocal, aabbLocal);

            if (choqueLocal._penetracion <= 0)
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

            // Convertimos los datos de vuelta al espacio mundial
            Vector3 puntoContacto = Vector3.Transform(choqueLocal._puntoContacto, cuboChocado.Orientacion) + cuboChocado.Centro;
            Vector3 normal = Vector3.TransformNormal(choqueLocal._normal, cuboChocado.Orientacion);
            normal.Normalize();

            return new DataChoque(puntoContacto, 0, normal);
        }

        // ----------------------------- Colisiones CilindroAABB - Esfera --------------------------------------------- //
        public static bool DetectarColisiones(BVCilindroAABB cilindroMovimiento, BVEsfera esferaChocado)
        {
            return DetectarColisiones(esferaChocado, cilindroMovimiento);
        }

        public static DataChoque ParametrosChoque(BVCilindroAABB cilindroMovimiento, BVEsfera esferaChocado)
        {
            DataChoque choque = ParametrosChoque(esferaChocado, cilindroMovimiento);
            return new DataChoque(choque._puntoContacto, choque._penetracion, -choque._normal);
        }

        // ----------------------------- Colisiones CilindroAABB - CuboAABB --------------------------------------------- //
        public static bool DetectarColisiones(BVCilindroAABB cilindroMovimiento, BVCuboAABB cuboChocado)
        {
            return DetectarColisiones(cuboChocado, cilindroMovimiento);
        }

        public static DataChoque ParametrosChoque(BVCilindroAABB cilindroMovimiento, BVCuboAABB cuboChocado)
        {
            DataChoque choque = ParametrosChoque(cuboChocado, cilindroMovimiento);
            return new DataChoque(choque._puntoContacto, choque._penetracion, -choque._normal);
        }

        // ----------------------------- Colisiones CilindroAABB - CilindroAABB --------------------------------------------- //
        public static bool DetectarColisiones(BVCilindroAABB cilindro1, BVCilindroAABB cilindro2)
        {
            // Comprobación en Y (altura)
            float distanciaY = Math.Abs(cilindro1._centro.Y - cilindro2._centro.Y);
            float sumaAlturas = cilindro1._alto / 2 + cilindro2._alto / 2;
            if (distanciaY > sumaAlturas)
                return false;

            // Comprobación en XZ (radio)
            Vector2 centro1XZ = new Vector2(cilindro1._centro.X, cilindro1._centro.Z);
            Vector2 centro2XZ = new Vector2(cilindro2._centro.X, cilindro2._centro.Z);
            float distanciaXZ = Vector2.Distance(centro1XZ, centro2XZ);

            return distanciaXZ <= (cilindro1._radio + cilindro2._radio);
        }

        public static DataChoque ParametrosChoque(BVCilindroAABB cilindro1, BVCilindroAABB cilindro2)
        {
            // Comprobación en Y
            float distanciaY = cilindro1._centro.Y - cilindro2._centro.Y;
            float penetracionY = (cilindro1._alto / 2 + cilindro2._alto / 2) - Math.Abs(distanciaY);

            // Comprobación en XZ
            Vector2 centro1XZ = new Vector2(cilindro1._centro.X, cilindro1._centro.Z);
            Vector2 centro2XZ = new Vector2(cilindro2._centro.X, cilindro2._centro.Z);
            Vector2 direccionXZ = centro1XZ - centro2XZ;
            float distanciaXZ = direccionXZ.Length();
            float penetracionXZ = (cilindro1._radio + cilindro2._radio) - distanciaXZ;

            Vector3 normal;
            float penetracion;
            Vector3 puntoContacto;

            if (penetracionXZ < penetracionY)
            {
                // Colisión lateral
                if (distanciaXZ > 0)
                    direccionXZ /= distanciaXZ;
                else
                    direccionXZ = Vector2.UnitX;

                normal = new Vector3(direccionXZ.X, 0, direccionXZ.Y);
                penetracion = penetracionXZ;
                puntoContacto = new Vector3(
                    cilindro2._centro.X + direccionXZ.X * cilindro2._radio,
                    (cilindro1._centro.Y + cilindro2._centro.Y) * 0.5f,
                    cilindro2._centro.Z + direccionXZ.Y * cilindro2._radio);
            }
            else
            {
                // Colisión en Y
                normal = new Vector3(0, Math.Sign(distanciaY), 0);
                penetracion = penetracionY;
                puntoContacto = new Vector3(
                    (cilindro1._centro.X + cilindro2._centro.X) * 0.5f,
                    cilindro2._centro.Y + (cilindro2._alto / 2) * Math.Sign(distanciaY),
                    (cilindro1._centro.Z + cilindro2._centro.Z) * 0.5f);
            }

            return new DataChoque(puntoContacto, penetracion, normal);
        }
        

        // ----------------------------- Colisiones Rayo - Rayo --------------------------------------------- //
        public static bool DetectarColisiones(BVRayo rayo1, BVRayo rayo2)
        {
            // Vector entre los puntos de partida de los rayos
            Vector3 w0 = rayo1._PuntoPartda - rayo2._PuntoPartda;
            
            // Productos escalares para la fórmula
            float a = Vector3.Dot(rayo1._Direccion, rayo1._Direccion);
            float b = Vector3.Dot(rayo1._Direccion, rayo2._Direccion);
            float c = Vector3.Dot(rayo2._Direccion, rayo2._Direccion);
            float d = Vector3.Dot(rayo1._Direccion, w0);
            float e = Vector3.Dot(rayo2._Direccion, w0);
            
            // Determinante - si es 0, los rayos son paralelos
            float determinante = a * c - b * b;
            
            // Si los rayos son casi paralelos
            if (Math.Abs(determinante) < 1e-6f)
            {
                // Los rayos son paralelos - verificamos si son colineales
                // Calculamos la distancia entre las líneas
                float distancia = Vector3.Cross(rayo1._Direccion, w0).Length() / rayo1._Direccion.Length();
                return distancia < 1e-6f; // Consideramos colisión si son esencialmente la misma línea
            }
            
            // Calculamos parámetros de intersección
            float s = (b * e - c * d) / determinante;
            float t = (a * e - b * d) / determinante;
            
            // Puntos de intersección en cada rayo
            Vector3 puntoEnRayo1 = rayo1._PuntoPartda + rayo1._Direccion * s;
            Vector3 puntoEnRayo2 = rayo2._PuntoPartda + rayo2._Direccion * t;
            
            // Verificamos si los puntos de intersección coinciden (dentro de un margen de error)
            return Vector3.DistanceSquared(puntoEnRayo1, puntoEnRayo2) < 1e-6f;
        }

        public static DataChoque ParametrosChoque(BVRayo rayo1, BVRayo rayo2)
        {
            Vector3 w0 = rayo1._PuntoPartda - rayo2._PuntoPartda;
            
            float a = Vector3.Dot(rayo1._Direccion, rayo1._Direccion);
            float b = Vector3.Dot(rayo1._Direccion, rayo2._Direccion);
            float c = Vector3.Dot(rayo2._Direccion, rayo2._Direccion);
            float d = Vector3.Dot(rayo1._Direccion, w0);
            float e = Vector3.Dot(rayo2._Direccion, w0);
            
            float determinante = a * c - b * b;
            
            if (Math.Abs(determinante) < 1e-6f)
            {
                // Rayos paralelos o colineales
                if (Vector3.Cross(rayo1._Direccion, w0).LengthSquared() < 1e-6f)
                {
                    // Rayos colineales - devolvemos un punto arbitrario (el punto de partida del primer rayo)
                    // La normal es perpendicular a la dirección del rayo
                    Vector3 normalN = Vector3.Normalize(Vector3.Cross(rayo1._Direccion, 
                        Math.Abs(rayo1._Direccion.X) > 0.1f ? Vector3.UnitY : Vector3.UnitX));
                    
                    return new DataChoque(rayo1._PuntoPartda, 0, normalN);
                }
                
                // Rayos paralelos pero no colineales - no hay colisión
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);
            }
            
            float s = (b * e - c * d) / determinante;
            float t = (a * e - b * d) / determinante;
            
            Vector3 puntoEnRayo1 = rayo1._PuntoPartda + rayo1._Direccion * s;
            Vector3 puntoEnRayo2 = rayo2._PuntoPartda + rayo2._Direccion * t;
            
            if (Vector3.DistanceSquared(puntoEnRayo1, puntoEnRayo2) > 1e-6f)
            {
                // Los rayos no se intersectan realmente
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);
            }
            
            // Calculamos la normal como el producto cruz de las direcciones de los rayos
            Vector3 normal = Vector3.Normalize(Vector3.Cross(rayo1._Direccion, rayo2._Direccion));
            
            // Si el producto cruz es cero (rayos paralelos pero ya manejado antes), usamos un vector perpendicular
            if (normal.LengthSquared() < 1e-6f)
            {
                normal = Vector3.Normalize(Vector3.Cross(rayo1._Direccion, 
                    Math.Abs(rayo1._Direccion.X) > 0.1f ? Vector3.UnitY : Vector3.UnitX));
            }
            
            // El punto de contacto es el punto medio entre los dos puntos calculados
            Vector3 puntoContacto = (puntoEnRayo1 + puntoEnRayo2) * 0.5f;
            
            return new DataChoque(puntoContacto, 0, normal);
        }

        // Método para detectar colisión entre rayo y cilindro AABB
        public static bool DetectarColisiones(BVCilindroAABB cilindro, BVRayo rayo){return DetectarColisiones( rayo,  cilindro);}
        public static bool DetectarColisiones(BVRayo rayo, BVCilindroAABB cilindro)
        {
            // Convertimos el cilindro a un sistema de coordenadas local donde el centro está en (0,0,0)
            Vector3 origenLocal = rayo._PuntoPartda - cilindro._centro;
            Vector3 direccionLocal = rayo._Direccion;

            // Proyectamos en el plano XZ (base del cilindro)
            Vector2 origenXZ = new Vector2(origenLocal.X, origenLocal.Z);
            Vector2 direccionXZ = new Vector2(direccionLocal.X, direccionLocal.Z);

            // Ecuación cuadrática para intersección con el cilindro infinito (en XZ)
            float a = direccionXZ.LengthSquared();
            float b = 2 * Vector2.Dot(origenXZ, direccionXZ);
            float c = origenXZ.LengthSquared() - (cilindro._radio * cilindro._radio);

            // Discriminante
            float discriminante = b * b - 4 * a * c;

            if (discriminante < 0)
                return false; // No hay intersección con el cilindro infinito

            // Calculamos los posibles puntos de intersección
            float sqrtDisc = (float)Math.Sqrt(discriminante);
            float t1 = (-b - sqrtDisc) / (2 * a);
            float t2 = (-b + sqrtDisc) / (2 * a);

            // Verificamos intersección con las tapas del cilindro
            float mitadAltura = cilindro._alto / 2;
            float tMin = Math.Min(t1, t2);
            float tMax = Math.Max(t1, t2);

            // Verificamos intersección con las tapas (planos y=±mitadAltura)
            if (Math.Abs(direccionLocal.Y) > 1e-6f)
            {
                float tTop = (mitadAltura - origenLocal.Y) / direccionLocal.Y;
                float tBottom = (-mitadAltura - origenLocal.Y) / direccionLocal.Y;

                // Verificamos si la intersección con las tapas está dentro del radio
                if (tTop > 0)
                {
                    Vector3 puntoTop = origenLocal + direccionLocal * tTop;
                    if (new Vector2(puntoTop.X, puntoTop.Z).LengthSquared() <= cilindro._radio * cilindro._radio)
                    {
                        return true;
                    }
                }

                if (tBottom > 0)
                {
                    Vector3 puntoBottom = origenLocal + direccionLocal * tBottom;
                    if (new Vector2(puntoBottom.X, puntoBottom.Z).LengthSquared() <= cilindro._radio * cilindro._radio)
                    {
                        return true;
                    }
                }
            }

            // Verificamos si la intersección con el cilindro está dentro de la altura
            if (tMax > 0)
            {
                Vector3 puntoMax = origenLocal + direccionLocal * tMax;
                if (Math.Abs(puntoMax.Y) <= mitadAltura)
                {
                    return true;
                }
            }

            if (tMin > 0)
            {
                Vector3 puntoMin = origenLocal + direccionLocal * tMin;
                if (Math.Abs(puntoMin.Y) <= mitadAltura)
                {
                    return true;
                }
            }

            return false;
        }

         // Método para obtener parámetros de choque entre rayo y cilindro AABB
        public static DataChoque ParametrosChoque(BVCilindroAABB cilindro, BVRayo rayo) {return ParametrosChoque(rayo,cilindro); }
        public static DataChoque ParametrosChoque(BVRayo rayo, BVCilindroAABB cilindro)
        {
            Vector3 origenLocal = rayo._PuntoPartda - cilindro._centro;
            Vector3 direccionLocal = rayo._Direccion;

            // Proyectamos en el plano XZ
            Vector2 origenXZ = new Vector2(origenLocal.X, origenLocal.Z);
            Vector2 direccionXZ = new Vector2(direccionLocal.X, direccionLocal.Z);

            // Ecuación cuadrática para el cilindro
            float a = direccionXZ.LengthSquared();
            float b = 2 * Vector2.Dot(origenXZ, direccionXZ);
            float c = origenXZ.LengthSquared() - (cilindro._radio * cilindro._radio);

            float discriminante = b * b - 4 * a * c;
            if (discriminante < 0)
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

            float sqrtDisc = (float)Math.Sqrt(discriminante);
            float t1 = (-b - sqrtDisc) / (2 * a);
            float t2 = (-b + sqrtDisc) / (2 * a);

            float mitadAltura = cilindro._alto / 2;
            float tMin = Math.Min(t1, t2);
            float tMax = Math.Max(t1, t2);

            // Variables para almacenar el mejor choque encontrado
            float mejorT = float.MaxValue;
            Vector3 normal = Vector3.Zero;
            bool esTapa = false;

            // Verificamos intersección con las tapas
            if (Math.Abs(direccionLocal.Y) > 1e-6f)
            {
                float tTop = (mitadAltura - origenLocal.Y) / direccionLocal.Y;
                if (tTop > 0 && tTop < mejorT)
                {
                    Vector3 puntoTop = origenLocal + direccionLocal * tTop;
                    if (new Vector2(puntoTop.X, puntoTop.Z).LengthSquared() <= cilindro._radio * cilindro._radio)
                    {
                        mejorT = tTop;
                        normal = Vector3.UnitY;
                        esTapa = true;
                    }
                }

                float tBottom = (-mitadAltura - origenLocal.Y) / direccionLocal.Y;
                if (tBottom > 0 && tBottom < mejorT)
                {
                    Vector3 puntoBottom = origenLocal + direccionLocal * tBottom;
                    if (new Vector2(puntoBottom.X, puntoBottom.Z).LengthSquared() <= cilindro._radio * cilindro._radio)
                    {
                        mejorT = tBottom;
                        normal = -Vector3.UnitY;
                        esTapa = true;
                    }
                }
            }

            // Verificamos intersección con el cuerpo del cilindro
            if (tMax > 0 && tMax < mejorT)
            {
                Vector3 puntoMax = origenLocal + direccionLocal * tMax;
                if (Math.Abs(puntoMax.Y) <= mitadAltura)
                {
                    mejorT = tMax;
                    Vector2 normalXZ = new Vector2(puntoMax.X, puntoMax.Z);
                    normalXZ.Normalize();
                    normal = new Vector3(normalXZ.X, 0, normalXZ.Y);
                    esTapa = false;
                }
            }

            if (tMin > 0 && tMin < mejorT)
            {
                Vector3 puntoMin = origenLocal + direccionLocal * tMin;
                if (Math.Abs(puntoMin.Y) <= mitadAltura)
                {
                    mejorT = tMin;
                    Vector2 normalXZ = new Vector2(puntoMin.X, puntoMin.Z);
                    normalXZ.Normalize();
                    normal = new Vector3(normalXZ.X, 0, normalXZ.Y);
                    esTapa = false;
                }
            }

            if (mejorT == float.MaxValue)
                return new DataChoque(Vector3.Zero, 0, Vector3.Zero);

            Vector3 puntoContactoLocal = origenLocal + direccionLocal * mejorT;
            Vector3 puntoContacto = puntoContactoLocal + cilindro._centro;

            // Para el caso de choque con el cuerpo del cilindro, calculamos la penetración
            float penetracion = 0;
            if (!esTapa)
            {
                Vector2 proyeccionXZ = new Vector2(puntoContactoLocal.X, puntoContactoLocal.Z);
                float distanciaAlBorde = cilindro._radio - proyeccionXZ.Length();
                penetracion = distanciaAlBorde;
            }

            return new DataChoque(puntoContacto, penetracion, normal);
        }

        
    }
}