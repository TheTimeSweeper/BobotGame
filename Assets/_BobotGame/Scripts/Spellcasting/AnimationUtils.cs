using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtils
{
    public static void PlayAnimationOnAnimator(Animator animator, AnimationStateStringOrInt animationState)
    {
        animator.Play(animationState);
    }
    public static void PlayAnimationOnAnimator(Animator animator, LayerStringOrInt layerIndex, AnimationStateStringOrInt animationState)
        => PlayAnimationOnAnimator(animator, layerIndex, animationState, -1, -1);
    public static void PlayAnimationOnAnimator(Animator animator, LayerStringOrInt layerIndex, AnimationStateStringOrInt animationState, AnimationStateStringOrInt playbackRateParam, float duration)
    {
        //reset animator speed so our future calculations aren't messed with
        animator.speed = 1;
        animator.Update(0);

        if (animationState == -1)
        {
            Debug.Log("invalid animationState hash", animator);
            return;
        }

        layerIndex.Init(animator);
        //we need a layerindex to read
        if (layerIndex < 0)
            return;

        //we have no param to plug in. play normally and leave
        if (playbackRateParam == -1)
        {
            animator.PlayInFixedTime(animationState, layerIndex, 0);
            animator.Update(0);
            return;
        }
        //play the animation at normal speed so we can read its info
        animator.SetFloat(playbackRateParam, 1f);
        animator.PlayInFixedTime(animationState, layerIndex, 0);
        animator.Update(0);

        //read the info and set the playbackrate accordingly
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        float animDuration = stateInfo.length;
        animator.SetFloat(playbackRateParam, animDuration / duration);
    }
}

public struct AnimationStateStringOrInt
{
    private static Dictionary<string, int> cachedHashes = new Dictionary<string, int>();
    private int animatorInt;

    public AnimationStateStringOrInt(string animationStateString)
    {
        this.animatorInt = GetOrCreateCachedHash(animationStateString);
    }
    public AnimationStateStringOrInt(int hash)
    {
        this.animatorInt = hash;
    }
    private static int GetOrCreateCachedHash(string animationStateString)
    {
        if (!cachedHashes.ContainsKey(animationStateString))
        {
            cachedHashes[animationStateString] = Animator.StringToHash(animationStateString);
        }
        return cachedHashes[animationStateString];
    }

    public static implicit operator int(AnimationStateStringOrInt animationParams)
    {
        return animationParams.animatorInt;
    }
    public static implicit operator AnimationStateStringOrInt(string animationStateString)
    {
        return new AnimationStateStringOrInt(animationStateString);
    }
    public static implicit operator AnimationStateStringOrInt(int manualInt)
    {
        return new AnimationStateStringOrInt(manualInt);
    }
}

public struct LayerStringOrInt
{
    private string layerName;
    private int layerIndex;

    public LayerStringOrInt(string animationStateString)
    {
        layerName = animationStateString;
        layerIndex = -1;
    }
    public LayerStringOrInt(int index)
    {
        layerName = null;
        layerIndex = index;
    }

    public static implicit operator int(LayerStringOrInt animationParams)
    {
        return animationParams.layerIndex;
    }
    public static implicit operator LayerStringOrInt(string animationStateString)
    {
        return new LayerStringOrInt(animationStateString);
    }
    public static implicit operator LayerStringOrInt(int manualInt)
    {
        return new LayerStringOrInt(manualInt);
    }

    public void Init(Animator animator)
    {
        if (layerIndex != -1)
            return; //already good
        if (string.IsNullOrEmpty(layerName))
            return;
        if (!animator)
            return;
        layerIndex = animator.GetLayerIndex(layerName);
    }
}