using UnityEngine;
using System.Collections;

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
            [AddComponentMenu("DarkHexxa/SimplePool/Components/Cull Off Screen")]
			public class CullOffScreen :  BasePoolComponent{
				#region implemented abstract members of BasePoolComponent

				public override void OnSpawn ()
				{
				}

				public override void OnDespawn ()
				{
				}

				#endregion

				void OnBecameInvisible()
				{
					pool.Despawn(gameObject);
				}

			}
		}
	}
}
