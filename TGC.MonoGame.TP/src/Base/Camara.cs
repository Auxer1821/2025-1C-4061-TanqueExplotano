// using System;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Input;

// namespace TGC.MonoGame.TP.src.Camaras
// {
//     public class Camara
//     {
//         private Vector2 _pastMousePosition;
//         public float MouseSensitivity { get; set; } = 5f;
//         private bool _changed;
//         private float _pitch;
//         private float _yaw = -90f;


//         public Vector3 Posicion { get; private set; }
//         public Vector3 Objetivo { get; private set; }
//         public Vector3 Arriba { get; private set; } = Vector3.Up;
//         public float Velocidad { get; set; } = 100f;

//         public Matrix Vista { get; private set; }
//         public Matrix Proyeccion { get; private set; }

//         //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//

//         public Camara(Vector3 posicionInicial, Vector3 objetivoInicial, float relacionAspecto)
//         {
//             Posicion = posicionInicial;
//             Objetivo = objetivoInicial + new Vector3(0, -10, 0);
//             _pastMousePosition = Mouse.GetState().Position.ToVector2();
//             if (_changed)
//             {
//                 ActualizarVista();
//             }
//             Proyeccion = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, relacionAspecto, 0.1f, 100000f);
//         }

//         //----------------------------------------------Update--------------------------------------------------//

//         public void Actualizar(GameTime gameTime)
//         {
//             var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
//             var teclado = Keyboard.GetState();
//             var direccionMovimiento = Vector3.Zero;

//             // Dirección hacia adelante (normalizada)
//             var frente = Vector3.Normalize(Objetivo - Posicion);
//             var derecha = Vector3.Normalize(Vector3.Cross(frente, Arriba));

//             if (teclado.IsKeyDown(Keys.I))
//                 direccionMovimiento += frente;
//             if (teclado.IsKeyDown(Keys.K))
//                 direccionMovimiento -= frente;
//             if (teclado.IsKeyDown(Keys.J))
//                 direccionMovimiento -= derecha;
//             if (teclado.IsKeyDown(Keys.L))
//                 direccionMovimiento += derecha;
//             if (teclado.IsKeyDown(Keys.O))
//                 direccionMovimiento += Arriba;
//             if (teclado.IsKeyDown(Keys.U))
//                 direccionMovimiento -= Arriba;

//             if (direccionMovimiento != Vector3.Zero)
//             {
//                 direccionMovimiento.Normalize();
//                 float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
//                 Posicion += direccionMovimiento * Velocidad * delta;
//                 Objetivo += direccionMovimiento * Velocidad * delta;
//             }
//             ProcessMouseMovement(elapsedTime);

//             if (_changed)
//             {
//                 ActualizarVista();
//             }
//         }

//         private void ActualizarVista()
//         {
//             Vista = Matrix.CreateLookAt(Posicion, Objetivo, Arriba);
//         }

//         private void ProcessMouseMovement(float elapsedTime)
//         {
//             var mouseState = Mouse.GetState();

//             if (mouseState.RightButton.Equals(ButtonState.Pressed))
//             {
//                 var mouseDelta = mouseState.Position.ToVector2() - _pastMousePosition;
//                 mouseDelta *= MouseSensitivity * elapsedTime;

//                 _yaw -= mouseDelta.X;
//                 _pitch += mouseDelta.Y;

//                 if (_pitch > 89.0f)
//                     _pitch = 89.0f;
//                 if (_pitch < -89.0f)
//                     _pitch = -89.0f;

//                 _changed = true;
//                 UpdateCameraVectors();

//                 Mouse.SetCursor(MouseCursor.Crosshair);
//             }

//             _pastMousePosition = Mouse.GetState().Position.ToVector2();
//         }
        
//         private void UpdateCameraVectors()
//         {
//             // Calculate the new Front vector
//             Vector3 tempFront;
//             tempFront.X = MathF.Cos(MathHelper.ToRadians(_yaw)) * MathF.Cos(MathHelper.ToRadians(_pitch));
//             tempFront.Y = MathF.Sin(MathHelper.ToRadians(_pitch));
//             tempFront.Z = MathF.Sin(MathHelper.ToRadians(_yaw)) * MathF.Cos(MathHelper.ToRadians(_pitch));

//             var frente = Vector3.Normalize(tempFront);

//             // Also re-calculate the Right and Up vector
//             // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
//             var derecha = Vector3.Normalize(Vector3.Cross(frente, Vector3.Up));
//             Arriba = Vector3.Normalize(Vector3.Cross(derecha, frente));
//         }
//     }
// }


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
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _changed = false;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);

            if (_changed)
                CalculateView();
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            var currentMovementSpeed = MovementSpeed;
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 5f;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Position += -RightDirection * currentMovementSpeed * elapsedTime;
                _changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Position += RightDirection * currentMovementSpeed * elapsedTime;
                _changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                Position += FrontDirection * currentMovementSpeed * elapsedTime;
                _changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                Position += -FrontDirection * currentMovementSpeed * elapsedTime;
                _changed = true;
            }
        }

        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                var mouseDelta = mouseState.Position.ToVector2() - _pastMousePosition;
                mouseDelta *= MouseSensitivity * elapsedTime;

                _yaw -= mouseDelta.X;
                _pitch += mouseDelta.Y;

                if (_pitch > 89.0f)
                    _pitch = 89.0f;
                if (_pitch < -89.0f)
                    _pitch = -89.0f;

                _changed = true;
                UpdateCameraVectors();

                if (_lockMouse)
                {
                    Mouse.SetPosition(_screenCenter.X, _screenCenter.Y);
                    Mouse.SetCursor(MouseCursor.Crosshair);
                }
                else
                {
                    Mouse.SetCursor(MouseCursor.Arrow);
                }
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
    }
}