using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Amulay.Utility
{
    public class Invoker : SingletonForce<Invoker>
    {
        private List<Action_Duration> delayActions = new List<Action_Duration>();
        private List<Action_Duration> updateActions = new List<Action_Duration>();
        private List<Action_Duration> unScaleUpdateActions = new List<Action_Duration>();

        public static Coroutine DoCoroutine(float delay, [NotNull] Action callBack) => instance.StartCoroutine(DoIenimerator(delay, callBack));
        public static void Kill(Coroutine coroutine) => instance.StopCoroutine(coroutine);
        private static IEnumerator DoIenimerator(float delay, Action callBack)
        {
            yield return new WaitForSeconds(delay);
            callBack();
        }

        public static Action_Duration Do(float delay, [NotNull] Action callback)
        {
            var action = new Action_Duration(delay, callback);
            instance.delayActions.Add(action);
            instance.enabled = true;
            return action;
        }

        public static Action_Duration DoAsync(float delay, [NotNull] Action callback)
        {
            var action = new Action_Duration(delay, callback);
            DoAsync((int)(delay * 1000), callback);
            return action;
        }

        private async void DoAsyncDelay(int delay, Action callback)
        {
            await Task.Delay(delay);
            if (instance == null)
                return;
            callback?.Invoke();
        }

        public static Action_Duration DoUpdate(float time, [NotNull] Action callback)
        {
            var action = new Action_Duration(time, callback);
            instance.updateActions.Add(action);
            instance.enabled = true;
            return action;
        }
        public static Action_Duration DoUnscaleUpdate(float time, [NotNull] Action callback)
        {
            var action = new Action_Duration(time, callback);
            instance.unScaleUpdateActions.Add(action);
            instance.enabled = true;
            return action;

        }

        private void Update()
        {
            if (updateActions.Count > 0)
                ScaledUpdate();

            if (unScaleUpdateActions.Count > 0)
                UnscaleUpdate();

            if (delayActions.Count > 0)
                DelayActions();
        }

        private void UnscaleUpdate()
        {
            float unscaledDeltaTime = Time.unscaledDeltaTime;
            for (int i = 0; i < unScaleUpdateActions.Count; i++)
            {
                if (unScaleUpdateActions[i].pause)
                    continue;
                unScaleUpdateActions[i].duration -= unscaledDeltaTime;
                if (unScaleUpdateActions[i].duration <= 0)
                {
                    unScaleUpdateActions.RemoveRange(i, 1);
                    i--;
                    // if (updateActions.Count == 0 && unScaleUpdateActions.Count == 0)
                    //     enabled = false;
                    continue;
                }
                unScaleUpdateActions[i].action.Invoke();
            }
        }

        private void ScaledUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < updateActions.Count; i++)
            {
                if (updateActions[i].pause)
                    continue;
                updateActions[i].duration -= deltaTime;
                if (updateActions[i].duration <= 0)
                {
                    updateActions.RemoveRange(i, 1);
                    i--;
                    // if (updateActions.Count == 0 && unScaleUpdateActions.Count == 0)
                    //     enabled = false;
                    continue;
                }
                updateActions[i].action.Invoke();
            }
        }

        private void DelayActions()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < delayActions.Count; i++)
            {
                if (delayActions[i].pause)
                    continue;
                delayActions[i].duration -= deltaTime;
                if (delayActions[i].duration <= 0)
                {
                    delayActions[i].action?.Invoke();
                    delayActions.RemoveRange(i, 1);
                    i--;
                    // if (updateActions.Count == 0 && unScaleUpdateActions.Count == 0 && delayActions.Count == 0)
                    //     enabled = false;
                    continue;
                }
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public class Action_Duration
        {
            public float duration;
            public Action action;
            public bool pause = false;

            public Action_Duration(float duration, Action action)
            {
                this.duration = duration;
                this.action = action;
            }

            public void Play()
            {
                pause = false;
            }
            public void Pause()
            {
                pause = true;
            }

            public void Stop()
            {
                duration = -1;
                action = null;
            }
        }
    }
}