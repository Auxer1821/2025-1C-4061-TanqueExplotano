using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.src.Graficos.Utils;


namespace TGC.MonoGame.TP.src.Moldes
{
    /// <summary>
    ///     Esta es la clase donde se guarda el  modelo; efecto y otras cosas que comparten las entidades iguale para dibujar 
    /// </summary>
    public abstract class IMolde
    {
        protected Effect _efecto;
        public void setProjection(Matrix projection)
        {
            _efecto.Parameters["Projection"].SetValue(projection);
        }
        public void setVista(Matrix vista)
        {
            _efecto.Parameters["View"].SetValue(vista);
        }
        public virtual void setCamara(Vector3 camaraPosition)
        {
            _efecto.Parameters["eyePosition"]?.SetValue(camaraPosition);
        }

        public virtual void Draw(Matrix Mundo, GraphicsDevice Graphics, ShadowMapping shadowMap){
            this.CargarShadowMapper(shadowMap);
            this.Draw(Mundo,Graphics);
        }


        public abstract void Draw(Matrix Mundo, GraphicsDevice Graphics);
        public virtual void setTime(GameTime time)
        {

        }

        public virtual void SetPosSOL(Vector3 posSOL)
        {
            _efecto.Parameters["lightPosition"]?.SetValue(posSOL);
        }

        protected void CargarShadowMapper(ShadowMapping shadowMap)
        {
            _efecto.Parameters["shadowMapSize"].SetValue(shadowMap.GetShadowMapSize());
            _efecto.Parameters["LightViewProjection"].SetValue(shadowMap.GetLightViewProjection());
            _efecto.Parameters["shadowMap"].SetValue(shadowMap.GetShadowMap());
        }

        public abstract void DibujarShadowMap(Matrix matrix, GraphicsDevice graphics);
    }
}