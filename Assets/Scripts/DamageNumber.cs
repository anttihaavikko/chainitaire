using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageNumber : MonoBehaviour
{
    public List<TMPro.TMP_Text> nums;

    public void QueueRemove()
    {
        Invoke("Remove", 3f);
    }

    public void SetNumber(string message, float scale = 1f)
    {
        nums.ForEach(num => num.text = message);
        transform.localScale *= scale;
    }

    // Update is called once per frame
    void Remove()
    {
        EffectManager.Instance.ReturnTextToPool(this);
    }
}
