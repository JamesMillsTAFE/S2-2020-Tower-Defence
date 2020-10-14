using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Utilities
{
    public class MathUtils
    {
        public static void DistanceAndDirection(out float _distance, out Vector3 _direction, Transform _from, Transform _to)
        {
            Vector3 heading = _to.position - _from.position;
            _distance = heading.magnitude;
            _direction = heading.normalized;
        }

        public static void DistanceAndDirection(out float _distance, out Vector3 _direction, Vector3 _from, Vector3 _to)
        {
            Vector3 heading = _to - _from;
            _distance = heading.magnitude;
            _direction = heading.normalized;
        }
    }
}