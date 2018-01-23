using RP.Math;
using System;

namespace G.Base
{
    public interface IEntity: IDisposable
    {
        int ID { get; }
        Position Position { get; }
        Vector3 Size { get; }
        double Mass { get; }

        void Start();        
        void Pause();
        void Resume();
    }
}