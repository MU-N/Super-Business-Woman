using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will cause a pause when met, preventing any other feedback lower in the sequence to run until it's complete.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will cause a pause when met, preventing any other feedback lower in the sequence to run until it's complete.")]
    [FeedbackPath("Pause/Pause")]
    public class MMFeedbackPause : MMFeedback
    {
        #if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.PauseColor; } }
        #endif
        public override YieldInstruction Pause { get { return _waitForSeconds; } }
        
        [Header("Pause")]
        /// the duration of the pause, in seconds
        [Tooltip("the duration of the pause, in seconds")]
        public float PauseDuration = 1f;
        /// if this is true, you'll need to call the Resume() method on the host MMFeedbacks for this pause to stop, and the rest of the sequence to play
        [Tooltip("if this is true, you'll need to call the Resume() method on the host MMFeedbacks for this pause to stop, and the rest of the sequence to play")]
        public bool ScriptDriven = false;
        
        /// the duration of this feedback is the duration of the pause
        public override float FeedbackDuration { get { return ApplyTimeMultiplier(PauseDuration); } set { PauseDuration = value; } }

        protected WaitForSeconds _waitForSeconds;

        /// <summary>
        /// On init we cache our wait for seconds
        /// </summary>
        /// <param name="owner"></param>
        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            CacheWaitForSeconds();
            ScriptDrivenPause = ScriptDriven;
        }

        /// <summary>
        /// On play we trigger our pause
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (Active)
            {
                StartCoroutine(PlayPause());
            }
        }

        /// <summary>
        /// Pause coroutine
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator PlayPause()
        {
            yield return Pause;
        }

        /// <summary>
        /// Caches our waitforseconds
        /// </summary>
        protected virtual void CacheWaitForSeconds()
        {
            _waitForSeconds = new WaitForSeconds(FeedbackDuration);
        }

        /// <summary>
        /// When changed, we cache our waitforseconds again
        /// </summary>
        protected virtual void OnValidate()
        {
            CacheWaitForSeconds();
        }
    }
}
