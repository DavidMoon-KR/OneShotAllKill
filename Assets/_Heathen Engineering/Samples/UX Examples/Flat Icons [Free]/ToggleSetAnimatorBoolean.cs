using UnityEngine;

namespace HeathenEngineering.UX.Samples
{
    public class ToggleSetAnimatorBoolean : MonoBehaviour
    {
        public Animator animator;
        public string booleanname;

        public void SetBoolean(bool value)
        {
            animator.SetBool(booleanname, value);
        }
    }
}
