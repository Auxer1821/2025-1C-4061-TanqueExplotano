using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Camaras;

namespace TGC.MonoGame.TP.src.Cameras
{
    public class FreeCamera : Camera
    {
        private readonly bool _lockMouse;

        private readonly Point _screenCenter;
        private bool _changed;

        private Vector2 _pastMousePosition;
        private float _pitch;

        private bool _bloquearMouse = true;
        private bool estaSacudida = false;
        private float tiempoSacudida = 1.2f;

        // Angles
        private float _yaw = -90f;

        public FreeCamera(float aspectRatio, Vector3 position, Point screenCenter) : this(aspectRatio, position)
        {
            _lockMouse = true;
            this._screenCenter = screenCenter;
        }

        public FreeCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Position = position;
            _pastMousePosition = Mouse.GetState().Position.ToVector2();
            UpdateCameraVectors();
            CalculateView();
        }

        public float MovementSpeed { get; set; } = 100f;
        public float MouseSensitivity { get; set; } = 5f;

        private void CalculateView()
        {
            Vista = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _changed = false;
            ProcessMouseMovement(elapsedTime);

            if (_changed)
                CalculateView();
        }

        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            desbloquearMouse();
            if (_bloquearMouse)
            {
                var mouseDelta = mouseState.Position.ToVector2() - _pastMousePosition;
                mouseDelta *= MouseSensitivity * elapsedTime;

                _yaw += mouseDelta.X;
                _pitch -= mouseDelta.Y;

                //bloquea la camara para que no mire hacia arriba o abajo
                if (_pitch > 39.0f)
                    _pitch = 39.0f;
                if (_pitch < -29.0f)
                    _pitch = -29.0f;

                _changed = true;
                UpdateCameraVectors();

                if (_lockMouse)
                {
                    Mouse.SetPosition(_screenCenter.X, _screenCenter.Y);
                }
                else
                {
                    Mouse.SetCursor(MouseCursor.Arrow);
                }
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }

            _pastMousePosition = Mouse.GetState().Position.ToVector2();
        }

        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(_yaw)) * MathF.Cos(MathHelper.ToRadians(_pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(_pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(_yaw)) * MathF.Cos(MathHelper.ToRadians(_pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }

        internal void setPosicion(Vector3 posicion, Vector3 direcion)
        {
            Position = posicion + new Vector3(0f, 10f, 0f) - direcion * 0f;//30f dir
        }

        // Actualiza la posición y dirección de la cámara  
        public void actualizarCamara(Vector3 posicion, Vector3 direcion, GameTime gameTime)
        {
            if (estaSacudida)
            {
                sacudida(posicion, direcion, gameTime);
            }
            else
            {
                setPosicion(posicion, direcion);
            }
        }

        internal Vector3 getDireccion()
        {
            return FrontDirection;
        }

        private void desbloquearMouse()
        {
            var tecladoState = Keyboard.GetState();
            if (tecladoState.IsKeyDown(Keys.P)) this._bloquearMouse = false;
            else if (tecladoState.IsKeyDown(Keys.O)) this._bloquearMouse = true;

        }

        public void sacudida(Vector3 posicion, Vector3 direcion, GameTime gameTime)
        {
            if (estaSacudida)
            {
                float intensidad = 0.4f; // Intensidad de la sacudida
                                         //deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                         // Calcular progreso del shake (1 al inicio, 0 al final)
                float progreso = tiempoSacudida / 1.2f; // Duración total de la sacudida

                // Reducir magnitud según progreso (efecto de decaimiento)
                float magnitud = intensidad * progreso * progreso;

                // Calcula el desplazamiento de la sacudida
                /*
                float desplazamientoX = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 50) * magnitud);
            float desplazamientoY = (float)(Math.Cos(gameTime.TotalGameTime.TotalSeconds * 50) * magnitud);
            */
            Vector3 randomOffset = new Vector3(
            (float)(new Random().NextDouble() * 2 - 1) * magnitud,
            (float)(new Random().NextDouble() * 2 - 1) * magnitud,
            0); // Normalmente no sacudimos en Z

            // Actualiza la posición de la cámara con el desplazamiento
            //setPosicion(posicion + new Vector3(desplazamientoX, desplazamientoY, 0), direcion);
            setPosicion(posicion + randomOffset, direcion);


                tiempoSacudida -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Desactiva la sacudida después de la duración
                if (tiempoSacudida <= 0)
                {
                    tiempoSacudida = 1.2f;
                    estaSacudida = false;
                }
            }
        }

        public void setSacudida(bool sacudida)
        {
            estaSacudida = sacudida;
        }
    }
}