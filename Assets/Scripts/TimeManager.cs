using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class TimeManager : MonoBehaviour
    {
        public float timeLimitRange = 0.05f;
        public float timePace = 0.444f;
        public bool flag = false;
        private bool _startCounting;
        private float _lastTime;


        public EventHandler<EventArgs> TimeNextCommandEventHandler;

        public void StartCounting()
        {
            AudioSource music = GetComponent<AudioSource>();
            _startCounting = true;
            music.Play();
            music.loop = true;
            _lastTime = Time.time;
        }

        void FixedUpdate()
        {
            if (_startCounting)
            {
                flag = true;
                float presentTime = Time.time;
                float timeDelta = presentTime - _lastTime;

                if (timeDelta < timeLimitRange / 2)
                {
                    flag = false;
                }
                else if (timePace + timeLimitRange / 2 < timeDelta)
                {
                    flag = false;
                }

                if (timeDelta - timePace > -0.02 && timeDelta - timePace < 0.02)
                {
                    _lastTime = Time.time - (timeDelta - timePace);
                    TimeNextCommandEventHandler.Invoke(this, new EventArgs());
                }
            }
        }


    }
}
