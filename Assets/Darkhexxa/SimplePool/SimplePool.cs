
using System;
using System.Collections;
using UnityEngine;
namespace Darkhexxa
{
	namespace SimplePool
	{
        using Components;
        using Core;

        /**
          * Simple Pool Component
          * give it a game object and it will spawn clones.
          */
		[AddComponentMenu("DarkHexxa/SimplePool/Pool")]
		public class SimplePool : MonoBehaviour
		{
            /**
              * @brief Data for the pool
              */
            [Serializable]
            public class PoolData
            {
                public GameObject prefab; ///> game object to clone
                public int maxCount = 0; ///> max number of clones
                public int batchCreateCount = 5; ///> number of clones to create when new objects are needed.
                public bool cullInactive = false; ///> if culling of incactive objects is desired.
                public float cullInterval = 10f; ///> the interval for the calling process.

                /**
                  * Constructor for PoolData
                  *
                  * @param  GameObject to be cloned
                  */
                public PoolData(GameObject prefab)
                {
                    this.prefab = prefab;
                    this.maxCount = 0;
                    this.batchCreateCount = 5;
                    this.cullInactive = false;
                    this.cullInterval = 10f;
                }

                /**
                  * Constructor for PoolData
                  *
                  * @param  GameObject to be cloned
                  * @param  int maxCount clone
                  * @param  int batchCreateCount to clone when new objects are required
                  * @param  bool cull inactive objects
                  * @param  float the cull interval
                  */
                public PoolData(GameObject prefab, int maxCount, int batchCreateCount, bool cullInactive, float cullInterval)
                {
                    this.prefab = prefab;
                    this.maxCount = maxCount;
                    this.batchCreateCount = batchCreateCount;
                    this.cullInactive = cullInactive;
                    this.cullInterval = cullInterval;
                }
                /**
                  * is equal to operator
                  *
                  * @param  first PoolData object
                  * @param  Second PoolData object
                  * @return true if objects are equal.
                  */
                public static bool operator == ( PoolData data1, PoolData data2 )
                {
                    // If both are null, or both are same instance, return true.
                    if (System.Object.ReferenceEquals(data1, data2))
                    {
                        return true;
                    }

                    // If one is null, but not both, return false.
                    if (((object)data1 == null) || ((object)data2 == null))
                    {
                        return false;
                    }

                    return (      data1.prefab.Equals(data2.prefab)
                               && data1.maxCount == data2.maxCount
                               && data1.cullInactive == data2.cullInactive );
                }

                /**
                  * Equals override
                  *
                  * @param  PoolData object
                  * @return true if objects are equal.
                  */
                public override bool Equals(object obj)
                {
                    if (obj == null)
                    {
                        return false;
                    }

                    PoolData p = obj as PoolData;
                    if ((System.Object)p == null)
                    {
                        return false;
                    }

                    return (this.prefab.Equals(p.prefab)
                               && this.maxCount == p.maxCount
                               && this.cullInactive == p.cullInactive);
                }

                /**
                  * is not equal to operator
                  *
                  * @param  first PoolData object
                  * @param  Second PoolData object
                  * @return true if objects are equal.
                  */
                public static bool operator !=(PoolData data1, PoolData data2)
                {
                    return (!data1.prefab.Equals(data2.prefab)
                               && data1.maxCount != data2.maxCount
                               && data1.cullInactive != data2.cullInactive);
                }
                /**
                  * generate hash code
                  *
                  * @return int hashcode.
                  */
                public override int GetHashCode()
                {
                    return base.GetHashCode();
                }
            }

            public PoolData data = new PoolData(null); ///< Pool data object for this pool.

            private int _count = 0; //< number of objects.

            /**
              * Full attrubute.
              *
              * @return returns true if the pool is not full
              */
			private bool isFull {
                get { return (_count < data.maxCount || data.maxCount == 0); }
			}

            ComponentList _inactive = null; ///< list of inactive objects created;
			ComponentList _active = null; ///< list of active objects spawned.

            /**
             * @brief Corutine for culling inactive objects
             */
			IEnumerator CullRoutine()
			{
                while (data.cullInactive)
				{
                    yield return new WaitForSeconds(data.cullInterval);

					//Debug.Log("Culling");

                    int cullCount = _inactive.Count - data.batchCreateCount;
					if (cullCount >0)
					{
						RemoveGameObjects(_inactive, cullCount);
					}
				}
			}

