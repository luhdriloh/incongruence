﻿using System;
using System.Collections;
using System.Collections.Generic;

using ActionGameFramework.Helpers;
using UnityEngine;

namespace ActionGameFramework.Projectiles
{
    public enum ProjectileEffect
    {
        REGULAR = 0,
        PIERCING = 1,
        BOUNCE = 2,
    };

    /// <summary>
    /// Simple IProjectile implementation for a projectile that flies in a straight line, optionally under m_Acceleration.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class LinearProjectile : MonoBehaviour, IProjectile
    {
        public ProjectileEffect _projectileEffect;
        protected Rigidbody2D _rigidbody;
        public ReturnToPool _returnToPool;
        public event Action fired;

        public bool _playerWeapon;
        protected bool _fired;
        public string _description;
        public float _acceleration;
        public float _accelerationDelta;
        public float _startSpeed;
        public int _damage;
        public int _muzzleFlashFrames;

        private float _timeToLive;
        private float _timeAlive;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            float accelerationToAdd = UnityEngine.Random.Range(0, _accelerationDelta);
            _acceleration -= accelerationToAdd;

            // negative acceleration, so flip the sign
            if (Mathf.Abs(_acceleration) >= Mathf.Epsilon)
            {
                _timeToLive = _startSpeed / -_acceleration;
            }
            else
            {
                _timeToLive = float.MaxValue;
            }

            _timeAlive = 0f;
        }

        /// <summary>
        /// Fires this projectile from a designated start point to a designated world coordinate.
        /// </summary>
        /// <param name="startPoint">Start point of the flight.</param>
        /// <param name="fireVector">Target point to fly to.</param>
        public void FireInDirection(Vector3 startPoint, Vector3 fireVector, float angle)
        {
            transform.eulerAngles = new Vector3(0f, 0f, angle);
            FireInDirection(startPoint, fireVector);
        }

        /// <summary>
        /// Fires this projectile from a designated start point to a designated world coordinate.
        /// </summary>
        /// <param name="startPoint">Start point of the flight.</param>
        /// <param name="targetPoint">Target point to fly to.</param>
        public virtual void FireAtPoint(Vector3 startPoint, Vector3 targetPoint)
        {
            startPoint.z = 1f;
            transform.position = startPoint;

            Fire(Ballistics.CalculateLinearFireVector2D(startPoint, targetPoint, _startSpeed));
        }

        /// <summary>
        /// Fires this projectile in a designated direction.
        /// </summary>
        /// <param name="startPoint">Start point of the flight.</param>
        /// <param name="fireVector">Vector representing direction of flight.</param>
        public virtual void FireInDirection(Vector3 startPoint, Vector3 fireVector)
        {
            transform.position = startPoint;

            // If we have no initial speed, we provide a small one to give the launch vector a baseline magnitude.
            if (Math.Abs(_startSpeed) < float.Epsilon)
            {
                _startSpeed = 0.001f;
            }

            Fire(fireVector.normalized * _startSpeed);
        }

        /// <summary>
        /// Fires this projectile at a designated starting velocity, overriding any starting speeds.
        /// </summary>
        /// <param name="startPoint">Start point of the flight.</param>
        /// <param name="fireVelocity">Vector3 representing launch velocity.</param>
        public void FireAtVelocity(Vector3 startPoint, Vector3 fireVelocity)
        {
            transform.position = startPoint;

            _startSpeed = fireVelocity.magnitude;

            Fire(fireVelocity);
        }

        protected virtual void FixedUpdate()
        {
            if (!_fired)
            {
                return;
            }

            _timeAlive += Time.deltaTime;

            // cases for when to put bullet back into pool
            if (_timeAlive >= _timeToLive)
            {
                _returnToPool(this);
            }

            _rigidbody.velocity += (Vector2)(transform.right * _acceleration * Time.deltaTime);
        }

        protected virtual void Fire(Vector3 firingVector)
        {
            _fired = true;

            float angle = Mathf.Atan2(firingVector.y, firingVector.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            _rigidbody.velocity = firingVector;

            if (fired != null)
            {
                fired();
            }

            _timeAlive = 0f;
        }

        private void ReturnObjectToPool()
        {
            _returnToPool(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool remove = false;
            
            if (collision.gameObject.layer == LayerMask.NameToLayer("player") && _playerWeapon == false)
            {
                collision.GetComponent<Player>().HealthChange(-_damage);
                remove = true;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("enemy") && _playerWeapon == true)
            {
                collision.GetComponent<Enemy>().TakeDamage(-_damage, transform.eulerAngles.z);
                remove = true;
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("mapedge") || (remove && _projectileEffect != ProjectileEffect.PIERCING))
            {
                // if it is a bounce bullet we can make it bounce
                _returnToPool(this);
            }
        }
    }
}