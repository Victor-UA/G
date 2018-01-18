using RP.Math;
using System;

namespace G.Base
{
    [Serializable]
    public class Position
    {
        public Position()
        {
            Location = new Vector3(0, 0, 0);
            Direction = new Vector3(0, 1, 0);
        }
        public Vector3 Location { get; set; } = new Vector3(0, 0, 0);

        private Vector3 _direction;
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }

            set
            {
                _direction = value;
                if (_direction.Magnitude == 0)
                {
                    _direction = new Vector3(0, 1, 0);
                }
                else
                {
                    _direction = value.Normalize();                
                }
            }
        }
    }
}