            /**
              * @brief generates new game objects.
              */
			private void AddNewGameObjects()
			{
                if (data.prefab != null)
				{
					//Debug.Log ("Adding New Game Objects to " + prefab.name + " pool");
                    for (int i = 0; i < data.batchCreateCount && isFull; i++)
					{
						//Debug.Log(i);
                        GameObject obj = GameObject.Instantiate(data.prefab) as GameObject;

						obj.SetActive(false);
						obj.transform.parent = transform;

						BasePoolComponent bhv = obj.GetComponent<BasePoolComponent>();

						if(bhv != null)
							bhv.OnCreatedByPool(this);

						_inactive.InsertAtTail(ref obj);
						_count++;
					}
				}
			}

            /**
              * @brief removes game objects from a given list.
              */
			void RemoveGameObjects(ComponentList list, int count)
			{
				for (int i = 0; i< count && !list.isEmpty; i++)
				{
					GameObject obj = list.RemoveTailGO();
					GameObject.Destroy(obj);
					_count--;
				}
			}

            /**
              * @brief spawns a prfab with the spawners position and rotation
              */
			public GameObject Spawn()
			{
				return Spawn (transform.position, transform.rotation);
			}

            /**
              * @brief spawns a clone at a given position and rotation
              * @param  position Vector3 position to spawn at
              * @param  rotation Quaternion rotation to set the new object to.
              * @return Spawned Game Object;
              */
            public GameObject Spawn( Vector3 position, Quaternion rotation )
			{
                if ( data.prefab == null)
				{
					return null;
				}

				if( _inactive.isEmpty && isFull)
				{
					AddNewGameObjects();
				}

				if(!_inactive.isEmpty)
				{
					GameObject obj = _inactive.RemoveHeadGO();
					if( obj != null )
					{

						obj.transform.position = position;
						obj.transform.rotation = rotation;

						if( rigidbody != null )
						{
							rigidbody.velocity = Vector3.zero;
							rigidbody.angularVelocity = Vector3.zero;
						}

						if( rigidbody2D != null )
						{
							rigidbody2D.velocity = Vector2.zero;
							rigidbody2D.angularVelocity = 0f;
						}

						
						obj.SetActive(true);

						BasePoolComponent cmp = obj.GetComponent<BasePoolComponent>();
						if( cmp != null )
						{
							cmp.OnSpawn();
						}


						_active.InsertAtTail(ref obj);

						return obj;
					}
				}

				return null;
			}

            /**
              * @brief despawns on object
              * @param  GameObject game objects to remove from the active scene.
              */
			public void Despawn(GameObject obj)
			{
				PoolListableComponent listable = obj.GetComponent<PoolListableComponent>();
				if( listable != null )
				{
					if( listable.list.Equals(_active) )
					{
						obj.SetActive(false);

						BasePoolComponent cmp = obj.GetComponent<BasePoolComponent>();

						if( cmp != null)
						{
							cmp.OnDespawn();
						}

						_active.Remove(obj);
						_inactive.InsertAtTail(ref obj);
					}
				}
			}

            /**
              * @brief unity start function
              * 
              * Sets the object pool and prefab up for use with the pool. 
              */
			void Start ()
			{



				StartCoroutine (CullRoutine ());


				/*
				for(int i = 0; i < 6; i++)
					Spawn();
					*/
			}

            /**
              * @brief set up active and inactive lists before start.
              * 
              * Sets the object pool and prefab up for use with the pool. 
              */
            void Awake()
            {

                if ( data.prefab != null )
                {
                    PoolListableComponent listable = data.prefab.GetComponent<PoolListableComponent> ();
                    if ( listable == null )
                    {
                        data.prefab.AddComponent<PoolListableComponent> (); ///< adds a listable component to the prefab objects so the clones can be listed.
                    }
                }

                _inactive = new ComponentList ();
                _active = new ComponentList ();

                AddNewGameObjects ();
            }

