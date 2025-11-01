using System;
using UnityEngine;

namespace CoreLogic.MVPPattern
{
    public class Presenter : MonoBehaviour
    {
        public event Action<Presenter> Closed;
        
        public void Close()
        {
            Closed?.Invoke(this);
        }
    }
}