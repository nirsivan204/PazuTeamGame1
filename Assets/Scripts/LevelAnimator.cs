using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.TextCore.Text;

public class LevelAnimator : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;

    public const string IDLE_ANIMATINO_NAME = "Idle";

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public TrackEntry PlayIdleAnimation()
    {
        return SetAddAnimation(IDLE_ANIMATINO_NAME, true, 0, false);
    }

    public TrackEntry SetAddAnimation
        (
            string animationName,
            bool loop,
            int trackIndex,
            bool add
        )
    {
        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("animationName is null or empty!");
            return null;
        }

        Spine.Animation animation = _skeletonAnimation.Skeleton.Data.FindAnimation(animationName);

        if (animation == null)
        {
            Debug.LogError("No animation found for animationName[" + animationName + "]");
            return new TrackEntry();
        }

        if (add)
        {
            return _skeletonAnimation.AnimationState.AddAnimation(trackIndex, animationName, loop, 0f);
        }
        else
        {
            return _skeletonAnimation.AnimationState.SetAnimation(trackIndex, animation, loop);
        }
    }

    public string GetAnimationName()
    {
        return _skeletonAnimation.AnimationName;
    }

    private float GetAnimationLength(string animationName)
    {
        return _skeletonAnimation.Skeleton.Data.FindAnimation(animationName).Duration;
    }


}