            /**
              * @brief static find function
              * 
              * Seaches though all pools in the scene for one that generates the prefab.
              * @param  GameObject the prefab of the pool you wish to find
              * @return a matching pool or null;
              */
            public static SimplePool FindPoolFor(GameObject prefab)
            {
                foreach (SimplePool pool in FindObjectsOfType<SimplePool>())
                {
                    if (pool.data.prefab.Equals(prefab))
                    {
                        return pool;
                    }
                }

                return null;
            }

            /**
              * @brief static find function
              * 
              * Seaches though all pools in the scene for one that generates the prefab.
              * @param  GameObject the prefab of the pool you wish to find
              * @return IEnumerable<SimplePool> yielding all matching pools;
              */
            public static System.Collections.Generic.IEnumerable<SimplePool> FindAllPoolsFor(GameObject prefab)
            {
                foreach (SimplePool pool in FindObjectsOfType<SimplePool>())
                {
                    if (pool.data.prefab.Equals(prefab))
                    {
                        yield return pool;
                    }
                }

                yield break;
            }

            /**
              * @brief static find function
              * 
              * Seaches though all pools in the scene for one that has a similar PoolData block
              * @param  GameObject the prefab of the pool you wish to find
              * @return a matching pool or null;
              */
            public static SimplePool FindPoolFor(PoolData data)
            {
                foreach (SimplePool pool in FindObjectsOfType<SimplePool>())
                {
                    if ( pool.data == data )
                    {
                        return pool;
                    }
                }

                return null;
            }

            /**
              * @brief static find function
              * 
              * Seaches though all pools in the scene for one that has a similar PoolData block
              * @param  GameObject the prefab of the pool you wish to find
              * @return IEnumerable<SimplePool> yielding all matching pools;
              */
            public static System.Collections.Generic.IEnumerable<SimplePool> FindAllPoolsFor(PoolData data)
            {
                foreach (SimplePool pool in FindObjectsOfType<SimplePool>())
                {
                    if (pool.data == data)
                    {
                        yield return pool;
                    }
                }

                yield break;
            }


            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  GameObject the prefab of the pool you wish to find/create
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(GameObject prefab)
            {
                PoolData data = new PoolData(prefab);
                return CreatePool(data);
            }

            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  GameObject the prefab of the pool you wish to find/create
              * @param  int maxium number of objects to clone
              * @param  int the number of objects to create when new objects are needed.
              * @param  bool cull the inactive objects on a set interval
              * @param  float the interval of the cull events.
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(GameObject prefab, int maxCount, int batchCreateCount, bool cullInactive, float cullInterval)
            {
                PoolData data = new PoolData(prefab, maxCount, batchCreateCount, cullInactive, cullInterval);
                return CreatePool(data);
            }

            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  string name of the GameObject the pool component is attached to
              * @param  GameObject the prefab of the pool you wish to find/create
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(string gameObjectName, GameObject prefab)
            {
                PoolData data = new PoolData(prefab);
                return CreatePool(gameObjectName, data);
            }

            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  string name of the GameObject the pool component is attached to
              * @param  GameObject the prefab of the pool you wish to find/create
              * @param  int maxium number of objects to clone
              * @param  int the number of objects to create when new objects are needed.
              * @param  bool cull the inactive objects on a set interval
              * @param  float the interval of the cull events.
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(string gameObjectName, GameObject prefab, int maxCount, int batchCreateCount, bool cullInactive, float cullInterval)
            {
                PoolData data = new PoolData(prefab, maxCount, batchCreateCount, cullInactive, cullInterval);
                
                return CreatePool(gameObjectName, data);
            }

            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  PoolData object that is used to create the pool component.
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(PoolData data)
            {
                if (data == null || data.prefab == null)
                    return null;

                return CreatePool(data.prefab.name + "_pool",data);
            }

            /**
              * @brief static create pool function
              * 
              * searches for a similar pool if non is found it will create one.
              * @param  string name of the GameObject the pool component is attached to
              * @param  PoolData object that is used to create the pool component.
              * @return a matching pool or a new pool;
              */
            public static SimplePool CreatePool(string gameObjectName, PoolData data)
            {

                if (data == null || data.prefab == null)
                    return null;

                SimplePool pool = FindPoolFor(data);

                if (pool == null)
                {
                    GameObject obj = new GameObject(data.prefab.name + "_pool");
                    pool = obj.AddComponent<SimplePool>();
                    pool.data = data;
                }

                return pool;
            }
		}
	}
}
