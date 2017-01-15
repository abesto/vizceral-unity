using UnityEngine;
using System;
using System.Linq;

namespace Model
{
    /// <summary>
    ///  https://github.com/Netflix/vizceral/blob/ad4b7f777c83fda89bdeb7a7a86a9aee3e5829de/src/base/connectionView.js
    /// </summary>
    public class ParticleRateCalculator
    {
        private float MaxVolume;
        private float MaxParticlesReleasedPerSecond = 70;
        private float LinearRatio;
        private Vector2[] RateMap;

        public ParticleRateCalculator(float maxVolume)
        {
            // maps the releationship of metric values to how many dots should be released per tick. use < 1 dots per release for fewer than 60 dots per second.
            // [[0, 0], [this.object.volumeGreatest, this.maxParticleReleasedPerTick]] is a straight linear releationship. not great for the left side of the normal distribution -- dots will fire too rarely.
            //  must be in ascending order.
            //  we dont want to a log because we really just want to boost the low end for our needs.
            MaxVolume = maxVolume;
            LinearRatio = MaxParticlesReleasedPerSecond / MaxVolume;
            RateMap = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(float.MinValue, SecondsPerReleaseToReleasesPerSecond(10)),
                new Vector2(1, SecondsPerReleaseToReleasesPerSecond(7)),
                new Vector2(10, SecondsPerReleaseToReleasesPerSecond(5))
            };
            if (maxVolume > 0)
            {
                ExtendRateMap(new Vector2(100, 100 * LinearRatio));
            }
            if (maxVolume > 100)
            {
                ExtendRateMap(new Vector2(maxVolume, MaxParticlesReleasedPerSecond));
            }
        }

        void ExtendRateMap(Vector2 item)
        {
            Vector2[] newRateMap = new Vector2[RateMap.Length + 1];
            RateMap.CopyTo(newRateMap, 0);
            newRateMap[RateMap.Length] = item;
            RateMap = newRateMap;
        }

        float SecondsPerReleaseToReleasesPerSecond(float seconds)
        {
            return 1 / seconds;
        }

        public float Calculate(float volume)
        {
            int i;
            for (i = 0; i < RateMap.Length - 1 && volume > RateMap[i].x; i++) { }
            i = Math.Max(0, i - 1);

            if (i == RateMap.Length - 1)
            {
                return (volume * RateMap[i].y) / RateMap[i].x;
            }

            Vector2 lower = RateMap[i];
            Vector2 upper = RateMap[i + 1];
            return Vector2.Lerp(lower, upper, volume).y;
        }

        public Metrics MetricsToEmitRate(Metrics metrics)
        {
            Metrics rates = new Metrics();
            rates.normal = Calculate(metrics.normal);
            rates.warning = Calculate(metrics.warning);
            rates.danger = Calculate(metrics.danger);
            return rates;
        }
    }
}
