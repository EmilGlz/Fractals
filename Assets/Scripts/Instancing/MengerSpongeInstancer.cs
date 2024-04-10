using UnityEngine;

namespace Assets.Scripts.Instancing
{
    public class MengerSpongeInstancer : Instancer<MengerSpongeInstancer>
    {
        protected override void UpdateAnimationParameters(Vector3 parentScale)
        {
            StartingAnimationScale = parentScale;
            EndingAnimationScale = Vector3.zero;
        }

        protected override void OnAnimationFinished()
        {
            CurrentScalingMeshes.Clear();
        }
    }
}
