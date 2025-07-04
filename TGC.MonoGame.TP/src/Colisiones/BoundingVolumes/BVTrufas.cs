using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Cameras;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase de esfera para los bounding Volume.
    /// </summary>
    public class BVTrufas 
    {
        
        // Variables
        public BoundingFrustum Frustum;

        private Plane[] _planes = new Plane[6];
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        public BVTrufas(Camaras.Camera camera)
        {
            Matrix nuevaProyeccion = Matrix.CreatePerspectiveFieldOfView(
               camera.FieldOfView, camera.AspectRatio, camera.NearPlane, camera.FarPlane - 1000f);
            Frustum  = new BoundingFrustum(camera.Vista * nuevaProyeccion);
            this.getPlanes();
        }

        public void UpdateFrustum(Camaras.Camera  camera)
        {
            Matrix nuevaProyeccion = Matrix.CreatePerspectiveFieldOfView(
               camera.FieldOfView, camera.AspectRatio, camera.NearPlane, camera.FarPlane - 1000f);
            Frustum.Matrix = camera.Vista * nuevaProyeccion;
            this.getPlanes();
        }


        //----------------------------------------------Funciones-de-Detección--------------------------------------------------// 

   
/*
        public bool colisiona(BoundingVolume volumen){
            return this.colisiona(volumen.GetCentro());
        }
*/

        public bool colisiona(Vector3 centro){
            //Vector3 centro = volumen.GetCentro();
            
            for (int i = 0; i < 6; i++)
            {

                PlaneIntersectionType result2 = PlaneIntersectionType.Front;

                result2 = this.PlanoContain(_planes[i], centro);
                if (result2 == PlaneIntersectionType.Front)
                {
                    return false;
                }
            }

            return true;
        }

        public PlaneIntersectionType PlanoContain(Plane plano , Vector3 punto){

            float result = plano.DotCoordinate(punto);
            if (result > 0f)
            {
                return PlaneIntersectionType.Front;//falso
            }

            if (result < 0f)
            {
                return PlaneIntersectionType.Back;//posible true
            }
            return PlaneIntersectionType.Intersecting;//true si o si
        } 

       
       public void getPlanes()
        {
            _planes[0] = Frustum.Near;
            _planes[1] = Frustum.Far;
            _planes[2] = Frustum.Left;
            _planes[3] = Frustum.Right;
            _planes[4] = Frustum.Top;
            _planes[5] = Frustum.Bottom;
        }
    }
    
    }