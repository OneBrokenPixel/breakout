using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
namespace Darkhexxa
{
    namespace SimplePool
    {
        using Core;
        namespace Components
        {
            /**
              * @brief A base pool component that despawns the attached object when it goes off screen.
              */
            [AddComponentMenu ( "DarkHexxa/SimplePool/Components/Cull Finshed Particle Systems" )]
            public class CullFinishedParticlesSystems : BasePoolComponent
            {
                ParticleSystem _particles;
                #region implemented abstract members of BasePoolComponent

                public override void OnSpawn ()
                {
                }

                public override void OnDespawn ()
                {
                    if( _particles != null )
                    {
                        _particles.Stop();
                        _particles.Clear();
                    }
                }

                #endregion

                void Awake()
                {
                    _particles = particleSystem;
                }

                void Update()
                {
                    if( _particles != null && _particles.isStopped )
                    {
                        pool.Despawn (this.gameObject);
                    }
                }

            }
        }
    }
}
