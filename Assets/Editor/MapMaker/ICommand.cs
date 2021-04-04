using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionTools
{
    public interface ICommand : INamed
    {
        int commandNum { get; set; }
        void Execute();
        void Clear();
        void Update();

        void Undo();
    }
}