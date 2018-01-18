using G.Base;
using System;
using System.Windows.Controls;

namespace G.Test.Models
{
    public interface IEntityModel: IDisposable
    {
        Entity Data { get; set; }
        void Draw(Canvas canvas);
    }
}