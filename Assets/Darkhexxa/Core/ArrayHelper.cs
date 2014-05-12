using System;
using System.Linq;

namespace Darkhexxa
{
    namespace Core
    {

        public static class ArrayHelper
        {

            public static bool Push<T>(ref T[] array, T obj, bool unique)
            {
                return AddAt(array.Length, ref array, obj, unique);
            }

            public static bool AddAt<T>(int index, ref T[] array, T obj, bool unique)
            {
                if (unique && array.Any(o => o.Equals(obj)))
                {
                    return false;
                }

                T[] newArray = new T[array.Length + 1];
                int i = 0;
                for (; i < index; i++)
                {

                    newArray[i] = array[i];
                }
                newArray[i] = obj;
                i++;
                for (; i < newArray.Length; i++)
                {
                    newArray[i] = array[i - 1];
                }

                array = newArray;

                return true;
            }

            public static T Pop<T>(ref T[] array)
            {
                return RemoveAt(array.Length, ref array);
            }

            public static T RemoveAt<T>(int index, ref T[] array)
            {
                T obj;
                T[] newArray = new T[array.Length - 1];
                int i = 0;
                for (; i < index; i++)
                {

                    newArray[i] = array[i];
                }

                obj = array[i];
                //i++;

                for (; i < newArray.Length; i++)
                {
                    newArray[i] = array[i + 1];
                }

                array = newArray;

                return obj;
            }
        }
    }
}