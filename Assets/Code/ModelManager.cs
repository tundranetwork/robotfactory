using System;
using System.Collections.Generic;
using RobotFactory.Model;
using UnityEngine;

namespace RobotFactory
{
    public class ModelManager : MonoBehaviour
    {
        private readonly List<object> _models = new List<object>();
        private List<Action<ModelManager>> _readyHandlers = new List<Action<ModelManager>>();
        private bool _ready = false;

        private void Start()
        {
            // Seed the models the game fundamentally needs
            _models.Add(new Factory(100, 100));

            // We're ready now
            _ready = true;

            // Check if anyone registered with us before the models were seeded
            foreach (var handler in _readyHandlers)
            {
                handler(this);
            }
            _readyHandlers = null;
        }

        /// Notify the the caller through the given handler as soon as the model manager is ready to provide models.
        /// This may be immediately if the model manager has already been initialized by Unity.
        public void Link(Action<ModelManager> handler)
        {
            if (!_ready)
            {
                _readyHandlers.Add(handler);
            }
            else
            {
                handler(this);
            }
        }

        public T Require<T>() where T : class
        {
            foreach (var model in _models)
            {
                if (model.GetType() == typeof(T))
                {
                    return (T) model;
                }
            }

            throw new InvalidOperationException("Unable to find model type");
        }
    }
}