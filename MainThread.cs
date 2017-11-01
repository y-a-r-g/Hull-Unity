using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Plugins {
    public class MainThread : MonoBehaviour {
        private readonly Queue<Action> _queue = new Queue<Action>();

        private static readonly object Lock = new object();
        private static MainThread _instance;

        private static MainThread Instance {
            get {
                lock (Lock) {
                    if (!_instance) {
                        _instance = FindObjectOfType<MainThread>();
                    }
                    if (!_instance) {
                        _instance = new GameObject("MainThread").AddComponent<MainThread>();
                    }
                    return _instance;
                }
            }
        }

        public static void Init() {
            var instance = Instance;
        }

        public static void Invoke(Action action) {
            Instance._queue.Enqueue(action);
        }

        private void Update() {
            while (_queue.Count > 0) {
                var action = _queue.Dequeue();
                action();
            }
        }
    }
}
