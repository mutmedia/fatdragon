using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    class TimeManager : MonoBehaviour
    {
        public float timeLimitRange;
        public float timePace;
        public bool flag = false;
        private bool _starCounting;
        private float _lastTime;

        public EventHandler<EventArgs> TimeNextCommand;

        public void StarCounting()
        {
            _starCounting = true;
            _lastTime = Time.time;
        }

        void Update()
        {
            if (_starCounting)
            {
                flag = true;
                float presentTime = Time.time;
                float timeDelta = presentTime - _lastTime;
                if (timeDelta < timePace - timeLimitRange / 2)
                {
                    flag = false;
                }
                else if (timePace + timeLimitRange / 2 < timeDelta)
                {
                    _lastTime = Time.time;
                    //TimeNextCommand.Invoke(this, new EventArgs());
                    flag = false;
                }
            }
        }


    }
}
